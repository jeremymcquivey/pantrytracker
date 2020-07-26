import { OnInit, Component, Input, ViewChild, AfterViewInit } from "@angular/core";

@Component({
    selector: 'date-range',
    templateUrl: './date-range.component.html',
    styleUrls: []
})
export class DateRangeComponent {
    @Input()
    public startDate: Date = new Date();

    @Input()
    public endDate: Date = new Date();

    public formatDate(date: Date): string {
        return date.toLocaleDateString("en-US");
    }
}