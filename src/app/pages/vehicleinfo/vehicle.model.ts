import { Injectable } from "@angular/core";

@Injectable()
export class Vehicle{
  
  id: number;  
  vehicleId: string;  
  vehicleNo: string;
  ownerName: string;
  model: string;
  modelNo: string;  
  vehicleType: string;
  typeOfBody: string;
  noOfWheels: number;
  vehicleCapacity: string;
  engineNumber: string;
  chasisNumber: string;
  insuranceDate: Date;
  fcDate: Date;
  npTaxDate: Date;
  permitDate: Date;
  isActive: boolean;

  constructor(_id?: number, _vehicleId?: string, _vehicleNo?: string, _vehicleType?: string, _noOfWheels?: number, _vehicleCapacity?: string,
               _ownerName?: string, _model?: string, _typeOfBody?: string, _engineNumber?: string, _chasisNumber?: string, _insuranceDate?: Date,
               _fcDate?: Date, _npTaxDate?: Date, _permitDate?: Date, _isActive?: boolean ) {
       this.id = _id || null,
       this.vehicleId = _vehicleId || null,
       this.vehicleNo = _vehicleNo || null,
       this.vehicleType = _vehicleType || null,
       this.noOfWheels = _noOfWheels || null,
       this.vehicleCapacity = _vehicleCapacity || null,
       this.ownerName = _ownerName || null,
       this.model = _model || null,
       this.typeOfBody = _typeOfBody || null,
       this.engineNumber = _engineNumber || null,
       this.chasisNumber = _chasisNumber || null,
       this.insuranceDate = _insuranceDate || null,
       this.fcDate = _fcDate || null,
       this.npTaxDate = _npTaxDate || null,
       this.permitDate = _permitDate || null,
       this.isActive = _isActive || null
  }
}