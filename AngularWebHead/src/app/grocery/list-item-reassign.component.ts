import { OnInit, Component } from '@angular/core';
import { RecipeService } from '../core/recipe.service';

@Component({
    selector: 'reassign-products',
    templateUrl: './list-item-reassign.component.html',
    styleUrls: ['../controls/fancy-form.component.css']
})
export class ListItemReassignComponent implements OnInit {
    //public UnmatchedDataSource = new MatTableDataSource<Ingredient>();
    //public VisibleColumns = [ 'name' ];

    /*@Input()
    public set UnmatchedItems(value: Ingredient[]) {
        this.UnmatchedDataSource.data = value;
    }*/

    constructor(private recipeService: RecipeService) { }

    ngOnInit(): void { }
}