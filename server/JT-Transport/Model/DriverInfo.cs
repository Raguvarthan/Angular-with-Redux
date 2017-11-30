using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;

namespace JT_Transport.Model
{
  /// <summary>
  /// Info of driver
  /// </summary>
  public class DriverInfo
  {
    /// <summary>
    /// Object id given by mongo db
    /// </summary>
    public ObjectId Id { get; set; }
    /// <summary>
    /// Id of driver
    /// </summary>
    public string DriverId { get; set; }
    /// <summary>
    /// Name of driver
    /// </summary>
    [Required]
    public string DriverName { get; set; }
    /// <summary>
    /// Contact number
    /// </summary>
    [Required]
    public long ContactNo { get; set; }
    /// <summary>
    /// Address of driver
    /// </summary>
    [Required]
    public string Address { get; set; }
    /// <summary>
    /// Defines if the driver is active or not
    /// </summary>
    [Required]
    public bool? IsActive { get; set; }
  }
}
