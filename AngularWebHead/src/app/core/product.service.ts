import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Constants } from '../constants';
import { Observable, from } from 'rxjs';
import { AuthService } from './auth.service';
import { Product, ProductVariety, ProductCode } from '../model/pantryline';

@Injectable()
export class ProductService {
    constructor(private httpClient: HttpClient, private _authService: AuthService) { }

    private ProductSearchType_RowId: number = 1;
    private ProductSearchType_Description: number = 2;

    addVariety(variety: ProductVariety): Observable<ProductVariety> {
        return from (this._authService.getAccessToken().then(accessToken => {
            var headers = new HttpHeaders().set('Authorization', `Bearer ${accessToken}`)
                                           .set('Content-Type', "application/json");
            return this.httpClient.post<ProductVariety>(Constants.recipeApi + `v1/Variety`, JSON.stringify(variety), { headers: headers }).toPromise();
        }));
    };

    getProduct(id: number): Observable<Product[]> {
        return from (this._authService.getAccessToken().then(accessToken => {
            var headers = new HttpHeaders().set('Authorization', `Bearer ${accessToken}`);
            return this.httpClient.get<Product[]>(Constants.recipeApi + `v1/Product/${id}?identifierType=${this.ProductSearchType_RowId}`, { headers: headers }).toPromise();
        }));
    }

    getProducts(searchText: string): Observable<Product[]> {
        return from (this._authService.getAccessToken().then(accessToken => {
            var headers = new HttpHeaders().set('Authorization', `Bearer ${accessToken}`);
            return this.httpClient.get<Product[]>(Constants.recipeApi + `v1/Product/${searchText}?identifierType=${this.ProductSearchType_Description}`, { headers: headers }).toPromise();
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
            return this.httpClient.get<ProductCode>(Constants.recipeApi + `v1/ProductCode/${code}`, { headers: headers }).toPromise();
        }));
    }

    addProductCode(code: ProductCode): Observable<ProductCode> {
        return from (this._authService.getAccessToken().then(accessToken => {
            var headers = new HttpHeaders().set('Authorization', `Bearer ${accessToken}`)
                                           .set('Content-Type', "application/json");
            return this.httpClient.post<ProductCode>(Constants.recipeApi + `v1/ProductCode`, JSON.stringify(code), { headers: headers }).toPromise();
        }));                          
    }
}