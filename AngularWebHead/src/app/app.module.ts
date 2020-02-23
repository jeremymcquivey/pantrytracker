import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatCardModule } from '@angular/material/card';

import {
  MatButtonModule,
  MatToolbarModule,
  MatDialogModule,
  MatTableModule,
  MatFormFieldModule,
  MatInputModule,
  MatSelectModule
} from '@angular/material';

import { AppRoutingModule } from './app-routing.module';

import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { ContactUsComponent } from './home/contact-us.component';
import { ProjectListComponent } from './projects/project-list.component';
import { ProjectComponent } from './projects/project.component';
import { AddEditMilestoneDialogComponent } from './projects/add-edit-milestone-dialog.component';
import { AdminModule } from './admin/admin.module';
import { CoreModule } from './core/core.module';
import { UnauthorizedComponent } from './home/unauthorized.component';
import { RecipeComponent } from './recipes/recipe.component';
import { ImageUploadComponent } from './io/imageupload.component';
import { FileSelectDirective } from 'ng2-file-upload';
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

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    ContactUsComponent,
    ProjectListComponent,
    ProjectComponent,
    AddEditMilestoneDialogComponent,
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
    FileSelectDirective,
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
    MatCardModule
  ],
  providers: [
  ],
  bootstrap: [AppComponent],
  entryComponents:[
    AddEditMilestoneDialogComponent
  ]
})
export class AppModule { }
