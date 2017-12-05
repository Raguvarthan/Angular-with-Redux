using System;
using System.Collections.Generic;
using System.Linq;
using JT_Transport.Logger;
using JT_Transport.Model;
using JT_Transport.Swagger;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Swashbuckle.AspNetCore.Examples;
using MH = JT_Transport.Helper.MongoHelper;
using SL = JT_Transport.Logger.ServerSideLogger;
using AL = JT_Transport.Logger.ActivityLogger;

namespace JT_Transport.Controllers
{
  /// <summary>
  /// Controller for all driver related methods
  /// </summary>
  [Produces("application/json")]
  [Route("api/[controller]")]
  public class DriverController : Controller
  {
    /// <summary>
    /// 
    /// </summary>
    public MongoClient _client;
    /// <summary></summary>
    public IMongoDatabase driver_db;
    /// <summary>
    /// 
    /// </summary>
    public IMongoCollection<BsonDocument> driverinfo_collection;
    /// <summary>
    /// 
    /// </summary>
    public IMongoCollection<DriverInfo> driverinfoCollection;
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
    public DriverController()
    {
      _client = MH.GetClient();
      driver_db = _client.GetDatabase("DriverDB");
      driverinfo_collection = driver_db.GetCollection<BsonDocument>("DriverInfo");
      log_db = _client.GetDatabase("LogDB");
      activitylog_collection = log_db.GetCollection<ActivityLoggerModel>("ActivityLog");
      driverinfoCollection = driver_db.GetCollection<DriverInfo>("DriverInfo");
    }

    /// <summary>
    /// Get all the drivers and their info
    /// </summary>
    /// <response code="200">Returns all the drivers and their info</response>
    /// <response code="404">No drivers found</response>
    /// <response code="400">Process ran into an exception</response>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(ResponseData), 200)]
    public ActionResult GetAllDrivers()
    {
      try
      {
        var getDrivers = MH.GetListOfObjects(driverinfo_collection, null, null, null, null).Result;
        if (getDrivers != null)
        {
          List<DriverInfo> driverInfo = new List<DriverInfo>();
          foreach (var driver in getDrivers)
          {
            driverInfo.Add(BsonSerializer.Deserialize<DriverInfo>(driver));
          }
          return Ok(new ResponseData
          {
            Code = "200",
            Message = "Success",
            Data = driverInfo
          });
        }
        else
        {
          return BadRequest(new ResponseData
          {
            Code = "404",
            Message = "No drivers found"
          });
        }
      }
      catch (Exception ex)
      {
        SL.CreateLog("DriverController", "GetAllDrivers", ex.Message);
        return BadRequest(new ResponseData
        {
          Code = "400",
          Message = "Failed",
          Data = ex.Message
        });
      }
    }

    /// <summary>
    /// Gets info of driver with given driver id
    /// </summary>
    /// <param name="driverId">Id of driver</param>
    /// <response code="200">Returns info of driver with given driver id</response>
    /// <response code="401">Bad Request</response>
    /// <response code="404">Drivers not found</response>
    /// <response code="400">Process ran into an exception</response>
    /// <returns></returns>
    [HttpGet("{driverId}")]
    [ProducesResponseType(typeof(ResponseData), 200)]
    public ActionResult GetInfoOfdriver(string driverId)
    {
      try
      {
        if (driverId != null)
        {
          var getDriver = MH.GetSingleObject(driverinfo_collection, "DriverId", driverId, null, null).Result;
          if (getDriver != null)
          {
            var driverInfo = BsonSerializer.Deserialize<DriverInfo>(getDriver);
            return Ok(new ResponseData
            {
              Code = "200",
              Message = "Success",
              Data = driverInfo
            });
          }
          else
          {
            return BadRequest(new ResponseData
            {
              Code = "404",
              Message = "Driver not found"
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
        SL.CreateLog("DriverController", "GetInfoOfDriver", ex.Message);
        return BadRequest(new ResponseData
        {
          Code = "400",
          Message = "Failed",
          Data = ex.Message
        });
      }
    }

    /// <summary>
    /// Insert new driver info
    /// </summary>
    /// <param name="data">Info of driver</param>
    /// <param name="username">UserName of user</param>
    /// <response code="200">Driver info inserted successfully</response>
    /// <response code="401">Driver with same driver name and address is already added</response>
    /// <response code="402">Driver info with same id is already added</response>
    /// <response code="403">Bad Request</response>
    /// <response code="400">Process ran into an exception</response>
    /// <returns></returns>
    [HttpPost("{username}")]
    [SwaggerRequestExample(typeof(DriverInfo), typeof(Example_InsertDriverInfo))]
    [ProducesResponseType(typeof(ResponseData), 200)]
    public ActionResult InsertDriverInfo([FromBody]DriverInfo data, string username)
    {
      try
      {
        if (data != null && username != null)
        {
          var checkDriver = MH.GetSingleObject(driverinfo_collection, "DriverName", data.DriverName, "Address", data.Address).Result;
          if (checkDriver != null)
          {
            return BadRequest(new ResponseData
            {
              Code = "401",
              Message = "Driver with same driver name and address is already added"
            });
          }
          else
          {
            #region Calculate driver id
            var getDrivers = MH.GetListOfObjects(driverinfo_collection, null, null, null, null).Result;
            if (getDrivers.Count == 0)
            {
              data.DriverId = "DR-1";
            }
            else
            {
              List<long> idList = new List<long>();
              foreach (var driver in getDrivers)
              {
                DriverInfo driverInfo = BsonSerializer.Deserialize<DriverInfo>(driver);
                long seperatedId = Convert.ToInt64(driverInfo.DriverId.Substring(driverInfo.DriverId.LastIndexOf('-') + 1));
                idList.Add(seperatedId);
              }
              var maxId = idList.Max();
              data.DriverId = "DR-" + (maxId + 1);
            }
            #endregion
            data.IsActive = true;
            var insert = MH.InsertNewDriverInfo(data, driverinfoCollection);
            if (insert == true)
            {
              AL.CreateLog(username, "InsertDriverInfo", null, data, activitylog_collection);
              return Ok(new ResponseData
              {
                Code = "200",
                Message = "Inserted",
                Data = data
              });
            }
            else
            {
              return BadRequest(new ResponseData
              {
                Code = "402",
                Message = "Driver info with same id is already added"
              });
            }
          }
        }
        else
        {
          return BadRequest(new ResponseData
          {
            Code = "403",
            Message = "Bad Request"
          });
        }
      }
      catch (Exception ex)
      {
        SL.CreateLog("DriverController", "InsertDriverInfo", ex.Message);
        return BadRequest(new ResponseData
        {
          Code = "400",
          Message = "Failed",
          Data = ex.Message
        });
      }
    }

    /// <summary>
    /// Update driver info
    /// </summary>
    /// <param name="data">Data to be updated</param>
    /// <param name="username">UserName of user</param>
    /// <param name="driverId">Id of driver</param>
    /// <returns></returns>
    /// <response code="200">Driver info updated successfully </response>
    /// <response code="401">BadRequest</response>
    /// <response code="404">Driver not found</response>
    /// <response code="400">Process ran into an exception</response>
    [HttpPut("{username}/{driverId}")]
    [SwaggerRequestExample(typeof(ExampleModel_DriverInfo), typeof(Example_UpdateDriverInfo))]
    [ProducesResponseType(typeof(ResponseData), 200)]
    public ActionResult UpdateDriverInfo([FromBody]ExampleModel_DriverInfo data, string username, string driverId)
    {
      try
      {
        if (data != null && username != null && driverId != null)
        {
          if (data.DriverName != null)
          {
            var updateDefinition = Builders<BsonDocument>.Update.Set("DriverName", data.DriverName);
            update = MH.UpdateSingleObject(driverinfo_collection, "DriverId", driverId, null, null, updateDefinition);
          }
          if (data.ContactNo != 0)
          {
            var updateDefinition = Builders<BsonDocument>.Update.Set("ContactNo", data.ContactNo);
            update = MH.UpdateSingleObject(driverinfo_collection, "DriverId", driverId, null, null, updateDefinition);
          }
          if (data.Address != null)
          {
            var updateDefinition = Builders<BsonDocument>.Update.Set("Address", data.Address);
            update = MH.UpdateSingleObject(driverinfo_collection, "DriverId", driverId, null, null, updateDefinition);
          }
          if (data.IsActive != null)
          {
            var updateDefinition = Builders<BsonDocument>.Update.Set("IsActive", data.IsActive);
            update = MH.UpdateSingleObject(driverinfo_collection, "DriverId", driverId, null, null, updateDefinition);
          }
          if (update != null)
          {
            AL.CreateLog(username, "UpdateDriverInfo", BsonSerializer.Deserialize<DriverInfo>(update), data, activitylog_collection);
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
              Message = "Driver info not found"
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
        SL.CreateLog("DriverController", "UpdateDriverInfo", ex.Message);
        return BadRequest(new ResponseData
        {
          Code = "400",
          Message = "Failed",
          Data = ex.Message
        });
      }
    }

    /// <summary>
    /// Make driver info inactive
    /// </summary>
    /// <param name="username">UserName of user</param>
    /// <param name="driverId">Id of driver</param>
    /// <returns></returns>
    /// <response code="200">Driver info made inactive</response>
    /// <response code="401">Bad Request</response>
    /// <response code="404">Driver info not found</response>
    /// <response code="400">Process ran into an exception</response>
    [HttpDelete("{username}/{driverId}")]
    [ProducesResponseType(typeof(ResponseData), 200)]
    public ActionResult MakeDriverInfoInActive(string username, string driverId)
    {
      try
      {
        if (username != null && driverId != null)
        {
          var getDriver = MH.GetSingleObject(driverinfo_collection, "DriverId", driverId, null, null).Result;
          if (getDriver != null)
          {
            var updateDefinition = Builders<BsonDocument>.Update.Set("IsActive", false);
            update = MH.UpdateSingleObject(driverinfo_collection, "DriverId", driverId, null, null, updateDefinition);
            var data = BsonSerializer.Deserialize<DriverInfo>(getDriver);
            data.IsActive = false;
            AL.CreateLog(username, "MakeDriverInfoInActive", BsonSerializer.Deserialize<DriverInfo>(getDriver), data, activitylog_collection);
            return Ok(new ResponseData
            {
              Code = "200",
              Message = "Driver info made inactive"
            });
          }
          else
          {
            return BadRequest(new ResponseData
            {
              Code = "404",
              Message = "Driver info not found"
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
        SL.CreateLog("driverController", "MakeDriverInfoInActive", ex.Message);
        return BadRequest(new ResponseData
        {
          Code = "400",
          Message = "Failed",
          Data = ex.Message
        });
      }
    }

    /// <summary>
    /// Make driver info active
    /// </summary>
    /// <param name="username">UserName of user</param>
    /// <param name="driverId">Id of Driver</param>
    /// <returns></returns>
    /// <response code="200">Driver info made active</response>
    /// <response code="401">Bad Request</response>
    /// <response code="404">Driver info not found</response>
    /// <response code="400">Process ran into an exception</response>
    [HttpPut("makeactive/{username}/{driverId}")]
    [ProducesResponseType(typeof(ResponseData), 200)]
    public ActionResult MakeDriverInfoActive(string username, string driverId)
    {
      try
      {
        if (User != null && driverId != null)
        {
          var getDriver = MH.GetSingleObject(driverinfo_collection, "DriverId", driverId, null, null).Result;
          if (getDriver != null)
          {
            var updateDefinition = Builders<BsonDocument>.Update.Set("IsActive", true);
            update = MH.UpdateSingleObject(driverinfo_collection, "DriverId", driverId, null, null, updateDefinition);
            var data = BsonSerializer.Deserialize<DriverInfo>(getDriver);
            data.IsActive = true;
            AL.CreateLog(username, "MakeDriverInfoActive", BsonSerializer.Deserialize<DriverInfo>(getDriver), data, activitylog_collection);
            return Ok(new ResponseData
            {
              Code = "200",
              Message = "Driver info made active"
            });
          }
          else
          {
            return BadRequest(new ResponseData
            {
              Code = "404",
              Message = "Driver info not found"
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
        SL.CreateLog("DriverController", "MakeDriverInfoActive", ex.Message);
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
