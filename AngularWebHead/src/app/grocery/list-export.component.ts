import { Component, OnInit, ViewChild } from "@angular/core";
import { MatTableDataSource } from "@angular/material/table";
import { GroceryItem, GroceryItemStatus } from "../model/product-grocery-list";
import { GroceryService } from "../core/grocery.service";
import { AuthService } from "../core/auth.service";
import { ActivatedRoute } from "@angular/router";
import { PantryLine } from "../model/pantryline";
import { InventoryTransactionComponent } from "../pantry/inventory-transaction.component";

@Component({
    selector: 'grocery-list-export',
    templateUrl: './list-export.component.html',
    styleUrls: ['list-view.component.css']
})
export class GroceryListExportComponent implements OnInit {
    private _listId: any;
    private _currentlyEditing: GroceryItem;

    public PurchasedItems: MatTableDataSource<GroceryItem> =
        new MatTableDataSource<GroceryItem>();

    public VisiblePurchasedColumns: string[] =
        ["name", "removed", "editPurchased"];

    @ViewChild('inventoryTransaction') inventoryDialog: InventoryTransactionComponent;

    constructor(private _groceryService: GroceryService, private _authService: AuthService, private _route: ActivatedRoute) { }

    ngOnInit(): void { 
        if(this._route.snapshot.params.listId) {
            this._listId = this._route.snapshot.params.listId;
            this.loadGroceryList();
        } else {
            this._authService.authContextChanged.subscribe(ctxt => {
                this._listId = ctxt.userProfile.id;
                this.loadGroceryList();
            });
        }
    }

    formatItem(item: GroceryItem) {
        return `${item?.quantity ?? 1} | ${item?.size ?? ''} ${item?.unit ?? ''} ${item?.variety?.description ?? ''} ${item?.product?.name ?? ''}`
            .replace('  ', ' ')
            .trim();
    }

    editItem(item: GroceryItem) {
        this._currentlyEditing = item;

        const inventory = new PantryLine();
        inventory.productId = item.productId;
        inventory.product = item.product;
        inventory.quantity = item.quantity;
        inventory.size = Number.parseFloat(item.size);
        inventory.unit = item.unit;
        inventory.variety = item.variety;
        inventory.varietyId = item.varietyId;

        this.inventoryDialog.LaunchWithLineItem(inventory);
    }

    removeItem(item: GroceryItem) {
        this._currentlyEditing.status = GroceryItemStatus.Archived;
        this._groceryService.updateGroceryItem(this._listId, this._currentlyEditing)
                            .subscribe(_ => {
                                this.removeFromList(this._currentlyEditing);
                            }, error => {
                                console.error(error);
                            }, () => {
                                this._currentlyEditing = null;
                            });
    }

    ignoreItem(item: GroceryItem) {
        if(!confirm(`This will delete ${item.product.name} from your purchase.`))
        {
            return;
        }

        item.status = GroceryItemStatus.Archived;
        this._groceryService.updateGroceryItem(this._listId, item)
                            .subscribe(_ => {
                                this.removeFromList(item);
                            }, error => {
                                console.error(error);
                            }, () => {
                            });
    }

    editCancelled() {
        this._currentlyEditing = null;
    }

    private loadGroceryList() {
        this._groceryService.getGroceryList(this._listId)
                            .subscribe(things => {
                                const purchasedThings = things.filter(p => p.status == GroceryItemStatus.Purchased);
                                this.PurchasedItems.data = purchasedThings;
                            });
    }

    private removeFromList(item: GroceryItem) {
        const removedIndex = this.PurchasedItems.data.findIndex(p => p.id == item.id);
        this.PurchasedItems.data.splice(removedIndex, 1);
        this.PurchasedItems.data = this.PurchasedItems.data;
    }
}