import { Injectable, Component } from '@angular/core';
import { Headers, Http, Response, RequestOptions } from '@angular/http';
import { Observable } from 'rxjs/Rx';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import { HttpClient } from "@angular/common/http";



@Injectable()


export class VendorAmountService {  
  baseUrl = 'http://localhost:3001/';
  constructor(
    
    private httpClient: HttpClient,
    
  ) {
    
  }

  getVendoramount() {
    return this.httpClient.get(this.baseUrl+'vendorPayment')
      .map((res) => res)
      .catch((error: any) => Observable.throw(error));
  }

  getSingleVendoramount(id) {
    return this.httpClient.get(this.baseUrl+'vendorPayment/'+id)
    .map((sRes)=> sRes)
    .catch((error: any) => Observable.throw(error));
  }
  
}
