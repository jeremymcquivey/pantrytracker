import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Constants } from '../constants';
import { Project } from '../model/project';
import { Observable } from 'rxjs/Observable';
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
        var accessToken = this._authService.getAccessToken();
        var headers = new HttpHeaders().set('Authorization', `Bearer ${accessToken}`);
        return this.httpClient.get<Recipe[]>(Constants.recipeApi + 'v1/Recipe', { headers: headers });
    }

    getRecipe(recipeId: string): Observable<Recipe> {
        var accessToken = this._authService.getAccessToken();
        var headers = new HttpHeaders().set('Authorization', `Bearer ${accessToken}`);
        return this.httpClient.get<Recipe>(Constants.recipeApi + 'v1/Recipe/' + recipeId, { headers: headers });
    }

    saveRecipe(recipe: Recipe) {
        var accessToken = this._authService.getAccessToken();
        var headers = new HttpHeaders().set('Authorization', `Bearer ${accessToken}`);

        if(recipe.id && recipe.id !== "") {
            return this.httpClient.put<Recipe>(Constants.recipeApi + 'v1/Recipe/' + recipe.id, recipe, { headers: headers });
        }
        return this.httpClient.post<Recipe>(Constants.recipeApi + 'v1/Recipe/', recipe, { headers: headers });
    }
    
    submitRawText(text: String) {
        var accessToken = this._authService.getAccessToken();
        const headers = new HttpHeaders().set('Authorization', `Bearer ${accessToken}`)
                                         .set('Content-Type', "application/json");

        return this.httpClient.post<Recipe>(Constants.recipeApi + 'v1/Recipe/preview/text', JSON.stringify(text), { headers: headers });
    }
    
    submitImage(text: String) {
        var accessToken = this._authService.getAccessToken();
        const headers = new HttpHeaders().set('Authorization', `Bearer ${accessToken}`)
                                         .set('Content-Type', "application/json");

        return this.httpClient.post<Recipe>(Constants.recipeApi + 'v1/Recipe/preview/image', JSON.stringify(text.replace(/^data:image\/(png|jpg|jpeg);base64,/, "")), { headers: headers });
    }

    addVariety(variety: ProductVariety): Observable<ProductVariety> {
        var accessToken = this._authService.getAccessToken();
        var headers = new HttpHeaders().set('Authorization', `Bearer ${accessToken}`)
                                       .set('Content-Type', "application/json");
                                       
        return this.httpClient.post<ProductVariety>(Constants.recipeApi + `v1/Product/${variety.productId}/variety`, JSON.stringify(variety), { headers: headers });
    };

    getProduct(id: number): Observable<Product> {
        var accessToken = this._authService.getAccessToken();
        var headers = new HttpHeaders().set('Authorization', `Bearer ${accessToken}`);

        return this.httpClient.get<Product>(Constants.recipeApi + `v1/Product/${id}`, { headers: headers });
    }

    getProducts(searchText: string): Observable<Product[]> {
        var accessToken = this._authService.getAccessToken();
        var headers = new HttpHeaders().set('Authorization', `Bearer ${accessToken}`);

        return this.httpClient.get<Product[]>(Constants.recipeApi + `v1/Product?searchText=${searchText}`, { headers: headers });
    }

    addProduct(newProduct: Product): Observable<Product> {
        var accessToken = this._authService.getAccessToken();
        var headers = new HttpHeaders().set('Authorization', `Bearer ${accessToken}`)
                                       .set('Content-Type', "application/json");

        return this.httpClient.post<Product>(Constants.recipeApi + `v1/Product`, JSON.stringify(newProduct), { headers: headers });
    }

    lookupCode(code: string): Observable<ProductCode> {
        var accessToken = this._authService.getAccessToken();
        var headers = new HttpHeaders().set('Authorization', `Bearer ${accessToken}`);

        return this.httpClient.get<ProductCode>(Constants.recipeApi + `v1/Product/search/code/${code}`, { headers: headers });
    }

    addProductCode(code: ProductCode): Observable<ProductCode> {
        var accessToken = this._authService.getAccessToken();
        var headers = new HttpHeaders().set('Authorization', `Bearer ${accessToken}`)
                                       .set('Content-Type', "application/json");

        return this.httpClient.post<ProductCode>(Constants.recipeApi + `v1/Product/${code.productId}/code/${code.code}`, JSON.stringify(code), { headers: headers });
    }
}