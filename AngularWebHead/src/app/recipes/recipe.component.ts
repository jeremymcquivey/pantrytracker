import { OnInit, Component, ViewChild } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { ProjectService } from "../core/project.service";
import { Recipe } from "../model/recipe";
import { Ingredient } from "../model/ingredient";
import { MatTableDataSource, MatDialog } from "@angular/material";
import { Direction } from "../model/direction";
import { FileUploader } from "ng2-file-upload";
import { TextEditorDialogComponent } from "../io/texteditor-dialog.component";
import { UploadFileDialogComponent } from "../io/uploadfile-dialog.component";

@Component({
    selector: 'app-recipe',
    templateUrl: 'recipe.component.html'
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

    public recipe: Recipe = {} as Recipe;
    public ingredients: Ingredient[];
    public directions: Direction[];
    
    directionColumns = ['description'];
    condensedColumns = ['name'];

    dataSource = new MatTableDataSource();
    directionDataSource = new MatTableDataSource();
    error: string;
    recipeId: string = ({} as Recipe).id;

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

    private showFileUploader() {
      this.uploadDialog.isVisible = true;
    }

    private closeFileUploader() {
      this.uploadDialog.isVisible = false;
    }

    private showTextPreview() {
      if(!this.rawText || this.rawText.trim().length == 0) {
        this.rawText = this.recipe.title;
      }

      this.previewEditor.isVisible = true;
      this.previewEditor.rawText = this.rawText;
    }

    private closeTextPreview() {
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
      this.recipe.ingredients = this.ingredients;
      this.recipe.directions = this.directions;

      this._projectService.saveRecipe(this.recipe).subscribe(recipe => {
        this._router.navigate(['recipe/' + recipe.id]);
      });
    }

    public submitFile(updatedText: string) {
      this.hasSubmitted = true;
      this.rawText = updatedText;
      this._projectService.submitRawText(this.rawText).subscribe(recipe => {
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
      this._projectService.submitImage(img.replace(/^data:image\/(png|jpg);base64,/, "")).toPromise()
        .then(function(data) {
          const recipe = data as Recipe;
          recipe.id = ({} as Recipe).id;
          component.initWithRecipe(recipe);
          component.rawText = recipe.rawText;
        })
    }
  }