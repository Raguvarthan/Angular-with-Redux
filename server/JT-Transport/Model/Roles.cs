using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;

namespace JT_Transport.Model
{
  /// <summary>
  /// Details of role
  /// </summary>
  public class Roles
  {
    /// <summary>
    /// Object id given by mongo db
    /// </summary>
    public ObjectId Id { get; set; }
    /// <summary>
    /// Id of role
    /// </summary>
    [Required]
    public int? RoleId { get; set; }
    /// <summary>
    /// Name of role
    /// </summary>
    [Required]
    public string RoleName { get; set; }
    /// <summary>
    /// Levels of access given to the role
    /// </summary>
    [Required]
    public string[] LevelOfAccess { get; set; }
    /// <summary>
    /// Defines if the vehicle is active or not
    /// </summary>
    [Required]
    public bool? IsActive { get; set; }
  }
}
