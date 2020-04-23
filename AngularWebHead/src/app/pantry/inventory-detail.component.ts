import { Component, OnInit, ViewChild } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { PantryService } from "../core/pantry.service";
import { MatTableDataSource } from "@angular/material/table";
import { PantryLineGrouping, Product, PantryLine } from "../model/pantryline";
import { ProductService } from "../core/product.service";
import { InventoryCorrectionComponent } from "./inventory-correction.component";

@Component({
selector: 'inventory-detail',
    templateUrl: 'inventory-detail.component.html',
    styleUrls: ['../controls/fancy-form.component.css',
                './inventory-detail.component.css']
})
export class InventoryDetailComponent implements OnInit {
    private _product: Product;
    public DataSource = new MatTableDataSource<PantryLineGrouping>();

    @ViewChild("correctionDialog")
    public correctionDialog: InventoryCorrectionComponent;
    
    public get isProductLoaded(): boolean {
        return !!this._product;
    }   

    public get VisibleColumns(): string[] {
        return ['name'];
    }

    public get ProductName(): string {
        return `${this._product?.name}` ?? null;
    }

    constructor(private _pantryService: PantryService, 
                private _productService: ProductService,
                private _route: ActivatedRoute) {
    }

    ngOnInit(): void {
        const productId = this._route.snapshot.params.productId;

        this._productService.getProduct(productId).subscribe(product => {
            this._product = product;
            this.getSummary();
        }, error => {
            console.error(error);
        });
    }

    getSummary() {
        this._pantryService.getProductSummary(this._product.id).subscribe(summary => {
            this.DataSource.data = summary;
        }, error => {
            console.error(error);
        });
    }

    openDialog(line: PantryLine): void {
        this.correctionDialog.isVisible = true;
        this.correctionDialog.pantryLine = line;
    }
}