import { Component, OnInit, Output, EventEmitter, Input } from "@angular/core";
import { Utils } from "../core/utils";
import { Project } from "../model/project";
import { Recipe } from "../model/recipe";
import { MatTableDataSource } from "@angular/material/table";
import { RecipeService } from "../core/recipe.service";
import { Router } from '@angular/router';

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

  @Input() addButtonVisible: boolean = true;

  @Output() onRecipeSelected: EventEmitter<Recipe> = new EventEmitter();

  constructor(private _recipeService: RecipeService, private _router: Router) {}

  ngOnInit() {
    this._recipeService.getRecipes().subscribe(recipes => {
      this.recipes = recipes;
      this.dataSource.data = recipes;
    }, error => Utils.formatError(error));
  }

  selectRecipe(recipe: Recipe) {
    this.onRecipeSelected.emit(recipe);

    if(this.onRecipeSelected.observers.length == 0) {
      this._router.navigate([`/recipe/${recipe.id}`]);
    }
  }
}