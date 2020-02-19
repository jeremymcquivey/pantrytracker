import { Component, OnInit, EventEmitter, Output, Input, ViewChild } from "@angular/core";
import { PantryService } from "../core/pantry.service";
import { PantryLine, Product, ProductVariety } from "../model/pantryline";
import { ProductService } from "../core/product.service";
import { ProductSearchComponent } from "../product/product-search.component";


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

    isVisible: boolean = false;
    isBusy: boolean = false;

    private Line: PantryLine;
    private Product: Product;
    private Varieties: ProductVariety[] = [];

    @ViewChild("productSearchDialog")
    public ProductSearch: ProductSearchComponent; 

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

        this.isBusy = true;
        this.Product = new Product();
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
        this.ProductSearch.SearchText = this.ProductSearchText;
        this.ProductSearch.productSearch();
    }

    updateProduct(product: Product) {
        this.Line.productId = product.id;
        this.Product = product;
        this.productName = product.name;
    }

    dismissDialog(): void {
        this.isVisible = false;
    }
}