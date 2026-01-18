import { Injectable } from '@angular/core';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';

@Injectable({
  providedIn: 'root',
})
export class Laoding {
  loadingRequestCount = 0;
  constructor(private spinner: NgxSpinnerService) {}
  loading() {
    this.loadingRequestCount++;
    this.spinner.show();
  }
  idle() {
    this.loadingRequestCount--;
    if (this.loadingRequestCount <= 0) {
      this.loadingRequestCount = 0;
      this.spinner.hide();
    }
    }
}
