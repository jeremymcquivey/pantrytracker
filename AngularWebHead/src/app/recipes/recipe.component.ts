import { OnInit, Component, ViewChild } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { Recipe } from "../model/recipe";
import { Ingredient } from "../model/ingredient";
import { MatTableDataSource } from "@angular/material/table";
import { MatDialog } from "@angular/material/dialog";
import { Direction } from "../model/direction";
import { FileUploader } from "ng2-file-upload";
import { TextEditorDialogComponent } from "../io/texteditor-dialog.component";
import { UploadFileDialogComponent } from "../io/uploadfile-dialog.component";
import { AuthService } from "../core/auth.service";
import { RecipeService } from "../core/recipe.service";

@Component({
    selector: 'app-recipe',
    templateUrl: 'recipe.component.html',
    styleUrls: ['../controls/fancy-form.component.css',
                './recipe.component.css']
  })
  export class RecipeComponent implements OnInit {
    @ViewChild("editorDialog")
    public previewEditor: TextEditorDialogComponent;

    @ViewChild("uploadFileDialog")
    public uploadDialog: UploadFileDialogComponent;

    public uploader:FileUploader = new FileUploader(null);
    public isUpdate:boolean;
    public hasSubmitted: boolean = false;
    public rawText: string = "";
    public isBusy: boolean = false;

    public recipe: Recipe = { title: '' } as Recipe;
    public ingredients: Ingredient[];
    public directions: Direction[];
    
    directionColumns = ['description'];
    condensedColumns = ['name'];

    dataSource = new MatTableDataSource();
    directionDataSource = new MatTableDataSource();
    error: string = '';
    recipeId: string = ({} as Recipe).id;

    constructor(
      private _route: ActivatedRoute,
      private _recipeService: RecipeService,
      private _router: Router,
      public _authService: AuthService,
      public dialog: MatDialog
    ) {}

    ngOnInit(): void {
      let recipeId = this._route.snapshot.params.recipeId;
      this.isUpdate = recipeId && recipeId != "0";

      if(this.isUpdate) {
        this._recipeService.getRecipe(recipeId).subscribe(recipe => {
          this.initWithRecipe(recipe);
        });
      }
      else { 
        this.ingredients = [];
        this.directions = []
        this.dataSource.data = this.ingredients;
        this.directionDataSource.data = this.directions;
      }
    }

    public showFileUploader() {
      this.uploadDialog.isVisible = true;
    }

    public closeFileUploader() {
      this.uploadDialog.isVisible = false;
    }

    public showTextPreview() {
      if(!this.rawText || this.rawText.trim().length == 0) {
        this.rawText = this.recipe.title + '\nIngredients\n\nDirections\n';
      }

      this.previewEditor.isVisible = true;
      this.previewEditor.rawText = this.rawText;
    }

    public closeTextPreview() {
      this.previewEditor.isVisible = false;
    }

    public initWithRecipe(recipe: Recipe) {
      this.recipeId = recipe.id;
      this.recipe = recipe;

      this.rawText = recipe.rawText;
      this.ingredients = recipe.ingredients;
      this.directions = recipe.directions;

      this.dataSource.data = this.ingredients;
      this.directionDataSource.data = this.directions;
    }

    public saveRecipe() {
      this.isBusy = true;
      this.recipe.ingredients = this.ingredients;
      this.recipe.directions = this.directions;

      this.ingredients.forEach((ing, i) => {
        ing.recipeId = this.recipe.id;
        ing.index = i;
      });

      this.recipe.directions.forEach((dir, i) => {
        dir.recipeId = this.recipe.id;
        dir.index = i;
      });

      this._recipeService.saveRecipe(this.recipe).subscribe(recipe => {
        this._router.navigate(['recipe/' + recipe.id]);
      }).add(() => {
        this.isBusy = false;
      });
    }

    public submitFile(updatedText: string) {
      this.hasSubmitted = true;
      this.rawText = updatedText;
      this._recipeService.submitRawText(this.rawText).subscribe(recipe => {
        this.recipe = recipe;
        recipe.id = this.recipeId;

        this.ingredients = recipe.ingredients;
        this.dataSource.data = this.ingredients;

        this.directions = recipe.directions;
        this.directionDataSource.data = this.directions;
      });
    }

    public processImage(img: string) {
      this.hasSubmitted = true;
      var component = this;
      this._recipeService.submitImage(img.replace(/^data:image\/(png|jpg);base64,/, "")).toPromise()
        .then(function(data) {
          const recipe = data as Recipe;
          recipe.id = ({} as Recipe).id;
          component.initWithRecipe(recipe);
          component.rawText = recipe.rawText;
        })
    }

    public isPremium() {
      return this._authService.authContext && this._authService.authContext.userIsPremium();
    }
  }