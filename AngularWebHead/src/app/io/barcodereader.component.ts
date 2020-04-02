import { EventEmitter, Component, ViewChild, Output } from "@angular/core";
import { ZXingScannerModule, ZXingScannerComponent } from '@zxing/ngx-scanner';

@Component({
    selector: 'read-barcode',
    templateUrl: './barcodereader.component.html',
    styleUrls: ['./uploadfile-dialog.component.css']
})
export class BarcodeReaderComponent {
    public isVisible = false;
    public allDevices = [];
    public scannerEnabled: boolean = false;
    
    @ViewChild("barcodeReader") scanner: ZXingScannerComponent;

    @Output() onReadValue: EventEmitter<string> = new EventEmitter<string>();

    @Output() deviceName: string;

    ngAfterViewInit() {
        this.scanner.updateVideoInputDevices().then(devices => {
            this.allDevices = devices;
        });
    }
    
    public startScan(deviceIndex: number = 0) {
        this.isVisible = true;

        if(!this.scanner.device || this.scanner.device.deviceId != this.allDevices[deviceIndex].deviceId)
        {
            this.scanner.device = this.allDevices[deviceIndex];
        }

        this.deviceName = this.scanner.device.label;
        this.scanner.askForPermission().then(permission => {
            if(permission) {
                this.scannerEnabled = true;
            }
        });
    }

    public hideDialog()
    {
        this.scannerEnabled = false;
        this.isVisible = false;
    }

    ngOnDestroy() {
        this.scannerEnabled = false;
    }

    scanSuccessHandler(event) {
        this.onReadValue.emit(event);
        this.hideDialog();
    }
}