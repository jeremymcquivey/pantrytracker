import { OnInit, Component, ViewChild, AfterViewInit } from '@angular/core';
import { MatTableDataSource }  from '@angular/material/table'
import { GroceryItem, GroceryItemStatus } from '../model/product-grocery-list';
import { GroceryService } from '../core/grocery.service';
import { AuthService } from '../core/auth.service';
import { ActivatedRoute, Router } from '@angular/router';
import { QuickProductAddComponent } from './quick-add.component';

@Component({
    selector: 'grocery-list-view',
    templateUrl: './list-view.component.html',
    styleUrls: ['../controls/fancy-form.component.css',
                './list-view.component.css']
})
export class GroceryListViewComponent implements OnInit, AfterViewInit {
    private _listId: string;
    private _groceryItems: GroceryItem[];
    private _grocerySource:MatTableDataSource<GroceryItem> = new MatTableDataSource<GroceryItem>();
    private _purchasedSource:MatTableDataSource<GroceryItem> = new MatTableDataSource<GroceryItem>();
    private _allColumns: string[] = ["name","removed","markPurchased"];
    private _allPurchasedColumns: string[] = ["name","undoPurchased"];
    private _nonEditColumns: string[] = ["name"]
    private _editMode: boolean = true;
    private _isBusy: boolean = false;

    public showTips: boolean = false;

    public get listId(): string {
        return this._listId;
    }

    public get tips(): string[] {
        return [
            "Generate a list from a recipe.",
            "Search for individual items.",
            "Add freeform text items."
        ];
    }

    public get EditMode(): boolean {
        return this._editMode;
    }

    public get VisibleColumns(): string[] {
        return this._editMode ? this._allColumns : this._nonEditColumns;
    }

    public get VisiblePurchasedColumns(): string[] {
        return this._editMode ? this._allPurchasedColumns : this._nonEditColumns;
    }

    public get GroceryItems(): MatTableDataSource<GroceryItem> {
        return this._grocerySource;
    }

    public get PurchasedItems(): MatTableDataSource<GroceryItem> {
        return this._purchasedSource;
    }

    @ViewChild('quickAdd') quickAdd: QuickProductAddComponent;

    constructor(private _groceryService: GroceryService, 
        private _authService: AuthService, 
        private _route: ActivatedRoute, 
        private _router: Router) { }

    ngAfterViewInit(): void {
        if(!!this._listId && !!this.quickAdd) {
            this.quickAdd.listId = this._listId;
        }
    }

    ngOnInit(): void { 
        if(this._route.snapshot.params.listId) {
            this._listId = this._route.snapshot.params.listId;
            this.loadGroceryList();
        } else if(!!this._authService.authContext && !!this._authService.authContext.userProfile) {
            this._listId = this._authService.authContext.userProfile.id;
            this.loadGroceryList();
        } else {
            this._authService.authContextChanged.subscribe(ctxt => {
                this._listId = ctxt.userProfile.id;
                this.quickAdd.listId = this._listId;
                this.loadGroceryList();
            });
        }
    }

    toggleEditMode() {
        this._editMode = !this._editMode;
    }

    formatItem(item: GroceryItem) {
        if(!!item && !!item.freeformText) {
            return item.freeformText;
        } else {
            return `${item?.quantity ?? 1} | ${item?.size ?? ''} ${item?.unit ?? ''} ${item?.variety?.description ?? ''} ${item?.product?.name ?? ''}`
                .replace('  ', ' ')
                .trim();
        }
    }

    removeItem(item: GroceryItem) {
        if(this._isBusy) {
            return;
        }

        this._isBusy = true;
        this._groceryService.removeGroceryItem(this._listId, item.id)
                            .subscribe(_ => {
                                this.removeFromList(item);
                            }, error => {
                                console.error(error);
                            }, ()=>{
                                this._isBusy = false;
                            });
    }

    acceptItem(item: GroceryItem) {
        if(this._isBusy) {
            return;
        }

        this._isBusy = true;
        item.status = GroceryItemStatus.Purchased;
        this._groceryService.updateGroceryItem(this._listId, item)
                            .subscribe(_ => {
                                this.removeFromList(item);
                                this.PurchasedItems.data.push(item);
                                this.PurchasedItems.data = this.PurchasedItems.data;
                            }, error => {
                                console.error(error);
                            }, () => {
                                this._isBusy = false;
                            });
    }

    freeformProductAdded(item: GroceryItem) {
        this.GroceryItems.data.push(item);
        this.GroceryItems.data = this.GroceryItems.data;
    }

    completeTransaction() {
        this._router.navigate([`grocery-list/${this._listId}/export`]);
    }

    public undoItem(item: GroceryItem) {
        if(this._isBusy) {
            return;
        }

        this._isBusy = true;
        item.status = GroceryItemStatus.Active;
        this._groceryService.updateGroceryItem(this._listId, item)
                            .subscribe(_ => {
                                this.removeFromPurchased(item);
                                this.GroceryItems.data.push(item);
                                this.GroceryItems.data = this.GroceryItems.data;
                            }, error => {
                                console.error(error);
                            }, () => {
                                this._isBusy = false;
                            });
    }

    private loadGroceryList() {
        this._groceryService.getGroceryList(this._listId)
                            .subscribe(things => {
                                const groceryThings = things.filter(p => p.status == GroceryItemStatus.Active);
                                this._groceryItems = groceryThings;
                                this._grocerySource.data = groceryThings;
                                
                                const purchasedThings = things.filter(p => p.status == GroceryItemStatus.Purchased);
                                this._purchasedSource.data = purchasedThings;
                            });
    }

    private removeFromList(item: GroceryItem) {
        const removedIndex = this._groceryItems.findIndex(p => p.id == item.id);
        this._groceryItems.splice(removedIndex, 1);
        this._grocerySource.data = this._groceryItems;
    }

    private removeFromPurchased(item: GroceryItem) {
        const removedIndex = this._purchasedSource.data.findIndex(p => p.id == item.id);
        this._purchasedSource.data.splice(removedIndex, 1);
        this._purchasedSource.data = this._purchasedSource.data;
    }
}