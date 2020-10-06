import { OnInit, Component, Input, Output, EventEmitter } from '@angular/core';
import { MatTableDataSource }  from '@angular/material/table'
import { RecipeService } from '../core/recipe.service';
import { RecipeProduct } from '../model/recipe';

@Component({
    selector: 'unmatched-products',
    templateUrl: './list-item-unmatched.component.html',
    styleUrls: ['../controls/fancy-form.component.css']
})
export class ListItemUnmatchedComponent implements OnInit {
    public UnmatchedDataSource = new MatTableDataSource<RecipeProduct>();
    public VisibleColumns = [ 'name' ];

    @Output()
    public onReassigned: EventEmitter<RecipeProduct> = new EventEmitter<RecipeProduct>();
    
    @Input()
    public set UnmatchedItems(value: RecipeProduct[]) {
        this.UnmatchedDataSource.data = value;
    }

    constructor() { }

    ngOnInit(): void { }

    formatIngredient(ingredient: RecipeProduct): string {
        const boxed = new RecipeProduct();
        Object.assign(boxed, ingredient);
        return boxed.plainText;
    }
    
    reassignLine(line: RecipeProduct) {
        this.onReassigned.emit(line);
    }
}