import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { AuthService } from "./auth.service";
import { Observable, from } from "rxjs";
import { PantryLine, PantryLineGrouping } from "../model/pantryline";
import { Constants } from "../constants";

@Injectable()
export class PantryService {
    constructor(private httpClient: HttpClient, private _authService: AuthService) { }
    
    getCurrentInventory(): Observable<PantryLineGrouping[]> {
        return from (this._authService.getAccessToken().then(accessToken => {
            var headers = new HttpHeaders().set('Authorization', `Bearer ${accessToken}`);
            return this.httpClient.get<PantryLineGrouping[]>(Constants.recipeApi + 'v1/Pantry/0/levelSummary?includeZeroValues=false', { headers: headers }).toPromise();
        }));
    }

    updateInventory(adjustment: PantryLine) {
        return from(this._authService.getAccessToken().then(accessToken => {
            var headers = new HttpHeaders().set('Authorization', `Bearer ${accessToken}`);
            return this.httpClient.post<PantryLine>(Constants.recipeApi + 'v1/PantryTransaction', adjustment, { headers: headers }).toPromise();
        }));
    }

    getProductSummary(productId: number): Observable<PantryLineGrouping[]> {
        return from(this._authService.getAccessToken().then(accessToken => {
            var headers = new HttpHeaders().set('Authorization', `Bearer ${accessToken}`);
            return this.httpClient.get<PantryLineGrouping[]>(`${Constants.recipeApi}v1/product/${productId}/levelSummary?pantryId=0&includeZeroValues=true`, { headers: headers }).toPromise();
        }));
    }
}