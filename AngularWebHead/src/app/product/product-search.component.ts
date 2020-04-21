import { Component, OnInit, Input, Output, EventEmitter, ViewChild, ElementRef } from "@angular/core";
import { Product } from "../model/pantryline";
import { ProductService } from "../core/product.service";
import { MatTableDataSource } from "@angular/material/table";
import { AddProductDialogComponent } from "./add-product-dialog.component";

@Component({
selector: 'product-search',
    templateUrl: 'product-search.component.html',
    styleUrls: ['../controls/fancy-form.component.css']
})
export class ProductSearchComponent implements OnInit {
    public dataSource = new MatTableDataSource();
    public hasSearched = false;
    public visibleColumns = ['name'];

    @ViewChild('searchTextBox')
    private _autoFocusedElement: ElementRef;

    @ViewChild("AddProductDialog")
    private AddProductDialog: AddProductDialogComponent;

    @Output() onSelected: EventEmitter<Product> = new EventEmitter();
    @Output() onSearched: EventEmitter<number> = new EventEmitter();

    @Input()
    public SearchText: string;

    constructor(private _productService: ProductService) { }

    ngOnInit() {
        setTimeout(() => {
            this._autoFocusedElement.nativeElement.focus();
        }, 0);
    }

    selectProduct(product: Product) {
        this.hasSearched = false;
        this.onSelected.emit(product);
    }

    addProduct() {
        this.AddProductDialog.Product = new Product();
        this.AddProductDialog.Product.name = this.SearchText;

        this.AddProductDialog.isVisible = true;
    }
  
    canAdd() {
      return this.hasSearched &&
             this.dataSource.data.length == 0;
    }

    productSearch(): void {
        this.dataSource.data = [];

        if(this.SearchText) {
            this._productService.getProducts(this.SearchText).subscribe(product => {
                this.hasSearched = true;
                this.onSearched.emit(product.length);
                this.dataSource.data = product;
            });
        }
    }
}