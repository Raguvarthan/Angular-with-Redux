import { Injectable, Component } from '@angular/core';
import { Headers, Http, Response, RequestOptions } from '@angular/http';
import { Observable } from 'rxjs/Rx';
import { NgRedux } from 'ng2-redux';
import { IAppState } from '../store/store';
import * as Const from '../store/actions';
import * as Conf from '../config/configuration';

import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import { HttpClient } from "@angular/common/http";
import { ApiService } from "./api.service";

@Injectable()


export class DriverViewService {  
  baseUrl = 'http://localhost:3001/';
  constructor(
    private apiService: ApiService,
    private httpClient: HttpClient,
    private ngRedux: NgRedux<IAppState>
  ) {
    
  }

  getDriver() {
    this.apiService.get('', { useAuth: false }, Conf.apiUrl.serverUrl +'Driver').then(
      (response: any) => {        
        if(response.code == '200') {
          this.ngRedux.dispatch({ type: Const.FETCH_ALL_DRIVER_SUCCESS, driverInfo: response.data });
        } else {
          throw response.error;
        }        
      })
      .catch((error: any) => {
      this.ngRedux.dispatch({ type: Const.FETCH_ALL_DRIVER_ERROR});
      });
  }

  updateDriver(driver) {
    this.apiService.put('Driver/' + 'Sample/' + driver.driverId, driver,
      { useAuth: false }, undefined).then(
      (response: any) => {
        console.log(response);
        if (response.code === '200') {
          this.ngRedux.dispatch({ type: Const.UPDATE_DRIVER_SUCCESS, driverInfo: driver });
        } else {
          throw response.error;
        }
      })
      .catch(
      (error: any) => {
        this.ngRedux.dispatch({ type: Const.UPDATE_DRIVER_ERROR });
      }
      );
  }
  addDriver(driver) {
      delete driver.id;
      delete driver.driverId;
      this.apiService.post('Driver/' + 'Sample', driver,
            { useAuth: false }, undefined).then(
            (response: any) => {
              console.log(response);
              if (response.code === '200') {
                this.ngRedux.dispatch({ type: Const.ADD_DRIVER_SUCCESS, driverInfo: response.data });
              } else {
                throw response.error;
              }
            })
            .catch(
            (error: any) => {
              this.ngRedux.dispatch({ type: Const.ADD_DRIVER_ERROR });
            }
            );
  }
}
