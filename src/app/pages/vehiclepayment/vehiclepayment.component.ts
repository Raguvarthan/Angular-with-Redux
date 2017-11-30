import { Component, OnInit } from '@angular/core';
import { VehicleAmountService } from '../../service/vehicleamount.service';
import * as _ from 'underscore';

@Component({
  selector: 'app-vehiclepayment',
  templateUrl: './vehiclepayment.component.html',
  styleUrls: ['./vehiclepayment.component.css']
})
export class VehiclePaymentComponent implements OnInit {
 
  vehiclePaymentViewData: any;
  
  constructor(
    private vehicleAmount :  VehicleAmountService
  ) { }

  ngOnInit() {
    this.getVendorAmountList();
  }
  
  getVendorAmountList(){
    this.vehicleAmount.getVechileamount()
    .subscribe((res)=>{
      this.vehiclePaymentViewData = res;
      _.each(this.vehiclePaymentViewData,(t: any)=>{
        t.totalAmt = 0;
        t.pendingAmt = 0;
        _.each(t.tripData,(amt: any)=>{
          t.totalAmt = t.totalAmt + amt.totalAmt;
          t.pendingAmt = t.pendingAmt + amt.pendingAmt;
        });        
      })
    })
  }
}
