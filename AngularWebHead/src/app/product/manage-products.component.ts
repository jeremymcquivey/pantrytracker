import { Component, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource } from '@angular/material';
import { Product } from '../model/pantryline';
import { ProductDetailsDialogComponent } from './product-details-dialog.component';
import { ProductSearchComponent } from './product-search.component';

@Component({
  selector: 'app-manage-projects',
  templateUrl: 'manage-products.component.html',
  styleUrls: ['manage-products.component.scss']
})
export class ManageProductsComponent implements OnInit {
  private searchResults: number = -1;

  @ViewChild("ProductDetailsDialog")
  ProductDetails: ProductDetailsDialogComponent;

  @ViewChild("ProductSearch")
  ProductSearch: ProductSearchComponent;

  error: string;
  dataSource = new MatTableDataSource();
  productMap: Map<string, Product[]> = new Map<string, Product[]>();

  ngOnInit() {
  }

  productClick(selectedProduct: Product) {
    this.ProductDetails.product = selectedProduct;
    this.ProductDetails.isVisible = true;
  }

  canAdd() {
    return this.ProductSearch.SearchText &&
           this.searchResults == 0;
  }

  searchReturned(resultCount: number) {
    this.searchResults = resultCount;
  }

  addProduct() {
    
  }
}
