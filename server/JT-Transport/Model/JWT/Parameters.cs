
namespace JT_Transport.Model
{
  /// <summary>Contains parameters to be sent through responce</summary>
  public class Parameters
  {
    /// <summary></summary>
    public string grant_type { get; set; }
    /// <summary></summary>
    public string refresh_token { get; set; }
    /// <summary></summary>
    public string client_id { get; set; }
    /// <summary></summary>
    public string client_secret { get; set; }
    /// <summary>Username of user who get the jwt</summary>
    public string username { get; set; }
    /// <summary>Password of user who get jwt</summary>
    public string password { get; set; }
    /// <summary>Fullname of user who get jwt</summary>
    public string fullname { get; set; }
  }
}
