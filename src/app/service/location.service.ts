import { Injectable, Component } from '@angular/core';
import { Headers, Http, Response, RequestOptions } from '@angular/http';
import { Observable } from 'rxjs/Rx';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import { HttpClient } from "@angular/common/http";

@Injectable()


export class LocationService {
    baseUrl = 'http://localhost:3000/';
    constructor(

        private httpClient: HttpClient,

    ) {
    }

    getLocation() {
        return this.httpClient.get('/assets/location.json')
            .map((lRes) => lRes)
            .catch((error: any) => Observable.throw(error));
    }



}
