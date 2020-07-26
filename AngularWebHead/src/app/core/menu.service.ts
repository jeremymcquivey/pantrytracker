import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { AuthService } from "./auth.service";
import { Observable, from } from "rxjs";
import { Constants } from "../constants";
import { MenuEntry } from "../model/menu";

@Injectable()
export class MenuService {
    constructor(private httpClient: HttpClient, private _authService: AuthService) { }
    
    getMenu(startDate: string, endDate: string): Observable<MenuEntry[]> {
        return from (this._authService.getAccessToken().then(accessToken => {
            var headers = new HttpHeaders().set('Authorization', `Bearer ${accessToken}`);
            return this.httpClient.get<MenuEntry[]>(Constants.recipeApi + `v1/MenuPlan?startDate=${startDate}&endDate=${endDate}`, { headers: headers }).toPromise();
        }));
    }
}