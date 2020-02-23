import { Component, Output, EventEmitter } from "@angular/core";
import { _MatChipListMixinBase } from "@angular/material";
import { ProductVariety } from "../model/pantryline";
import { ProductService } from "../core/product.service";

@Component({
    selector: 'add-variety',
    templateUrl: 'add-variety.component.html'
})
export class AddVarietyComponent {
    private blankVariety: ProductVariety = new ProductVariety();
    private networkIsBusy: boolean = false;
    private productId: number = 0;

    @Output() onVarietyAdded: EventEmitter<ProductVariety> = new EventEmitter<ProductVariety>();

    constructor(private _productService: ProductService) { }

    public resetVariety(productId: number) {
        this.productId = productId;
        this.resetBlankVariety();
    }

    public addVariety() {
        if(this.networkIsBusy)
        {
            return;
        }

        if(this.blankVariety.description.length > 0)
        {
            this.networkIsBusy = true;
            this._productService.addVariety(this.blankVariety).subscribe(variety => {
                this.onVarietyAdded.emit(variety);
                this.resetBlankVariety();
                this.networkIsBusy = false;
            }, error => { console.error(error); this.networkIsBusy = false; });
        }
    }

    private resetBlankVariety() {
        this.blankVariety = new ProductVariety();
        this.blankVariety.productId = this.productId;
    }
}