import { OnInit, Component } from '@angular/core';
import { RecipeService } from '../core/recipe.service';
import { PantryLine } from '../model/pantryline';
import { Ingredient } from '../model/ingredient';
import { ProductGroceryList } from '../model/product-grocery-list';
import { ActivatedRoute } from '@angular/router';
import { MockRecipeData } from '../core/mock-recipe-data';

@Component({
    selector: 'recipe-products',
    templateUrl: './recipe-products.component.html',
    styleUrls: ['../controls/fancy-form.component.css']
})
export class RecipeProductsComponent implements OnInit {
    private _recipeId: string;
    private _data: ProductGroceryList;

    public VisibleColumns = [ 'name' ];
    public isRecipeLoaded = true;

    public get MatchedDataSource(): PantryLine[] {
        return !!this._data ? this._data.matched : []; //?.
    }

    public get UnmatchedDataSource(): Ingredient[] {
        return !!this._data ? this._data.unmatched : []; //?.
    }
    
    public get IgnoredDataSource(): Ingredient[] {
        return !!this._data ? this._data.ignored : []; //?.
    };

    public get hasUnmatchedItems(): boolean {
        return !!this._data && !!this._data.unmatched ? this._data.unmatched.length > 0 : false; //?.
    }

    public get RecipeName(): string {
        return "Chicken Noodle Soup";
    }

    constructor(private _recipeService: RecipeService, private _route: ActivatedRoute) { }

    ngOnInit(): void {
        this._recipeId = this._route.snapshot.params.recipeId;
        this._recipeService.getRecipeProductList(this._recipeId).subscribe(list => {
            this._data = list;
        }, error => {
            this._data = MockRecipeData.MockRecipeProductList; //TODO: Don't do this. This is just for offline mode debugging.
            console.error(error);
        });
    }
}