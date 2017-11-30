import { Component, OnInit, Input } from '@angular/core';
import { TripWiseExpenseDetails, BillableExpenseDetails, ExpenseDetails, OtherExpenseDetails } from '../expenses.model';
import { DieselExpenseDetails } from '../expenses.model';
import { LocationService } from '../../../service/location.service';
import { VehicleViewService } from '../../../service/vehicleview.service';
import * as _ from 'underscore';



@Component({
  selector: 'app-expenses-insert-update',
  templateUrl: './expenses-insert-update.component.html',
  styleUrls: ['./expenses-insert-update.component.css']
})
export class ExpensesInsertUpdateComponent implements OnInit {
  vehicleData: any;
  cityLocation: any = [];
  locationData: any;

  @Input() expensesData: any;
  
  constructor(
    private locationSer: LocationService,    
    private vehicleSer: VehicleViewService,
  ) { }

  ngOnInit() {

    console.log(this.expensesData);
    this.getlocationList();
    this.getVehicleList();
  }

  addTrip(){
    this.expensesData.expenses.expenseInfo.tripWiseExpenses.tripWiseExpenseDetails.push(TripWiseExpenseDetails);
  }

  addCashDiesel(){
    this.expensesData.expenses.expenseInfo.dieselExpenses.cashDieselExpenses.dieselExpenseDetails.push(DieselExpenseDetails)
    
  }

  addCMSDiesel(){
    this.expensesData.expenses.expenseInfo.dieselExpenses.cmsDieselExpenses.dieselExpenseDetails.push(DieselExpenseDetails)
  }

  addBillable(){
    this.expensesData.expenses.expenseInfo.billableExpenses.billableExpenseDetails.push(BillableExpenseDetails)
  }

  addPCexpenses(){
    this.expensesData.expenses.expenseInfo.pcExpenses.expenseDetails.push(ExpenseDetails)
  }

  addRtoExpenses(){
    this.expensesData.expenses.expenseInfo.rtoExpenses.expenseDetails.push(ExpenseDetails)
  }
  addOtherExpneses(){
    this.expensesData.expenses.expenseInfo.otherexpenses.otherExpenseDetails.push(OtherExpenseDetails)
  }


  getlocationList() {
    this.locationSer.getLocation()
      .subscribe((locRes) => {
        this.locationData = _.map(locRes, (location: any) => {
          if (location.state == 'Tamil Nadu') {
            return location;
          }
        })
        this.locationData = _.without(this.locationData, undefined);
        _.map(this.locationData, (c: any) => {
          if(c.city !== ''){
            this.cityLocation.push(c.city)
          }          
        });
        
      });
  }

  getVehicleList() {
    // this.vehicleSer.getVechile()
    //   .subscribe((vehicleRes) => {
    //     this.vehicleData = _.map(vehicleRes, (v: any) => {
    //       return this.vehicleData = v.vehicleNo;
    //     });

    //   })
  }

}
