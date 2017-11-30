using System;
using System.Threading.Tasks;
using JT_Transport.Model;
using Microsoft.AspNetCore.Mvc;
using SL = JT_Transport.Logger.ServerSideLogger;
using AL = JT_Transport.Logger.ActivityLogger;
using MH = JT_Transport.Helper.MongoHelper;
using TH = JT_Transport.Helper.TokenHelper;
using GH = JT_Transport.Helper.GlobalHelper;
using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;
using Swashbuckle.AspNetCore.Examples;
using JT_Transport.Swagger;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using Microsoft.Extensions.Options;
using JT_Transport.Model.Repositories;
using JT_Transport.Model.JWT;
using JT_Transport.Logger;

namespace JT_Transport.Controllers
{
  /// <summary>
  /// Controller for all authentication methods
  /// </summary>
  [Produces("application/json")]
  [Route("api/[controller]")]
  public class AuthController : Controller
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
    /// <summary>
    /// 
    /// </summary>
    public IMongoCollection<RegisterModel> usersCollection;
    /// <summary></summary>
    public IMongoDatabase log_db;
    /// <summary>
    /// 
    /// </summary>
    public IMongoCollection<ActivityLoggerModel> activitylog_collection;
    /// <summary></summary>
    private IOptions<Audience> _settings;
    /// <summary></summary>
    private IRTokenRepository _repo;
    /// <summary></summary>
    public PasswordHasher<RegisterModel> passwordHasher = new PasswordHasher<RegisterModel>();

    /// <summary></summary>
    /// <param name="settings"></param>
    /// <param name="repo"></param>
    public AuthController(IOptions<Audience> settings, IRTokenRepository repo)
    {
      _client = MH.GetClient();
      auth_db = _client.GetDatabase("AuthDB");
      users_collection = auth_db.GetCollection<BsonDocument>("Users");
      usersCollection = auth_db.GetCollection<RegisterModel>("Users");
      log_db = _client.GetDatabase("LogDB");
      activitylog_collection = log_db.GetCollection<ActivityLoggerModel>("ActivityLog");
      this._settings = settings;
      this._repo = repo;
    }

    /// <summary>
    /// Login in user
    /// </summary>
    /// <param name="data">User credentials</param>
    /// <returns></returns>
    /// <response code="999">User logged in successfully and returns jwt token</response>
    /// <response code="401">User not verified</response>
    /// <response code="402">User not active</response>
    /// <response code="403">Incorrect passowrd</response>
    /// <response code="909">Can not add token to database</response>
    /// <response code="404">User not found</response>
    /// <response code="400">Process ran into an exception</response>
    [HttpPost("login")]
    [SwaggerRequestExample(typeof(LoginModel), typeof(Example_LoginModel))]
    [ProducesResponseType(typeof(ResponseData), 200)]
    public ActionResult Login([FromBody]LoginModel data)
    {
      try
      {
        var check = MH.CheckForData(users_collection, "UserName", data.UserName, null, null).Result;
        if (check == true)
        {
          var getUser = BsonSerializer.Deserialize<RegisterModel>(MH.GetSingleObject(users_collection, "UserName", data.UserName, null, null).Result);
          if (getUser.UserVerified == true && getUser.IsActive == true)
          {
            RegisterModel registerModel = new RegisterModel { UserName = data.UserName, FullName = getUser.FullName, Password = data.Password };
            if (passwordHasher.VerifyHashedPassword(registerModel, getUser.Password, data.Password).ToString() == "Success")
            {
              Parameters parameters = new Parameters { username = getUser.UserName, fullname = getUser.FullName };
              var result = TH.DoPassword(parameters, _repo, _settings, users_collection);
              return Ok(Json(result));
            }
            else
            {
              return BadRequest(new ResponseData
              {
                Code = "402",
                Message = "Incorrect password"
              });
            }
          }
          else
          {
            if (getUser.UserVerified == false)
            {
              return BadRequest(new ResponseData
              {
                Code = "401",
                Message = "User not verified"
              });
            }
            else
            {
              return BadRequest(new ResponseData
              {
                Code = "402",
                Message = "User not active"
              });
            }
          }
        }
        else
        {
          return BadRequest(new ResponseData
          {
            Code = "404",
            Message = "User not found"
          });
        }
      }
      catch (Exception ex)
      {
        SL.CreateLog("AuthController", "Login", ex.Message);
        return BadRequest(new ResponseData
        {
          Code = "400",
          Message = "Failed",
          Data = ex.Message
        });
      }
    }

    /// <summary>
    /// Register new user
    /// </summary>
    /// <param name="data">Details of user</param>
    /// <returns></returns>
    /// <response code="200">User registered successfully</response>
    /// <response code="401">User already registered</response>
    /// <response code="402">User already registered and is made active</response>
    /// <response code="400">Process ran into an exception</response>
    [HttpPost("register")]
    [SwaggerRequestExample(typeof(RegisterModel), typeof(Example_RegisterModel))]
    [ProducesResponseType(typeof(ResponseData), 200)]
    public async Task<ActionResult> Register([FromBody]RegisterModel data)
    {
      try
      {
        var getUser = MH.GetSingleObject(users_collection, "UserName", data.UserName, null, null).Result;
        if (getUser == null)
        {
          data.Password = passwordHasher.HashPassword(data, data.Password);
          data.UserRole = "User";
          data.UserVerified = false;
          data.IsActive = false;
          await usersCollection.InsertOneAsync(data);
          AL.CreateLog(null, "Register", null, data, activitylog_collection);
          return Ok(new ResponseData
          {
            Code = "200",
            Message = "Registered"
          });
        }
        else
        {
          var userDetails = BsonSerializer.Deserialize<RegisterModel>(getUser);
          if (userDetails.IsActive == true)
          {
            return BadRequest(new ResponseData
            {
              Code = "401",
              Message = "User already registered"
            });
          }
          else
          {
            var updateDefinition = Builders<BsonDocument>.Update.Set("IsActive", true).Set("UserVerified", false);
            var update = MH.UpdateSingleObject(users_collection, "UserName", data.UserName, null, null, updateDefinition);
            var details = userDetails;
            details.IsActive = true;
            details.UserVerified = false;
            AL.CreateLog(null, "Register", userDetails, details, activitylog_collection);
            return BadRequest(new ResponseData
            {
              Code = "402",
              Message = "User already registered and is made active"
            });
          }
        }
      }
      catch (Exception ex)
      {
        SL.CreateLog("AuthController", "Register", ex.Message);
        return BadRequest(new ResponseData
        {
          Code = "400",
          Message = "Failed",
          Data = ex.Message
        });
      }
    }

    /// <summary>
    /// Verify registered user
    /// </summary>
    /// <param name="adminname">Name of admin who verifies the user</param>
    /// <param name="username">UserName of user</param>
    /// <returns></returns>
    /// <response code="200">User verified successfully</response>
    /// <response code="401">User not found</response>
    /// <response code="400">Process ran into an exception</response>
    [HttpPost("verifyuser/{adminname}/{username}")]
    [ProducesResponseType(typeof(ResponseData), 200)]
    public ActionResult VerifyUser(string adminname, string username)
    {
      try
      {
        var check = MH.CheckForData(users_collection, "UserName", username, null, null).Result;
        if (check == true)
        {
          var updateDefinition = Builders<BsonDocument>.Update.Set("UserVerified", true).Set("IsActive", true);
          var update = MH.UpdateSingleObject(users_collection, "UserName", username, null, null, updateDefinition);
          AL.CreateLog(adminname, "VerifyUser", null, MH.GetSingleObject(users_collection, "UserName", username, null, null), activitylog_collection);
          return Ok(new ResponseData
          {
            Code = "200",
            Message = "User verified"
          });
        }
        else
        {
          return BadRequest(new ResponseData
          {
            Code = "404",
            Message = "User not found"
          });
        }
      }
      catch (Exception ex)
      {
        SL.CreateLog("AuthController", "VerifyUser", ex.Message);
        return BadRequest(new ResponseData
        {
          Code = "400",
          Message = "Failed",
          Data = ex.Message
        });
      }
    }

    /// <summary>
    /// Deactivate user
    /// </summary>
    /// <param name="adminname">Name of admin who verifies the user</param>
    /// <param name="username">UserName of user</param>
    /// <returns></returns>
    /// <response code="200">User deactivated successfully</response>
    /// <response code="401">User not found</response>
    /// <response code="400">Process ran into an exception</response>
    [HttpDelete("deactivateuser/{adminname}/{username}")]
    public ActionResult DeactivateUser(string adminname, string username)
    {
      try
      {
        var getUser = MH.GetSingleObject(users_collection, "UserName", username, null, null).Result;
        if (getUser != null)
        {
          var updateDefinition = Builders<BsonDocument>.Update.Set("IsActive", false);
          var update = MH.UpdateSingleObject(users_collection, "UserName", username, null, null, updateDefinition);
          var userDetails = BsonSerializer.Deserialize<RegisterModel>(getUser);
          var data = userDetails;
          data.IsActive = false;
          AL.CreateLog(adminname, "DeactivateUser", userDetails, data, activitylog_collection);
          return Ok(new ResponseData
          {
            Code = "200",
            Message = "User deactivated"
          });
        }
        else
        {
          return BadRequest(new ResponseData
          {
            Code = "404",
            Message = "User not found"
          });
        }
      }
      catch (Exception ex)
      {
        SL.CreateLog("AuthController", "DeactivateUser", ex.Message);
        return BadRequest(new ResponseData
        {
          Code = "400",
          Message = "Failed",
          Data = ex.Message
        });
      }
    }
  }
}
