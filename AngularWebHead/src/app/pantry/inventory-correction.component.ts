import { Component, OnInit, Input, Output, EventEmitter } from "@angular/core";
import { PantryService } from "../core/pantry.service";
import { PantryLine, TransactionType } from "../model/pantryline";

@Component({
selector: 'inventory-correction',
    templateUrl: 'inventory-correction.component.html',
    styleUrls: ['../controls/dialog.component.css',
                '../controls/fancy-form.component.css']
})
export class InventoryCorrectionComponent implements OnInit {
    isVisible: boolean = false;
    private originalQuantity: string;
    private Line: PantryLine;

    @Output() onUpdate: EventEmitter<PantryLine> = new EventEmitter();

    @Input() set pantryLine(value: PantryLine) {
        this.Line = value;
        this.originalQuantity = value.quantity;
      }
      get pantryLine(): PantryLine {
        return this.Line;
      }

    constructor(private _pantryService: PantryService) {
    }

    ngOnInit(): void { }

    updateInventory(): PantryLine {
        //TODO: Convert string (fractions) to number like API does.
        var updateAmount = +this.originalQuantity - +this.Line.quantity;

        if(updateAmount == 0)
        {
            this.dismissDialog();
        }

        var adjustment = {
            unit: this.Line.unit,
            productId: this.Line.product.id,
            quantity: `${updateAmount}`,
            transactionType: TransactionType.SystemAdjustment,
            varietyId: this.Line.varietyId,
            size: this.Line.size,
        } as PantryLine;
        
        this._pantryService.updateInventory(adjustment).subscribe(lineItem => {
            this.onUpdate.emit(lineItem);
            this.dismissDialog();
        });

        return adjustment;
    }

    dismissDialog(): void {
        this.isVisible = false;
    }
}