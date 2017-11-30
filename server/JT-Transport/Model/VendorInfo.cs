using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;

namespace JT_Transport.Model
{
  /// <summary>
  /// Details of vendor
  /// </summary>
  public class VendorInfo
  {
    /// <summary>
    /// Object id given by mongo db
    /// </summary>
    public ObjectId Id { get; set; }
    /// <summary>
    /// Id of vendor
    /// </summary>
    public string VendorId { get; set; }
    /// <summary>
    /// Name of vendor
    /// </summary>
    [Required]
    public string VendorName { get; set; }
    /// <summary>
    /// Contact name of vendor
    /// </summary>
    [Required]
    public string ContactName { get; set; }
    /// <summary>
    /// Contact number of vendor
    /// </summary>
    [Required]
    public long ContactNo { get; set; }
    /// <summary>
    /// Address of vendor
    /// </summary>
    [Required]
    public string Address { get; set; }
    /// <summary>
    /// Defines if the vendor is active or not
    /// </summary>
    [Required]
    public bool? IsActive { get; set; }
  }
}
