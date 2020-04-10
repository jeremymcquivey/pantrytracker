import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { HomeComponent } from './home/home.component';
import { RecipeListComponent } from './product/recipe-list.component';
import { ContactUsComponent } from './home/contact-us.component';
import { UnauthorizedComponent } from './home/unauthorized.component';
import { RecipeComponent } from './recipes/recipe.component';
import { PantryListComponent } from './pantry/pantry-list.component';
import { SigninRedirectCallbackComponent } from './home/signin-redirect-callback.component';
import { SignoutRedirectCallbackComponent } from './home/signout-redirect-callback.component';

const routes: Routes = [
    { path: '', component: HomeComponent },
    { path: 'contact-us', component: ContactUsComponent },
    { path: 'recipes', component: RecipeListComponent},
    { path: 'unauthorized', component: UnauthorizedComponent },
    { path: 'new-recipe', component: RecipeComponent},
    { path: 'recipe/:recipeId', component: RecipeComponent},
    { path: 'pantry/inventory', component: PantryListComponent },
    { path: 'signin-callback', component: SigninRedirectCallbackComponent },
    { path: 'signout-callback', component: SignoutRedirectCallbackComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }