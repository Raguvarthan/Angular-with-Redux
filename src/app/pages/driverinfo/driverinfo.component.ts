import { Component, OnInit, TemplateRef } from '@angular/core';
import { BsModalService } from 'ngx-bootstrap/modal';
import { BsModalRef } from 'ngx-bootstrap/modal/bs-modal-ref.service';

import { Driver } from './driver.model';
import { NgForm } from '@angular/forms';
import { DriverViewService } from '../../service/driverview.service';
import { IAppState } from '../../store/store';
import { NgRedux, select } from 'ng2-redux';
import * as Const from '../../store/actions';

@Component({
  selector: 'app-driverinfo',
  templateUrl: './driverinfo.component.html',
  styleUrls: ['./driverinfo.component.css']
})
export class DriverInfoComponent implements OnInit {
  driverData: Driver;  
  editdriverinfoView = false;
  modalRef: BsModalRef;
  formAction: string;
  @select('driverInfo') driverViewData;

  constructor(
    private ngRedux: NgRedux<IAppState>,
    private driver: DriverViewService,
    private modalService: BsModalService
  ) {
    this.driverData = new Driver();
  }

  ngOnInit() {
    this.driver.getDriver();
  }

  crudType(action, driver) {
    console.log(action);
    if (action === 'add') {
      this.formAction = 'Add';
      this.driverData = new Driver();
    } else {
      this.formAction = 'Edit';
      this.driverData = driver;
    }
  }
  insertDriver(driver) {
    this.driver.addDriver(driver);
  }
  updateDriver(driver) {
    this.driver.updateDriver(driver);
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
