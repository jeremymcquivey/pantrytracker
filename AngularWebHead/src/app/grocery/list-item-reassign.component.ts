import { OnInit, Component, Output, EventEmitter, Input } from '@angular/core';
import { Location } from '@angular/common';
import { Product, ProductVariety } from '../model/pantryline';
import { RecipeProduct } from '../model/recipe';

@Component({
    selector: 'reassign-products-dialog',
    templateUrl: './list-item-reassign.component.html',
    styleUrls: ['../controls/fancy-form.component.css',
                '../controls/dialog.component.css']
})
export class ListItemReassignComponent implements OnInit {
    private _isVisible = false;
    private _product: Product;
    private _lineItem: RecipeProduct;

    public SelectedVariety: ProductVariety;

    public get product(): Product {
        return this._product;
    }

    public get isVisible(): boolean {
        return this._isVisible;
    }

    public get Line(): RecipeProduct {
        return this._lineItem;
    }

    @Input()
    public allowIgnore: boolean = true;

    @Output()
    public onProductReassigned: EventEmitter<RecipeProduct> = new EventEmitter<RecipeProduct>();

    @Output()
    public onProductIgnored: EventEmitter<RecipeProduct> = new EventEmitter<RecipeProduct>();

    constructor(private location: Location) { }

    ngOnInit(): void { }

    clearProduct() {
        this._product = null;
        this.SelectedVariety = null;
    }

    launchDialog(value: RecipeProduct) {
        this._lineItem = value;
        this._isVisible = true;
        this._product = value?.product;
    }

    dismissDialog() {
        //TODO: Clear form
        this._isVisible = false;
    }

    selectProduct(product: Product) {
        this._product = product;
    }

    varietyAdded(addedVariety: ProductVariety) {
        this.product?.varieties.push(addedVariety);
        this.SelectedVariety = this.product?.varieties.find(variety => variety.id == addedVariety.id);
    }

    reassignProduct() {
        if(!this._product) {
            console.error('can\'t save without more info');
            return;
        }

        this._lineItem.product = this._product;
        this._lineItem.productId = this._product.id;
        this._lineItem.varietyId = this.SelectedVariety?.id;
        this._lineItem.variety = this.SelectedVariety;

        this.onProductReassigned.emit(this._lineItem);
        this.dismissDialog();
    }

    ignoreProduct() {
        this._lineItem.product = null;
        this._lineItem.productId = null;
        this._lineItem.variety = null;
        this._lineItem.varietyId = null;

        this.onProductIgnored.emit(this._lineItem);
        this.dismissDialog();
    }

    closeDialog() {
        this._isVisible = false;
    }
}