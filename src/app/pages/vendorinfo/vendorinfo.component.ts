import { Component, OnInit, TemplateRef } from '@angular/core';
import { BsModalService } from 'ngx-bootstrap/modal';
import { BsModalRef } from 'ngx-bootstrap/modal/bs-modal-ref.service';

import { Vendor } from './Vendor.model';
import { NgForm } from '@angular/forms';
import { VendorViewService } from '../../service/vendorview.service';
import { IAppState } from '../../store/store';
import { NgRedux, select } from 'ng2-redux';
import * as Const from '../../store/actions';

@Component({
  selector: 'app-vendorinfo',
  templateUrl: './vendorinfo.component.html',
  styleUrls: ['./vendorinfo.component.css']
})
export class VendorInfoComponent implements OnInit {
  vendorData: Vendor;  
  editVendorinfoView = false;
  modalRef: BsModalRef;
  formAction: string;
  @select('vendorInfo') vendorViewData;

  constructor(
    private ngRedux: NgRedux<IAppState>,
    private vendor: VendorViewService,
    private modalService: BsModalService
  ) {
    this.vendorData = new Vendor();
  }

  ngOnInit() {
    this.vendor.getVendor();
  }

  crudType(action, vendor) {
    console.log(action);
    if (action === 'add') {
      this.formAction = 'Add';
      this.vendorData = new Vendor();
    } else {
      this.formAction = 'Edit';
      this.vendorData = vendor;
    }
  }
  insertVendor(vendor) {
    this.vendor.addVendor(vendor);
  }
  updateVendor(vendor) {
    this.vendor.updateVendor(vendor);
  }
  openModal(template: TemplateRef<any>) {
    this.modalRef = this.modalService.show(template);
    this.ngRedux.subscribe(() => {    
    if( this.ngRedux.getState().modal == Const.UPDATED_CLOSE_MODAL) {
      this.modalRef.hide();      
    }  
    })
  }
}
