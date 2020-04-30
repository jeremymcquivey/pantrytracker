import { EventEmitter, Component, ViewChild, Output } from "@angular/core";
import { ZXingScannerComponent } from '@zxing/ngx-scanner';

@Component({
    selector: 'read-barcode',
    templateUrl: './barcodereader.component.html',
    styleUrls: ['./barcodereader.component.css',
                '../controls/dialog.component.css',
                '../controls/fancy-form.component.css']
})
export class BarcodeReaderComponent {
    private _selectedDevice: MediaDeviceInfo = null;

    public allDevices: MediaDeviceInfo[] = [];
    public isVisible: boolean = false;
    public scannerEnabled: boolean = false;

    set SelectedDevice(value: MediaDeviceInfo) {
        this._selectedDevice = value;
        this.playDevice(value);
    } get SelectedDevice(): MediaDeviceInfo {
        return this._selectedDevice;
    }
    
    @ViewChild("barcodeReader") scanner: ZXingScannerComponent;

    @Output() onReadValue: EventEmitter<string> = new EventEmitter<string>();

    get deviceName(): string {
        return this.SelectedDevice?.label;
    };

    ngAfterViewInit() {
        this.scanner.updateVideoInputDevices().then(devices => {
            this.allDevices = devices;
        });
    }
    
    public startScan() {
        this.isVisible = true;

        if(this.allDevices.length > 0) {
            this.SelectedDevice = this.allDevices[0];
        }
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

    private playDevice(device: MediaDeviceInfo) {
        if(device) {
            if(!this.scanner.device || this.scanner.device.deviceId != device.deviceId)
            {
                this.scanner.device = device;
            }

            this.scanner.askForPermission().then(permission => {
                if(permission) {
                    this.scannerEnabled = true;
                }
            });
        } 
        else {
            this.scanner.device = null;
            this.scannerEnabled = false;
        }
    }
}