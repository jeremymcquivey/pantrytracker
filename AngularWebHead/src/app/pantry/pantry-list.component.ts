import { Component, OnInit, ViewChild } from "@angular/core";
import { MatTableDataSource } from "@angular/material/table";
import { PantryService } from "../core/pantry.service";
import { InventoryCorrectionComponent } from "./inventory-correction.component";
import { PantryLine, PantryLineGrouping } from "../model/pantryline";
import { InventoryTransactionComponent } from "./inventory-transaction.component";

@Component({
selector: 'pantry-list',
    templateUrl: 'pantry-list.component.html'
})
export class PantryListComponent implements OnInit {
    private recipeData: PantryLineGrouping[];
    dataSource = new MatTableDataSource();
    visibleColumns = ['name'];

    @ViewChild("correctionDialog")
    public correctionDialog: InventoryCorrectionComponent;

    @ViewChild("inventoryTransaction")
    public transactionDialog: InventoryTransactionComponent;

    constructor(private _pantryService: PantryService) {}

    ngOnInit(): void {
        this.getInventory();
    }

    getInventory(): void {
        this._pantryService.getCurrentInventory().subscribe(recipeData => {
            this.recipeData = recipeData;
            this.dataSource.data = this.recipeData;
        });
    }

    openDialog(line: PantryLine): void {
        this.correctionDialog.isVisible = true;
        this.correctionDialog.pantryLine = line;
    }

    addTransaction(): void {
        this.transactionDialog.LaunchDialog();
    }
}