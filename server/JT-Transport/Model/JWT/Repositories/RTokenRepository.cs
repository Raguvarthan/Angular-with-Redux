using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using MH = JT_Transport.Helper.MongoHelper;

namespace JT_Transport.Model.Repositories
{
  /// <summary>Repository for Token</summary>
  public class RTokenRepository : IRTokenRepository
  {
    /// <summary>
    /// 
    /// </summary>
    public static MongoClient _client;
    /// <summary></summary>
    public static IMongoDatabase token_db;

    /// <summary></summary>
    public RTokenRepository()
    {
      _client = MH.GetClient();
      token_db = _client.GetDatabase("TokenDB");
    }

    /// <summary>Add token</summary>
    /// <param name="token"></param>
    public async Task<bool> AddToken(RToken token)
    {
      var collection = token_db.GetCollection<RToken>("RToken");
      try
      {
        await collection.InsertOneAsync(token);
        return true;
      }
      catch
      {
        return false;
      }
    }
    /// <summary>Expire token</summary>
    /// <param name="token"></param>
    public async Task<bool> ExpireToken(RToken token)
    {
      var filter = Builders<RToken>.Filter.Eq("client_id", token.ClientId) & Builders<RToken>.Filter.Eq("refresh_token", token.RefreshToken); ;
      var update = Builders<RToken>.Update.Set("isstop", token.IsStop);
      var collection = token_db.GetCollection<RToken>("RToken");
      var result = await collection.UpdateOneAsync(filter, update);
      return result.ModifiedCount > 0;
    }

    /// <summary>Get token</summary>
    /// <param name="refresh_token"></param>
    /// <param name="client_id"></param>
    public async Task<RToken> GetToken(string refresh_token, string client_id)
    {
      var filter = "{ client_id: '" + client_id + "' , refresh_token: '" + refresh_token + "'}";
      var collection = token_db.GetCollection<RToken>("RToken");
      IAsyncCursor<RToken> cursor = await collection.FindAsync(filter);
      return cursor.FirstOrDefault();
    }
  }
}
