import { Component, OnInit, Input, Output, EventEmitter, ViewChild } from "@angular/core";
import { Product } from "../model/pantryline";
import { ProductSearchComponent } from "./product-search.component";

@Component({
selector: 'product-search-dialog',
    templateUrl: 'product-search-dialog.component.html',
    styleUrls: ['product-search-dialog.component.css']
})
export class ProductSearchDialogComponent implements OnInit {
    public isVisible: boolean = false;

    @ViewChild("productSearch")
    public ProductSearch: ProductSearchComponent;

    @Output() onSelected: EventEmitter<Product> = new EventEmitter();

    @Input()
    set SearchText(value: string) {
        this.ProductSearch.SearchText = value;
    }
    get SearchText(): string {
        return this.ProductSearch.SearchText;
    }

    constructor() {
    }

    ngOnInit(): void { }

    selectProduct(product: Product) {
        this.onSelected.emit(product);
        this.dismissDialog();
    }

    productSearch(): void {
        this.ProductSearch.productSearch();
    }

    dismissDialog(): void {
        this.isVisible = false;
    }
}