import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { AuthService } from './auth.service';
import { ProductGroceryList } from '../model/product-grocery-list';
import { from, Observable } from 'rxjs';
import { Constants } from '../constants';
import { Recipe, RecipeProductPreference } from '../model/recipe';

@Injectable()
export class RecipeService {
    constructor(private httpClient: HttpClient, private _authService: AuthService) { }

    getRecipeProductList(recipeId: string): Observable<ProductGroceryList> {
      return from (this._authService.getAccessToken().then(accessToken => {
          var headers = new HttpHeaders().set('Authorization', `Bearer ${accessToken}`);
          return this.httpClient.get<ProductGroceryList>(`${Constants.recipeApi}v1/ShoppingList/Preview/Recipe/${recipeId}`, { headers: headers }).toPromise();
      }));
    }

    getRecipes(): Observable<Recipe[]> {
        return from (this._authService.getAccessToken().then(accessToken => {
            var headers = new HttpHeaders().set('Authorization', `Bearer ${accessToken}`);
            return this.httpClient.get<Recipe[]>(Constants.recipeApi + 'v1/Recipe', { headers: headers }).toPromise();
        }));
    }

    getRecipe(recipeId: string): Observable<Recipe> {
        return from (this._authService.getAccessToken().then(accessToken => {
            var headers = new HttpHeaders().set('Authorization', `Bearer ${accessToken}`);
            return this.httpClient.get<Recipe>(Constants.recipeApi + 'v1/Recipe/' + recipeId, { headers: headers }).toPromise();
        }));
    }

    saveRecipe(recipe: Recipe) {
        return from (this._authService.getAccessToken().then(accessToken => {
            var headers = new HttpHeaders().set('Authorization', `Bearer ${accessToken}`);
            
            if(recipe.id && recipe.id !== "") {
                return this.httpClient.put<Recipe>(Constants.recipeApi + 'v1/Recipe/' + recipe.id, recipe, { headers: headers }).toPromise();
            }
            return this.httpClient.post<Recipe>(Constants.recipeApi + 'v1/Recipe/', recipe, { headers: headers }).toPromise();
        }));
    }

    setProductPreference(preference: RecipeProductPreference): Observable<RecipeProductPreference> {
        return from (this._authService.getAccessToken().then(accessToken => {
            var headers = new HttpHeaders().set('Authorization', `Bearer ${accessToken}`);
            return this.httpClient.post<RecipeProductPreference>(Constants.recipeApi + 'v1/UserMatch', preference, { headers: headers }).toPromise();
        }));
    }
    
    submitRawText(text: String) {
        return from (this._authService.getAccessToken().then(accessToken => {
            var headers = new HttpHeaders().set('Authorization', `Bearer ${accessToken}`)
                                            .set('Content-Type', "application/json");
            return this.httpClient.post<Recipe>(Constants.recipeApi + 'v1/Recipe/preview/text', JSON.stringify(text), { headers: headers }).toPromise();
        }));
    }
    
    submitImage(text: String) {
        return from (this._authService.getAccessToken().then(accessToken => {
            var headers = new HttpHeaders().set('Authorization', `Bearer ${accessToken}`)
                                            .set('Content-Type', "application/json");
            return this.httpClient.post<Recipe>(Constants.recipeApi + 'v1/Recipe/preview/image', JSON.stringify(text.replace(/^data:image\/(png|jpg|jpeg);base64,/, "")), { headers: headers }).toPromise();
        }));
    }
}