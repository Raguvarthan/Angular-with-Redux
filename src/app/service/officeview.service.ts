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


export class OfficeViewService {  
  baseUrl = 'http://localhost:3001/';
  constructor(
    private apiService: ApiService,
    private httpClient: HttpClient,
    private ngRedux: NgRedux<IAppState>
  ) {
    
  }

  getOffice() {
    this.apiService.get('', { useAuth: false }, Conf.apiUrl.serverUrl +'Office').then(
      (response: any) => {        
        if(response.code == '200') {
          this.ngRedux.dispatch({ type: Const.FETCH_ALL_OFFICE_SUCCESS, officeInfo: response.data });
        } else {
          throw response.error;
        }        
      })
      .catch((error: any) => {
      this.ngRedux.dispatch({ type: Const.FETCH_ALL_OFFICE_ERROR});
      });
  }

  updateOffice(office) {
    this.apiService.put('Office/' + 'Sample/' + office.officeId, office,
      { useAuth: false }, undefined).then(
      (response: any) => {
        console.log(response);
        if (response.code === '200') {
          this.ngRedux.dispatch({ type: Const.UPDATE_OFFICE_SUCCESS, officeInfo: office });
        } else {
          throw response.error;
        }
      })
      .catch(
      (error: any) => {
        this.ngRedux.dispatch({ type: Const.UPDATE_OFFICE_ERROR });
      }
      );
  }
  addOffice(office) {
      delete office.id;
      delete office.officeId;      
      this.apiService.post('Office/' + 'Sample', office,
            { useAuth: false }, undefined).then(
            (response: any) => {
              console.log(response);
              if (response.code === '200') {
                this.ngRedux.dispatch({ type: Const.ADD_OFFICE_SUCCESS, officeInfo: response.data });
              } else {
                throw response.error;
              }
            })
            .catch(
            (error: any) => {
              this.ngRedux.dispatch({ type: Const.ADD_OFFICE_ERROR });
            }
            );
  }
}
