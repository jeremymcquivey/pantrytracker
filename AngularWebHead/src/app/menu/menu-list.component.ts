import { Component, OnInit, ViewChild, AfterViewInit } from "@angular/core";
import { DateRangeComponent } from "../controls/date-range.component";
import { MatTableDataSource } from "@angular/material/table";
import { MenuGroup, MenuEntry } from "../model/menu";

@Component({
    selector: 'menu-list',
    templateUrl: './menu-list.component.html',
    styleUrls: ['../grocery/list-view.component.css']
})
export class MenuListComponent implements OnInit, AfterViewInit {
    private numDays = 14;
    public TableData = new MatTableDataSource<MenuGroup>();

    public get Days(): MenuGroup[] {
        let days = [];
        for(let i = 1; i <= this.numDays; i++) {
            let date = this.addDays(this.startDate, i);
            days.push({
                date: date,
                entries: this.filterByDate(date)
            } as MenuGroup);
        }
        return days;
    }

    public get MenuItems(): MenuEntry[] {
        return [
            {recipeName: 'Cheese & Crackers', date: new Date('7/26/2020')} as MenuEntry,
            {recipeName: 'Pot Stickers', date: new Date('7/27/2020')} as MenuEntry,
            {recipeName: 'Stuffing', date: new Date('7/27/2020')} as MenuEntry,
            {recipeName: 'Turkey and Ham', date: new Date('7/28/2020')} as MenuEntry,
            {recipeName: 'Other Stuff', date: new Date('7/29/2020')} as MenuEntry
        ];
    }

    public get startDate(): Date {
        return new Date();
    }

    public get endDate(): Date {
        return this.addDays(this.startDate, this.numDays);
    }

    public get VisibleColumns(): string[] {
        return ['stuff'];
    };

    @ViewChild('calendarDates') calendarDates: DateRangeComponent;

    ngOnInit(): void {

    }

    public filterByDate(date: Date) {

        return this.MenuItems.filter(item => 
            {
                //console.log('date comparison', {date1: item.date, date2: date, comp: item.date == date});
                return item.date.getFullYear() === date.getFullYear() &&
                       item.date.getMonth() === date.getMonth() &&
                       item.date.getDate() === date.getDate();
            });
    }

    ngAfterViewInit(): void {
        this.calendarDates.startDate = this.startDate;
        this.calendarDates.endDate = this.endDate;

        this.TableData.data = this.Days;
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