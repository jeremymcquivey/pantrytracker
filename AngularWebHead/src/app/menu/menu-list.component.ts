import { Component, OnInit, ViewChild, AfterViewInit } from "@angular/core";
import { DateRangeComponent } from "../controls/date-range.component";
import { MatTableDataSource } from "@angular/material/table";
import { MenuGroup, MenuEntry } from "../model/menu";
import { MenuService } from "../core/menu.service";
import { Recipe } from "../model/recipe";
import { RecipeListDialogComponent } from "../recipes/recipe-list-dialog.component";

@Component({
    selector: 'menu-list',
    templateUrl: './menu-list.component.html',
    styleUrls: ['../grocery/list-view.component.css']
})
export class MenuListComponent implements OnInit, AfterViewInit {
    private _defaultNumDays = 14;
    public TableData = new MatTableDataSource<MenuGroup>();
    private _startDate: Date;
    private _endDate: Date;
    private _selectedDayForAdding: number;

    private _menuItems: MenuEntry[] = [];

    public get VisibleColumns(): string[] {
        return ['stuff'];
    };

    public editMode: boolean[] = [false, false, false, false, false];

    @ViewChild('calendarDates') calendarDates: DateRangeComponent;

    @ViewChild('recipeList') recipeList: RecipeListDialogComponent;

    constructor(private _menuService: MenuService) {}

    ngOnInit(): void {
        this._startDate = this.addDays(new Date(), 0);
        this._endDate = this.addDays(new Date(), this._defaultNumDays);
    }

    public toggleEditMode(i: number, enabled: boolean) {
        this.editMode[i] = enabled;
    }

    ngAfterViewInit(): void {
        setTimeout(() => {
            this.calendarDates.startDate = this._startDate;
            this.calendarDates.endDate = this._endDate;

            this.refreshData();
        });
    }

    private refreshData() {
        this._menuService.getMenu(this._startDate.toLocaleDateString("en-US"), this._endDate.toLocaleDateString("en-US"))
            .subscribe(menuItems => {
                this._menuService.setDateObject(menuItems);
                this.TableData.data = menuItems;
            }, error => {
                // TODO: Do some error handling stuff.
            }, () => {
                // TODO: Do some finally stuff.
            });
    }

    public recipeSelected(recipe: Recipe) {
        let newItem = {
            id: 0,
            date: this.TableData.data[this._selectedDayForAdding].key,
            recipeId: recipe.id
        } as MenuEntry;

        this._menuService.addEntry(newItem).subscribe(meal => {
            meal.recipeName = recipe.title;
            this.TableData.data[this._selectedDayForAdding].value.push(meal);
        }, error => {}, () => {});
    }
    
    public addMeal(i: number) {
        this._selectedDayForAdding = i;
        this.recipeList.isVisible = true;
    }

    public removeMeal(i: number, meal: MenuEntry) {
        if(!confirm(`This will delete ${meal.recipeName} from your menu.`)) {
            return;
        }

        this._menuService.deleteEntry(meal.id).subscribe(_ => {
            let index = this.TableData.data[i].value.findIndex(p => p.id === meal.id);
            this.TableData.data[i].value.splice(index, 1);
        }, error => {}, () => {});
    }

    private addDays(date: Date, days: number): Date {
        let copy = new Date();
        copy.setDate(date.getDate() + days);
        return copy;
    }

    public dateRangeChanged(evt: any) {
        this._startDate = evt.start;
        this._endDate = evt.end;
        this.refreshData();
    }

    public formatLongDate(date: Date): string {
        var options = {   
            day: 'numeric',
            month: 'long', 
            year: 'numeric',
            weekday: 'long'
        }

        return date.toLocaleDateString("en-US", options);
    }
}