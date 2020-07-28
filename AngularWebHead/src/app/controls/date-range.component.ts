import { OnInit, Component, Input, ViewChild, AfterViewInit, Output, EventEmitter } from "@angular/core";

@Component({
    selector: 'date-range',
    templateUrl: './date-range.component.html',
    styleUrls: []
})
export class DateRangeComponent {
    
    public startDate: Date = new Date();

    public endDate: Date = new Date();

    @Output() rangeUpdated: EventEmitter<any> = new EventEmitter();

    public dateRangeUpdated($event: any, type: string) {
        let newDate = new Date($event.srcElement.value);
        switch(type) {
            case 'start':
                this.startDate = newDate;
                this.rangeUpdated.emit({start: this.startDate, end: this.endDate});
                break;
            case 'end':
                this.endDate = newDate;
                this.rangeUpdated.emit({start: this.startDate, end: this.endDate});
                break;
        }
    }

    public formatHtmlDate(date: Date): string {
        return date.toISOString().split('T')[0];
    }

    public formatDate(date: Date): string {
        return date?.toLocaleDateString("en-US") || 'Unknown';
    }
}