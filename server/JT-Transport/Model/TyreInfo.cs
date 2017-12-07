using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;

namespace JT_Transport.Model
{
  /// <summary>
  /// Info of tyre
  /// </summary>
  public class TyreInfo
  {
    /// <summary>
    /// Object id given by mongo db
    /// </summary>
    public ObjectId Id { get; set; }
    /// <summary>
    /// Id of Tyre
    /// </summary>
    [Required]
    public string TyreId { get; set; }
    /// <summary>
    /// Status of type
    /// </summary>
    [Required]
    public string TyreStatus { get; set; }
    /// <summary>
    /// Purchase deatails of tyre
    /// </summary>
    [Required]
    public PurchaseDetails PurchaseDetails { get; set; }
    /// <summary>
    /// Vehicle details
    /// </summary>
    [Required]
    public List<VehicleDetails> VehicleDetails { get; set; }
    /// <summary>
    /// RethreadingDetails
    /// </summary>
    [Required]
    public List<RethreadingDetails> RethreadingDetails { get; set; }
    /// <summary>
    /// Disposal details of tyre
    /// </summary>
    [Required]
    public DisposalDetails DisposalDetails { get; set; }
    /// <summary>
    /// Total KM runned
    /// </summary>
    [Required]
    public long? TotalKMRunned { get; set; }
    /// <summary>
    /// Flag to define if tyre is active or not
    /// </summary>
    [Required]
    public bool? IsActive { get; set; }
  }

  /// <summary>
  /// Purchase details of tyre
  /// </summary>
  public class PurchaseDetails
  {
    /// <summary>
    /// Brand name
    /// </summary>
    [Required]
    public string BrandName { get; set; }
    /// <summary>
    /// Price of tyre
    /// </summary>
    [Required]
    public long? PriceOfTyre { get; set; }
    /// <summary>
    /// Model of tire
    /// </summary>
    [Required]
    public string TyreModel { get; set; }
    /// <summary>
    /// Tire number
    /// </summary>
    [Required]
    public string TyreNo { get; set; }
    /// <summary>
    /// Name of vendor
    /// </summary>
    [Required]
    public string VendorName { get; set; }
    /// <summary>
    /// Date of purchase
    /// </summary>
    [Required]
    public DateTime? PurchaseDate { get; set; }
    /// <summary>
    /// Bill Number
    /// </summary>
    [Required]
    public string BillNo { get; set; }
  }

  /// <summary>
  /// Details of vehicle
  /// </summary>
  public class VehicleDetails
  {
    /// <summary>
    /// Id of detail
    /// </summary>
    [Required]
    public int? Id { get; set; }
    /// <summary>
    /// Vehicle number
    /// </summary>
    [Required]
    public string VehicleNumber { get; set; }
    /// <summary>
    /// From date 
    /// </summary>
    [Required]
    public DateTime? FromDate { get; set; }
    /// <summary>
    /// To date
    /// </summary>
    [Required]
    public DateTime? ToDate { get; set; }
    /// <summary>
    /// Start KM
    /// </summary>
    [Required]
    public long? StartKM { get; set; }
    /// <summary>
    /// End KM
    /// </summary>
    [Required]
    public long? EndKM { get; set; }
    /// <summary>
    /// Total KM
    /// </summary>
    [Required]
    public long? TotalKM { get; set; }
    /// <summary>
    /// Reason for removing
    /// </summary>
    [Required]
    public string ReasonForRemoving { get; set; }
    /// <summary>
    /// Tyre position
    /// </summary>
    [Required]
    public string PositionOfTyre { get; set; }
  }

  /// <summary>
  /// Details of rethreading
  /// </summary>
  public class RethreadingDetails
  {
    /// <summary>
    /// Rethreading id
    /// </summary>
    [Required]
    public int? Id { get; set; }
    /// <summary>
    /// Vendor name
    /// </summary>
    [Required]
    public string VendorName { get; set; }
    /// <summary>
    /// Given date
    /// </summary>
    [Required]
    public DateTime? GivenDate { get; set; }
    /// <summary>
    /// Cost for rethreading
    /// </summary>
    [Required]
    public long? RethreadingCost { get; set; }
    /// <summary>
    /// Taken date
    /// </summary>
    [Required]
    public DateTime? TakenDate { get; set; }
    /// <summary>
    /// Receipt Number
    /// </summary>
    [Required]
    public string ReceiptNo { get; set; }
    /// <summary>
    /// Voucher number
    /// </summary>
    [Required]
    public string VoucherNo { get; set; }
    /// <summary>
    /// Amount paid date
    /// </summary>
    [Required]
    public DateTime? AmountPaidDate { get; set; }
  }

  /// <summary>
  /// Disposal details of tyre
  /// </summary>
  public class DisposalDetails
  {
    /// <summary>
    /// Vendor name
    /// </summary>
    [Required]
    public string VendorName { get; set; }
    /// <summary>
    /// Tire disposal date
    /// </summary>
    [Required]
    public DateTime? DisposalDate { get; set; }
    /// <summary>
    /// Tire sold price
    /// </summary>
    [Required]
    public long? SoldPrice { get; set; }
  }
}
