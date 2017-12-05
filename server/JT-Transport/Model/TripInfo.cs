using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;

namespace JT_Transport.Model
{
  /// <summary>
  /// Details of trip
  /// </summary>
  public class TripInfo
  {
    /// <summary>
    /// Object id given by mongo db
    /// </summary>
    public ObjectId Id { get; set; }
    /// <summary>
    /// Id of trip
    /// </summary>
    public string TripId { get; set; }
    /// <summary>
    /// Vehicle number
    /// </summary>
    [Required]
    public string VehicleNo { get; set; }
    /// <summary>
    /// Name of vendor
    /// </summary>
    [Required]
    public string VendorName { get; set; }
    /// <summary>
    /// Date of loading
    /// </summary>
    [Required]
    public DateTime? LoadDate { get; set; }
    /// <summary>
    /// Date of dropping
    /// </summary>
    [Required]
    public DateTime? DropDate { get; set; }
    /// <summary>
    /// Info of location
    /// </summary>
    [Required]
    public LocationInfo Location { get; set; }
    /// <summary>
    /// Office info
    /// </summary>
    [Required]
    public OfficeDetails OfficeInfo { get; set; }
    /// <summary>
    /// Info of payment
    /// </summary>
    [Required]
    public List<PaymentDetails> PaymentInfo { get; set; }
    /// <summary>
    /// Drivers Name
    /// </summary>
    [Required]
    public string DriverName { get; set; }
    /// <summary>
    /// Type of payment
    /// </summary>
    [Required]
    public string TypeOfPayment { get; set; }
    /// <summary>
    /// Number of tons for which the payment is to be maid
    /// </summary>
    [Required]
    public int? NoOfTons { get; set; }
    /// <summary>
    /// Rate oer ton
    /// </summary>
    [Required]
    public long? RatePerTon { get; set; }
    /// <summary>
    /// Total estimated amount
    /// </summary>
    public long? TotalAmount { get; set; }
    /// <summary>
    /// Amount discounted per ton
    /// </summary>
    [Required]
    public long? Crossing { get; set; }
    /// <summary>
    /// Advance amount paid
    /// </summary>
    [Required]
    public long? AdvanceAmount { get; set; }
    /// <summary>
    /// Amount accepted by driver
    /// </summary>
    [Required]
    public long? DriverAcceptedAmount { get; set; }
    /// <summary>
    /// Self amount
    /// </summary>
    [Required]
    public long? SelfAmount { get; set; }
    /// <summary>
    /// Paid amount
    /// </summary>
    [Required]
    public long? PaidAmount { get; set; }
    /// <summary>
    /// Balance amount to be paid
    /// </summary>
    [Required]
    public long? BalanceAmount { get; set; }
    /// <summary>
    /// JT advance
    /// </summary>
    [Required]
    public long? JTAdvance { get; set; }
    /// <summary>
    /// Diesel cost
    /// </summary>
    [Required]
    public long? DieselCost { get; set; }
    /// <summary>
    /// Unload ton
    /// </summary>
    [Required]
    public int? UnloadTon { get; set; }
    /// <summary>
    /// Advance balance amount
    /// </summary>
    [Required]
    public long? AdvanceBalanceAmount { get; set; }
    /// <summary>
    /// Loading charges
    /// </summary>
    [Required]
    public long? LoadingCharges { get; set; }
    /// <summary>
    /// Unloading charges
    /// </summary>
    [Required]
    public long? UnloadingCharges { get; set; }
    /// <summary>
    /// Total amount to be paid
    /// </summary>
    [Required]
    public long? VehicleAmount { get; set; }
    /// <summary>
    /// Rounf off amount
    /// </summary>
    [Required]
    public long? RoundOffAmount { get; set; }
    /// <summary>
    /// Defines if the trip info is active or not
    /// </summary>
    [Required]
    public bool? IsActive { get; set; }
  }

  /// <summary>
  /// Info of location
  /// </summary>
  public class LocationInfo
  {
    /// <summary>
    /// Place of pick up
    /// </summary>
    [Required]
    public string PickUpPlace { get; set; }
    /// <summary>
    /// Thaluka of place where pick up is made
    /// </summary>
    [Required]
    public string PickUpThaluka { get; set; }
    /// <summary>
    /// Place where shipment is dropped
    /// </summary>
    [Required]
    public string DropPlace { get; set; }
    /// <summary>
    /// Thaluka where shipment is dropped
    /// </summary>
    [Required]
    public string DropThaluka { get; set; }
  }

  /// <summary>
  /// 
  /// </summary>
  public class OfficeDetails
  {
    /// <summary>
    /// Name of office
    /// </summary>
    [Required]
    public string OfficeName { get; set; }
    /// <summary>
    /// Contact name of office
    /// </summary>
    [Required]
    public string ContactName { get; set; }
  }

  /// <summary>
  /// Payment details of trip
  /// </summary>
  public class PaymentDetails
  {
    /// <summary>
    /// Name of office
    /// </summary>
    [Required]
    public long? AmountReceived { get; set; }
    /// <summary>
    /// Balance amount 
    /// </summary>
    [Required]
    public long? RunningBalanceAmount { get; set; }
    /// <summary>
    /// Contact name of office
    /// </summary>
    [Required]
    public DateTime? Date { get; set; }
    /// <summary>
    /// Amount paid to
    /// </summary>
    [Required]
    public string PaidTo { get; set; }
  }
}
