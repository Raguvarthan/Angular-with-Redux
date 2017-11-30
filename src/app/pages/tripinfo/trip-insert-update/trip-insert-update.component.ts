import { Component, OnInit, DoCheck, Input, Output, EventEmitter } from '@angular/core';
import { LocationService } from '../../../service/location.service';
import { VendorViewService } from '../../../service/vendorview.service';
import { VehicleViewService } from '../../../service/vehicleview.service';
import { Trip, Advance, Location } from './trip.model';
import { NgForm } from '@angular/forms';
import * as _ from 'underscore';

import { NgRedux, select } from 'ng2-redux';
import { IAppState } from '../../../store/store';
import * as Const from '../../../store/actions';
import { TripService } from "../../../service/trip.service";

@Component({
  selector: 'app-trip-insert-update',
  templateUrl: './trip-insert-update.component.html',
  styleUrls: ['./trip-insert-update.component.css']
})
export class TripInsertUpdateComponent implements OnInit, DoCheck {
//  @select(vehicleInfo => { vehicleInfo.vehicleNo}) vehicleData;

  @Input() public tripData: any = { location: {}, officeInfo: {}};

  @Input() public actionType: string;

  @Input()  public vendorData: any[];
  @Input() public vehicleData: any[];
  @Input() public cityLocation: any[];
  @Output() cancelClicked = new EventEmitter<boolean>();
  constructor(
    private ngRedux: NgRedux<IAppState>,
    private tripService: TripService
  ) {
  //  this.tripData = new Trip();    
   }

   ngOnInit(){
   }

   ngDoCheck() {
    this.tripData.totalAmount = this.tripData.ratePerTon * this.tripData.noOfTons;
    this.tripData.vehicleAmount = (this.tripData.ratePerTon - this.tripData.crossing) * this.tripData.noOfTons;
    this.tripData.paidAmount = this.tripData.driverAcceptedAmount + this.tripData.selfAmount;
    this.tripData.balanceAmount = this.tripData.vehicleAmount - this.tripData.paidAmount;
   }

   typofPay() {
    console.log(this.tripData.typeofPayment);
  }

  onSubmit(form: NgForm) {
    console.log(form);
    this.ngRedux.subscribe(() => {
    if( this.ngRedux.getState().modal == Const.UPDATED_CLOSE_MODAL) {
      this.cancelClicked.emit(true);
    }
    })
    if(this.actionType == 'update') {
    this.tripService.updateTrip(form);
     } else if(this.actionType == 'add')  {
    this.tripService.addTrip(form);
    }
  }
  closeModal(event: any) {
   this.cancelClicked.emit(true);
  }
}
