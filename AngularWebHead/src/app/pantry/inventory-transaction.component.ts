import { Component, OnInit, EventEmitter, Output, ViewChild, Input, ElementRef } from "@angular/core";
import { PantryService } from "../core/pantry.service";
import { PantryLine, Product, ProductVariety, ProductCode, TransactionType } from "../model/pantryline";
import { ProductService } from "../core/product.service";
import { ProductSearchDialogComponent } from "../product/product-search-dialog.component";
import { AddProductCodeComponent } from "../product/add-product-code.component";
import { AddVarietyComponent } from "../product/add-variety.component";
import { BarcodeReaderComponent } from '../io/barcodereader.component';

@Component({
selector: 'inventory-transaction',
    templateUrl: 'inventory-transaction.component.html',
    styleUrls: ['../io/uploadfile-dialog.component.css']
})
export class InventoryTransactionComponent implements OnInit {
    private _lastTransactionType: number = TransactionType.Addition;
    productName: string;
    loadBarcode: boolean = false;
    preassignedVariety: string;
    ProductSearchText: string;

    warnings: string[] = [];
    public isVisible: boolean = false;
    public isBusy: boolean = false;

    public Line: PantryLine;
    public Product: Product;
    public Varieties: ProductVariety[] = [];

    @Input() set transactionMode(value: number) {
        this._lastTransactionType = value;
        this.Line.transactionType = value;
    } get transactionMode(): number {
        return this.Line.transactionType;
    }

    @ViewChild("barcodeInput")
    private autoFocusedInput: ElementRef;

    @ViewChild("readBarcode") 
    private reader: BarcodeReaderComponent;

    @ViewChild("productSearchDialog")
    private ProductSearchDialog: ProductSearchDialogComponent; 

    @ViewChild("addVariety")
    private addVariety: AddVarietyComponent;

    @ViewChild("addProductCodeDialog")
    private AddProductCodeDialog: AddProductCodeComponent;

    @Output() onUpdate: EventEmitter<PantryLine> = new EventEmitter();

    constructor(private _pantryService: PantryService, 
                private _productService: ProductService) {
        this.Line = new PantryLine();
    }

    ngOnInit(): void { }

    LaunchDialog() {
        this.isVisible = true;
        this.Line = new PantryLine();
        this.Line.transactionType = this._lastTransactionType;

        this.Product = new Product();
        this.productName = "";
        this.Varieties = [];
        this.ProductSearchText = "";
        this.ProductSearchDialog.SearchText = "";
        this.warnings = [];

        setTimeout(() => { 
            this.autoFocusedInput.nativeElement.focus();
        }, 0);
    }

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
        this.loadBarcode = false;
        if(this.Line.code.length == 0)
        {
            return;
        }

        this.warnings = [];
        this.isBusy = true;

        this._productService.lookupCode(this.Line.code).subscribe(productCode => {
            if(productCode) {
                this.Line.size = productCode.size;
                this.Line.unit = productCode.unit;
                this.Line.varietyId = productCode.varietyId;
                this.Line.productId = productCode.productId;
                this.Line.varietyId = productCode.varietyId;

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
                this.loadBarcode = true;
                this.warnings.push("Barcode not registered.");
                this.warnings.push("Please select a product to register the new barcode.");
                return;
            }
            else this.registerNewProductCode();
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
                    this.Line.varietyId = varieties[0].id;
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
        this.Line.product = product;
        this.Line.productId = product.id;
        this.Line.unit = product.defaultUnit;
        this.Product = product;
        this.productName = product.name;

        if(product.varieties.length > 0)
        {
            this.Varieties = product.varieties;
        }

        if(this.loadBarcode) {
            this.registerNewProductCode();
        }
    }

    registerNewProductCode() {
        this.loadBarcode = false;

        let newCode = new ProductCode();
        newCode.code = this.Line.code;
        newCode.product = this.Product;
        newCode.productId = this.Line.productId;

        this.AddProductCodeDialog.Code = newCode;
        this.AddProductCodeDialog.isVisible = true;
        this.isBusy = false;
        this.warnings = [];
    }

    newCodeSaved(code: ProductCode) {
        this.Line.code = code.code;
        this.Line.product = code.product;
        this.Line.productId = code.productId;
        this.Line.size = code.size;
        this.Line.unit = code.unit;
        this.Line.variety = code.variety;
        this.Line.varietyId = code.varietyId;
        this.Line.varietyId = code.varietyId;
    }

    varietyAdded(variety: ProductVariety) {
        this.Varieties.push(variety);
        this.Line.varietyId = variety.id;
    }

    dismissDialog(): void {
        this.isVisible = false;
    }

    launchScanner() {
        this.reader.startScan();
    }

    barcodeReadHandler(text) {
        this.Line.code = text;
        this.lookupUPC();
    }
}