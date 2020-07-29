import { OnInit, Component, ViewChild, TemplateRef, AfterViewInit } from '@angular/core';
import { RecipeService } from '../core/recipe.service';
import { ProductGroceryList, GroceryItem, GroceryItemStatus } from '../model/product-grocery-list';
import { ActivatedRoute, Router } from '@angular/router';
import { ListItemReassignComponent } from './list-item-reassign.component';
import { ListItemMatchedComponent } from './list-item-matched.component';
import { ListItemIgnoredComponent } from './list-item-ignored.component';
import { RecipeProduct, RecipeProductPreference, Recipe } from '../model/recipe';
import { ListItemUnmatchedComponent } from './list-item-unmatched.component';
import { AuthService } from '../core/auth.service';
import { GroceryService } from '../core/grocery.service';
import { TabGroupComponent } from '../controls/tab-group.component';

@Component({
    selector: 'recipe-products',
    templateUrl: './recipe-products.component.html',
    styleUrls: ['../controls/fancy-form.component.css']
})
export class RecipeProductsComponent implements OnInit, AfterViewInit {
    private _isBusy: boolean = false;
    private _data: ProductGroceryList;

    public VisibleColumns = [ 'name' ];
    public isRecipeLoaded = true;
    public tabs = [];

    @ViewChild('reassignDialog')
    private reassignDialog: ListItemReassignComponent;

    @ViewChild('matchedItems')
    private matchedItems: ListItemMatchedComponent;

    @ViewChild('ignoredItems')
    private ignoredItems: ListItemIgnoredComponent;

    @ViewChild('unmatchedItems')
    private unmatchedItems: ListItemUnmatchedComponent;

    @ViewChild('matchedContent')
    private matchedContent: TemplateRef<any>;

    @ViewChild('ignoredContent')
    private ignoredContent: TemplateRef<any>;

    @ViewChild('unmatchedContent')
    private unmatchedContent: TemplateRef<any>;

    @ViewChild('tabView')
    private tabView: TabGroupComponent;

    public get MatchedDataSource(): RecipeProduct[] {
        return this._data?.matched ?? [];
    }

    public get UnmatchedDataSource(): RecipeProduct[] {
        return this._data?.unmatched ?? [];
    }
    
    public get IgnoredDataSource(): RecipeProduct[] {
        return this._data?.ignored ?? [];
    };

    public get hasMatchedItems(): boolean {
        return (this._data?.matched?.length ?? 0) > 0;
    }

    public get hasUnmatchedItems(): boolean {
        return (this._data?.unmatched?.length ?? 0) > 0;
    }

    public get isBusy(): boolean {
        return this._isBusy;
    }

    public AllRecipes: Recipe[];
    public SelectedIndex: number = 0;
    public RecipeSource: string = '';

    public get SelectedRecipe(): Recipe {
        return this.AllRecipes && this.AllRecipes.length > 0 ?
            this.AllRecipes[this.SelectedIndex] :
            {} as Recipe;
    }

    constructor(private _recipeService: RecipeService, 
                private _router: Router,
                private _authService: AuthService, 
                private _groceryService: GroceryService) { }

    ngOnInit(): void {
        this.AllRecipes = this._recipeService.sharedRecipeList;
        this.SelectedIndex = 0;

        this.getRecipeProducts();
    }

    loadPreviousRecipe() {
        if(this._isBusy) {
            return;
        }

        this.SelectedIndex--;
        this.getRecipeProducts();
    }

    loadNextRecipe() {
        if(this._isBusy) {
            return;
        }

        this.SelectedIndex++;
        this.getRecipeProducts();
    }

    private getRecipeProducts() {
        if(this._isBusy) {
            return;
        }

        if(this.AllRecipes.length < this.SelectedIndex) {
            return;
        }

        this._isBusy = true;
        this._data = { unmatched: [], matched: [], ignored: [] };
        this._recipeService.getRecipeProductList(this.AllRecipes[this.SelectedIndex].id).subscribe(list => {
            this._data = list;
            this.tabView.selectedTab = this.hasUnmatchedItems ? 0 : 1;
        }, error => {
            console.error(error);
        }, () => { this._isBusy = false; });
    }

    ngAfterViewInit(): void {
        this.tabs.push({ title: 'Unmatched', template: this.unmatchedContent, id: 'unmatched-tab'});
        this.tabs.push({ title: 'Matched', template: this.matchedContent, id: 'matched-tab'});
        this.tabs.push({ title: 'Ignored', template: this.ignoredContent, id: 'ignored-tab'});
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
            varietyId: value.varietyId,
            Unit: value.unit,
            Size: value.size,
            Quantity: value.quantityString
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
            varietyId: null,
        } as RecipeProductPreference).subscribe(line => {
            let ignoredProduct = this.removeMatched(line) ??
                                 this.removedUnmatched(line);
            
            this.addIgnored(ignoredProduct);
        }, error => {
            console.error(error);
        })
    }

    addMatchedItemsToShoppingList() {
        if(this._isBusy) {
            return;
        }

        if(this._data.matched.length <= 0) {
            alert('there are no matched items to add.');
        }

        if(this._data.unmatched.length > 0 && !confirm('You have unmatched items that will not be added to the list.')) {
            return;
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
                                if(this.AllRecipes.length == this.SelectedIndex + 1) {
                                    this._router.navigate([`grocery-list/${listId}`]);
                                } else {
                                    this._isBusy = false;
                                    this.loadNextRecipe();
                                }
                            }, error => {
                                this._isBusy = false;
                                console.error(error);
                            }, () => {
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