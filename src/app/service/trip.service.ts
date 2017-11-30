import { Injectable, Component } from '@angular/core';
import { Headers, Http, Response, RequestOptions } from '@angular/http';
import { Observable } from 'rxjs/Rx';
import { ApiService } from './api.service';
import * as Conf from '../config/configuration';
import * as Const from '../store/actions';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import { HttpClient } from "@angular/common/http";
import { NgRedux } from "ng2-redux/lib";
import { IAppState } from "../store/store";

@Injectable()


export class TripService {  
  baseUrl = 'http://localhost:3001/';
  constructor(
    private apiService: ApiService,
    private httpClient: HttpClient,
    private ngRedux: NgRedux<IAppState>
    
  ) {
    
  }

  getTripInfo() {
      this.apiService.get('', { useAuth: false }, Conf.apiUrl.serverUrl +'Trip').then(
      (response: any) => {        
        if(response.code == '200') {
          this.ngRedux.dispatch({ type: Const.FETCH_ALL_TRIP_SUCCESS, tripInfo: response.data });
        } else {
          throw response.error;
        }        
      })
      .catch((error: any) => {
        this.ngRedux.dispatch({ type: Const.FETCH_ALL_TRIP_ERROR });
      });
  }

  updateTrip(trip) {    
    this.apiService.put('Trip/' + 'Sample/' + trip.tripId, trip,
            { useAuth: false }, undefined).then(
            (response: any) => {
              console.log(response);
              if (response.code === '200') {
                this.ngRedux.dispatch({ type: Const.UPDATE_TRIP_SUCCESS, tripInfo: trip });
              } else {
                throw response.error;
              }
            })
            .catch(
            (error: any) => {
              this.ngRedux.dispatch({ type: Const.UPDATE_TRIP_ERROR });
            }
            );
  } 
  addTrip(trip) {    
    this.apiService.post('Trip/' + 'Sample', trip,
            { useAuth: false }, undefined).then(
            (response: any) => {
              console.log(response);
              if (response.code === '200') {
                this.ngRedux.dispatch({ type: Const.ADD_TRIP_SUCCESS, tripInfo: trip });
              } else {
                throw response.error;
              }
            })
            .catch(
            (error: any) => {
              this.ngRedux.dispatch({ type: Const.ADD_TRIP_ERROR });
            }
            );
  }
}
