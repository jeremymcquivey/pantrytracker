import { Component, Output, EventEmitter, Input } from "@angular/core";
import { GroceryItem } from "../model/product-grocery-list";
import { GroceryService } from "../core/grocery.service";

@Component({
    selector: 'quick-product-add',
    templateUrl: 'quick-add.component.html',
    styleUrls: ['../controls/fancy-form.component.css']
})
export class QuickProductAddComponent {
    public freeformText: string = '';
    public networkIsBusy: boolean = false;

    @Input() listId: string;

    @Output() onItemAdded: EventEmitter<GroceryItem> = new EventEmitter<GroceryItem>();

    constructor(private _groceryService: GroceryService) { }

    public quickAdd() {
        if(this.freeformText.length < 1) {
            return;
        }
        
        if(this.networkIsBusy)
        {
            return;
        }

        let item = {
            freeformText: this.freeformText
        } as GroceryItem;

        this.networkIsBusy = true;
        this._groceryService.addGroceryItem(this.listId, item).subscribe(newItem => {
            this.onItemAdded.emit(newItem);
            this.resetForm();
        }, error => { 
            console.error(error);
        }, () => {
            this.networkIsBusy = false;
        });
    }

    private resetForm() {
        this.freeformText = '';
    }
}