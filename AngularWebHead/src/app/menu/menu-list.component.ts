import { Component, OnInit, ViewChild, AfterViewInit } from "@angular/core";
import { DateRangeComponent } from "../controls/date-range.component";
import { MatTableDataSource } from "@angular/material/table";
import { MenuGroup, MenuEntry } from "../model/menu";
import { MenuService } from "../core/menu.service";

@Component({
    selector: 'menu-list',
    templateUrl: './menu-list.component.html',
    styleUrls: ['../grocery/list-view.component.css']
})
export class MenuListComponent implements OnInit, AfterViewInit {
    private _numDays = 14;
    public TableData = new MatTableDataSource<MenuGroup>();
    private _startDate: Date;
    private _endDate: Date;

    private get Days(): MenuGroup[] {
        let days = [];
        for(let i = 1; i <= this._numDays; i++) {
            let date = this.addDays(this._startDate, i);
            days.push({
                date: date,
                entries: this.filterByDate(date)
            } as MenuGroup);
        }
        return days;
    }

    private _menuItems: MenuEntry[] = [];

    public get VisibleColumns(): string[] {
        return ['stuff'];
    };

    public editMode: boolean[] = [false, false, false, false, false];

    @ViewChild('calendarDates') calendarDates: DateRangeComponent;

    constructor(private _menuService: MenuService) {}

    ngOnInit(): void {
        this._startDate = new Date();
        this._endDate = this.addDays(new Date(), this._numDays);
    }

    public toggleEditMode(i: number, enabled: boolean) {
        this.editMode[i] = enabled;
    }

    public filterByDate(date: Date) {
        return this._menuItems.filter(item => 
            {
                return item.date.getFullYear() === date.getFullYear() &&
                       item.date.getMonth() === date.getMonth() &&
                       item.date.getDate() === date.getDate();
            });
    }

    ngAfterViewInit(): void {
        setTimeout(() => {
            this.calendarDates.startDate = this._startDate;
            this.calendarDates.endDate = this._endDate;

            this._menuService.getMenu(this._startDate.toLocaleDateString("en-US"), this._endDate.toLocaleDateString("en-US"))
                .subscribe(menuItems => {
                    for(let thing of menuItems) {
                        thing.date = new Date(thing.date);
                    }

                    this._menuItems = menuItems;
                    this.TableData.data = this.Days;
                }, error => {
                    // TODO: Do some error handling stuff.
                }, () => {
                    // TODO: Do some finally stuff.
                });
        });
    }
    
    public addMeal(i: number) {
        let newItem = {} as MenuEntry;
        Object.assign(newItem, this._menuItems[0]);
        newItem.date = this.TableData.data[i].date;
        this.TableData.data[i].entries.push(newItem);
    }

    public removeMeal(i: number, meal: MenuEntry) {
        if(!confirm(`This will delete ${meal.recipeName} from your menu.`)) {
            return;
        }
        this._menuService.deleteEntry(meal.id).subscribe(_ => {
            let index = this.TableData.data[i].entries.findIndex(p => p === meal);
            this.TableData.data[i].entries.splice(index, 1);
        }, error => {}, () => {});
    }

    private addDays(date: Date, days: number): Date {
        let copy = new Date();
        copy.setDate(date.getDate() + days);
        return copy;
    }

    public dateRangeChanged(evt: object) {
        // TODO: Handle range changes here.
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