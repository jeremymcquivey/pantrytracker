import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UserProfile } from '../model/user-profile';

@Injectable()
export class AccountService {
    userProfile: UserProfile;
    constructor(private _httpClient: HttpClient) { }
    getAllUsers() { } //Observable<UserProfile[]>
}