import { NgModule } from '@angular/core';
import { ProjectService } from './project.service';
import { AccountService } from './account.service';
import { PantryService } from './pantry.service';
import { AuthService } from './auth.service';
import { AuthInterceptor } from './auth.interceptor';
import {HTTP_INTERCEPTORS} from '@angular/common/http';

@NgModule({
    imports: [],
    exports: [],
    declarations: [],
    providers: [ 
        { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
        ProjectService,
        AccountService,
        PantryService,
        AuthService
    ],
})
export class CoreModule { }
