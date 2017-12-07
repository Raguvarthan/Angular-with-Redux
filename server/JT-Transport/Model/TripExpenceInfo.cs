using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;

namespace JT_Transport.Model
{
  /// <summary>
  /// 
  /// </summary>
  public class TripExpenceInfo
  {
    /// <summary>
    /// 
    /// </summary>
    public ObjectId Id { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [Required]
    public string TripExpenseId { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [Required]
    public string VehicleNo { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [Required]
    public string Driver1Name { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [Required]
    public string Driver2Name { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [Required]
    public string CleanerName { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [Required]
    public DateTime? EntryDate { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [Required]
    public DateTime? StartDate { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [Required]
    public DateTime? EndDate { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [Required]
    public long? StartKM { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [Required]
    public long? EndKM { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [Required]
    public long? TotalKM { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [Required]
    public long? AdvanceAmount { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [Required]
    public int? AverageMileage { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [Required]
    public Expenses Expenses { get; set; }
    /// <summary>
    /// Defines if the trip expense is active or not
    /// </summary>
    [Required]
    public bool? IsActive { get; set; }
  }

  /// <summary>
  /// 
  /// </summary>
  public class Expenses
  {
    /// <summary>
    /// 
    /// </summary>
    [Required]
    public long? TotalIncome { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [Required]
    public long? TotalExpenses { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [Required]
    public long? NetProfit { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [Required]
    public ExpenseInfo ExpenseInfo { get; set; }
  }

  /// <summary>
  /// 
  /// </summary>
  public class ExpenseInfo
  {
    /// <summary>
    /// 
    /// </summary>
    [Required]
    public DieselExpenses DieselExpenses { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [Required]
    public TripWiseExpenses TripWiseExpenses { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [Required]
    public RTOAndPCExpenses RTOExpenses { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [Required]
    public RTOAndPCExpenses PCExpenses { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [Required]
    public long? TollExpenses { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [Required]
    public long? DriverBata { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [Required]
    public int? DriverBataPercentage { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [Required]
    public long? DriverBonus { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [Required]
    public BillableExpenses BillableExpenses { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [Required]
    public OtherExpenses Otherexpenses { get; set; }
  }

  /// <summary>
  /// 
  /// </summary>
  public class DieselExpenses
  {
    /// <summary>
    /// 
    /// </summary>
    [Required]
    public long? TotalQuantity { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [Required]
    public long? TotalAmount { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [Required]
    public DieselExpenseInfo CMSDieselExpenses { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [Required]
    public DieselExpenseInfo CashDieselExpenses { get; set; }
  }

  /// <summary>
  /// 
  /// </summary>
  public class DieselExpenseInfo
  {
    /// <summary>
    /// 
    /// </summary>
    [Required]
    public long? TotalQuantity { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [Required]
    public long? TotalAmount { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [Required]
    public List<DieselExpenseDetails> DieselExpenseDetails { get; set; }
  }

  /// <summary>
  /// 
  /// </summary>
  public class DieselExpenseDetails
  {
    /// <summary>
    /// 
    /// </summary>
    [Required]
    public DateTime? Date { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [Required]
    public long? Quantity { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [Required]
    public long? Amount { get; set; }
  }

  /// <summary>
  /// 
  /// </summary>
  public class TripWiseExpenses
  {
    /// <summary>
    /// 
    /// </summary>
    [Required]
    public int? TotalLoadInTon { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [Required]
    public long? TotalVehicleAmount { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [Required]
    public long? TotalCommission { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [Required]
    public long? TotalLoadingCharges { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [Required]
    public long? TotalUnloadingCharges { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [Required]
    public List<TripWiseExpenseDetails> TripWiseExpenseDetails { get; set; }
  }

  /// <summary>
  /// 
  /// </summary>
  public class TripWiseExpenseDetails
  {
    /// <summary>
    /// 
    /// </summary>
    [Required]
    public DateTime? TripStartDate { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [Required]
    public string FromLocation { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [Required]
    public string ToLocation { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [Required]
    public int? LoadInTon { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [Required]
    public long? VehicleAmount { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [Required]
    public long? Commission { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [Required]
    public int? CommissionPercentage { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [Required]
    public long? LoadingCharges { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [Required]
    public long? UnloadingCharges { get; set; }
  }

  /// <summary>
  /// 
  /// </summary>
  public class RTOAndPCExpenses
  {
    /// <summary>
    /// 
    /// </summary>
    [Required]
    public long? TotalExpenses { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [Required]
    public List<RTOAndPCExpenseDetails> ExpenseDetails { get; set; }
  }

  /// <summary>
  /// 
  /// </summary>
  public class RTOAndPCExpenseDetails
  {
    /// <summary>
    /// 
    /// </summary>
    [Required]
    public string Place { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [Required]
    public long? TotalAmount { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [Required]
    public long? Amount1 { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [Required]
    public long? Amount2 { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [Required]
    public long? Amount3 { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [Required]
    public long? Amount4 { get; set; }
  }

  /// <summary>
  /// 
  /// </summary>
  public class BillableExpenses
  {
    /// <summary>
    /// 
    /// </summary>
    [Required]
    public long? TotalBillableExpenses { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [Required]
    public List<ExpenseFields> BillableExpenseDetails { get; set; }
  }

  /// <summary>
  /// 
  /// </summary>
  public class OtherExpenses
  {
    /// <summary>
    /// 
    /// </summary>
    [Required]
    public long? TotalOtherExpenses { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [Required]
    public List<ExpenseFields> OtherExpenseDetails { get; set; }
  }

  /// <summary>
  /// 
  /// </summary>
  public class ExpenseFields
  {
    /// <summary>
    /// 
    /// </summary>
    [Required]
    public string ExpenseName { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [Required]
    public long? Amount { get; set; }
  }
}
