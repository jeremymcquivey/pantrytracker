import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { AuthService } from "./auth.service";
import { Observable, from } from "rxjs";
import { Constants } from "../constants";
import { MenuEntry, MenuGroup } from "../model/menu";
import { PantryLine } from "../model/pantryline";

@Injectable()
export class MenuService {
    constructor(private httpClient: HttpClient, private _authService: AuthService) { }
    
    getMenu(startDate: string, endDate: string): Observable<MenuGroup[]> {
        return from (this._authService.getAccessToken().then(accessToken => {
            var headers = new HttpHeaders().set('Authorization', `Bearer ${accessToken}`);
            return this.httpClient.get<MenuGroup[]>(Constants.recipeApi + `v1/MenuPlan?startDate=${startDate}&endDate=${endDate}`, { headers: headers }).toPromise();
        }));
    }

    deleteEntry(id: number): Observable<any> {
        return from (this._authService.getAccessToken().then(accessToken => {
            var headers = new HttpHeaders().set('Authorization', `Bearer ${accessToken}`);
            return this.httpClient.delete(Constants.recipeApi + `v1/MenuPlan/${id}`, { headers: headers }).toPromise();
        }));
    }

    addEntry(entry: MenuEntry): Observable<MenuEntry> {
        return from (this._authService.getAccessToken().then(accessToken => {
            var headers = new HttpHeaders().set('Authorization', `Bearer ${accessToken}`);
            return this.httpClient.post<MenuEntry>(Constants.recipeApi + 'v1/MenuPlan', entry, { headers: headers }).toPromise();
        }));
    }

    public setDateObject(groupList: MenuGroup[]) {
        for(let group of groupList) {
            group.key = new Date(group.key);
            for(let item of group.value) {
                item.date = new Date(item.date);
            }
        }
    }
}