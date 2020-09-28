import { OnInit, Component, Input, Output, EventEmitter } from '@angular/core';
import { MatTableDataSource }  from '@angular/material/table'
import { RecipeService } from '../core/recipe.service';
import { RecipeProduct } from '../model/recipe';

@Component({
    selector: 'ignored-products',
    templateUrl: './list-item-ignored.component.html',
    styleUrls: ['../controls/fancy-form.component.css']
})
export class ListItemIgnoredComponent implements OnInit {
    public IgnoredDataSource = new MatTableDataSource<RecipeProduct>();
    public VisibleColumns = [ 'name' ];

    @Output()
    public onReassigned: EventEmitter<RecipeProduct> = new EventEmitter<RecipeProduct>();
    
    @Input()
    public set IgnoredItems(value: RecipeProduct[]) {
        this.IgnoredDataSource.data = value;
    }

    constructor(private recipeService: RecipeService) { }

    ngOnInit(): void { }
    
    reassignLine(line: RecipeProduct) {
        this.onReassigned.emit(line);
    }
}