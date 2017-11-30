using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JT_Transport.Model;
using JT_Transport.Model.JWT;
using JT_Transport.Model.Repositories;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson.Serialization;
using Newtonsoft.Json;
using MH = JT_Transport.Helper.MongoHelper;
using SL = JT_Transport.Logger.ServerSideLogger;
using GH = JT_Transport.Helper.GlobalHelper;
using System.Diagnostics;
using MongoDB.Driver;
using MongoDB.Bson;
using JT_Transport.Logger;

namespace JT_Transport.Helper
{
  /// <summary>
  /// 
  /// </summary>
  public class TokenHelper
  {
    /// <summary>Get the access-token by username and password</summary>
    /// <param name="parameters"></param>
    /// <param name="_repo"></param>
    /// <param name="_settings"></param>
    /// <param name="users_collection"></param>
    public static ResponseData DoPassword(Parameters parameters, IRTokenRepository _repo, IOptions<Audience> _settings, IMongoCollection<BsonDocument> users_collection)
    {
      try
      {
        var refresh_token = Guid.NewGuid().ToString().Replace("-", "");
        var rToken = new RToken
        {
          ClientId = parameters.username,
          RefreshToken = refresh_token,
          Id = Guid.NewGuid().ToString(),
          IsStop = 0
        };
        if (_repo.AddToken(rToken).Result)
        {
          dynamic UserInfo = new System.Dynamic.ExpandoObject();
          UserInfo.FirstName = parameters.fullname;
          UserInfo.UserName = parameters.username;
          return new ResponseData
          {
            Code = "999",
            Message = "OK",
            Content = UserInfo,
            Data = GetJwt(parameters.username, refresh_token, _settings, BsonSerializer.Deserialize<RegisterModel>(MH.GetSingleObject(users_collection, "UserName", parameters.username, null, null).Result).UserRole)
          };
        }
        else
        {
          return new ResponseData
          {
            Code = "909",
            Message = "can not add token to database",
            Data = null
          };
        }
      }
      catch (Exception ex)
      {
        SL.CreateLog("TokenHelper", "DoPassword", ex.Message);
        return new ResponseData
        {
          Code = "400",
          Message = "Failed",
          Data = null
        };
      }
    }

    /// <summary>Get the access_token by refresh_token</summary>
    /// <param name="parameters"></param>
    /// <param name="_repo"></param>
    /// <param name="_settings"></param>
    /// <param name="users_collection"></param>
    public static ResponseData DoRefreshToken(Parameters parameters, IRTokenRepository _repo, IOptions<Audience> _settings, IMongoCollection<BsonDocument> users_collection)
    {
      try
      {
        try
        {
          var token = _repo.GetToken(parameters.refresh_token, parameters.client_id).Result;
          if (token == null)
          {
            return new ResponseData
            {
              Code = "905",
              Message = "can not refresh token",
              Data = null
            };
          }
          if (token.IsStop == 1)
          {
            return new ResponseData
            {
              Code = "906",
              Message = "refresh token has expired",
              Data = null
            };
          }
          var refresh_token = Guid.NewGuid().ToString().Replace("-", "");
          token.IsStop = 1;
          var updateFlag = _repo.ExpireToken(token).Result;
          var addFlag = _repo.AddToken(new RToken
          {
            ClientId = parameters.client_id,
            RefreshToken = refresh_token,
            Id = Guid.NewGuid().ToString(),
            IsStop = 0
          });
          if (updateFlag && addFlag.Result)
          {
            return new ResponseData
            {
              Code = "999",
              Message = "OK",
              Data = GetJwt(parameters.client_id, refresh_token, _settings, BsonSerializer.Deserialize<RegisterModel>(MH.GetSingleObject(users_collection, "UserName", parameters.client_id, null, null).Result).UserRole)
            };
          }
          else
          {
            return new ResponseData
            {
              Code = "910",
              Message = "can not expire token or a new token",
              Data = null
            };
          }
        }
        catch (Exception ex)
        {
          SL.CreateLog("TokenHelper", "DoRefreshToken", ex.Message);
          return new ResponseData
          {
            Code = "400",
            Message = "Failed",
            Data = null
          };
        }
      }
      catch (Exception ex)
      {
        SL.CreateLog("TokenHelper", "DoRefreshToken", ex.Message);
        return new ResponseData
        {
          Code = "400",
          Message = "Failed"
        };
      }
    }

    /// <summary>Get JWT</summary>
    /// <param name="username"></param>
    /// <param name="refreshToken"></param>
    /// <param name="_settings"></param>
    /// <param name="userRole"></param>
    public static string GetJwt(string username, string refreshToken, IOptions<Audience> _settings, string userRole)
    {
      try
      {
        var now = DateTime.UtcNow;
        var claims = new Claim[]
        {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, now.ToUniversalTime().ToString(), ClaimValueTypes.Integer64),
                new Claim(ClaimTypes.Role,userRole)
        };
        var symmetricKeyAsBase64 = _settings.Value.Secret;
        var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);
        var signingKey = new SymmetricSecurityKey(keyByteArray);
        var jwt = new JwtSecurityToken(
            issuer: _settings.Value.Iss,
            audience: _settings.Value.Aud,
            claims: claims,
            notBefore: now,
            expires: now.Add(TimeSpan.FromMinutes(5)),
            signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256));
        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
        var response = new
        {
          accessToken = encodedJwt,
          expiresIn = (int)TimeSpan.FromMinutes(1).TotalSeconds,
          refreshToken = refreshToken,
        };
        return JsonConvert.SerializeObject(response, new JsonSerializerSettings { Formatting = Formatting.Indented });
      }
      catch (Exception ex)
      {
        SL.CreateLog("TokenHelper", "GetJwt", ex.Message);
        return null;
      }
    }
  }
}
