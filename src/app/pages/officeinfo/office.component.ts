import { Component, OnInit, TemplateRef } from '@angular/core';
import { BsModalService } from 'ngx-bootstrap/modal';
import { BsModalRef } from 'ngx-bootstrap/modal/bs-modal-ref.service';
import { IAppState } from '../../store/store';
import { NgRedux, select } from 'ng2-redux';
import * as Const from '../../store/actions';
import { Office } from './office.model';
import { NgForm } from '@angular/forms';
import { OfficeViewService } from '../../service/officeview.service';


@Component({
  selector: 'app-office',
  templateUrl: './office.component.html',
  styleUrls: ['./office.component.css']
})
export class OfficeInfoComponent implements OnInit {

  officeData: Office;
  editofficeinfoView = false;
  modalRef: BsModalRef;
  formAction: string;
  @select('officeInfo') officeViewData;
  constructor(
    private ngRedux: NgRedux<IAppState>,
    private data: OfficeViewService,
    private modalService: BsModalService
  ) {
    this.officeData = new Office();
   this.ngRedux.subscribe(() => {
    console.log(this.ngRedux.getState());
    })
 }

  ngOnInit() {
    this.listoffice();

  }

  listoffice() {
    this.data.getOffice();
  }

  crudType(action, office) {
    console.log(action);
    if (action === 'add') {
      this.formAction = 'Add';
      this.officeData = new Office();
    } else {
      this.formAction = 'Edit';
      this.officeData = office;
    }
  }

  insertoffice(office) {
    this.data.addOffice(office);
  }

  updateoffice(office) {
    this.data.updateOffice(office);
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
