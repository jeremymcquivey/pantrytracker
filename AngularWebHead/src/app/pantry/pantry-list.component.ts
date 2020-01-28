import { Component, OnInit } from "@angular/core";
import { MatTableDataSource } from "@angular/material";
import { PantryLine } from "../model/pantryline";
import { PantryService } from "../core/pantry.service";

@Component({
selector: 'pantry-list',
    templateUrl: 'pantry-list.component.html'
})
export class PantryListComponent implements OnInit {
    private recipeData: any[];
    dataSource = new MatTableDataSource();
    visibleColumns = ['name'];

    constructor(
        private _pantryService: PantryService
      ) {}

    ngOnInit(): void {
        this._pantryService.getCurrentInventory().subscribe(recipeData => {
            this.recipeData = recipeData;
            this.dataSource.data = this.recipeData;
        });
    }
}