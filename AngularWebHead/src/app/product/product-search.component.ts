import { Component, OnInit, Input, Output, EventEmitter, ViewChild } from "@angular/core";
import { Product } from "../model/pantryline";
import { ProductService } from "../core/product.service";
import { MatTableDataSource, MatInput } from "@angular/material";
import { AddProductDialogComponent } from "./add-product-dialog.component";

@Component({
selector: 'product-search',
    templateUrl: 'product-search.component.html'
})
export class ProductSearchComponent implements OnInit {
    private dataSource = new MatTableDataSource();
    private hasSearched = false;
    private visibleColumns = ['name'];

    @ViewChild("AddProductDialog")
    private AddProductDialog: AddProductDialogComponent;

    @Output() onSelected: EventEmitter<Product> = new EventEmitter();
    @Output() onSearched: EventEmitter<number> = new EventEmitter();

    @Input()
    public SearchText: string;

    @Input()
    get HasResults(): boolean {
        return this.dataSource.data.length > 0;
    }

    constructor(private _productService: ProductService) {
    }

    ngOnInit(): void { }

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