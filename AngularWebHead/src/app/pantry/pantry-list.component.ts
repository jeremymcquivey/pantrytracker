import { Component, OnInit, ViewChild } from "@angular/core";
import { MatTableDataSource } from "@angular/material/table";
import { PantryService } from "../core/pantry.service";
import { InventoryCorrectionComponent } from "./inventory-correction.component";
import { PantryLine, PantryLineGrouping, TransactionType } from "../model/pantryline";
import { InventoryTransactionComponent } from "./inventory-transaction.component";

@Component({
selector: 'pantry-list',
    templateUrl: 'pantry-list.component.html',
    styleUrls: ['pantry-list.component.css']
})
export class PantryListComponent implements OnInit {
    dataSource = new MatTableDataSource<PantryLineGrouping>();
    visibleColumns = ['name'];

    @ViewChild("inventoryTransaction")
    public transactionDialog: InventoryTransactionComponent;

    constructor(private _pantryService: PantryService) {}

    ngOnInit(): void {
        this.getInventory();
    }

    getInventory(): void {
        this._pantryService.getCurrentInventory().subscribe(recipeData => {
            this.dataSource.data = recipeData;
        });
    }

    addInventory(): void {
        this.transactionDialog.transactionMode = TransactionType.Addition;
        this.transactionDialog.LaunchDialog();
    }

    removeInventory(): void {
        this.transactionDialog.transactionMode = TransactionType.Usage;
        this.transactionDialog.LaunchDialog();
    }
}