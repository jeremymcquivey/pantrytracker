import { Component, OnInit, ViewChildren, ViewChild } from '@angular/core';
import {
  MatTableDataSource,
  MatDialog
} from '@angular/material';
import { ProjectService } from '../core/project.service';
import { Project } from '../model/project';
import { Product } from '../model/pantryline';
import { ProductDetailsDialogComponent } from './product-details-dialog.component';

@Component({
  selector: 'app-manage-projects',
  templateUrl: 'manage-products.component.html',
  styleUrls: ['manage-products.component.scss']
})
export class ManageProductsComponent implements OnInit {
  @ViewChild("ProductDetailsDialog")
  ProductDetails: ProductDetailsDialogComponent;

  selectedLetter = 'C';
  displayedColumns = ['name'];
  error: string;
  dataSource = new MatTableDataSource();
  productMap: Map<string, Product[]> = new Map<string, Product[]>();

  constructor(
    private _projectService: ProjectService,
    public dialog: MatDialog
  ) { }

  ngOnInit() {
    this._projectService.getProducts(this.selectedLetter).subscribe(productGroup => {
      this.productMap.set(this.selectedLetter, productGroup);
      this.dataSource.data = this.productMap.get(this.selectedLetter);
    }, error => { console.error(error); });
  }

  productClick(selectedProduct: Product) { //}: Product {
    this.ProductDetails.product = selectedProduct;
    this.ProductDetails.isVisible = true;
  }

  addProject() {
    /*const dialogRef = this.dialog.open(AddProjectDialogComponent, {
      width: '348px',
      data: { name: '' }
    });
    dialogRef.afterClosed().subscribe(result => {
      if (result !== undefined) {
        const newProj = new Project();
        newProj.name = result;
        this._projectService.addProject(newProj).subscribe(project => {
          this.projects.push(project);
          this.dataSource.data = this.projects;
        }, error => this.error = Utils.formatError(error));
      }
    });*/
  }

  deleteProject(project: Project) {
    /*const dialogRef = this.dialog.open(DeleteDialogComponent, {
      width: '348px',
      data: { entityName: 'Project', message: `Are you sure you want to delete project ${project.name}?` }
    });
    dialogRef.afterClosed().subscribe(result => {
      if (result !== undefined) {
        this._projectService.deleteProject(project).subscribe(() => {
          this.projects.splice(this.projects.indexOf(project), 1);
          this.dataSource.data = this.projects;
        }, error => this.error = Utils.formatError(error));
      }
    });*/
  }

}
