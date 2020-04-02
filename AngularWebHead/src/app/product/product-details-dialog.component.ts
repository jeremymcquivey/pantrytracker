import { Output, EventEmitter, Component, Input, ViewChild } from "@angular/core";
import { Product, ProductVariety, ProductCode } from "../model/pantryline";
import { ProductService } from "../core/product.service";
import { MatTableDataSource } from "@angular/material/table";
import { AddProductCodeComponent } from "./add-product-code.component";
import { AddVarietyComponent } from "./add-variety.component";

@Component({
    selector: 'product-details-dialog',
    templateUrl: 'product-details-dialog.component.html',
    styleUrls: ['../io/uploadfile-dialog.component.css']
})
export class ProductDetailsDialogComponent {
    private _product: Product = new Product();
    
    networkIsBusy: boolean = false;
    blankCode: ProductCode = new ProductCode();
    varietyDataSource = new MatTableDataSource();
    codeDataSource = new MatTableDataSource();
    varietyColumns = ["name"];
    codeColumns = ["code","variety","size", "brand"];

    @ViewChild("addProductCodeDialog")
    private AddProductCodeDialog: AddProductCodeComponent;

    @ViewChild("addVariety")
    private AddVariety: AddVarietyComponent;

    @Input() 
    set product(value: Product) {
        this._product = value || new Product();
        this.GetDetails(value.id);
        this.resetBlankCode();
    }
    get product(): Product {
        return this._product;
    }

    @Output() onVarietyAdded: EventEmitter<ProductVariety> = new EventEmitter<ProductVariety>();

    public isVisible: boolean = false;

    constructor(private _projectService: ProductService) {}

    public addCode() {
        if(this.networkIsBusy)
        {
            return;
        }

        if(this.blankCode.code.length > 0)
        {
            this.AddProductCodeDialog.Code = this.blankCode;
            this.AddProductCodeDialog.isVisible = true;
        }
    }

    private varietyAdded(variety: ProductVariety) {
        this._product.varieties.push(variety);
        this.varietyDataSource.data = this._product.varieties;
        this.onVarietyAdded.emit(variety);
    }

    private cancelDialog() {
        this.isVisible = false;
    }

    private GetDetails(productId: number) {
        this.varietyDataSource.data = [];
        this.codeDataSource.data = [];

        this.networkIsBusy = true;
        this._projectService.getProduct(productId).subscribe(product => {
            this._product = product;
            this.AddVariety.resetVariety(this._product.id);
            this.varietyDataSource.data = product.varieties;
            this.codeDataSource.data = product.codes;
            this.networkIsBusy = false;
        }, error => { console.error(error); this.networkIsBusy = false; });
    }

    public codeAdded() {
        this.GetDetails(this._product.id);
        this.resetBlankCode();
    }

    private resetBlankCode() {
        this.blankCode = new ProductCode();
        this.blankCode.product = this._product;
        this.blankCode.productId = this._product.id;
    }
}