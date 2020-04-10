import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Constants } from '../constants';
import { Project } from '../model/project';
import { Observable, from } from 'rxjs';
import { Milestone } from '../model/milestone';
import { UserPermission } from '../model/user-permission';
import { UserProfile } from '../model/user-profile';
import { MilestoneStatus } from '../model/milestone-status';
import { AuthService } from './auth.service';
import { Recipe } from '../model/recipe';
import { Product, ProductVariety, ProductCode } from '../model/pantryline';

@Injectable()
export class ProductService {
    constructor(private httpClient: HttpClient, private _authService: AuthService) { }
    
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

    addVariety(variety: ProductVariety): Observable<ProductVariety> {
        return from (this._authService.getAccessToken().then(accessToken => {
            var headers = new HttpHeaders().set('Authorization', `Bearer ${accessToken}`)
                                           .set('Content-Type', "application/json");
            return this.httpClient.post<ProductVariety>(Constants.recipeApi + `v1/Product/${variety.productId}/variety`, JSON.stringify(variety), { headers: headers }).toPromise();
        }));
    };

    getProduct(id: number): Observable<Product> {
        return from (this._authService.getAccessToken().then(accessToken => {
            var headers = new HttpHeaders().set('Authorization', `Bearer ${accessToken}`);
            return this.httpClient.get<Product>(Constants.recipeApi + `v1/Product/${id}`, { headers: headers }).toPromise();
        }));
    }

    getProducts(searchText: string): Observable<Product[]> {
        return from (this._authService.getAccessToken().then(accessToken => {
            var headers = new HttpHeaders().set('Authorization', `Bearer ${accessToken}`);
            return this.httpClient.get<Product[]>(Constants.recipeApi + `v1/Product?searchText=${searchText}`, { headers: headers }).toPromise();
        }));
    }

    addProduct(newProduct: Product): Observable<Product> {
        return from (this._authService.getAccessToken().then(accessToken => {
            var headers = new HttpHeaders().set('Authorization', `Bearer ${accessToken}`)
                                           .set('Content-Type', "application/json");
            return this.httpClient.post<Product>(Constants.recipeApi + `v1/Product`, JSON.stringify(newProduct), { headers: headers }).toPromise();
        }));
    }

    lookupCode(code: string): Observable<ProductCode> {
        return from (this._authService.getAccessToken().then(accessToken => {
            var headers = new HttpHeaders().set('Authorization', `Bearer ${accessToken}`);
            return this.httpClient.get<ProductCode>(Constants.recipeApi + `v1/Product/search/code/${code}`, { headers: headers }).toPromise();
        }));
    }

    addProductCode(code: ProductCode): Observable<ProductCode> {
        return from (this._authService.getAccessToken().then(accessToken => {
            var headers = new HttpHeaders().set('Authorization', `Bearer ${accessToken}`)
                                           .set('Content-Type', "application/json");
            return this.httpClient.post<ProductCode>(Constants.recipeApi + `v1/Product/${code.productId}/code/${code.code}`, JSON.stringify(code), { headers: headers }).toPromise();
        }));                          
    }
}