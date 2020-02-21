import { Component, OnInit, EventEmitter, Output, Input, ViewChild } from "@angular/core";
import { PantryService } from "../core/pantry.service";
import { PantryLine, Product, ProductVariety, ProductCode } from "../model/pantryline";
import { ProductService } from "../core/product.service";
import { ProductSearchDialogComponent } from "../product/product-search-dialog.component";
import { AddProductCodeComponent } from "../product/add-product-code.component";

@Component({
selector: 'inventory-transaction',
    templateUrl: 'inventory-transaction.component.html',
    styleUrls: ['../io/uploadfile-dialog.component.css']
})
export class InventoryTransactionComponent implements OnInit {
    productName: string;
    public selectedVariety: number;
    preassignedVariety: string;
    ProductSearchText: string;

    warnings: string[] = [];
    isVisible: boolean = false;
    isBusy: boolean = false;

    private Line: PantryLine;
    private Product: Product;
    private Varieties: ProductVariety[] = [];

    @ViewChild("productSearchDialog")
    public ProductSearchDialog: ProductSearchDialogComponent; 

    @ViewChild("addProductCodeDialog")
    public AddProductCodeDialog: AddProductCodeComponent;

    @Output() onUpdate: EventEmitter<PantryLine> = new EventEmitter();

    constructor(private _pantryService: PantryService, 
                private _productService: ProductService) {
        this.Line = new PantryLine();
    }

    ngOnInit(): void { }

    updateInventory(): PantryLine {
        if(!this.Line.productId || !this.Line.size || !this.Line.quantity || !this.Line.unit)
        {
            alert("missing required information");
            return;
        }

        this._pantryService.updateInventory(this.Line).subscribe(line => {
            if(line) {
                this.onUpdate.emit(line);
                this.dismissDialog();
            }
        });
    }

    lookupUPC(): void {
        if(this.Line.code.length == 0)
        {
            return;
        }

        this.warnings = [];
        this.isBusy = true;
        this.Varieties = [];

        this._productService.lookupCode(this.Line.code).subscribe(productCode => {
            if(productCode) {
                this.Line.size = productCode.size;
                this.Line.unit = productCode.unit;
                this.Line.varietyId = productCode.varietyId;
                this.Line.productId = productCode.productId;

                if(productCode.productId) {
                    this.getProductById(productCode.productId);
                }
            }
            else{
                alert("Code not found.");
            }

            this.isBusy = false;
        }, error => {
            if(!this.Line.productId) {
                this.warnings.push("Barcode not registered.");
                this.warnings.push("Please select a product then rescan the barcode.");
                return;
            }

            this.AddProductCodeDialog.Code = new ProductCode();
            this.AddProductCodeDialog.Code.code = this.Line.code;
            this.AddProductCodeDialog.Code.product = this.Product;
            this.AddProductCodeDialog.Code.productId = this.Line.productId;

            console.log(this.AddProductCodeDialog.Code);

            this.AddProductCodeDialog.isVisible = true;
            this.isBusy = false;
        });
    }

    getProductById(productId: number): void {
        this.isBusy = true;
        this._productService.getProduct(productId).subscribe(product => {
            if(product)
            {
                this.Product = product;
                this.Varieties = product.varieties;
                this.productName = this.Product.name;

                var varieties = this.Product.varieties.filter(p => p.id === this.Line.varietyId);
                if(varieties.length == 1)
                {
                    this.selectedVariety = varieties[0].id;
                    this.preassignedVariety = varieties[0].description;
                    this.Line.variety = varieties[0];
                }
            }

            this.isBusy = false;
        });
    }

    searchProduct(): void {
        this.ProductSearchDialog.isVisible = true;
        this.ProductSearchDialog.SearchText = this.ProductSearchText;
        this.ProductSearchDialog.productSearch();
    }

    updateProduct(product: Product) {
        this.Varieties = [];
        this.Line.productId = product.id;
        this.Product = product;
        this.productName = product.name;

        if(product.varieties.length > 0)
        {
            this.Varieties = product.varieties;
        }
    }

    newCodeSaved(code: ProductCode) {
        console.log(code);

        this.Line.code = code.code;
        this.Line.product = code.product;
        this.Line.productId = code.productId;
        this.Line.size = code.size;
        this.Line.unit = code.unit;
        this.Line.variety = code.variety;
        this.Line.varietyId = code.varietyId;
    }

    dismissDialog(): void {
        this.isVisible = false;
    }
}