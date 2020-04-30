import { OnInit, Component, ViewChild } from '@angular/core';
import { RecipeService } from '../core/recipe.service';
import { ProductGroceryList, GroceryItem, GroceryItemStatus } from '../model/product-grocery-list';
import { ActivatedRoute, Router } from '@angular/router';
import { ListItemReassignComponent } from './list-item-reassign.component';
import { ListItemMatchedComponent } from './list-item-matched.component';
import { ListItemIgnoredComponent } from './list-item-ignored.component';
import { RecipeProduct, RecipeProductPreference } from '../model/recipe';
import { ListItemUnmatchedComponent } from './list-item-unmatched.component';
import { AuthService } from '../core/auth.service';
import { GroceryService } from '../core/grocery.service';

@Component({
    selector: 'recipe-products',
    templateUrl: './recipe-products.component.html',
    styleUrls: ['../controls/fancy-form.component.css']
})
export class RecipeProductsComponent implements OnInit {
    private _isBusy: boolean = false;
    private _recipeId: string;
    private _data: ProductGroceryList;
    private _recipeName: string;

    public VisibleColumns = [ 'name' ];
    public isRecipeLoaded = true;

    @ViewChild('reassignDialog')
    private reassignDialog: ListItemReassignComponent;

    @ViewChild('matchedItems')
    private matchedItems: ListItemMatchedComponent;

    @ViewChild('ignoredItems')
    private ignoredItems: ListItemIgnoredComponent;

    @ViewChild('unmatchedItems')
    private unmatchedItems: ListItemUnmatchedComponent;

    public get MatchedDataSource(): RecipeProduct[] {
        return this._data?.matched ?? [];
    }

    public get UnmatchedDataSource(): RecipeProduct[] {
        return this._data?.unmatched ?? [];
    }
    
    public get IgnoredDataSource(): RecipeProduct[] {
        return this._data?.ignored ?? [];
    };

    public get hasUnmatchedItems(): boolean {
        return (this._data?.unmatched?.length ?? 0) > 0;
    }

    public get RecipeName(): string {
        return this._recipeName;
    }

    public get isBusy(): boolean {
        return this._isBusy;
    }

    constructor(private _recipeService: RecipeService, 
                private _route: ActivatedRoute, 
                private _router: Router,
                private _authService: AuthService, 
                private _groceryService: GroceryService) { }

    ngOnInit(): void {
        this._recipeId = this._route.snapshot.params.recipeId;
        //TODO: Find a better way to get just the recipe name.
        this._recipeService.getRecipe(this._recipeId).subscribe(recipe => {
            this._recipeName = recipe.title;
            this.isRecipeLoaded = true;
        });
        this._recipeService.getRecipeProductList(this._recipeId).subscribe(list => {
            this._data = list;
        }, error => {
            console.error(error);
        });
    }

    reassignValue(value: RecipeProduct) {
        this.reassignDialog.launchDialog(value);
    }

    reassignIgnoredValue(value: RecipeProduct) {
        this.reassignDialog.allowIgnore = false;
        this.reassignDialog.launchDialog(value);
    }

    onProductReassigned(value: RecipeProduct) {
        this._recipeService.setProductPreference({
            recipeId: value.recipeId,
            matchingText: value.plainText,
            productId: value.productId,
            varietyId: value.varietyId
        } as RecipeProductPreference).subscribe(line => {
            let previousProduct = this.removeMatched(line) ??
                                  this.removedUnmatched(line) ??
                                  this.removeIgnored(line);

            this.addMatched(previousProduct);
        }, error => {
            console.error(error);
        })
    }

    onProductIgnored(value: RecipeProduct) {
        this._recipeService.setProductPreference({
            recipeId: value.recipeId,
            matchingText: value.plainText,
            productId: null,
            varietyId: null
        } as RecipeProductPreference).subscribe(line => {
            let ignoredProduct = this.removeMatched(line) ??
                                 this.removedUnmatched(line);
            
            this.addIgnored(ignoredProduct);
        }, error => {
            console.error(error);
        })
    }

    addMatchedItemsToShoppingList() {
        if(this._data.matched.length <= 0) {
            alert('there are no matched items to add.');
        }

        this._isBusy = true;
        const listId = this._authService.authContext.userProfile.id;

        const stuff = this._data.matched.map(p => ({
            id: 0,
            quantity: p.quantityString,
            status: GroceryItemStatus.Active,
            productId: p.product?.id,
            varietyId: p.variety?.id,
            unit: p.unit,
            size: p.size,
            product: null,
            variety: null,
            code: null
        } as GroceryItem));

        this._groceryService.addBulkItemsToList(listId, stuff)
                            .subscribe(_ => {
                                this._router.navigate(['grocery-list']);
                            }, error => {
                                console.error(error);
                            }, () => {
                                this._isBusy = false;
                            });
    }

    private removeMatched(value: RecipeProductPreference): RecipeProduct {
        const index = this._data.matched.findIndex(line => line.recipeId === value.recipeId && line.plainText === value.matchingText);
    
        if(index >= 0) {
            let temp = this._data.matched[index];
            this._data.matched.splice(index, 1);
            this.matchedItems.MatchedItems = this.MatchedDataSource;
            return temp;
        }

        return null;
    }

    private removedUnmatched(value: RecipeProductPreference): RecipeProduct {
        const index = this._data.unmatched.findIndex(line => line.recipeId === value.recipeId && line.plainText === value.matchingText);
    
        if(index >= 0) {
            let temp = this._data.unmatched[index];
            this._data.unmatched.splice(index, 1);
            this.unmatchedItems.UnmatchedItems = this.UnmatchedDataSource;
            return temp;
        }

        return null;
    }

    private removeIgnored(value: RecipeProductPreference): RecipeProduct {
        const index = this._data.ignored.findIndex(line => line.recipeId === value.recipeId && line.plainText === value.matchingText);
    
        if(index >= 0) {
            let temp = this._data.ignored[index];
            this._data.ignored.splice(index, 1);
            this.ignoredItems.IgnoredItems = this.IgnoredDataSource;
            return temp;
        }

        return null;
    }

    private addIgnored(value: RecipeProduct) {
        this._data.ignored.push(value);
        this.ignoredItems.IgnoredItems = this.IgnoredDataSource;
    }

    private addMatched(value: RecipeProduct) {
        this._data.matched.push(value);
        this.matchedItems.MatchedItems = this.MatchedDataSource;
    }
}