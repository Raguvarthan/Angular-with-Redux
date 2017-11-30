import { Component, OnInit, Input, TemplateRef } from '@angular/core';
import { BsModalService } from 'ngx-bootstrap/modal';
import { BsModalRef } from 'ngx-bootstrap/modal/bs-modal-ref.service';

@Component({
  selector: 'app-vehicletrip',
  templateUrl: './vehicletrip.component.html',
  styleUrls: ['./vehicletrip.component.css']
})
export class VehicleTripComponent implements OnInit {
@Input() tripData;
@Input() itemIndex;
modalRef: BsModalRef;

log(event: boolean) {
  console.log(`Accordion has been ${event ? 'opened' : 'closed'}`);
}
  constructor(
    private modalService: BsModalService
  ) { }

  ngOnInit() {
    console.log(this.tripData)
  }

  openModal(template: TemplateRef<any>) {
    console.log('click modal');
    this.modalRef = this.modalService.show(template);
  }

}
