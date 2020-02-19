import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';

import {
  MatFormFieldModule,
  MatDialogModule,
  MatButtonModule,
  MatToolbarModule,
  MatTableModule,
  MatInputModule,
  MatSelectModule
} from '@angular/material';

import { ManagePermissionsComponent } from './manage-permissions.component';
import { AdminRoutingModule } from './admin-routing.module';

@NgModule({
  imports: [
    BrowserModule,
    FormsModule,
    MatFormFieldModule,
    MatDialogModule,
    MatButtonModule,
    MatToolbarModule,
    MatTableModule,
    MatInputModule,
    MatSelectModule,
    AdminRoutingModule
  ],
  exports: [],
  declarations: [
    ManagePermissionsComponent
  ],
  providers: [],
  entryComponents: [
  ]
})
export class AdminModule {}
