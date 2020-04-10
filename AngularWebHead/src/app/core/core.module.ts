import { NgModule } from '@angular/core';
import { ProductService } from './product.service';
import { AccountService } from './account.service';
import { PantryService } from './pantry.service';
import { AuthService } from './auth.service';
import { AuthInterceptor } from './auth.interceptor';
import {HTTP_INTERCEPTORS} from '@angular/common/http';
import { AdminRouteGuard } from './admin-route-guard';

@NgModule({
    imports: [],
    exports: [],
    declarations: [],
    providers: [ 
        { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
        ProductService,
        AccountService,
        PantryService,
        AuthService,
        AdminRouteGuard
    ],
})
export class CoreModule { }
