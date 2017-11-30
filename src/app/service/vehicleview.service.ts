import { Injectable, Component } from '@angular/core';
import { Headers, Http, Response, RequestOptions } from '@angular/http';
import { Observable } from 'rxjs/Rx';
import { NgRedux } from 'ng2-redux';
import { IAppState } from '../store/store';
import * as Const from '../store/actions';

import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import { HttpClient } from "@angular/common/http";
import * as Conf from '../config/configuration';
import { ApiService } from './api.service';

@Injectable()


export class VehicleViewService {
  baseUrl = 'http://localhost:3001/';
  constructor(private httpClient: HttpClient, private ngRedux: NgRedux<IAppState>,
    private apiService: ApiService) {
  }

  getVechile() {
    this.apiService.get('', { useAuth: false }, Conf.apiUrl.serverUrl +'Vehicle').then(
      (response: any) => {        
        if(response.code == '200') {
          this.ngRedux.dispatch({ type: Const.FETCH_ALL_VECHICLES_SUCCESS, vehicleInfo: response.data });
        } else {
          throw response.error;
        }        
      })
      .catch((error: any) => {
        console.log(error);
      });
  }

  updateVechile(vehicle) {
    this.apiService.put('Vehicle/' + 'Sample/' + vehicle.vehicleId, vehicle,
            { useAuth: false }, undefined).then(
            (response: any) => {
              console.log(response);
              if (response.code === '200') {
                this.ngRedux.dispatch({ type: Const.UPDATE_VECHICLE_SUCCESS, vehicleInfo: vehicle });
              } else {
                throw response.error;
              }
            })
            .catch(
            (error: any) => {
              this.ngRedux.dispatch({ type: Const.UPDATE_VECHICLE_ERROR });
            }
            );
  }
  addVechile(vehicle) {
      delete vehicle.id;
      delete vehicle.vehicleId;
      this.apiService.post('Vehicle/' + 'Sample', vehicle,
            { useAuth: false }, undefined).then(
            (response: any) => {
              console.log(response);
              if (response.code === '200') {
                this.ngRedux.dispatch({ type: Const.ADD_VECHICLE_SUCCESS, vehicleInfo: response.data });
              } else {
                throw response.error;
              }
            })
            .catch(
            (error: any) => {
              this.ngRedux.dispatch({ type: Const.ADD_VECHICLE_ERROR });
            }
            );
  }

}
