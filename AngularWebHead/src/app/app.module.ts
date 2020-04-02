import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatDialogModule } from '@angular/material/dialog';
import { MatTableModule } from '@angular/material/table';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { ZXingScannerModule } from '@zxing/ngx-scanner';

import { AppRoutingModule } from './app-routing.module';

import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { ContactUsComponent } from './home/contact-us.component';
import { RecipeListComponent } from './product/recipe-list.component';
import { AdminModule } from './admin/admin.module';
import { CoreModule } from './core/core.module';
import { UnauthorizedComponent } from './home/unauthorized.component';
import { RecipeComponent } from './recipes/recipe.component';
import { ImageUploadComponent } from './io/imageupload.component';
import { UploadFileDialogComponent } from './io/uploadfile-dialog.component';
import { TxtUploadComponent } from './io/txtupload.component';
import { TextEditorDialogComponent } from './io/texteditor-dialog.component';
import { PantryListComponent } from './pantry/pantry-list.component'
import { InventoryCorrectionComponent } from './pantry/inventory-correction.component'
import { InventoryTransactionComponent } from './pantry/inventory-transaction.component'
import { ProductSearchComponent } from './product/product-search.component'
import { ProductSearchDialogComponent } from './product/product-search-dialog.component'
import { ManageProductsComponent } from './product/manage-products.component'
import { ProductDetailsDialogComponent } from './product/product-details-dialog.component'
import { AddProductDialogComponent } from './product/add-product-dialog.component'
import { AddProductCodeComponent } from './product/add-product-code.component'
import { AddVarietyComponent } from './product/add-variety.component'
import { FileUploadModule } from 'ng2-file-upload';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    ContactUsComponent,
    RecipeListComponent,
    UnauthorizedComponent,
    RecipeComponent,
    ImageUploadComponent,
    TxtUploadComponent,
    UploadFileDialogComponent,
    TextEditorDialogComponent,
    PantryListComponent,
    ProductSearchComponent,
    ProductSearchDialogComponent,
    InventoryCorrectionComponent,
    InventoryTransactionComponent,
    AddProductDialogComponent,
    ManageProductsComponent,
    AddProductCodeComponent,
    AddVarietyComponent,
    ProductDetailsDialogComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    FormsModule,
    BrowserAnimationsModule,
    MatButtonModule,
    MatToolbarModule,
    MatDialogModule,
    MatTableModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    AdminModule,
    CoreModule,
    AppRoutingModule,
    MatCardModule,
    FileUploadModule,    
    ZXingScannerModule
  ],
  providers: [],
  bootstrap: [AppComponent],
  entryComponents:[]
})
export class AppModule { }