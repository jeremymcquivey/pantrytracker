import { OnInit, Component, Input } from '@angular/core';
import { MatTableDataSource }  from '@angular/material/table'
import { RecipeService } from '../core/recipe.service';
import { Ingredient } from '../model/ingredient';

@Component({
    selector: 'unmatched-products',
    templateUrl: './list-item-unmatched.component.html',
    styleUrls: ['../controls/fancy-form.component.css']
})
export class ListItemUnmatchedComponent implements OnInit {
    public UnmatchedDataSource = new MatTableDataSource<Ingredient>();
    public VisibleColumns = [ 'name' ];

    @Input()
    public set UnmatchedItems(value: Ingredient[]) {
        this.UnmatchedDataSource.data = value;
    }

    constructor(private recipeService: RecipeService) { }

    ngOnInit(): void { }
}