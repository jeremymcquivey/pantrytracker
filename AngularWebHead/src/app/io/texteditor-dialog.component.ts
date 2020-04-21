import { Input, Output, EventEmitter, Component, ViewChild, ElementRef } from "@angular/core";

@Component({
    selector: 'texteditor-dialog',
    templateUrl: 'texteditor-dialog.component.html',
    styleUrls: ['../controls/dialog.component.css',
                '../controls/fancy-form.component.css']
})
export class TextEditorDialogComponent {
    private _isVisible: boolean = false;

    @Output() onTextUpdated: EventEmitter<any> = new EventEmitter<any>();
    
    @Input() rawText: string;
    set isVisible(value: boolean) {
        this._isVisible = value;
        setTimeout(() => { 
            this.autoFocusElement.nativeElement.focus();
        }, 0);
        
    } get isVisible(): boolean {
        return this._isVisible;
    }

    @ViewChild('textArea')
    private autoFocusElement: ElementRef;

    public showTips: boolean = false;

    public tips: string[] = [
        'The first line is the recipe\'s name',
        'Place each ingredient and direction on a new line.',
        'Place the word "Ingredients" on a new line above your ingredients.',
        'Place the word "Directions" on a new line above your directions.'
    ];

    private reprocessText(txt) {
        this.onTextUpdated.emit(txt);
    }

    private cancelDialog() {
        this.isVisible = false;
    }
}