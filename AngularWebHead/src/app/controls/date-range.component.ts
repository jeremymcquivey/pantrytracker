import { OnInit, Component, Input, ViewChild, AfterViewInit } from "@angular/core";

@Component({
    selector: 'date-range',
    templateUrl: './date-range.component.html',
    styleUrls: []
})
export class DateRangeComponent {
    
    public startDate: Date = new Date();

    public endDate: Date = new Date();

    public formatDate(date: Date): string {
        return date.toLocaleDateString("en-US");
    }
}