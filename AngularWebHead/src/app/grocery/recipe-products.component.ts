import { OnInit, Component } from '@angular/core';
import { RecipeService } from '../core/recipe.service';
import { PantryLine } from '../model/pantryline';
import { Ingredient } from '../model/ingredient';
import { ProductGroceryList } from '../model/product-grocery-list';

@Component({
    selector: 'recipe-products',
    templateUrl: './recipe-products.component.html',
    styleUrls: ['../controls/fancy-form.component.css']
})
export class RecipeProductsComponent implements OnInit {
    private _data: ProductGroceryList;

    public VisibleColumns = [ 'name' ];
    public isRecipeLoaded = true;

    public get MatchedDataSource(): PantryLine[] {
        return this._data ? this._data.Matched : null; //?.
    }

    public get UnmatchedDataSource(): Ingredient[] {
        return this._data ? this._data.Unmatched : null; //?.
    }
    
    public get IgnoredDataSource(): Ingredient[] {
        return this._data ? this._data.Ignored : null; //?.
    };

    public get hasUnmatchedItems(): boolean {
        return this._data.Unmatched.length > 0; //?. false.
    }

    public get RecipeName(): string {
        return "Chicken Noodle Soup";
    }

    constructor(private recipeService: RecipeService) { }

    ngOnInit(): void {
        this._data = this.recipeService.getProducts();
    }
}