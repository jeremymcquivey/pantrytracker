import { Output, EventEmitter, Component, Input, ViewChild } from "@angular/core";
import { ProductCode, ProductVariety } from "../model/pantryline";
import { ProductService } from "../core/product.service";

@Component({
    selector: 'add-product-code',
    templateUrl: 'add-product-code.component.html',
    styleUrls: ['../controls/dialog.component.css',
                '../controls/fancy-form.component.css']
})
export class AddProductCodeComponent {
    private networkIsBusy: boolean = false;
    
    set selectedVariety(value: ProductVariety) {
        this.Code.variety = value;
        this.Code.varietyId = value?.id;
    } get selectedVariety(): ProductVariety {
        return this.Code?.variety;
    }

    @Input()
    public isVisible: boolean = false;

    @Input() Code: ProductCode;

    @Output() onSaved: EventEmitter<any> = new EventEmitter<any>();

    constructor(private _productService: ProductService) {}

    private saveProductCode() {
        if(this.networkIsBusy)
        {
            return;
        }

        if(this.Code.code.length >= 4 && this.Code.code.length <= 13)
        {
            this.networkIsBusy = true;
            this._productService.addProductCode(this.Code).subscribe(code => {
                this.networkIsBusy = false;
                this.onSaved.emit(code);
                this.cancelDialog();
            }, error => { console.error(error); this.networkIsBusy = false; });
        }
    }

    private varietyAdded(variety: ProductVariety) {
        this.Code.product.varieties.push(variety);
        this.selectedVariety = variety;
    }

    private cancelDialog() {
        this.isVisible = false;
    }
}