import { Component, OnInit } from '@angular/core';
import { TripExpensesService } from '../../service/tripexpenses.service';


@Component({
  selector: 'app-expenses',
  templateUrl: './expenses.component.html',
  styleUrls: ['./expenses.component.css']
})
export class ExpensesComponent implements OnInit {
  tripExpensesList: any;

  constructor(
    private tripExpenses: TripExpensesService
  ) { }

  ngOnInit() {
    this.getListofExpenses();
  }
  
  getListofExpenses(){
    this.tripExpenses.getTripExpensesInfo()
    .subscribe((expensesRes)=>{
      this.tripExpensesList = expensesRes;
     }, error => {
       console.log(error)
     })
  }
}
