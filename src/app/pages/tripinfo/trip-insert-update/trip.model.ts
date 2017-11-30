import { Injectable } from "@angular/core";

@Injectable()

export class Trip {
    
        id: number;
        tripId: string;
        vendorName: string;
        loadDate: string;
        dropDate: string;
        vehicleNo: string;
        location: Location;
        
        ton: string;
        rate: number;
        totalAmt: number;
        crossing: number;
        vehicleAmt: number;
        typeOfPayment: string;
        advanceAmt: number;
        advance: Advance;
        toPay: ToPay;        
    

    constructor(
        _id?: number,
        _loadDate?: string,
        _vehicleNo?: string,
        _location?: Location,
        _vendorName?: string,
        _ton?: string,
        _rate?: number,
        _totalAmt?: number,
        _crossing?: number,
        _vehicleAmt?: number,
        _typeOfPayment?: string,
        _advanceAmt?: number,
        _advance?: Advance,
        _toPay?: ToPay
        
    ) {
        this.id = _id || null,
        this.loadDate= _loadDate || null,
        this.vehicleNo= _vehicleNo || null,
        this.location= _location || null,
        this.vendorName = _vendorName || null,
        this.ton = _ton || null,
        this.rate = _rate || null,
        this.totalAmt = _totalAmt || null,
        this.crossing = _crossing || null,
        this.vehicleAmt = _vehicleAmt || null,
        this.typeOfPayment = _typeOfPayment || null,
        this.advanceAmt = _advanceAmt || null,
        this.advance =  _advance || null ,
        this.toPay = _toPay || null
        
    }


}



export class Location {
    pickUpPlace: string;
    pickUpThalukka: string;
    dropPlace: string;
    dropThalukka: string;
    constructor(
        _pickUpPlace?:string,
        _pickUpThalukka?:string,
        _dropPlace?:string,
        _dropThalukka?: string
    ){
      this.pickUpPlace = _pickUpPlace || null,
      this.pickUpThalukka = _pickUpThalukka || null,
      this.dropPlace = _dropPlace || null,
      this.dropThalukka = _dropThalukka || null
    }


}

export class Advance{
    driverAccepted: number;
    selfAccount: number;
    paid: number;
    balance: number;

    constructor(
        _driverAccepted?: number,
        _selfAccount?: number,
        _paid?: number,
        _balance?: number

    ){
        this.driverAccepted = _driverAccepted || null,
        this.balance = _balance || null,
        this.selfAccount = _selfAccount || null,
        this.paid = _paid || null    
    }
}

export class ToPay{

}