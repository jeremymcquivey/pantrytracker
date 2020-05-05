import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { HomeComponent } from './home/home.component';
import { RecipeListComponent } from './recipes/recipe-list.component';
import { ContactUsComponent } from './home/contact-us.component';
import { UnauthorizedComponent } from './home/unauthorized.component';
import { RecipeComponent } from './recipes/recipe.component';
import { PantryListComponent } from './pantry/pantry-list.component';
import { SigninRedirectCallbackComponent } from './home/signin-redirect-callback.component';
import { SignoutRedirectCallbackComponent } from './home/signout-redirect-callback.component';
import { InventoryDetailComponent } from './pantry/inventory-detail.component';
import { RecipeProductsComponent } from './grocery/recipe-products.component';
import { GroceryListViewComponent } from './grocery/list-view.component';

const routes: Routes = [
    { path: '', component: HomeComponent },
    { path: 'contact-us', component: ContactUsComponent },
    { path: 'recipes', component: RecipeListComponent},
    { path: 'unauthorized', component: UnauthorizedComponent },
    { path: 'new-recipe', component: RecipeComponent},
    { path: 'recipe/:recipeId', component: RecipeComponent},
    { path: 'recipe/:recipeId/products', component: RecipeProductsComponent},
    { path: 'pantry/inventory', component: PantryListComponent },
    { path: 'pantry/product/:productId', component: InventoryDetailComponent},
    { path: 'signin-callback', component: SigninRedirectCallbackComponent },
    { path: 'signout-callback', component: SignoutRedirectCallbackComponent },
    { path: 'grocery-list/:listId', component: GroceryListViewComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }