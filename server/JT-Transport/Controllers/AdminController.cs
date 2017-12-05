using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using SL = JT_Transport.Logger.ServerSideLogger;
using AL = JT_Transport.Logger.ActivityLogger;
using MH = JT_Transport.Helper.MongoHelper;
using GH = JT_Transport.Helper.GlobalHelper;
using JT_Transport.Model;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using Swashbuckle.AspNetCore.Examples;
using JT_Transport.Swagger;
using JT_Transport.Logger;

namespace JT_Transport.Controllers
{
  /// <summary>
  /// Method ot perform all admin operations
  /// </summary>
  [Produces("application/json")]
  [Route("api/[controller]")]
  public class AdminController : Controller
  {
    /// <summary>
    /// 
    /// </summary>
    public MongoClient _client;
    /// <summary></summary>
    public IMongoDatabase role_db;
    /// <summary>
    /// 
    /// </summary>
    public IMongoCollection<BsonDocument> roles_collection;
    /// <summary>
    /// 
    /// </summary>
    public IMongoCollection<Roles> rolesCollection;
    /// <summary></summary>
    public IMongoDatabase log_db;
    /// <summary>
    /// 
    /// </summary>
    public IMongoCollection<ActivityLoggerModel> activitylog_collection;
    /// <summary>
    /// 
    /// </summary>
    public BsonDocument update;

    /// <summary>
    /// 
    /// </summary>
    public AdminController()
    {
      _client = MH.GetClient();
      role_db = _client.GetDatabase("RoleDB");
      roles_collection = role_db.GetCollection<BsonDocument>("Roles");
      log_db = _client.GetDatabase("LogDB");
      activitylog_collection = log_db.GetCollection<ActivityLoggerModel>("ActivityLog");
      rolesCollection = role_db.GetCollection<Roles>("Roles");
    }

    /// <summary>
    /// Get all the roles
    /// </summary>
    /// <returns></returns>
    /// <response code="200">Returns all the roles added to db</response>
    /// <response code="404">No roles found</response>
    /// <response code="400">Process ran into an exception</response>
    [HttpGet("roles")]
    [ProducesResponseType(typeof(ResponseData), 200)]
    public ActionResult GetAllRoles()
    {
      try
      {
        var getRoles = MH.GetListOfObjects(roles_collection, null, null, null, null).Result;
        if (getRoles != null)
        {
          List<Roles> roles = new List<Roles>();
          foreach (var role in getRoles)
          {
            roles.Add(BsonSerializer.Deserialize<Roles>(role));
          }
          return Ok(new ResponseData
          {
            Code = "200",
            Message = "Success",
            Data = roles
          });
        }
        else
        {
          return BadRequest(new ResponseData
          {
            Code = "404",
            Message = "No roles found"
          });
        }
      }
      catch (Exception ex)
      {
        SL.CreateLog("AdminController", "GetAllRoles", ex.Message);
        return BadRequest(new ResponseData
        {
          Code = "400",
          Message = "Failed",
          Data = ex.Message
        });
      }
    }

    /// <summary>
    /// Get role with given role name
    /// </summary>
    /// <returns></returns>
    /// <param name="rolename">Name of role</param>
    /// <response code="200">Returns details of role with given name</response>
    /// <response code="401">Bad Request</response>
    /// <response code="404">Role not found</response>
    /// <response code="400">Process ran into an exception</response>
    [HttpGet("role/{rolename}")]
    [ProducesResponseType(typeof(ResponseData), 200)]
    public ActionResult GetRole(string rolename)
    {
      try
      {
        if (rolename != null)
        {
          var getRole = MH.GetSingleObject(roles_collection, "RoleName", rolename, null, null).Result;
          if (getRole != null)
          {
            return Ok(new ResponseData
            {
              Code = "200",
              Message = "Success",
              Data = BsonSerializer.Deserialize<Roles>(getRole)
            });
          }
          else
          {
            return BadRequest(new ResponseData
            {
              Code = "404",
              Message = "Role not found"
            });
          }
        }
        else
        {
          return BadRequest(new ResponseData
          {
            Code = "401",
            Message = "Bad Request"
          });
        }
      }
      catch (Exception ex)
      {
        SL.CreateLog("AdminController", "GetRole", ex.Message);
        return BadRequest(new ResponseData
        {
          Code = "400",
          Message = "Failed",
          Data = ex.Message
        });
      }
    }

    /// <summary>
    /// Insert new role
    /// </summary>
    /// <param name="data">Details of role</param>
    /// <param name="username">name of user who adds the role</param>
    /// <returns></returns>
    /// <response code="200">Role inserted successfully</response>
    /// <response code="401">Role with the same name is found</response>
    /// <response code="402">Role with the same name is found and its made active</response>
    /// <response code="403">Role with same id is already added</response>
    /// <response code="405">Bad Request</response>
    /// <response code="400">Process ran into an exception</response>
    [HttpPost("role/insert/{username}")]
    [SwaggerRequestExample(typeof(Roles), typeof(Example_InsertRole))]
    [ProducesResponseType(typeof(ResponseData), 200)]
    public ActionResult InsertRole([FromBody]Roles data, string username)
    {
      try
      {
        if (data != null && username != null)
        {
          var getRole = MH.GetSingleObject(roles_collection, "RoleName", data.RoleName, null, null).Result;
          if (getRole == null)
          {
            data.IsActive = true;
            var insert = MH.InsertNewRole(data, rolesCollection);
            if (insert == true)
            {
              AL.CreateLog(username, "InsertRole", null, data, activitylog_collection);
              return Ok(new ResponseData
              {
                Code = "200",
                Message = "Inserted",
                Data = data
              });
            }
            else if (insert == false)
            {
              return BadRequest(new ResponseData
              {
                Code = "403",
                Message = "Role with same id is already added"
              });
            }
            else
            {
              return BadRequest(new ResponseData
              {
                Code = "400",
                Message = "Failed",
                Data = insert
              });
            }
          }
          else
          {
            var roleDetails = BsonSerializer.Deserialize<Roles>(getRole);
            if (roleDetails.IsActive == true)
            {
              return BadRequest(new ResponseData
              {
                Code = "401",
                Message = "Role with the same name is found"
              });
            }
            else
            {
              var updateDefinition = Builders<BsonDocument>.Update.Set("IsActive", true);
              update = MH.UpdateSingleObject(roles_collection, "RoleName", data.RoleName, null, null, updateDefinition);
              var detail = roleDetails;
              detail.IsActive = true;
              AL.CreateLog(username, "InsertRole", roleDetails, detail, activitylog_collection);
              return BadRequest(new ResponseData
              {
                Code = "402",
                Message = "Role with the same name is found and is made active"
              });
            }
          }
        }
        else
        {
          return BadRequest(new ResponseData
          {
            Code = "405",
            Message = "Bad request"
          });
        }
      }
      catch (Exception ex)
      {
        SL.CreateLog("AdminController", "InsertRole", ex.Message);
        return BadRequest(new ResponseData
        {
          Code = "400",
          Message = "Failed",
          Data = ex.Message
        });
      }
    }

    /// <summary>
    /// Update details of role
    /// </summary>
    /// <param name="data">Data to be updated</param>
    /// <param name="username">Name of user who makes the update</param>
    /// <param name="rolename">Name of role for whoes details needs to be updated</param>
    /// <returns></returns>
    /// <response code="200">Role updated successfully</response>
    /// <response code="401">Bad Request</response>
    /// <response code="404">Role not found</response>
    /// <response code="400">Process ran into an exception</response>
    [HttpPut("role/{username}/{rolename}")]
    [SwaggerRequestExample(typeof(ExampleModel_Roles), typeof(Example_UpdateRole))]
    [ProducesResponseType(typeof(ResponseData), 200)]
    public ActionResult UpdateRole([FromBody]ExampleModel_Roles data, string username, string rolename)
    {
      try
      {
        if (data != null && username != null && rolename != null)
        {
          var getRole = MH.GetSingleObject(roles_collection, "RoleName", rolename, null, null).Result;
          if (getRole != null)
          {
            if (data.LevelOfAccess != null)
            {
              var updateDefinition = Builders<BsonDocument>.Update.Set("LevelOfAccess", data.LevelOfAccess);
              var update = MH.UpdateSingleObject(roles_collection, "RoleName", rolename, null, null, updateDefinition);
            }
            if (data.IsActive != null)
            {
              var updateDefinition = Builders<BsonDocument>.Update.Set("IsActive", data.IsActive);
              var update = MH.UpdateSingleObject(roles_collection, "RoleName", rolename, null, null, updateDefinition);
            }
            AL.CreateLog(username, "UpdateRole", BsonSerializer.Deserialize<Roles>(getRole), data, activitylog_collection);
            return Ok(new ResponseData
            {
              Code = "200",
              Message = "Updated"
            });
          }
          else
          {
            return BadRequest(new ResponseData
            {
              Code = "404",
              Message = "Role not found"
            });
          }
        }
        else
        {
          return BadRequest(new ResponseData
          {
            Code = "401",
            Message = "Bad request"
          });
        }
      }
      catch (Exception ex)
      {
        SL.CreateLog("AdminController", "UpdateRole", ex.Message);
        return BadRequest(new ResponseData
        {
          Code = "400",
          Message = "Failed",
          Data = ex.Message
        });
      }
    }

    /// <summary>
    /// Make a role inactive
    /// </summary>
    /// <param name="username">Name of user who makes the role inactive</param>
    /// <param name="rolename">Name of role for whoes details needs to be updated</param>
    /// <returns></returns>
    /// <response code="200">Role updated successfully</response>
    /// <response code="401">Bad Request</response>
    /// <response code="404">Role not found</response>
    /// <response code="400">Process ran into an exception</response>
    [HttpDelete("role/{username}/{rolename}")]
    [ProducesResponseType(typeof(ResponseData), 200)]
    public ActionResult MakeRoleInActive(string username, string rolename)
    {
      try
      {
        if (username != null && rolename != null)
        {
          var getRole = MH.GetSingleObject(roles_collection, "RoleName", rolename, null, null).Result;
          if (getRole != null)
          {
            var updateDefinition = Builders<BsonDocument>.Update.Set("IsActive", false);
            update = MH.UpdateSingleObject(roles_collection, "RoleName", rolename, null, null, updateDefinition);
            var data = BsonSerializer.Deserialize<Roles>(getRole);
            data.IsActive = false;
            AL.CreateLog(username, "MakeRoleInActive", BsonSerializer.Deserialize<Roles>(getRole), data, activitylog_collection);
            return Ok(new ResponseData
            {
              Code = "200",
              Message = "Role make inactive"
            });
          }
          else
          {
            return BadRequest(new ResponseData
            {
              Code = "404",
              Message = "Role not found"
            });
          }
        }
        else
        {
          return BadRequest(new ResponseData
          {
            Code = "401",
            Message = "Bad Request"
          });
        }
      }
      catch (Exception ex)
      {
        SL.CreateLog("AdminController", "MakeRoleInActive", ex.Message);
        return BadRequest(new ResponseData
        {
          Code = "400",
          Message = "Failed",
          Data = ex.Message
        });
      }
    }

    /// <summary>
    /// Make a role active
    /// </summary>
    /// <param name="username">Name of user who makes the role active</param>
    /// <param name="rolename">Name of role for whoes details needs to be updated</param>
    /// <returns></returns>
    /// <response code="200">Role updated successfully</response>
    /// <response code="401">Bad Request</response>
    /// <response code="404">Role not found</response>
    /// <response code="400">Process ran into an exception</response>
    [HttpPut("role/makeactive/{username}/{rolename}")]
    [ProducesResponseType(typeof(ResponseData), 200)]
    public ActionResult MakeRoleActive(string username, string rolename)
    {
      try
      {
        if (username != null && rolename != null)
        {
          var getRole = MH.GetSingleObject(roles_collection, "RoleName", rolename, null, null).Result;
          if (getRole != null)
          {
            var updateDefinition = Builders<BsonDocument>.Update.Set("IsActive", true);
            update = MH.UpdateSingleObject(roles_collection, "RoleName", rolename, null, null, updateDefinition);
            var data = BsonSerializer.Deserialize<Roles>(getRole);
            data.IsActive = true;
            AL.CreateLog(username, "MakeRoleActive", BsonSerializer.Deserialize<Roles>(getRole), data, activitylog_collection);
            return Ok(new ResponseData
            {
              Code = "200",
              Message = "Role made active"
            });
          }
          else
          {
            return BadRequest(new ResponseData
            {
              Code = "404",
              Message = "Role not found"
            });
          }
        }
        else
        {
          return BadRequest(new ResponseData
          {
            Code = "401",
            Message = "Bad Request"
          });
        }
      }
      catch (Exception ex)
      {
        SL.CreateLog("AdminController", "MakeRoleActive", ex.Message);
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
