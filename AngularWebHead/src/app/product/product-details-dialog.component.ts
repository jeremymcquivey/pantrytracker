import { Output, EventEmitter, Component, Input, ViewChild } from "@angular/core";
import { Product, ProductVariety, ProductCode } from "../model/pantryline";
import { ProductService } from "../core/product.service";
import { _MatChipListMixinBase, MatTableDataSource } from "@angular/material";
import { AddProductCodeComponent } from "./add-product-code.component";

@Component({
    selector: 'product-details-dialog',
    templateUrl: 'product-details-dialog.component.html',
    styleUrls: ['../io/uploadfile-dialog.component.css']
})
export class ProductDetailsDialogComponent {
    private _product: Product = new Product();
    
    networkIsBusy: boolean = false;
    blankVariety: ProductVariety = new ProductVariety();
    blankCode: ProductCode = new ProductCode();
    varietyDataSource = new MatTableDataSource();
    codeDataSource = new MatTableDataSource();
    varietyColumns = ["name"];
    codeColumns = ["code","variety","size", "brand"];

    @ViewChild("addProductCodeDialog")
    private AddProductCodeDialog: AddProductCodeComponent;

    @Input() 
    set product(value: Product) {
        this._product = value || new Product();
        this.GetDetails(value.id);
        this.resetBlankVariety();
        this.resetBlankCode();
    }
    get product(): Product {
        return this._product;
    }

    @Output() onImageSelected: EventEmitter<any> = new EventEmitter<any>();
    @Output() onTextSelected: EventEmitter<any> = new EventEmitter<any>();

    public isVisible: boolean = false;

    constructor(private _projectService: ProductService) {}

    public addVariety() {
        if(this.networkIsBusy)
        {
            return;
        }

        if(this.blankVariety.description.length > 0)
        {
            this.networkIsBusy = true;
            this._projectService.addVariety(this.blankVariety).subscribe(variety => {
                this._product.varieties.push(variety);
                this.varietyDataSource.data = this._product.varieties;
                this.resetBlankVariety();
                this.networkIsBusy = false;
            }, error => { console.error(error); this.networkIsBusy = false; });
        }
    }

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

    private cancelDialog() {
        this.isVisible = false;
    }

    private GetDetails(productId: number) {
        this.varietyDataSource.data = [];
        this.codeDataSource.data = [];

        this.networkIsBusy = true;
        this._projectService.getProduct(productId).subscribe(product => {
            this._product = product;
            this.varietyDataSource.data = product.varieties;
            this.codeDataSource.data = product.codes;
            this.networkIsBusy = false;
        }, error => { console.error(error); this.networkIsBusy = false; });
    }

    private codeAdded() {
        this.GetDetails(this._product.id);
        this.resetBlankCode();
    }

    private resetBlankVariety() {
        this.blankVariety = new ProductVariety();
        this.blankVariety.productId = this._product.id;
    }

    private resetBlankCode() {
        this.blankCode = new ProductCode();
        this.blankCode.product = this._product;
        this.blankCode.productId = this._product.id;
    }
}