import { Output, EventEmitter, Component } from "@angular/core";

@Component({
    selector: 'upload-file-dialog',
    templateUrl: 'uploadfile-dialog.component.html',
    styleUrls: ['./uploadfile-dialog.component.css']
})
export class UploadFileDialogComponent {
    @Output() onImageSelected: EventEmitter<any> = new EventEmitter<any>();
    @Output() onTextSelected: EventEmitter<any> = new EventEmitter<any>();
    @Output() onCancelSelected: EventEmitter<any> = new EventEmitter<any>();

    private processImage(img) {
        this.onImageSelected.emit(img);
    }

    private processText(data) {
        this.onTextSelected.emit(data);
    }

    private cancelDialog() {
        this.onCancelSelected.emit();
    }
}