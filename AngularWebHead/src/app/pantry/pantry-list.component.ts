import { Component, OnInit, ViewChild } from "@angular/core";
import { MatTableDataSource } from "@angular/material";
import { PantryService } from "../core/pantry.service";
import { InventoryCorrectionComponent } from "./inventory-correction.component";
import { PantryLine } from "../model/pantryline";

@Component({
selector: 'pantry-list',
    templateUrl: 'pantry-list.component.html'
})
export class PantryListComponent implements OnInit {
    private recipeData: any[];
    dataSource = new MatTableDataSource();
    visibleColumns = ['name'];

    @ViewChild("correctionDialog")
    public correctionDialog: InventoryCorrectionComponent;

    constructor(
        private _pantryService: PantryService
      ) {}

    ngOnInit(): void {
        this._pantryService.getCurrentInventory().subscribe(recipeData => {
            this.recipeData = recipeData;
            this.dataSource.data = this.recipeData;
        });
    }

    openDialog(line: PantryLine): void {
        this.correctionDialog.isVisible = true;
        this.correctionDialog.pantryLine = line;
    }
}