import { Component, OnInit } from '@angular/core';
import * as _ from 'underscore';
import { VendorAmountService } from '../../service/vendoramount.service';

@Component({
  selector: 'app-vendorpayment',
  templateUrl: './vendorpayment.component.html',
  styleUrls: ['./vendorpayment.component.css']
})
export class VendorPaymentComponent implements OnInit {

  vendorPaymentViewData: any;
  
  constructor(
    private vendorAmount :  VendorAmountService
  ) { }

  ngOnInit() {
    this.getVendorAmountList();
  }
  
  getVendorAmountList(){
    this.vendorAmount.getVendoramount()
    .subscribe((res)=>{
      this.vendorPaymentViewData = res;
      _.each(this.vendorPaymentViewData,(t: any)=>{
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
