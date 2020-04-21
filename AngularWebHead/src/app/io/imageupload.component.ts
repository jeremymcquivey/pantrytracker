import { Output, EventEmitter, Component } from "@angular/core";
import { FileUploader } from "ng2-file-upload";

@Component({
    selector: 'upload-image',
    templateUrl: 'imageupload.component.html',
    styleUrls: ['../controls/fancy-form.component.css']
})
export class ImageUploadComponent {
    @Output() onProcessImage: EventEmitter<any> = new EventEmitter();
    public uploader:FileUploader = new FileUploader(null);

    public onImageSelected(event: EventEmitter<File[]>) {
        const file: File = event[0];
    
        var component = this;
        this.readBase64Image(file)
          .then(function(img) {
            component.onProcessImage.emit(img);
        });
      }
  
      private readBase64Image(file): Promise<any> {
        var reader  = new FileReader();
        var future = new Promise((resolve, reject) => {
          reader.addEventListener("load", function () {
            resolve(reader.result);
          }, false);
    
          reader.addEventListener("error", function (event) {
            reject(event);
          }, false);
    
          reader.readAsDataURL(file);
        });
        return future;
      }
}