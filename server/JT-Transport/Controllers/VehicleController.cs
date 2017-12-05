using System;
using System.Threading.Tasks;
using JT_Transport.Model;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Examples;
using SL = JT_Transport.Logger.ServerSideLogger;
using AL = JT_Transport.Logger.ActivityLogger;
using MH = JT_Transport.Helper.MongoHelper;
using GH = JT_Transport.Helper.GlobalHelper;
using MongoDB.Driver;
using JT_Transport.Swagger;
using System.Collections.Generic;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using JT_Transport.Logger;

namespace JT_Transport.Controllers
{
  /// <summary>
  /// Controller for all vehicle related methods
  /// </summary>
  [Produces("application/json")]
  [Route("api/[controller]")]
  public class VehicleController : Controller
  {
    /// <summary>
    /// 
    /// </summary>
    public MongoClient _client;
    /// <summary></summary>
    public IMongoDatabase vehicle_db;
    /// <summary>
    /// 
    /// </summary>
    public IMongoCollection<BsonDocument> vehicleinfo_collection;
    /// <summary>
    /// 
    /// </summary>
    public IMongoCollection<VehicleInfo> vehicleinfoCollection;
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
    public VehicleController()
    {
      _client = MH.GetClient();
      vehicle_db = _client.GetDatabase("VehicleDB");
      vehicleinfo_collection = vehicle_db.GetCollection<BsonDocument>("VehicleInfo");
      log_db = _client.GetDatabase("LogDB");
      activitylog_collection = log_db.GetCollection<ActivityLoggerModel>("ActivityLog");
      vehicleinfoCollection = vehicle_db.GetCollection<VehicleInfo>("VehicleInfo");
    }

    /// <summary>
    /// Get all the vehicles and their info
    /// </summary>
    /// <response code="200">Returns all the vehicles and their info</response>
    /// <response code="404">No vehicles found</response>
    /// <response code="400">Process ran into an exception</response>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(ResponseData), 200)]
    public ActionResult GetAllVehicles()
    {
      try
      {
        var getVehicles = MH.GetListOfObjects(vehicleinfo_collection, null, null, null, null).Result;
        if (getVehicles != null)
        {
          List<VehicleInfo> vehicleInfo = new List<VehicleInfo>();
          foreach (var vehicle in getVehicles)
          {
            vehicleInfo.Add(BsonSerializer.Deserialize<VehicleInfo>(vehicle));
          }
          return Ok(new ResponseData
          {
            Code = "200",
            Message = "Success",
            Data = vehicleInfo
          });
        }
        else
        {
          return BadRequest(new ResponseData
          {
            Code = "404",
            Message = "No vehicles found"
          });
        }
      }
      catch (Exception ex)
      {
        SL.CreateLog("VehicleController", "GetAllVehicle", ex.Message);
        return BadRequest(new ResponseData
        {
          Code = "400",
          Message = "Failed",
          Data = ex.Message
        });
      }
    }

    /// <summary>
    /// Get all the vehicles and their info
    /// </summary>
    /// <param name="vehicleId">Id of vehicle</param>
    /// <response code="200">Returns info of vehicle with given vehicle id</response>
    /// <response code="401">Bad Request</response>
    /// <response code="404">Vehicle not found</response>
    /// <response code="400">Process ran into an exception</response>
    /// <returns></returns>
    [HttpGet("{vehicleId}")]
    [ProducesResponseType(typeof(ResponseData), 200)]
    public ActionResult GetInfoOfVehicle(string vehicleId)
    {
      try
      {
        var getVehicle = MH.GetSingleObject(vehicleinfo_collection, "VehicleId", vehicleId, null, null).Result;
        if (getVehicle != null)
        {
          var vehicleInfo = BsonSerializer.Deserialize<VehicleInfo>(getVehicle);
          return Ok(new ResponseData
          {
            Code = "200",
            Message = "Success",
            Data = vehicleInfo
          });
        }
        else
        {
          return BadRequest(new ResponseData
          {
            Code = "404",
            Message = "Vehicle not found"
          });
        }
      }
      catch (Exception ex)
      {
        SL.CreateLog("VehicleController", "GetInfoOfVehicle", ex.Message);
        return BadRequest(new ResponseData
        {
          Code = "400",
          Message = "Failed",
          Data = ex.Message
        });
      }
    }

    /// <summary>
    /// Insert new vehicle info
    /// </summary>
    /// <param name="data">Info of vehicle</param>
    /// <param name="username">UserName of user</param>
    /// <response code="200">Vehicle info inserted successfully</response>
    /// <response code="401">Vehicle info with same id is already added</response>
    /// <response code="402">Vehicle with same reg number is found</response>
    /// <response code="403">Bad Request</response>
    /// <response code="400">Process ran into an exception</response>
    /// <returns></returns>
    [HttpPost("{username}")]
    [SwaggerRequestExample(typeof(VehicleInfo), typeof(Example_InsertVehicleInfo))]
    [ProducesResponseType(typeof(ResponseData), 200)]
    public ActionResult InsertVehicleInfo([FromBody]VehicleInfo data, string username)
    {
      try
      {
        if (data != null && username != null)
        {
          var check = MH.CheckForData(vehicleinfo_collection, "VehicleNo", data.VehicleNo, null, null).Result;
          if (check == false)
          {
            #region Calculate Vehicle id
            var getVehicles = MH.GetListOfObjects(vehicleinfo_collection, null, null, null, null).Result;
            if (getVehicles.Count == 0)
            {
              data.VehicleId = "VC-1";
            }
            else
            {
              List<long> idList = new List<long>();
              foreach (var vehicle in getVehicles)
              {
                VehicleInfo vehicleInfo = BsonSerializer.Deserialize<VehicleInfo>(vehicle);
                long seperatedId = Convert.ToInt64(vehicleInfo.VehicleId.Substring(vehicleInfo.VehicleId.LastIndexOf('-') + 1));
                idList.Add(seperatedId);
              }
              var maxId = idList.Max();
              data.VehicleId = "VC-" + (maxId + 1);
            }
            #endregion
            data.IsActive = true;
            var insert = MH.InsertNewVehicleInfo(data, vehicleinfoCollection);
            if (insert == true)
            {
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
                Code = "401",
                Message = "Vehicle info with same id is already added"
              });
            }
          }
          else
          {
            return BadRequest(new ResponseData
            {
              Code = "402",
              Message = "Vehicle with same reg number is found"
            });
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
        SL.CreateLog("VehicleController", "InsertVehicleInfo", ex.Message);
        return BadRequest(new ResponseData
        {
          Code = "400",
          Message = "Failed",
          Data = ex.Message
        });
      }
    }

    /// <summary>
    /// Update vehicle info
    /// </summary>
    /// <param name="data">Data to be updated</param>
    /// <param name="username">UserName of user</param>
    /// <param name="vehicleId">Id of vehicle</param>
    /// <returns></returns>
    /// <response code="200">Vehicle info updated successfully </response>
    /// <response code="401">Update failed</response>
    /// <response code="402">Bad Request</response>
    /// <response code="404">Vehicle not found</response>
    /// <response code="400">Process ran into an exception</response>
    [HttpPut("{username}/{vehicleId}")]
    [SwaggerRequestExample(typeof(ExampleModel_VehicleInfo), typeof(Example_UpdateVehicleInfo))]
    [ProducesResponseType(typeof(ResponseData), 200)]
    public ActionResult UpdateVehicleInfo([FromBody]ExampleModel_VehicleInfo data, string username, string vehicleId)
    {
      try
      {
        if (data != null && username != null && vehicleId != null)
        {
          var getVehicle = MH.GetSingleObject(vehicleinfo_collection, "VehicleId", vehicleId, null, null).Result;
          if (getVehicle != null)
          {
            if (data.VehicleNo != null)
            {
              var updateDefinition = Builders<BsonDocument>.Update.Set("VehicleNo", data.VehicleNo);
              update = MH.UpdateSingleObject(vehicleinfo_collection, "VehicleId", vehicleId, null, null, updateDefinition);
            }
            if (data.OwnerName != null)
            {
              var updateDefinition = Builders<BsonDocument>.Update.Set("OwnerName", data.OwnerName);
              update = MH.UpdateSingleObject(vehicleinfo_collection, "VehicleId", vehicleId, null, null, updateDefinition);
            }
            if (data.Model != null)
            {
              var updateDefinition = Builders<BsonDocument>.Update.Set("Model", data.Model);
              update = MH.UpdateSingleObject(vehicleinfo_collection, "VehicleId", vehicleId, null, null, updateDefinition);
            }
            if (data.ModelNo != 0)
            {
              var updateDefinition = Builders<BsonDocument>.Update.Set("ModelNo", data.ModelNo);
              update = MH.UpdateSingleObject(vehicleinfo_collection, "VehicleId", vehicleId, null, null, updateDefinition);
            }
            if (data.VehicleType != null)
            {
              var updateDefinition = Builders<BsonDocument>.Update.Set("VehicleType", data.VehicleType);
              update = MH.UpdateSingleObject(vehicleinfo_collection, "VehicleId", vehicleId, null, null, updateDefinition);
            }
            if (data.TypeOfBody != null)
            {
              var updateDefinition = Builders<BsonDocument>.Update.Set("TypeOfBody", data.TypeOfBody);
              update = MH.UpdateSingleObject(vehicleinfo_collection, "VehicleId", vehicleId, null, null, updateDefinition);
            }
            if (data.NoOfWheels != 0)
            {
              var updateDefinition = Builders<BsonDocument>.Update.Set("NoOfWheels", data.NoOfWheels);
              update = MH.UpdateSingleObject(vehicleinfo_collection, "VehicleId", vehicleId, null, null, updateDefinition);
            }
            if (data.VehicleCapacity != null)
            {
              var updateDefinition = Builders<BsonDocument>.Update.Set("VehicleCapacity", data.VehicleCapacity);
              update = MH.UpdateSingleObject(vehicleinfo_collection, "VehicleId", vehicleId, null, null, updateDefinition);
            }
            if (data.EngineNumber != null)
            {
              var updateDefinition = Builders<BsonDocument>.Update.Set("EngineNumber", data.EngineNumber);
              update = MH.UpdateSingleObject(vehicleinfo_collection, "VehicleId", vehicleId, null, null, updateDefinition);
            }
            if (data.ChasisNumber != null)
            {
              var updateDefinition = Builders<BsonDocument>.Update.Set("ChasisNumber", data.ChasisNumber);
              update = MH.UpdateSingleObject(vehicleinfo_collection, "VehicleId", vehicleId, null, null, updateDefinition);
            }
            if (data.InsuranceDate != null)
            {
              var updateDefinition = Builders<BsonDocument>.Update.Set("InsuranceDate", data.InsuranceDate);
              update = MH.UpdateSingleObject(vehicleinfo_collection, "VehicleId", vehicleId, null, null, updateDefinition);
            }
            if (data.FCDate != null)
            {
              var updateDefinition = Builders<BsonDocument>.Update.Set("FCDate", data.FCDate);
              update = MH.UpdateSingleObject(vehicleinfo_collection, "VehicleId", vehicleId, null, null, updateDefinition);
            }
            if (data.NPTaxDate != null)
            {
              var updateDefinition = Builders<BsonDocument>.Update.Set("NPTaxDate", data.NPTaxDate);
              update = MH.UpdateSingleObject(vehicleinfo_collection, "VehicleId", vehicleId, null, null, updateDefinition);
            }
            if (data.PermitDate != null)
            {
              var updateDefinition = Builders<BsonDocument>.Update.Set("PermitDate", data.PermitDate);
              update = MH.UpdateSingleObject(vehicleinfo_collection, "VehicleId", vehicleId, null, null, updateDefinition);
            }
            if (data.IsActive != null)
            {
              var updateDefinition = Builders<BsonDocument>.Update.Set("IsActive", data.IsActive);
              update = MH.UpdateSingleObject(vehicleinfo_collection, "VehicleId", vehicleId, null, null, updateDefinition);
            }
            if (update != null)
            {
              AL.CreateLog(username, "UpdateVehicleInfo", BsonSerializer.Deserialize<VehicleInfo>(getVehicle), data, activitylog_collection);
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
                Code = "401",
                Message = "Update failed"
              });
            }
          }
          else
          {
            return BadRequest(new ResponseData
            {
              Code = "404",
              Message = "Vehicle info not found"
            });
          }
        }
        else
        {
          return BadRequest(new ResponseData
          {
            Code = "402",
            Message = "Bad request"
          });
        }
      }
      catch (Exception ex)
      {
        SL.CreateLog("VehicleController", "UpdateVehicleInfo", ex.Message);
        return BadRequest(new ResponseData
        {
          Code = "400",
          Message = "Failed",
          Data = ex.Message
        });
      }
    }

    /// <summary>
    /// Make vehicle info inactive
    /// </summary>
    /// <param name="username">UserName of user</param>
    /// <param name="vehicleId">Id of vehicle</param>
    /// <returns></returns>
    /// <response code="200">Vehicle info made inactive</response>
    /// <response code="401">Bad Request</response>
    /// <response code="404">Vehicle info not found</response>
    /// <response code="400">Process ran into an exception</response>
    [HttpDelete("{username}/{vehicleId}")]
    [ProducesResponseType(typeof(ResponseData), 200)]
    public ActionResult MakeVehicleInfoInActive(string username, string vehicleId)
    {
      try
      {
        if (username != null && vehicleId != null)
        {
          var getVehicle = MH.GetSingleObject(vehicleinfo_collection, "VehicleId", vehicleId, null, null).Result;
          if (getVehicle != null)
          {
            var updateDefinition = Builders<BsonDocument>.Update.Set("IsActive", false);
            update = MH.UpdateSingleObject(vehicleinfo_collection, "VehicleId", vehicleId, null, null, updateDefinition);
            var data = BsonSerializer.Deserialize<VendorInfo>(getVehicle);
            data.IsActive = false;
            AL.CreateLog(username, "MakeVehicleInfoInActive", BsonSerializer.Deserialize<VendorInfo>(getVehicle), data, activitylog_collection);
            return Ok(new ResponseData
            {
              Code = "200",
              Message = "Vehicle info made inactive"
            });
          }
          else
          {
            return BadRequest(new ResponseData
            {
              Code = "404",
              Message = "Vehicle info not found"
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
        SL.CreateLog("VehicleController", "MakeVehicleInfoInActive", ex.Message);
        return BadRequest(new ResponseData
        {
          Code = "400",
          Message = "Failed",
          Data = ex.Message
        });
      }
    }

    /// <summary>
    /// Make vehicle info active
    /// </summary>
    /// <param name="username">UserName of user</param>
    /// <param name="vehicleId">Id of vehicle</param>
    /// <returns></returns>
    /// <response code="200">Vehicle info made active</response>
    /// <response code="401">Bad Request</response>
    /// <response code="404">Vehicle info not found</response>
    /// <response code="400">Process ran into an exception</response>
    [HttpPut("makeactive/{username}/{vehicleId}")]
    [ProducesResponseType(typeof(ResponseData), 200)]
    public ActionResult MakeVehicleInfoActive(string username, string vehicleId)
    {
      try
      {
        if (username != null && vehicleId != null)
        {
          var getVehicle = MH.GetSingleObject(vehicleinfo_collection, "VehicleId", vehicleId, null, null).Result;
          if (getVehicle != null)
          {
            var updateDefinition = Builders<BsonDocument>.Update.Set("IsActive", true);
            update = MH.UpdateSingleObject(vehicleinfo_collection, "VehicleId", vehicleId, null, null, updateDefinition);
            var data = BsonSerializer.Deserialize<VendorInfo>(getVehicle);
            data.IsActive = true;
            AL.CreateLog(username, "MakeVehicleInfoActive", BsonSerializer.Deserialize<VendorInfo>(getVehicle), data, activitylog_collection);
            return Ok(new ResponseData
            {
              Code = "200",
              Message = "Vehicle info made active"
            });
          }
          else
          {
            return BadRequest(new ResponseData
            {
              Code = "404",
              Message = "Vehicle info not found"
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
        SL.CreateLog("VehicleController", "MakeVehicleInfoActive", ex.Message);
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
