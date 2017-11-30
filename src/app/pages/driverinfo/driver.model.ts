import { Injectable } from "@angular/core";

@Injectable()
export class Driver {

    id: number;
    driverId: string;
    driverName: string;
    driverAddress: string;
    contactNo: number;   

    constructor(
        _id?: number,
        _driverId?: string,
        _driverName?: string,
        _driverAddress?: string,
        _contactNo?: number,
    ) {
        this.id = _id || null,
        this.driverId = _driverId || null,
        this.driverName = _driverName || null,
        this.driverAddress = _driverAddress || null,
        this.contactNo = _contactNo || null
    }
}