import { Component, OnInit, Input, Output, EventEmitter } from "@angular/core";
import { Product } from "../model/pantryline";
import { ProductService } from "../core/product.service";
import { MatTableDataSource } from "@angular/material";

@Component({
selector: 'product-search',
    templateUrl: 'product-search.component.html',
    styleUrls: ['product-search.component.css']
})
export class ProductSearchComponent implements OnInit {
    private isVisible: boolean = false;

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
        this.dismissDialog();
    }

    productSearch(): void {
        this.dataSource.data = [];
        this.isVisible = true;

        if(this.SearchText) {
            this._productService.getProducts(this.SearchText).subscribe(product => {
                this.dataSource.data = product;
            });
        }
    }

    dismissDialog(): void {
        this.isVisible = false;
    }
}