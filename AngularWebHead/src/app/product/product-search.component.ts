import { Component, OnInit, Input, Output, EventEmitter } from "@angular/core";
import { Product } from "../model/pantryline";
import { ProductService } from "../core/product.service";
import { MatTableDataSource } from "@angular/material";

@Component({
selector: 'product-search',
    templateUrl: 'product-search.component.html'
})
export class ProductSearchComponent implements OnInit {
    private dataSource = new MatTableDataSource();
    private visibleColumns = ['name'];

    @Output() onSelected: EventEmitter<Product> = new EventEmitter();

    @Input()
    public SearchText: string;

    constructor(private _productService: ProductService) {
    }

    ngOnInit(): void { }

    selectProduct(product: Product) {
        this.onSelected.emit(product);
    }

    productSearch(): void {
        this.dataSource.data = [];

        if(this.SearchText) {
            this._productService.getProducts(this.SearchText).subscribe(product => {
                this.dataSource.data = product;
            });
        }
    }
}