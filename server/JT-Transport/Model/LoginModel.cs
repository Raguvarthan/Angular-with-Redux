using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;

namespace JT_Transport.Model
{
  /// <summary>
  /// Login model for authentication
  /// </summary>
  public class LoginModel
  {
    /// <summary>
    /// Username of user
    /// </summary>
    [Required]
    public string UserName { get; set; }
    /// <summary>
    /// Password of user
    /// </summary>
    [Required]
    public string Password { get; set; }
  }

  /// <summary>
  /// Register model for authentication
  /// </summary>
  public class RegisterModel
  {
    /// <summary>
    /// Object id given by mongo db
    /// </summary>
    public ObjectId Id { get; set; }
    /// <summary>
    /// Username of user
    /// </summary>
    [Required]
    public string UserName { get; set; }
    /// <summary>
    /// Fullname of user
    /// </summary>
    [Required]
    public string FullName { get; set; }
    /// <summary>
    /// Password of user
    /// </summary>
    [Required]
    public string Password { get; set; }
    /// <summary>
    /// Role of user
    /// </summary>
    public string UserRole { get; set; }
    /// <summary>
    /// Flag to define if the user is verified or not
    /// </summary>
    public bool? UserVerified { get; set; }
    /// <summary>
    /// Defines if the vehicle is active or not
    /// </summary>
    [Required]
    public bool? IsActive { get; set; }
  }
}
