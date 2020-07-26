import { NgModule } from '@angular/core';
import { ProductService } from './product.service';
import { AccountService } from './account.service';
import { PantryService } from './pantry.service';
import { RecipeService } from './recipe.service';
import { AuthService } from './auth.service';
import { AuthInterceptor } from './auth.interceptor';
import {HTTP_INTERCEPTORS} from '@angular/common/http';
import { AdminRouteGuard } from './admin-route-guard';
import { GroceryService } from './grocery.service';
import { MenuService } from './menu.service'

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
        RecipeService,
        GroceryService,
        MenuService,
        AdminRouteGuard
    ],
})
export class CoreModule { }
