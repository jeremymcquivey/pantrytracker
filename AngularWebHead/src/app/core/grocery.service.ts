import { Observable, from } from "rxjs";
import { AuthService } from "./auth.service";
import { GroceryItem } from "../model/product-grocery-list";
import { HttpHeaders, HttpClient } from "@angular/common/http";
import { Constants } from "../constants";
import { Injectable } from "@angular/core";

@Injectable()
export class GroceryService {

    constructor(private _authService: AuthService, private _httpClient: HttpClient) { }

    getGroceryList(listId: string): Observable<GroceryItem[]> {
        return from (this._authService.getAccessToken().then(accessToken => {
            var headers = new HttpHeaders().set('Authorization', `Bearer ${accessToken}`);
            return this._httpClient.get<GroceryItem[]>(Constants.recipeApi + 'v1/ShoppingList/' + listId + '/items', { headers: headers }).toPromise();
        }));
    }

    addGroceryItem(listId: string, item: GroceryItem): Observable<GroceryItem> {
        return from (this._authService.getAccessToken().then(accessToken => {
            var headers = new HttpHeaders().set('Authorization', `Bearer ${accessToken}`);
            return this._httpClient.post<GroceryItem>(Constants.recipeApi + 'v1/ShoppingList/' + listId + '/item', item, { headers: headers }).toPromise();
        }));
    }

    removeGroceryItem(listId: string, itemId: number): Observable<any> {
        return from (this._authService.getAccessToken().then(accessToken => {
            var headers = new HttpHeaders().set('Authorization', `Bearer ${accessToken}`);
            return this._httpClient.delete<any>(Constants.recipeApi + 'v1/ShoppingList/' + listId + '/item/' + itemId, { headers: headers }).toPromise();
        }));
    }

    updateGroceryItem(listId: string, item: GroceryItem) {
        return from (this._authService.getAccessToken().then(accessToken => {
            var headers = new HttpHeaders().set('Authorization', `Bearer ${accessToken}`);
            return this._httpClient.put<any>(Constants.recipeApi + 'v1/ShoppingList/' + listId + '/item/' + item.id, item, { headers: headers }).toPromise();
        }));
    }

    addBulkItemsToList(listId: string, items: GroceryItem[]): Observable<GroceryItem[]> {
        return from (this._authService.getAccessToken().then(accessToken => {
            var headers = new HttpHeaders().set('Authorization', `Bearer ${accessToken}`);
            return this._httpClient.post<GroceryItem[]>(Constants.recipeApi + 'v1/ShoppingList/' + listId + '/items', items, { headers: headers }).toPromise();
        }));
    }
}