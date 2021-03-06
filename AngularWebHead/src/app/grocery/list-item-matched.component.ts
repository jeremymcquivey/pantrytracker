import { OnInit, Component, Input, Output, EventEmitter } from '@angular/core';
import { MatTableDataSource }  from '@angular/material/table'
import { RecipeProduct } from '../model/recipe';

@Component({
    selector: 'matched-products',
    templateUrl: './list-item-matched.component.html',
    styleUrls: ['../controls/fancy-form.component.css']
})
export class ListItemMatchedComponent implements OnInit {
    public MatchedDataSource = new MatTableDataSource<RecipeProduct>();
    public VisibleColumns = [ 'name' ];

    @Output()
    public onReassigned: EventEmitter<RecipeProduct> = new EventEmitter<RecipeProduct>();

    @Input()
    public set MatchedItems(value: RecipeProduct[]) {
        this.MatchedDataSource.data = value;
    }

    constructor() { }

    ngOnInit(): void { }

    reassignLine(line: RecipeProduct) {
        this.onReassigned.emit(line);
    }

    formatIngredient(ingredient: RecipeProduct): string {
        const boxed = new RecipeProduct();
        Object.assign(boxed, ingredient);
        return boxed.fullDescription;
    }
}