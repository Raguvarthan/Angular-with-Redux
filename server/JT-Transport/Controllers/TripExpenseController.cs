using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SL = JT_Transport.Logger.ServerSideLogger;
using AL = JT_Transport.Logger.ActivityLogger;
using MH = JT_Transport.Helper.MongoHelper;
using GH = JT_Transport.Helper.GlobalHelper;
using JT_Transport.Model;
using MongoDB.Driver;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using Swashbuckle.AspNetCore.Examples;
using JT_Transport.Swagger;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using JT_Transport.Logger;

namespace JT_Transport.Controllers
{
  /// <summary>
  /// 
  /// </summary>
  [Produces("application/json")]
  [Route("api/[controller]")]
  public class TripExpenseController : Controller
  {
    /// <summary>
    /// 
    /// </summary>
    public MongoClient _client;
    /// <summary></summary>
    public IMongoDatabase tripexpense_db;
    /// <summary>
    /// 
    /// </summary>
    public IMongoCollection<BsonDocument> tripexpenseinfo_collection;
    /// <summary>
    /// 
    /// </summary>
    public IMongoCollection<TripExpenceInfo> tripexpenseinfoCollection;
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
    public TripExpenseController()
    {
      _client = MH.GetClient();
      tripexpense_db = _client.GetDatabase("TripExpenseDB");
      tripexpenseinfo_collection = tripexpense_db.GetCollection<BsonDocument>("TripExpenseInfo");
      log_db = _client.GetDatabase("LogDB");
      activitylog_collection = log_db.GetCollection<ActivityLoggerModel>("ActivityLog");
      tripexpenseinfoCollection = tripexpense_db.GetCollection<TripExpenceInfo>("TripExpenseInfo");
    }

    /// <summary>
    /// Get all the trips and their expense details
    /// </summary>
    /// <response code="200">Returns all the info of expenses made on all trips</response>
    /// <response code="404">No vendors found</response>
    /// <response code="400">Process ran into an exception</response>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(ResponseData), 200)]
    public ActionResult GetAllTripExpenses()
    {
      try
      {
        var getTrips = MH.GetListOfObjects(tripexpenseinfo_collection, null, null, null, null).Result;
        if (getTrips != null)
        {
          List<TripExpenceInfo> tripExpenseInfo = new List<TripExpenceInfo>();
          foreach (var trip in getTrips)
          {
            tripExpenseInfo.Add(BsonSerializer.Deserialize<TripExpenceInfo>(trip));
          }
          return Ok(new ResponseData
          {
            Code = "200",
            Message = "Success",
            Data = tripExpenseInfo
          });
        }
        else
        {
          return BadRequest(new ResponseData
          {
            Code = "404",
            Message = "No trips found"
          });
        }
      }
      catch (Exception ex)
      {
        SL.CreateLog("TripExpenseController", "GetAllTripExpenses", ex.Message);
        return BadRequest(new ResponseData
        {
          Code = "400",
          Message = "Failed",
          Data = ex.Message
        });
      }
    }

    /// <summary>
    /// Gets info of trip and its info with given tripexpense id
    /// </summary>
    /// <param name="tripExpenseId">Id of vendor</param>
    /// <response code="200">Returns info of vendor with given vendor id</response>
    /// <response code="401">Bad request</response>
    /// <response code="404">Vendors not found</response>
    /// <response code="400">Process ran into an exception</response>
    /// <returns></returns>
    [HttpGet("{tripExpenseId}")]
    [ProducesResponseType(typeof(ResponseData), 200)]
    public ActionResult GetInfoOfTrip(string tripExpenseId)
    {
      try
      {
        var getTrip = MH.GetSingleObject(tripexpenseinfo_collection, "TripExpenseId", tripExpenseId, null, null).Result;
        if (getTrip != null)
        {
          if (tripExpenseId != null)
          {
            var tripExpenseInfo = BsonSerializer.Deserialize<TripExpenceInfo>(getTrip);
            return Ok(new ResponseData
            {
              Code = "200",
              Message = "Success",
              Data = tripExpenseInfo
            });
          }
          else
          {
            return BadRequest(new ResponseData
            {
              Code = "404",
              Message = "Trip not found"
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
        SL.CreateLog("TripExpenseController", "GetInfoOfTrip", ex.Message);
        return BadRequest(new ResponseData
        {
          Code = "400",
          Message = "Failed",
          Data = ex.Message
        });
      }
    }

    /// <summary>
    /// Insert new expense details for trip
    /// </summary>
    /// <param name="data">Info of trip expenses</param>
    /// <param name="username">UserName of user</param>
    /// <response code="200">Expense info of trip inserted successfully</response>
    /// <response code="401">Trip expense info with same id is already added</response>
    /// <response code="402">Bad Request</response>
    /// <response code="400">Process ran into an exception</response>
    /// <returns></returns>
    [HttpPost("{username}")]
    [SwaggerRequestExample(typeof(TripExpenceInfo), typeof(Example_InsertTripExpenseInfo))]
    [ProducesResponseType(typeof(ResponseData), 200)]
    public ActionResult InsertTripExpenseInfo([FromBody]TripExpenceInfo data, string username)
    {
      try
      {
        if (data != null && username != null)
        {
          #region Calculate trip expense id
          var getTrips = MH.GetListOfObjects(tripexpenseinfo_collection, null, null, null, null).Result;
          if (getTrips.Count == 0)
          {
            data.TripExpenseId = "TRE-1";
          }
          else
          {
            List<long> idList = new List<long>();
            foreach (var trip in getTrips)
            {
              TripExpenceInfo tripExpenseInfo = BsonSerializer.Deserialize<TripExpenceInfo>(trip);
              long seperatedId = Convert.ToInt64(tripExpenseInfo.TripExpenseId.Substring(tripExpenseInfo.TripExpenseId.LastIndexOf('-') + 1));
              idList.Add(seperatedId);
            }
            var maxId = idList.Max();
            data.TripExpenseId = "TRE-" + (maxId + 1);
          }
          #endregion
          data.IsActive = true;
          var insert = MH.InsertNewTripExpenseInfo(data, tripexpenseinfoCollection);
          if (insert == true)
          {
            AL.CreateLog(username, "InsertTripExpenseInfo", null, data, activitylog_collection);
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
              Message = "Trip expense info with same id is already added"
            });
          }
        }
        else
        {
          return BadRequest(new ResponseData
          {
            Code = "402",
            Message = "Bad Request"
          });
        }
      }
      catch (Exception ex)
      {
        SL.CreateLog("TripExpenseController", "InsertTripExpenseInfo", ex.Message);
        return BadRequest(new ResponseData
        {
          Code = "400",
          Message = "Failed",
          Data = ex.Message
        });
      }
    }

    /// <summary>
    /// Make trip expense info iniactive
    /// </summary>
    /// <param name="username">UserName of user</param>
    /// <param name="tripExpenseId">Id of trip expense</param>
    /// <returns></returns>
    /// <response code="200">Trip expense info is made inactive</response>
    /// <response code="401">Bad request</response>
    /// <response code="404">Trip expense info not found</response>
    /// <response code="400">Process ran into an exception</response>
    [HttpDelete("{username}/{tripExpenseId}")]
    public ActionResult MakeTripExpenseInfoInActive(string username, string tripExpenseId)
    {
      try
      {
        if (username != null && tripExpenseId != null)
        {
          var getTrip = MH.GetSingleObject(tripexpenseinfo_collection, "TripExpenseId", tripExpenseId, null, null).Result;
          if (getTrip != null)
          {
            var updateDefinition = Builders<BsonDocument>.Update.Set("IsActive", false);
            update = MH.UpdateSingleObject(tripexpenseinfo_collection, "TripExpenseId", tripExpenseId, null, null, updateDefinition);
            var data = BsonSerializer.Deserialize<TripExpenceInfo>(getTrip);
            data.IsActive = false;
            AL.CreateLog(username, "MakeTripExpenseInfoInActive", BsonSerializer.Deserialize<TripExpenceInfo>(getTrip), data, activitylog_collection);
            return Ok(new ResponseData
            {
              Code = "200",
              Message = "Trip expense info is made inactive"
            });
          }
          else
          {
            return BadRequest(new ResponseData
            {
              Code = "404",
              Message = "Trip expense info not found"
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
        SL.CreateLog("TripExpenseController", "MakeTripExpenseInfoInActive", ex.Message);
        return BadRequest(new ResponseData
        {
          Code = "400",
          Message = "Failed",
          Data = ex.Message
        });
      }
    }

    /// <summary>
    /// Make trip expense info iactive
    /// </summary>
    /// <param name="username">UserName of user</param>
    /// <param name="tripExpenseId">Id of trip expense</param>
    /// <returns></returns>
    /// <response code="200">Trip expense info is made active</response>
    /// <response code="401">Bad request</response>
    /// <response code="404">Trip expense info not found</response>
    /// <response code="400">Process ran into an exception</response>
    [HttpPut("makeactive/{username}/{tripExpenseId}")]
    public ActionResult MakeTripExpenseInfoActive(string username, string tripExpenseId)
    {
      try
      {
        if (username != null && tripExpenseId != null)
        {
          var getTrip = MH.GetSingleObject(tripexpenseinfo_collection, "TripExpenseId", tripExpenseId, null, null).Result;
          if (getTrip != null)
          {
            var updateDefinition = Builders<BsonDocument>.Update.Set("IsActive", true);
            update = MH.UpdateSingleObject(tripexpenseinfo_collection, "TripExpenseId", tripExpenseId, null, null, updateDefinition);
            var data = BsonSerializer.Deserialize<TripExpenceInfo>(getTrip);
            data.IsActive = true;
            AL.CreateLog(username, "MakeTripExpenseInfoActive", BsonSerializer.Deserialize<TripExpenceInfo>(getTrip), data, activitylog_collection);
            return Ok(new ResponseData
            {
              Code = "200",
              Message = "Trip expense info is made active"
            });
          }
          else
          {
            return BadRequest(new ResponseData
            {
              Code = "404",
              Message = "Trip expense info not found"
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
        SL.CreateLog("TripExpenseController", "MakeTripExpenseInfoActive", ex.Message);
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
