using System;
using JT_Transport.Model;
using JT_Transport.Model.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Examples;
using TH = JT_Transport.Helper.TokenHelper;
using SL = JT_Transport.Logger.ServerSideLogger;
using GH = JT_Transport.Helper.GlobalHelper;
using MH = JT_Transport.Helper.MongoHelper;
using JT_Transport.Swagger;
using JT_Transport.Model.JWT;
using MongoDB.Driver;
using MongoDB.Bson;
using JT_Transport.Logger;

namespace JT_Transport.Controllers
{
  /// <summary>Controller to get JWT token</summary>
  [Produces("application/json")]
  [Route("api/token")]
  public class TokenController : Controller
  {
    /// <summary>
    /// 
    /// </summary>
    public MongoClient _client;
    /// <summary></summary>
    public IMongoDatabase auth_db;
    /// <summary>
    /// 
    /// </summary>
    public IMongoCollection<BsonDocument> users_collection;
    /// <summary></summary>
    private IOptions<Audience> _settings;
    /// <summary></summary>
    private IRTokenRepository _repo;

    /// <summary></summary>
    /// <param name="settings"></param>
    /// <param name="repo"></param>
    public TokenController(IOptions<Audience> settings, IRTokenRepository repo)
    {
      _client = MH.GetClient();
      auth_db = _client.GetDatabase("AuthDB");
      users_collection = auth_db.GetCollection<BsonDocument>("Users");
      this._settings = settings;
      this._repo = repo;
    }

    /// <summary>Get JWT token</summary>
    /// <param name="parameters"></param>
    /// <response code="999">Returns JWT token</response> 
    /// <response code="909">Can not add token to database</response> 
    /// <response code="905">Can not refresh token</response> 
    /// <response code="906">Refresh token has expired</response> 
    /// <response code="910">Can not expire token or a new token</response> 
    /// <response code="901">Null of parameters</response> 
    /// <response code="904">Bad request</response> 
    /// <response code="400">If process run into a exception</response> 
    [HttpGet("auth")]
    [SwaggerRequestExample(typeof(Parameters), typeof(Example_GetJWT))]
    [ProducesResponseType(typeof(ResponseData), 999)]
    public ActionResult GetJWT([FromQuery]Parameters parameters)
    {
      try
      {
        if (parameters == null)
        {
          return Json(new ResponseData
          {
            Code = "901",
            Message = "null of parameters",
            Data = null
          });
        }

        if (parameters.grant_type == "password")
        {
          return Ok(Json(TH.DoPassword(parameters, _repo, _settings, users_collection)));
        }
        else if (parameters.grant_type == "refresh_token")
        {
          return Ok(Json(TH.DoRefreshToken(parameters, _repo, _settings, users_collection)));
        }
        else
        {
          return Json(new ResponseData
          {
            Code = "904",
            Message = "bad request",
            Data = null
          });
        }
      }
      catch (Exception ex)
      {
        SL.CreateLog("TokenController", "GetJWT", ex.Message);
        return BadRequest(new ResponseData
        {
          Code = "400",
          Message = "Failed",
          Data = null
        });
      }
    }
  }
}
