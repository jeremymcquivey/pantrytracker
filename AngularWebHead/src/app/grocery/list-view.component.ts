import { OnInit, Component } from '@angular/core';
import { MatTableDataSource }  from '@angular/material/table'
import { GroceryItem, GroceryItemStatus } from '../model/product-grocery-list';
import { GroceryService } from '../core/grocery.service';
import { AuthService } from '../core/auth.service';

@Component({
    selector: 'grocery-list-view',
    templateUrl: './list-view.component.html',
    styleUrls: ['../controls/fancy-form.component.css',
                './list-view.component.css']
})
export class GroceryListViewComponent implements OnInit {
    private _listId: string = 'c21e47a0-202e-44a7-a708-e31ecd3058db';
    private _groceryItems: GroceryItem[];
    private _grocerySource:MatTableDataSource<GroceryItem> = new MatTableDataSource<GroceryItem>();
    private _allColumns: string[] = ["name","removed","markPurchased"];
    private _nonEditColumns: string[] = ["name"]
    private _editMode: boolean = false;

    public showTips: boolean = false;

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

    public get GroceryItems(): MatTableDataSource<GroceryItem> {
        return this._grocerySource;
    }

    constructor(private _groceryService: GroceryService, private _authService: AuthService) { }

    ngOnInit(): void { 
        //TODO: This is temporary; once we support multiple lists, we'll use legit list ids.
        this._authService.fetchSecurityContext()
                         .subscribe(ctxt => {
                            this._listId = ctxt.userProfile.id;
                            this.loadGroceryList();
                         }, error => console.error(error));
    }

    toggleEditMode() {
        this._editMode = !this._editMode;
    }

    formatItem(item: GroceryItem) {
        return `${item?.quantity ?? 1} | ${item?.size ?? ''} ${item?.unit ?? ''} ${item?.variety?.description ?? ''} ${item?.product?.name ?? ''}`
            .replace('  ', ' ')
            .trim();
    }

    removeItem(item: GroceryItem) {
        this._groceryService.removeGroceryItem(this._listId, item.id)
                            .subscribe(_ => {
                                this.removeFromList(item);
                            }, error => {
                                console.error(error);
                            });
    }

    acceptItem(item: GroceryItem) {
        item.status = GroceryItemStatus.Purchased;
        this._groceryService.updateGroceryItem(this._listId, item)
                            .subscribe(_ => {
                                this.removeFromList(item);
                            }, error => {
                                console.error(error);
                            });
    }

    private loadGroceryList() {
        this._groceryService.getGroceryList(this._listId)
                            .subscribe(things => {
                                this._groceryItems = things;
                                this._grocerySource.data = things;
                            });
    }

    private removeFromList(item: GroceryItem) {
        const removedIndex = this._groceryItems.findIndex(p => p.id == item.id);
        this._groceryItems.splice(removedIndex, 1);
        this._grocerySource.data = this._groceryItems;
    }
}