import { Input, Output, EventEmitter, Component } from "@angular/core";

@Component({
    selector: 'texteditor-dialog',
    templateUrl: 'texteditor-dialog.component.html',
    styleUrls: ['./uploadfile-dialog.component.css']
})
export class TextEditorDialogComponent {
    @Output() onTextUpdated: EventEmitter<any> = new EventEmitter<any>();
    
    @Input() rawText: string;
    public isVisible: boolean = false;

    private reprocessText(txt) {
        this.onTextUpdated.emit(txt);
    }

    private cancelDialog() {
        this.isVisible = false;
    }
}