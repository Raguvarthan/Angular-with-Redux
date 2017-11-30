using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace JT_Transport.Model.Repositories
{
  /// <summary>Interface for Token Repository</summary>
  public interface IRTokenRepository
  {
    /// <summary>Add token</summary>
    /// <param name="token"></param>
    Task<bool> AddToken(RToken token);
    /// <summary>Expire token</summary>
    /// <param name="token"></param>
    Task<bool> ExpireToken(RToken token);
    /// <summary>Get token</summary>
    /// <param name="refresh_token"></param>
    /// <param name="client_id"></param>
    Task<RToken> GetToken(string refresh_token, string client_id);
  }

  /// <summary>Contains datas regrading RToken</summary>
  public class RToken
  {
    /// <summary></summary>
    public string Id { get; set; }
    /// <summary></summary>
    [BsonElement("client_id")]
    public string ClientId { get; set; }
    /// <summary></summary>
    [BsonElement("refresh_token")]
    public string RefreshToken { get; set; }
    /// <summary></summary>
    [BsonElement("isstop")]
    public int IsStop { get; set; }
  }
}
