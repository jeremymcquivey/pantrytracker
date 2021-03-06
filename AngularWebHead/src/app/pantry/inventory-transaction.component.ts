import { Component, OnInit, EventEmitter, Output, ViewChild, Input, ElementRef } from "@angular/core";
import { PantryService } from "../core/pantry.service";
import { PantryLine, Product, ProductVariety, ProductCode, TransactionType } from "../model/pantryline";
import { ProductService } from "../core/product.service";
import { ProductSearchDialogComponent } from "../product/product-search-dialog.component";
import { AddProductCodeComponent } from "../product/add-product-code.component";
import { BarcodeReaderComponent } from '../io/barcodereader.component';

@Component({
selector: 'inventory-transaction',
    templateUrl: 'inventory-transaction.component.html',
    styleUrls: ['../controls/dialog.component.css',
                '../controls/fancy-form.component.css',
                './inventory-transaction.component.css']
})
export class InventoryTransactionComponent implements OnInit {
    private _lastTransactionType: number = TransactionType.Addition;

    upcDescription: string;
    productName: string;
    loadBarcode: boolean = false;
    ProductSearchText: string;

    warnings: string[] = [];
    public isVisible: boolean = false;
    public isBusy: boolean = false;

    public Line: PantryLine;
    public Product: Product;
    public Varieties: ProductVariety[] = [];
    public SelectedVariety: ProductVariety = null;

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

    @ViewChild("addProductCodeDialog")
    private AddProductCodeDialog: AddProductCodeComponent;

    @Output() onUpdate: EventEmitter<PantryLine> = new EventEmitter();

    @Output() onCancel: EventEmitter<any> = new EventEmitter();

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
        this.Varieties = [];
        this.SelectedVariety = null;
        this.productName = "";
        this.ProductSearchText = "";
        this.ProductSearchDialog.SearchText = "";
        this.warnings = [];
        this.upcDescription = null;

        setTimeout(() => { 
            this.autoFocusedInput.nativeElement.focus();
        }, 0);
    }

    LaunchWithLineItem(item: PantryLine) {
        this.isVisible = true;
        this.Line = item;
        this.Line.transactionType = TransactionType.Addition;
        this.productName = "";
        this.ProductSearchText = "";
        this.ProductSearchDialog.SearchText = "";
        this.warnings = [];
        this.upcDescription = null;

        this.getProductById(item.productId, (varieties) =>
        {
            const foundVariety = varieties.findIndex(v => v.id === item.varietyId);
            this.SelectedVariety = foundVariety > 0 ? varieties[foundVariety] : null;
        });
    }

    updateInventory(): PantryLine {
        if(!this.Line.productId || !this.Line.size || !this.Line.quantity || !this.Line.unit)
        {
            alert("missing required information");
            return;
        }

        this.Line.varietyId = this.SelectedVariety?.id;
        this._pantryService.updateInventory(this.Line).subscribe(line => {
            if(line) {
                this.onUpdate.emit(line);
                this.dismissDialog(false);
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
                this.upcDescription = productCode.description;

                if(productCode.productId) {
                    this.getProductById(productCode.productId, (varieties) => {
                        if(varieties.length == 1)
                        {
                            this.Line.varietyId = varieties[0].id;
                            this.SelectedVariety = varieties[0];
                            this.Line.variety = varieties[0];
                        }
                    });
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

    getProductById(productId: number, varietyCallback): void {
        this.isBusy = true;
        this._productService.getProduct(productId).subscribe(products => {
            if(products.length > 0 && !!products[0])
            {
                this.Product = products[0];
                this.Varieties = products[0].varieties;
                this.productName = this.Product.name;

                var varieties = this.Product.varieties.filter(p => p.id === this.Line.varietyId);
                varietyCallback(varieties);
            }

            this.isBusy = false;
        });
    }

    searchProduct(): void {
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

        this.ProductSearchText = '';
        this.ProductSearchDialog.SearchText = '';
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

    varietyAdded(addedVariety: ProductVariety) {
        this.Varieties.push(addedVariety);
        this.SelectedVariety = this.Varieties.find(variety => variety.id == addedVariety.id);
    }

    dismissDialog(isCancelled: boolean = false): void {
        if(isCancelled) {
            this.onCancel.emit();
        }
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