import { Injectable } from '@angular/core'

@Injectable()

export class Expenses {
    tripExpenseId: number;
    vehicleNo: string;
    entryDate: string;
    advanceAmount: number;
    driver1Name: string;
    driver2Name: string;
    cleanerName: string;
    startDate: string;
    endDate: string;
    startKM: number;
    endKM: number;
    totalKM: number;
    averageMileage: number;
    diesel: number;
    expenses: ExpensesDetails;

    constructor(
        _tripExpenseId?: number,
        _vehicleNo?: string,
        _entryDate?: string,
        _advanceAmount?: number,
        _driver1Name?: string,
        _driver2Name?: string,
        _cleanerName?: string,
        _startDate?: string,
        _endDate?: string,
        _startKM?: number,
        _endKM?: number,
        _totalKM?: number,
        _averageMileage?: number,
        _diesel?: number,
        _expenses?: ExpensesDetails

    ) {
        this.tripExpenseId = _tripExpenseId || null,
            this.vehicleNo = _vehicleNo || null,
            this.entryDate = _entryDate || null,
            this.advanceAmount = _advanceAmount || null,
            this.driver1Name = _driver1Name || null,
            this.driver2Name = _driver2Name || null,
            this.startDate = _startDate || null,
            this.endDate = _endDate || null,
            this.startKM = _startKM || null,
            this.endKM = _endKM || null,
            this.totalKM = _totalKM || null
        this.averageMileage = _averageMileage || null,
            this.diesel = _diesel || null,
            this.expenses = _expenses || null
    }
}


export class ExpensesDetails {
    totalIncome: number;
    totalExpenses: number;
    balanceAmount: number;
    expenseInfo: ExpenseInfo;
    constructor(
        _totalIncome?: number,
        _totalExpenses?: number,
        _balanceAmount?: number,
        _expenseInfo?: ExpenseInfo
    ){
        this.totalIncome = _totalIncome || null,
        this.totalExpenses = _totalExpenses || null,
        this.balanceAmount = _balanceAmount || null
        this.expenseInfo = _expenseInfo || null
    }

}

export class ExpenseInfo {
    dieselExpenses: DieselExpenses;
    tripWiseExpenses: TripWiseExpenses;
    rtoExpenses: RtoExpenses;
    pcExpenses: PcExpenses;
    billableExpenses: BillableExpenses;
    otherexpenses: Otherexpenses;
    tollExpenses: number;
    driverBata: number;
    cleanerBata: number;
    driverBonus: number;
    constructor(
        _dieselExpenses?: DieselExpenses,
        _tripWiseExpenses?: TripWiseExpenses,
        _rtoExpenses?: RtoExpenses,
        _pcExpenses?: PcExpenses,
        _billableExpenses?: BillableExpenses,
        _otherexpenses?: Otherexpenses,
        _tollExpenses?: number,
        _driverBata?: number,
        _cleanerBata?: number,
        _driverBonus?: number,
    ){
        this.dieselExpenses = _dieselExpenses || null,
        this.tripWiseExpenses = _tripWiseExpenses || null,
        this.rtoExpenses = _rtoExpenses || null,
        this.pcExpenses = _pcExpenses || null,
        this.billableExpenses = _billableExpenses || null,
        this.otherexpenses = _otherexpenses || null,
        this.tollExpenses = _tollExpenses || null,
        this.driverBata = _driverBata || null,
        this.cleanerBata = _cleanerBata || null,
        this.driverBonus = _driverBonus || null
    }
}

export class DieselExpenses {
    totalQuantity: number;
    totalAmount: number;
    cmsDieselExpenses: CmsDieselExpenses;
    cashDieselExpenses: CashDieselExpenses;
    constructor(
        _totalQuantity?: number,
        _totalAmount?: number,
        _cmsDieselExpenses?: CmsDieselExpenses,
        _cashDieselExpenses?: CashDieselExpenses,
    ){
        this.totalQuantity = _totalQuantity || null,
        this.totalAmount = _totalAmount || null,
        this.cmsDieselExpenses = _cmsDieselExpenses || null,
        this.cashDieselExpenses = _cashDieselExpenses || null
    }
}

export class CmsDieselExpenses {
    totalQuantity: number;
    totalAmount: number;
    dieselExpenseDetails: DieselExpenseDetails[];
    constructor(
        _totalQuantity?: number,
        _totalAmount?: number,
        _dieselExpenseDetails?: DieselExpenseDetails[]
    ){
        this.totalQuantity = _totalQuantity || null,
        this.totalAmount = _totalAmount || null,
        this.dieselExpenseDetails = _dieselExpenseDetails || null
    }
}

export class CashDieselExpenses {
    totalQuantity: number;
    totalAmount: number;
    dieselExpenseDetails: DieselExpenseDetails[];
    constructor(
        _totalQuantity?: number,
        _totalAmount?: number,
        _dieselExpenseDetails?: DieselExpenseDetails[]
    ){
        this.totalQuantity = _totalQuantity || null,
        this.totalAmount = _totalAmount || null,
        this.dieselExpenseDetails = _dieselExpenseDetails || null
    }
}

export class DieselExpenseDetails {
    date: string;
    quantity: number;
    amount: number;
    constructor(
        _date?: string,
        _quantity?: number,
        _amount?: number,
    ){
        this.date = _date || null;
        this.quantity = _quantity || null;
        this.amount = _amount || null
    }
}

export class TripWiseExpenses {
    totalLoadInTon: number;
    totalVehicleAmount: number;
    totalCommission: number;
    totalLoadingCharges: number;
    totalUnloadingCharges: number;
    tripWiseExpenseDetails: TripWiseExpenseDetails[];
    constructor(
        _totalLoadInTon?: number,
        _totalVehicleAmount?: number,
        _totalCommission?: number,
        _totalLoadingCharges?: number,
        _totalUnloadingCharges?: number,
        _tripWiseExpenseDetails?: TripWiseExpenseDetails[]
    ){
        this.totalLoadInTon = _totalLoadInTon || null,
        this.totalVehicleAmount = _totalVehicleAmount || null,
        this.totalCommission = _totalCommission || null,
        this.totalLoadingCharges = _totalLoadingCharges || null,
        this.totalUnloadingCharges = _totalUnloadingCharges || null
        this.tripWiseExpenseDetails= _tripWiseExpenseDetails ||null
    }
}

export class TripWiseExpenseDetails {
    tripStartDate: string;
    fromLocation: string;
    toLocation: string;
    loadInTon: number;
    vehicleAmount: number;
    commission: number;
    loadingCharges: number;
    unloadingCharges: number;
    constructor(
        _tripStartDate?: string,
        _fromLocation?: string,
        _toLocation?: string,
        _loadInTon?: number,
        _vehicleAmount?: number,
        _commission?: number,
        _loadingCharges?: number,
        _unloadingCharges?: number,
    ){
        this.tripStartDate = _tripStartDate || null,
        this.fromLocation = _fromLocation || null,
        this.toLocation = _fromLocation || null,
        this.loadInTon = _loadInTon || null,
        this.vehicleAmount = _vehicleAmount || null,
        this.commission = _commission || null,
        this.loadingCharges = _loadingCharges || null,
        this.unloadingCharges = _unloadingCharges || null
    }
}

export class RtoExpenses {
    totalExpenses: number;
    expenseDetails: ExpenseDetails[];
    constructor(
        _totalExpenses: number,
        _expenseDetails: ExpenseDetails[]
    ){
        this.totalExpenses = _totalExpenses || null;
        this.expenseDetails = _expenseDetails || null;
    }
}

export class PcExpenses {
    totalExpenses: number;
    expenseDetails: ExpenseDetails[];
    constructor(
        _totalExpenses: number,
        _expenseDetails: ExpenseDetails[]
    ){
        this.totalExpenses = _totalExpenses || null;
        this.expenseDetails = _expenseDetails || null;
    }
}

export class ExpenseDetails {
    place: string;
    amount: number;
    constructor(
        _place?: string,
        _amount?: number
    ){
        this.place = _place || null,
        this.amount = _amount || null
    }
}

export class BillableExpenses {
    totalBillableExpenses: number;
    billableExpenseDetails: BillableExpenseDetails[];
    constructor(
        _totalBillableExpenses?: number,
        _billableExpenseDetails?: BillableExpenseDetails[]
    ){
        this.totalBillableExpenses = _totalBillableExpenses || null,
        this.billableExpenseDetails = _billableExpenseDetails || null
    }
}

export class BillableExpenseDetails {
    expenseName: string;
    amount: number;
    constructor(
        _expenseName?: string,
        _amount?: number
    ){
        this.expenseName = _expenseName || null,
        this.amount = _amount || null
    }
}


export class Otherexpenses {
    totalOtherExpenses: number;
    otherExpenseDetails: OtherExpenseDetails[];
    constructor(
        _totalOtherExpenses?: number,
        _otherExpenseDetails?: OtherExpenseDetails[]
    ){
        this.totalOtherExpenses = _totalOtherExpenses || null,
        this.otherExpenseDetails = _otherExpenseDetails || null;
    }
}

export class OtherExpenseDetails {
    expenseName: string;
    amount: number;
    constructor(
        _expenseName?: string,
        _amount?: number
    ){
        this.expenseName = _expenseName || null,
        this.amount = _amount || null
    }
}



