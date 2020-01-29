import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { AuthService } from "./auth.service";
import { Observable } from "rxjs";
import { PantryLine } from "../model/pantryline";
import { Constants } from "../constants";

@Injectable()
export class PantryService {
    constructor(private httpClient: HttpClient, private _authService: AuthService) { }
    
    getCurrentInventory(): Observable<PantryLine[]> {
        var accessToken = this._authService.getAccessToken();
        var headers = new HttpHeaders().set('Authorization', `Bearer ${accessToken}`);
        return this.httpClient.get<PantryLine[]>(Constants.recipeApi + 'v1/Pantry', { headers: headers });
    }

    updateInventory(adjustment: PantryLine) {
        var accessToken = this._authService.getAccessToken();
        var headers = new HttpHeaders().set('Authorization', `Bearer ${accessToken}`);
        return this.httpClient.post<PantryLine>(Constants.recipeApi + 'v1/Pantry', adjustment, { headers: headers });
    }
}