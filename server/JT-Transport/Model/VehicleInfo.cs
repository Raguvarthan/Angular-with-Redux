using System;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;

namespace JT_Transport.Model
{
  /// <summary>
  /// Details of vehicle
  /// </summary>
  public class VehicleInfo
  {
    /// <summary>
    /// Object id given by mongo db
    /// </summary>
    public ObjectId Id { get; set; }
    /// <summary>
    /// Id given to vehicle
    /// </summary>
    [Required]
    public string VehicleId { get; set; }
    /// <summary>
    /// Registration number of vehicle
    /// </summary>
    [Required]
    public string VehicleNo { get; set; }
    /// <summary>
    /// Vehicle owner name
    /// </summary>
    [Required]
    public string OwnerName { get; set; }
    /// <summary>
    /// Model year of the vehicle
    /// </summary>
    [Required]
    public string Model { get; set; }
    /// <summary>
    /// Model number of vehicle
    /// </summary>
    [Required]
    public int ModelNo { get; set; }
    /// <summary>
    /// Type of vehicle
    /// </summary>
    [Required]
    public string VehicleType { get; set; }
    /// <summary>
    /// Body type of vehicle
    /// </summary>
    [Required]
    public string TypeOfBody { get; set; }
    /// <summary>
    /// Number of wheels in vehicle
    /// </summary>
    [Required]
    public int NoOfWheels { get; set; }
    /// <summary>
    /// Capacity of vehicle
    /// </summary>
    [Required]
    public string VehicleCapacity { get; set; }
    /// <summary>
    /// Engine number of vehicle
    /// </summary>
    [Required]
    public string EngineNumber { get; set; }
    /// <summary>
    /// Chasis number of vehicle
    /// </summary>
    [Required]
    public string ChasisNumber { get; set; }
    /// <summary>
    /// Insurance date of vehicle
    /// </summary>
    [Required]
    public DateTime? InsuranceDate { get; set; }
    /// <summary>
    /// FC date of vehicle
    /// </summary>
    [Required]
    public DateTime? FCDate { get; set; }
    /// <summary>
    /// NP Tax date for vehicle
    /// </summary>
    [Required]
    public DateTime? NPTaxDate { get; set; }
    /// <summary>
    /// Permit date of vehicle
    /// </summary>
    [Required]
    public DateTime? PermitDate { get; set; }
    /// <summary>
    /// Defines if the vehicle is active or not
    /// </summary>
    [Required]
    public bool? IsActive { get; set; }
  }
}
