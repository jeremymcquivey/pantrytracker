import { Output, EventEmitter, Component, Input, ViewChild } from "@angular/core";
import { ProductCode, Product, ProductVariety } from "../model/pantryline";
import { ProductService } from "../core/product.service";

@Component({
    selector: 'add-product-code',
    templateUrl: 'add-product-code.component.html',
    styleUrls: ['../io/uploadfile-dialog.component.css']
})
export class AddProductCodeComponent {
    private networkIsBusy: boolean = false;
    public isVisible: boolean = false;

    @Input() 
    public Code: ProductCode = new ProductCode();

    @Output() onSaved: EventEmitter<any> = new EventEmitter<any>();

    constructor(private _productService: ProductService) {}

    public saveProductCode() {
        if(this.networkIsBusy)
        {
            return;
        }

        if(this.Code.code.length >= 12 && this.Code.code.length <= 13)
        {
            this.networkIsBusy = true;
            this._productService.addProductCode(this.Code).subscribe(code => {
                this.networkIsBusy = false;
                this.onSaved.emit(code);
                this.cancelDialog();
            }, error => { console.error(error); this.networkIsBusy = false; });
        }
    }

    private cancelDialog() {
        this.isVisible = false;
    }
}