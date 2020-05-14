import { Component, OnInit, ElementRef, ViewChildren, Input } from "@angular/core";

@Component({
    selector: 'my-tab-group',
    templateUrl: 'tab-group.component.html',
    styleUrls: ['tab-group.component.css']
})
export class TabGroupComponent implements OnInit {
    
    @Input()
    public selectedTab: number = 0;

    @Input()
    public Tabs: any[];

    @ViewChildren("tabLink")
    private tabLinks: ElementRef[] = [];

    ngOnInit() {}

    cbChecked(i) {
        return this.selectedTab === i;
    }

    tabClickHandler(evt, i) {
        this.selectedTab = evt.srcElement.checked ? i : -1;

        this.tabLinks.filter(cb => cb.nativeElement.checked && evt.srcElement.id !== cb.nativeElement.id)
                 .forEach(cb => cb.nativeElement.checked = false);
    }
}