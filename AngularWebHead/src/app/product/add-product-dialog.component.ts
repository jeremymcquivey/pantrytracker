import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Product } from '../model/pantryline';
import { ProductService } from '../core/product.service';

@Component({
  selector: 'add-product-dialog',
  templateUrl: 'add-product-dialog.component.html',
  styleUrls: ['product-search-dialog.component.css']
})
export class AddProductDialogComponent implements OnInit {
    error: string;
    isVisible: boolean = false;
    isBusy: boolean = false;

    @Input()
    public Product: Product;

    @Output()
    public onAdded: EventEmitter<Product> = new EventEmitter<Product>();

    constructor(private _productService: ProductService) { }

    ngOnInit() {
        this.Product = new Product();
    }

    saveProduct() {
        if(this.isBusy) {
            return;
        }

        this.isBusy = true;
        this._productService.addProduct(this.Product).subscribe(product => {
            this.isBusy = false;
            this.onAdded.emit(product);
            this.dismissDialog();
        }, error => {
            this.isBusy = false;
        });
    }

    dismissDialog() {
        this.isVisible = false;
    }
}
