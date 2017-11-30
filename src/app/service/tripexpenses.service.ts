import { Injectable, Component } from '@angular/core';
import { Headers, Http, Response, RequestOptions } from '@angular/http';
import { Observable } from 'rxjs/Rx';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import { HttpClient } from "@angular/common/http";

@Injectable()


export class TripExpensesService {  
  baseUrl = 'http://localhost:3001/';
  constructor(
    
    private httpClient: HttpClient,
    
  ) {
    
  }

  getTripExpensesInfo() {
    return this.httpClient.get(this.baseUrl+'tripExpenses')
      .map((res) => res)
      .catch((error: any) => Observable.throw(error));
  }


  
}
