import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { HomeComponent } from './home/home.component';
import { ProjectListComponent } from './projects/project-list.component';
import { ProjectComponent } from './projects/project.component';
import { ContactUsComponent } from './home/contact-us.component';
import { UnauthorizedComponent } from './home/unauthorized.component';
import { RecipeComponent } from './recipes/recipe.component';
import { PantryListComponent } from './pantry/pantry-list.component';

const routes: Routes = [
    { path: '', component: HomeComponent },
    { path: 'contact-us', component: ContactUsComponent },
    { path: 'recipes', component: ProjectListComponent},
    { path: 'project/:projectId', component: ProjectComponent },
    { path: 'unauthorized', component: UnauthorizedComponent },
    { path: 'new-recipe', component: RecipeComponent},
    { path: 'recipe/:recipeId', component: RecipeComponent},
    { path: 'pantry/inventory', component: PantryListComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }