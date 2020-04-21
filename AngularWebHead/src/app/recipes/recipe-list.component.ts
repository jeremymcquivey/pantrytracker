import { Component, OnInit } from "@angular/core";
import { ProductService } from "../core/product.service";
import { Utils } from "../core/utils";
import { Project } from "../model/project";
import { Recipe } from "../model/recipe";
import { MatTableDataSource } from "@angular/material/table";

@Component({
  selector: "app-projects",
  templateUrl: "./recipe-list.component.html",
  styleUrls: ['./recipe-list.component.css']
})
export class RecipeListComponent implements OnInit {
  displayedColumns = ['name'];
  error: string;
  dataSource = new MatTableDataSource<Recipe>();
  projects: Project[];
  recipes: Recipe[];

  constructor(private _projectService: ProductService) {}

  ngOnInit() {
    this._projectService.getRecipes().subscribe(recipes => {
      this.recipes = recipes;
      this.dataSource.data = recipes;
    }, error => Utils.formatError(error));
  }
}