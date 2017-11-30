import { Injectable } from "@angular/core";

@Injectable()
export class Office{
  
  officeId: number;  
  officeName: string;
  contactName: string;
  contactNumber: number;
  address: string;

  constructor(
    _officeId?: number,
      _officeName?: string,
      _contactName?: string,
      _contactNumber?: number,
      _address?: string
  ){
       this.officeId = _officeId || null,
       this.officeName = _officeName || null,
       this.contactName = _contactName || null,
       this.contactNumber = _contactNumber || null,
       this.address = _address || null
  }

}