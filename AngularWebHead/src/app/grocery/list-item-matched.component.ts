import { OnInit, Component, Input } from '@angular/core';
import { MatTableDataSource }  from '@angular/material/table'
import { RecipeService } from '../core/recipe.service';
import { PantryLine } from '../model/pantryline';

@Component({
    selector: 'matched-products',
    templateUrl: './list-item-matched.component.html',
    styleUrls: ['../controls/fancy-form.component.css']
})
export class ListItemMatchedComponent implements OnInit {
    public MatchedDataSource = new MatTableDataSource<PantryLine>();
    public VisibleColumns = [ 'name' ];

    @Input()
    public set MatchedItems(value: PantryLine[]) {
        this.MatchedDataSource.data = value;
    }

    constructor(private recipeService: RecipeService) { }

    ngOnInit(): void { }
}