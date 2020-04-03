import { Component, Output, EventEmitter, Input } from "@angular/core";
import { ProductVariety, Product } from "../model/pantryline";
import { ProductService } from "../core/product.service";

@Component({
    selector: 'add-variety',
    templateUrl: 'add-variety.component.html'
})
export class AddVarietyComponent {
    public blankVariety: ProductVariety = new ProductVariety();
    public networkIsBusy: boolean = false;
    
    @Input() product: Product;

    @Output() onVarietyAdded: EventEmitter<ProductVariety> = new EventEmitter<ProductVariety>();

    constructor(private _productService: ProductService) { }

    public addVariety() {
        if(this.networkIsBusy)
        {
            return;
        }

        if(this.blankVariety.description.length > 0)
        {
            this.networkIsBusy = true;
            this.blankVariety.productId = this.product.id;
            
            this._productService.addVariety(this.blankVariety).subscribe(variety => {
                this.onVarietyAdded.emit(variety);
                this.resetBlankVariety();
                this.networkIsBusy = false;
            }, error => { console.error(error); this.networkIsBusy = false; });
        }
    }

    private resetBlankVariety() {
        this.blankVariety = new ProductVariety();
        this.blankVariety.productId = this.product.id;
    }
}