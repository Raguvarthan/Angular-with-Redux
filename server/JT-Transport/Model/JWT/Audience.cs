
namespace JT_Transport.Model.JWT
{
  /// <summary>Contains data to get jwt</summary>
  public class Audience
  {
    /// <summary></summary>
    public string Secret { get; set; }
    /// <summary></summary>
    public string Iss { get; set; }
    /// <summary></summary>
    public string Aud { get; set; }
  }
}
