import { Injectable } from "@angular/core";

@Injectable()
export class Vendor {

    id: number;
    vendorId: string;
    vendorName: string;
    vendorAddress: string;
    contactNo: number;
    contactName: string;

    constructor(
        _id?: number,
        _vendorId?: string,
        _vendorName?: string,
        _vendorAddress?: string,
        _contactNo?: number,
        _contactName?: string
    ) {
        this.id = _id || null,
        this.vendorId = _vendorId || null,
            this.vendorName = _vendorName || null,
            this.vendorAddress = _vendorAddress || null,
            this.contactNo = _contactNo || null,
            this.contactName = _contactName || null
    }

}