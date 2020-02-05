import { Output, EventEmitter, Component, Input } from "@angular/core";
import { Product } from "../model/pantryline";
import { ProjectService } from "../core/project.service";
import { _MatChipListMixinBase, MatTableDataSource } from "@angular/material";

@Component({
    selector: 'product-details-dialog',
    templateUrl: 'product-details-dialog.component.html',
    styleUrls: ['../io/uploadfile-dialog.component.css']
})
export class ProductDetailsDialogComponent {
    private _product: Product = new Product();
    varietyDataSource = new MatTableDataSource();
    codeDataSource = new MatTableDataSource();
    varietyColumns = ["name"];
    codeColumns = ["code","variety"];

    @Input() 
    set product(value: Product) {
        this._product = value || new Product();
        this.GetDetails(value.id);
    }
    get product(): Product {
        return this._product;
    }

    @Output() onImageSelected: EventEmitter<any> = new EventEmitter<any>();
    @Output() onTextSelected: EventEmitter<any> = new EventEmitter<any>();

    public isVisible: boolean = false;

    constructor(private _projectService: ProjectService) {}

    private cancelDialog() {
        this.isVisible = false;
    }

    private GetDetails(productId: number)
    {
        this._projectService.getProduct(productId).subscribe(product => {
            this._product = product;
            this.varietyDataSource.data = product.varieties;
            this.codeDataSource.data = product.codes;
        }, error => { console.error(error); });
    }
}