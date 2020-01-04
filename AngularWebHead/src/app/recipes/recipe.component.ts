import { OnInit, Component, EventEmitter } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { ProjectService } from "../core/project.service";
import { Recipe } from "../model/recipe";
import { Ingredient } from "../model/ingredient";
import { MatTableDataSource, MatDialog } from "@angular/material";
import { Direction } from "../model/direction";
import { FileUploader } from "ng2-file-upload";

@Component({
    selector: 'app-recipe',
    templateUrl: 'recipe.component.html'
  })
  export class RecipeComponent implements OnInit {
    public uploader:FileUploader = new FileUploader(null);
    public isUpdate:boolean;
    public showFileDialog:boolean = false;
    public showRawText:boolean = true;
    public rawText: String = "";

    recipe: Recipe = {} as Recipe;
    ingredients: Ingredient[];
    directions: Direction[];
    
    displayedColumns = ['quantity', 'unit', 'name', 'actions'];
    directionColumns = ['index', 'description', 'actions'];
    dataSource = new MatTableDataSource();
    directionDataSource = new MatTableDataSource();
    error: string;

    constructor(
      private _route: ActivatedRoute,
      private _projectService: ProjectService,
      private _router: Router,
      public dialog: MatDialog
    ) {}

    ngOnInit(): void {
      let recipeId = this._route.snapshot.params.recipeId;
      this.isUpdate = recipeId && recipeId != "0";

      if(this.isUpdate) {
        this._projectService.getRecipe(recipeId).subscribe(recipe => {
          this.initWithRecipe(recipe);
        });
      }
      else {
        this.ingredients = [];
        this.directions = [];
        this.dataSource.data = this.ingredients;
        this.directionDataSource.data = this.directions;
      }
    }

    private openDialog() {
      this.showFileDialog = true;
    }

    private closeDialog() {
      this.showFileDialog = false;
    }

    public initWithRecipe(recipe: Recipe) {
      this.recipe = recipe;
      this.ingredients = recipe.ingredients;
      this.directions = recipe.directions;
      this.reorderList();
      this.reorderDirections();

      this.dataSource.data = this.ingredients;
      this.directionDataSource.data = this.directions;
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

    public addDirection() {
      this.directions.push({
        index: this.directions.length + 1,
      } as Direction);
      
      this.directionDataSource.data = this.directions;
    }

    public deleteDirection(direction: Direction) {
      var index = this.directions.indexOf(direction);
      this.directions.splice(index, 1);

      this.reorderDirections();
      this.directionDataSource.data = this.directions;
    }

    public decreaseDirectionPriority(direction) {
      var index = this.directions.indexOf(direction);
      var temp = this.directions[index + 1];
      this.directions[index + 1] = direction;
      this.directions[index] = temp;

      this.reorderDirections();
      this.directionDataSource.data = this.directions;
    }

    public increaseDirectionPriority(direction) {
      var index = this.directions.indexOf(direction);
      var temp = this.directions[index - 1];
      this.directions[index - 1] = direction;
      this.directions[index] = temp;

      this.reorderDirections();
      this.directionDataSource.data = this.directions;
    }

    public saveRecipe() {
      this.recipe.ingredients = this.ingredients;
      this.recipe.directions = this.directions;

      this._projectService.saveRecipe(this.recipe).subscribe(recipe => {
        this._router.navigate(['recipe/' + recipe.id]);
      });
    }

    private reorderList()
    {
      for(let i = 1; i <= this.ingredients.length; i++)
      {
        this.ingredients[i-1].index = i;
      }
    }

    private reorderDirections()
    {
      for(let i = 1; i <= this.directions.length; i++)
      {
        this.directions[i-1].index = i;
      }
    }

    public submitFile() {
      this._projectService.submitRawText(this.rawText).subscribe(recipe => {
        this.recipe = recipe;
        recipe.id = ({} as Recipe).id;
        this.ingredients = recipe.ingredients;
        this.directions = recipe.directions;
        this.reorderList();
        this.reorderDirections();

        this.dataSource.data = this.ingredients;
        this.directionDataSource.data = this.directions;
      });
    }

    public processImage(img: string) {
      var component = this;
      this._projectService.submitImage(img.replace(/^data:image\/(png|jpg);base64,/, "")).toPromise()
        .then(function(data) {
          const recipe = data as Recipe;
          recipe.id = ({} as Recipe).id;
          component.initWithRecipe(recipe);
          component.rawText = recipe.rawText;
        })
    }
  }