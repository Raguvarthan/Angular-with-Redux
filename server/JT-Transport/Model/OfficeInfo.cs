using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;

namespace JT_Transport.Model
{
  /// <summary>
  /// Info about office
  /// </summary>
  public class OfficeInfo
  {
    /// <summary>
    /// Object id given by mongo db
    /// </summary>
    public ObjectId Id { get; set; }
    /// <summary>
    /// Id of office
    /// </summary>
    [Required]
    public string OfficeId { get; set; }
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
    /// <summary>
    /// Contact number of office
    /// </summary>
    [Required]
    public long ContactNo { get; set; }
    /// <summary>
    /// Address of office
    /// </summary>
    [Required]
    public string Address { get; set; }
    /// <summary>
    /// Defines if the office is active or not
    /// </summary>
    [Required]
    public bool? IsActive { get; set; }
  }
}
