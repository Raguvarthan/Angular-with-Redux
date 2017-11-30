import { PagesComponent } from './pages.component';
import { VehicleInfoComponent } from './vehicleinfo/vehicleinfo.component';
import { VendorInfoComponent } from './vendorinfo/vendorinfo.component';
import { VehiclePaymentComponent } from './vehiclepayment/vehiclepayment.component';
import { VendorPaymentComponent } from './vendorpayment/vendorpayment.component';
import { TripInfoComponent } from './tripinfo/tripinfo.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { VehiclePendingPaymentComponent } from 
'./vehiclepayment/vehicle-pending-payment/vehicle-pending-payment.component';
import { VehicleTripComponent } from './vehiclepayment/vehicletrip/vehicletrip.component';
import { VendorTripComponent } from './vendorpayment/vendortrip/vendortrip.component';
import { TripInsertUpdateComponent } from './tripinfo/trip-insert-update/trip-insert-update.component';
import { OfficeInfoComponent } from './officeinfo/office.component';
import { ExpensesComponent } from './expenses/expenses.component';
import { VehicleViewUpdateComponent } from "./vehicleinfo/vehicleviewupdate/vehicle-view-update.component";
import { DriverInfoComponent } from "./driverinfo/driverinfo.component";

export const PagesComponentBarrel = [
PagesComponent, VehicleInfoComponent, VendorInfoComponent, VehiclePaymentComponent,
VendorPaymentComponent, TripInfoComponent, TripInsertUpdateComponent, DashboardComponent,
VehiclePendingPaymentComponent, VehicleTripComponent, VendorTripComponent, OfficeInfoComponent,
VehicleViewUpdateComponent, ExpensesComponent, DriverInfoComponent];

