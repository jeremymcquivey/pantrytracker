import { OnInit, Component } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { ProjectService } from "../core/project.service";
import { Recipe } from "../model/recipe";
import { Ingredient } from "../model/ingredient";
import { MatTableDataSource } from "@angular/material";

@Component({
    selector: 'app-recipe',
    templateUrl: 'recipe.component.html',
    styleUrls: ['recipe.component.scss']
  })
  export class RecipeComponent implements OnInit {
    recipe: Recipe = {} as Recipe;
    ingredients: Ingredient[];
    
    displayedColumns = ['quantity', 'unit', 'name', 'actions'];
    dataSource = new MatTableDataSource();
    error: string;

    constructor(
      private _route: ActivatedRoute,
      private _projectService: ProjectService
    ) {}

    ngOnInit(): void {
      var recipeId = this._route.snapshot.params.recipeId;

      if(recipeId) {
        this._projectService.getRecipe(recipeId).subscribe(recipe => {
          this.recipe = recipe;
          this.ingredients = recipe.ingredients;
          this.reorderList();
  
          this.dataSource.data = this.ingredients;
        });
      }
      else {
        this.recipe.ingredients = [];
        this.dataSource.data = this.ingredients;
      }
    }

    public addIngredient() {
      this.ingredients.push({
        index: this.ingredients.length + 1,
      } as Ingredient);
      
      this.dataSource.data = this.ingredients;
    }

    public deleteIngredient(ingredient: Ingredient) {
      var index = this.ingredients.indexOf(ingredient);
      this.ingredients.splice(index, 1);

      this.reorderList();
      this.dataSource.data = this.ingredients;
    }

    public decreasePriority(ingredient) {
      var index = this.ingredients.indexOf(ingredient);
      var temp = this.ingredients[index + 1];
      this.ingredients[index + 1] = ingredient;
      this.ingredients[index] = temp;

      this.reorderList();
      this.dataSource.data = this.ingredients;
    }

    public increasePriority(ingredient) {
      var index = this.ingredients.indexOf(ingredient);
      var temp = this.ingredients[index - 1];
      this.ingredients[index - 1] = ingredient;
      this.ingredients[index] = temp;

      this.reorderList();
      this.dataSource.data = this.ingredients;
    }

    public saveRecipe() {
      this.recipe.ingredients = this.ingredients;

      this._projectService.saveRecipe(this.recipe).subscribe(recipe => {
        this.ngOnInit();
      });
    }

    private reorderList()
    {
      for(let i = 1; i <= this.ingredients.length; i++)
      {
        this.ingredients[i-1].index = i;
      }
    }
  }