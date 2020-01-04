import { Output, EventEmitter, Component } from "@angular/core";
import { FileUploader } from "ng2-file-upload";

@Component({
    selector: 'upload-text',
    templateUrl: 'txtupload.component.html'
})
export class TxtUploadComponent {
    @Output() onProcessFile: EventEmitter<any> = new EventEmitter();
    public uploader:FileUploader = new FileUploader(null);

    public onFileSelected(event: EventEmitter<File[]>) {
        const file: File = event[0];
    
        var component = this;
        this.readBase64(file)
          .then(function(data) {
              component.onProcessFile.emit(data);
        })
      }
    
      private readBase64(file): Promise<any> {
        var reader  = new FileReader();
        var future = new Promise((resolve, reject) => {
          reader.addEventListener("load", function () {
            resolve(reader.result);
          }, false);
    
          reader.addEventListener("error", function (event) {
            reject(event);
          }, false);
    
          reader.readAsText(file);
        });
        return future;
      }
}