
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using JT_Transport.Model;

namespace JT_Transport.Swagger
{
  /// <summary>
  /// Info of vendor
  /// </summary>
  public class ExampleModel_VendorInfo
  {
    /// <summary>
    /// Id of vendor
    /// </summary>
    public string VendorId { get; set; }
    /// <summary>
    /// Name of vendor
    /// </summary>
    public string VendorName { get; set; }
    /// <summary>
    /// Contact name of vendor
    /// </summary>
    public string ContactName { get; set; }
    /// <summary>
    /// Contact number of vendor
    /// </summary>
    public long ContactNo { get; set; }
    /// <summary>
    /// Address of vendor
    /// </summary>
    public string Address { get; set; }
    /// <summary>
    /// Defines if the vendor is active or not
    /// </summary>
    public bool? IsActive { get; set; }
  }

  /// <summary>
  /// Details of vehicle
  /// </summary>
  public class ExampleModel_VehicleInfo
  {
    /// <summary>
    /// Registration number of vehicle
    /// </summary>
    public string VehicleNo { get; set; }
    /// <summary>
    /// Vehicle owner name
    /// </summary>
    public string OwnerName { get; set; }
    /// <summary>
    /// Model year of the vehicle
    /// </summary>
    public string Model { get; set; }
    /// <summary>
    /// Model number of vehicle
    /// </summary>
    public int ModelNo { get; set; }
    /// <summary>
    /// Type of vehicle
    /// </summary>
    public string VehicleType { get; set; }
    /// <summary>
    /// Body type of vehicle
    /// </summary>
    public string TypeOfBody { get; set; }
    /// <summary>
    /// Number of wheels in vehicle
    /// </summary>
    public int NoOfWheels { get; set; }
    /// <summary>
    /// Capacity of vehicle
    /// </summary>
    public string VehicleCapacity { get; set; }
    /// <summary>
    /// Engine number of vehicle
    /// </summary>
    public string EngineNumber { get; set; }
    /// <summary>
    /// Chasis number of vehicle
    /// </summary>
    public string ChasisNumber { get; set; }
    /// <summary>
    /// Insurance date of vehicle
    /// </summary>
    public DateTime? InsuranceDate { get; set; }
    /// <summary>
    /// FC date of vehicle
    /// </summary>
    public DateTime? FCDate { get; set; }
    /// <summary>
    /// NP Tax date for vehicle
    /// </summary>
    public DateTime? NPTaxDate { get; set; }
    /// <summary>
    /// Permit date of vehicle
    /// </summary>
    public DateTime? PermitDate { get; set; }
    /// <summary>
    /// Defines if the vehicle is active or not
    /// </summary>
    public bool? IsActive { get; set; }
  }

  /// <summary>
  /// Details of role
  /// </summary>
  public class ExampleModel_Roles
  {
    /// <summary>
    /// Levels of access given to the role
    /// </summary>
    public string[] LevelOfAccess { get; set; }
    /// <summary>
    /// Defines if the roles is active or not
    /// </summary>
    public bool? IsActive { get; set; }
  }

  /// <summary>
  /// Info about office
  /// </summary>
  public class ExampleModel_OfficeInfo
  {
    /// <summary>
    /// Name of office
    /// </summary>
    public string OfficeName { get; set; }
    /// <summary>
    /// Contact name of office
    /// </summary>
    public string ContactName { get; set; }
    /// <summary>
    /// Contact number of office
    /// </summary>
    public long ContactNo { get; set; }
    /// <summary>
    /// Address of office
    /// </summary>
    public string Address { get; set; }
    /// <summary>
    /// Defines if the address is active or not
    /// </summary>
    public bool? IsActive { get; set; }
  }

  /// <summary>
  /// Info of driver
  /// </summary>
  public class ExampleModel_DriverInfo
  {
    /// <summary>
    /// Name of driver
    /// </summary>
    public string DriverName { get; set; }
    /// <summary>
    /// Contact number
    /// </summary>
    public long ContactNo { get; set; }
    /// <summary>
    /// Address of driver
    /// </summary>
    public string Address { get; set; }
    /// <summary>
    /// Defines if the driver is active or not
    /// </summary>
    public bool? IsActive { get; set; }
  }

  /// <summary>
  /// Info of tyre
  /// </summary>
  public class ExampleModel_TyreInfo
  {
    /// <summary>
    /// Status of type
    /// </summary>
    public string TyreStatus { get; set; }
    /// <summary>
    /// Purchase deatails of tyre
    /// </summary>
    public ExampleModel_PurchaseDetails PurchaseDetails { get; set; }
    /// <summary>
    /// Disposal details of tyre
    /// </summary>
    public DisposalDetails DisposalDetails { get; set; }
    /// <summary>
    /// Total KM runned
    /// </summary>
    public long? TotalKMRunned { get; set; }
    /// <summary>
    /// Flag to define if tyre is active or not
    /// </summary>
    public bool? IsActive { get; set; }
  }

  /// <summary>
  /// Purchase details of tyre
  /// </summary>
  public class ExampleModel_PurchaseDetails
  {
    /// <summary>
    /// Brand name
    /// </summary>
    public string BrandName { get; set; }
    /// <summary>
    /// Price of tyre
    /// </summary>
    public long? PriceOfTyre { get; set; }
    /// <summary>
    /// Model of tire
    /// </summary>
    public string TyreModel { get; set; }
    /// <summary>
    /// Tire number
    /// </summary>
    public string TyreNo { get; set; }
    /// <summary>
    /// Name of vendor
    /// </summary>
    public string VendorName { get; set; }
    /// <summary>
    /// Date of purchase
    /// </summary>
    public DateTime? PurchaseDate { get; set; }
    /// <summary>
    /// Bill Number
    /// </summary>
    public string BillNo { get; set; }
  }
}
