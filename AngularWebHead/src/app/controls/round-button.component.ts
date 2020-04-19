import { Component, Input, OnInit } from "@angular/core";

@Component({
    selector: 'round-button',
    templateUrl: 'round-button.component.html',
    styleUrls: ['round-button.component.css']
})
export class RoundButtonComponent implements OnInit {
    @Input() float: string;

    @Input() altText : string;

    @Input() buttonText : string;

    ngOnInit() {
    }
}