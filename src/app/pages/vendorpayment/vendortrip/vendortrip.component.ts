import { Component, OnInit, Input, TemplateRef } from '@angular/core';
import { BsModalService } from 'ngx-bootstrap/modal';
import { BsModalRef } from 'ngx-bootstrap/modal/bs-modal-ref.service';

@Component({
  selector: 'app-vendortrip',
  templateUrl: './vendortrip.component.html',
  styleUrls: ['./vendortrip.component.css']
})
export class VendorTripComponent implements OnInit {
  @Input() tripData;
  @Input() itemIndex;
  modalRef: BsModalRef;
  constructor(
    private modalService: BsModalService
  ) { }

  ngOnInit() {
  }
  openModal(template: TemplateRef<any>) {
    console.log('click modal');
    this.modalRef = this.modalService.show(template);
  }
}
