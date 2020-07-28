import { Component, ViewChild, AfterViewInit, Output, EventEmitter, Input } from "@angular/core";
import { RecipeListComponent } from "./recipe-list.component";
import { Recipe } from "../model/recipe";

@Component({
    selector: 'recipe-list-dialog',
    templateUrl: 'recipe-list-dialog.component.html',
    styleUrls: ['../controls/dialog.component.css',
                '../controls/fancy-form.component.css']
})
 export class RecipeListDialogComponent implements AfterViewInit {
    @ViewChild('recipeList') list: RecipeListComponent;

    @Input() isVisible: boolean = false;
    @Output() onRecipeSelected: EventEmitter<Recipe> = new EventEmitter();

    ngAfterViewInit(): void {
        setTimeout(() => {
            this.list.addButtonVisible = false;
        });
    }

    public recipeSelected(recipe: Recipe) {
        this.onRecipeSelected.emit(recipe);
        this.dismissDialog();
    }

    public dismissDialog() {
        this.isVisible = false;
    }
}