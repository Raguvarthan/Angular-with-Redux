import { Injectable, Component } from '@angular/core';
import { Headers, Http, Response, RequestOptions } from '@angular/http';
import { Observable } from 'rxjs/Rx';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import { HttpClient } from "@angular/common/http";
import { NgRedux } from 'ng2-redux';
import { IAppState } from '../store/store';
import * as Const from '../store/actions';
import * as Conf from '../config/configuration';
import { ApiService } from "./api.service";

@Injectable()


export class VendorViewService {
  baseUrl = 'http://localhost:3001/';
  constructor(
    private apiService: ApiService,
    private httpClient: HttpClient,
    private ngRedux: NgRedux<IAppState>
  ) {

  }

  getVendor() {
    this.apiService.get('', { useAuth: false }, Conf.apiUrl.serverUrl + 'Vendor').then(
      (response: any) => {
        if (response.code == '200') {
          this.ngRedux.dispatch({ type: Const.FETCH_ALL_VENDORS_SUCCESS, vendorInfo: response.data });
        } else {
          throw response.error;
        }
      })
      .catch((error: any) => {
        this.ngRedux.dispatch({ type: Const.FETCH_ALL_VENDORS_ERROR });
      });
  }

  updateVendor(vendor) {
    this.apiService.put('Vendor/' + 'Sample/' + vendor.vendorId, vendor,
      { useAuth: false }, undefined).then(
      (response: any) => {
        console.log(response);
        if (response.code === '200') {
          this.ngRedux.dispatch({ type: Const.UPDATE_VENDOR_SUCCESS, vendorInfo: vendor });
        } else {
          throw response.error;
        }
      })
      .catch(
      (error: any) => {
        this.ngRedux.dispatch({ type: Const.UPDATE_VENDOR_SUCCESS });
      }
      );
  }

  addVendor(vendor) {
    // this.httpClient.post(this.baseUrl + 'vendor', vendor)
    //   .subscribe((updated) => {
    //     this.ngRedux.dispatch({ type: Const.ADD_VENDOR_SUCCESS, vendorInfo: updated });
    //   }, (err) => {
    //     this.ngRedux.dispatch({ type: Const.ADD_VENDOR_ERROR });
    //   });
      delete vendor.id;
      delete vendor.vendorId;
      this.apiService.post('Vendor/' + 'Sample', vendor,
            { useAuth: false }, undefined).then(
            (response: any) => {
              console.log(response);
              if (response.code === '200') {
                this.ngRedux.dispatch({ type: Const.ADD_VENDOR_SUCCESS, vendorInfo: response.data });
              } else {
                throw response.error;
              }
            })
            .catch(
            (error: any) => {
              this.ngRedux.dispatch({ type: Const.ADD_VENDOR_ERROR });
            }
            );
  }


}
