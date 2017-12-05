using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using SL = JT_Transport.Logger.ServerSideLogger;
using AL = JT_Transport.Logger.ActivityLogger;
using MH = JT_Transport.Helper.MongoHelper;
using GH = JT_Transport.Helper.GlobalHelper;
using JT_Transport.Model;
using MongoDB.Bson.Serialization;
using Swashbuckle.AspNetCore.Examples;
using JT_Transport.Swagger;
using MongoDB.Bson;
using JT_Transport.Logger;

namespace JT_Transport.Controllers
{
  /// <summary>
  /// Controller for all trip related methods
  /// </summary>
  [Produces("application/json")]
  [Route("api/[controller]")]
  public class TripController : Controller
  {
    /// <summary>
    /// 
    /// </summary>
    public MongoClient _client;
    /// <summary></summary>
    public IMongoDatabase trip_db;
    /// <summary>
    /// 
    /// </summary>
    public IMongoCollection<BsonDocument> tripinfo_collection;
    /// <summary>
    /// 
    /// </summary>
    public IMongoCollection<TripInfo> tripinfoCollection;
    /// <summary></summary>
    public IMongoDatabase log_db;
    /// <summary>
    /// 
    /// </summary>
    public IMongoCollection<ActivityLoggerModel> activitylog_collection;

    /// <summary>
    /// 
    /// </summary>
    public TripController()
    {
      _client = MH.GetClient();
      trip_db = _client.GetDatabase("TripDB");
      tripinfo_collection = trip_db.GetCollection<BsonDocument>("TripInfo");
      log_db = _client.GetDatabase("LogDB");
      activitylog_collection = log_db.GetCollection<ActivityLoggerModel>("ActivityLog");
      tripinfoCollection = trip_db.GetCollection<TripInfo>("TripInfo");
    }

    /// <summary>
    /// Get all the trips and their info
    /// </summary>
    /// <response code="200">Returns all the trips and their info</response>
    /// <response code="404">No trips found</response>
    /// <response code="400">Process ran into an exception</response>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(ResponseData), 200)]
    public ActionResult GetAllTrips()
    {
      try
      {
        var getTrips = MH.GetListOfObjects(tripinfo_collection, null, null, null, null).Result;
        if (getTrips != null)
        {
          List<TripInfo> tripInfo = new List<TripInfo>();
          foreach (var trip in getTrips)
          {
            tripInfo.Add(BsonSerializer.Deserialize<TripInfo>(trip));
          }
          return Ok(new ResponseData
          {
            Code = "200",
            Message = "Success",
            Data = tripInfo
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
        SL.CreateLog("TripController", "GetAllTrips", ex.Message);
        return BadRequest(new ResponseData
        {
          Code = "400",
          Message = "Failed",
          Data = ex.Message
        });
      }
    }

    /// <summary>
    /// Get all the trips and their info
    /// </summary>
    /// <param name="tripId">Id of trip</param>
    /// <response code="200">Returns info of trip with given trip id</response>
    /// <response code="401">Bad request</response>
    /// <response code="404">Trip not found</response>
    /// <response code="400">Process ran into an exception</response>
    /// <returns></returns>
    [HttpGet("{tripId}")]
    [ProducesResponseType(typeof(ResponseData), 200)]
    public ActionResult GetInfoOfTrip(string tripId)
    {
      try
      {
        if (tripId != null)
        {
          var getTrip = MH.GetSingleObject(tripinfo_collection, "TripId", tripId, null, null).Result;
          if (getTrip != null)
          {
            var tripInfo = BsonSerializer.Deserialize<TripInfo>(getTrip);
            return Ok(new ResponseData
            {
              Code = "200",
              Message = "Success",
              Data = tripInfo
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
        SL.CreateLog("TripController", "GetInfoOfTrip", ex.Message);
        return BadRequest(new ResponseData
        {
          Code = "400",
          Message = "Failed",
          Data = ex.Message
        });
      }
    }

    /// <summary>
    /// Get all the trips and their info
    /// </summary>
    /// <response code="200">Returns info of trip with unpaid balance amount</response>
    /// <response code="401">No trip with balance amount unpaid is found</response>
    /// <response code="404">No trip infos found</response>
    /// <response code="400">Process ran into an exception</response>
    /// <returns></returns>
    [HttpGet("unpaidbalance")]
    [ProducesResponseType(typeof(ResponseData), 200)]
    public ActionResult GetTripInfoWithUnPaidBalanceAmount()
    {
      try
      {
        var getTrips = MH.GetListOfObjects(tripinfo_collection, "IsActive", true, null, null).Result;
        if (getTrips != null)
        {
          List<TripInfo> infoList = new List<TripInfo>();
          foreach (var info in getTrips)
          {
            var data = BsonSerializer.Deserialize<TripInfo>(info);
            if (data.BalanceAmount > 0)
            {
              infoList.Add(data);
            }
          }
          if (infoList != null)
          {
            return Ok(new ResponseData
            {
              Code = "200",
              Message = "Success",
              Data = infoList
            });
          }
          else
          {
            return BadRequest(new ResponseData
            {
              Code = "401",
              Message = "No trip with balance amount unpaid is found"
            });
          }
        }
        else
        {
          return BadRequest(new ResponseData
          {
            Code = "404",
            Message = "No trip infos found"
          });
        }
      }
      catch (Exception ex)
      {
        SL.CreateLog("TripController", "GetTripInfoWithUnPaidBalanceAmount", ex.Message);
        return BadRequest(new ResponseData
        {
          Code = "400",
          Message = "Failed",
          Data = ex.Message
        });
      }
    }

    /// <summary>
    /// Insert new trip info
    /// </summary>
    /// <param name="data">Info of trip</param>
    /// <param name="username">UserName of user</param>
    /// <response code="200">Trip info inserted successfully</response>
    /// <response code="401">Trip info with same id is already added</response>
    /// <response code="402">Bad request</response>
    /// <response code="400">Process ran into an exception</response>
    /// <returns></returns>
    [HttpPost("{username}")]
    [SwaggerRequestExample(typeof(TripInfo), typeof(Example_InsertTripInfo))]
    [ProducesResponseType(typeof(ResponseData), 200)]
    public ActionResult InsertTripInfo([FromBody]TripInfo data, string username)
    {
      try
      {
        if (data != null && username != null)
        {
          #region Calculate trip id
          var getTrips = MH.GetListOfObjects(tripinfo_collection, null, null, null, null).Result;
          if (getTrips.Count == 0)
          {
            data.TripId = "TP-1";
          }
          else
          {
            List<long> idList = new List<long>();
            foreach (var trip in getTrips)
            {
              TripInfo tripInfo = BsonSerializer.Deserialize<TripInfo>(trip);
              long seperatedId = Convert.ToInt64(tripInfo.TripId.Substring(tripInfo.TripId.LastIndexOf('-') + 1));
              idList.Add(seperatedId);
            }
            var maxId = idList.Max();
            data.TripId = "TP-" + (maxId + 1);
          }
          #endregion
          data.IsActive = true;
          var insert = MH.InsertNewTripInfo(data, tripinfoCollection);
          if (insert == true)
          {
            AL.CreateLog(username, "InsertTripInfo", null, data, activitylog_collection);
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
              Message = "Trip info with same id is already added"
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
        SL.CreateLog("TripController", "InsertTripInfo", ex.Message);
        return BadRequest(new ResponseData
        {
          Code = "400",
          Message = "Failed",
          Data = ex.Message
        });
      }
    }

    /// <summary>
    /// Update trip info
    /// </summary>
    /// <param name="data">Data to be updated</param>
    /// <param name="username">UserName of user</param>
    /// <param name="tripId">Id of trip</param>
    /// <returns></returns>
    /// <response code="200">Trip info updated successfully </response>
    /// <response code="404">Bad Request</response>
    /// <response code="404">Trip not found</response>
    /// <response code="400">Process ran into an exception</response>
    [HttpPut("{username}/{tripId}")]
    [SwaggerRequestExample(typeof(TripInfo), typeof(Example_InsertTripInfo))]
    [ProducesResponseType(typeof(ResponseData), 200)]
    public async Task<ActionResult> UpdateTripInfo([FromBody]TripInfo data, string username, string tripId)
    {
      try
      {
        if (data != null && username != null && tripId != null)
        {
          var getTrip = MH.GetSingleObject(tripinfo_collection, "TripId", tripId, null, null).Result;
          if (getTrip != null)
          {
            var delete = MH.DeleteSingleObject(tripinfo_collection, "TripId", tripId, null, null);
            var tripData = BsonSerializer.Deserialize<TripInfo>(getTrip);
            data.Id = tripData.Id;
            data.TripId = tripData.TripId;
            await trip_db.GetCollection<TripInfo>("TripInfo").InsertOneAsync(data);
            AL.CreateLog(username, "UpdateTripInfo", BsonSerializer.Deserialize<TripInfo>(getTrip), data, activitylog_collection);
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
              Message = "Trip info not found"
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
        SL.CreateLog("TripController", "UpdateTripInfo", ex.Message);
        return BadRequest(new ResponseData
        {
          Code = "400",
          Message = "Failed",
          Data = ex.Message
        });
      }
    }

    /// <summary>
    /// Make payment for trip
    /// </summary>
    /// <param name="tripId">Id of trip</param>
    /// <param name="username">Username of user</param>
    /// <param name="data">details of payment</param>
    /// <returns></returns>
    /// <response code="200">Payment successfully made</response>
    /// <response code="401">Payment update failed</response>
    /// <response code="402">Bad Request</response>
    /// <response code="402">Paid amount is higher than the balance amount</response>
    /// <response code="404">Trip not found</response>
    /// <response code="400">Process ran into an exception</response>
    [HttpPut("makepayment/{username}/{tripId}")]
    [SwaggerRequestExample(typeof(PaymentDetails), typeof(Example_UpdatePaymentInfo))]
    public ActionResult MakePaymentForTrip([FromBody] PaymentDetails data, string username, string tripId)
    {
      try
      {
        if (data != null && username != null && tripId != null)
        {
          var getTrip = MH.GetSingleObject(tripinfo_collection, "TripId", tripId, null, null).Result;
          if (getTrip != null)
          {
            if (data.AmountReceived != 0)
            {
              var tripDetails = BsonSerializer.Deserialize<TripInfo>(getTrip);
              if (data.AmountReceived > tripDetails.BalanceAmount)
              {
                return BadRequest(new ResponseData
                {
                  Code = "403",
                  Message = "Paid amount is higher than the balance amount"
                });
              }
              else
              {
                List<PaymentDetails> paymentList = new List<PaymentDetails>();
                if (tripDetails.PaymentInfo != null)
                {
                  foreach (var payment in tripDetails.PaymentInfo)
                  {
                    paymentList.Add(payment);
                  }
                }
                var balanceAmount = tripDetails.BalanceAmount - data.AmountReceived;
                data.RunningBalanceAmount = balanceAmount;
                paymentList.Add(data);
                var updateBalanceAmount = MH.UpdateSingleObject(tripinfo_collection, "TripId", tripId, null, null, Builders<BsonDocument>.Update.Set("BalanceAmount", balanceAmount));
                var paidAmount = tripDetails.PaidAmount + data.AmountReceived;
                var updatePaidAmount = MH.UpdateSingleObject(tripinfo_collection, "TripId", tripId, null, null, Builders<BsonDocument>.Update.Set("PaidAmount", paidAmount));
                var updatePaymentInfo = MH.UpdateSingleObject(tripinfo_collection, "TripId", tripId, null, null, Builders<BsonDocument>.Update.Set("PaymentInfo", paymentList));
                if (updateBalanceAmount != null && updatePaidAmount != null && updatePaymentInfo != null)
                {
                  var updatedDetails = BsonSerializer.Deserialize<TripInfo>(MH.GetSingleObject(tripinfo_collection, "TripId", tripId, null, null).Result);
                  AL.CreateLog(username, "MakePaymentForTrip", BsonSerializer.Deserialize<TripInfo>(getTrip), updatedDetails, activitylog_collection);
                  return Ok(new ResponseData
                  {
                    Code = "200",
                    Message = "Payment made successfully",
                    Data = updatedDetails
                  });
                }
                else
                {
                  return BadRequest(new ResponseData
                  {
                    Code = "401",
                    Message = "Payment update failed"
                  });
                }
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
          else
          {
            return BadRequest(new ResponseData
            {
              Code = "404",
              Message = "Trip info not found"
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
        SL.CreateLog("TripController", "MakePaymentForTrip", ex.Message);
        return BadRequest(new ResponseData
        {
          Code = "400",
          Message = "Failed",
          Data = ex.Message
        });
      }
    }

    /// <summary>
    /// Make trip info inactive
    /// </summary>
    /// <param name="username">UserName of user</param>
    /// <param name="tripId">Id of trip</param>
    /// <returns></returns>
    /// <response code="200">Trip info made inactive</response>
    /// <response code="401">Bad request</response>
    /// <response code="404">Trip not found</response>
    /// <response code="400">Process ran into an exception</response>
    [HttpDelete("{username}/{tripId}")]
    [ProducesResponseType(typeof(ResponseData), 200)]
    public ActionResult MakeTripInfoInActive(string username, string tripId)
    {
      try
      {
        if (tripId != null && username != null)
        {
          var getTrip = MH.GetSingleObject(tripinfo_collection, "TripId", tripId, null, null).Result;
          if (getTrip != null)
          {
            var updateDefinition = Builders<BsonDocument>.Update.Set("IsActive", false);
            var update = MH.UpdateSingleObject(tripinfo_collection, "TripId", tripId, null, null, updateDefinition);
            var data = BsonSerializer.Deserialize<TripInfo>(getTrip);
            data.IsActive = false;
            AL.CreateLog(username, "MakeTripInfoInActive", BsonSerializer.Deserialize<TripInfo>(getTrip), data, activitylog_collection);
            return Ok(new ResponseData
            {
              Code = "200",
              Message = "Trip info made inactive"
            });
          }
          else
          {
            return BadRequest(new ResponseData
            {
              Code = "404",
              Message = "Trip info not found"
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
        SL.CreateLog("TripController", "MakeTripInfoInActive", ex.Message);
        return BadRequest(new ResponseData
        {
          Code = "400",
          Message = "Failed",
          Data = ex.Message
        });
      }
    }

    /// <summary>
    /// Make trip info active
    /// </summary>
    /// <param name="username">UserName of user</param>
    /// <param name="tripId">Id of trip</param>
    /// <returns></returns>
    /// <response code="200">Trip info made active</response>
    /// <response code="401">Bad request</response>
    /// <response code="404">Trip not found</response>
    /// <response code="400">Process ran into an exception</response>
    [HttpPut("makeactive/{username}/{tripId}")]
    [ProducesResponseType(typeof(ResponseData), 200)]
    public ActionResult MakeTripInfoActive(string username, string tripId)
    {
      try
      {
        if (username != null && tripId != null)
        {
          var getTrip = MH.GetSingleObject(tripinfo_collection, "TripId", tripId, null, null).Result;
          if (getTrip != null)
          {
            var updateDefinition = Builders<BsonDocument>.Update.Set("IsActive", true);
            var update = MH.UpdateSingleObject(tripinfo_collection, "TripId", tripId, null, null, updateDefinition);
            var data = BsonSerializer.Deserialize<TripInfo>(getTrip);
            data.IsActive = true;
            AL.CreateLog(username, "MakeTripInfoActive", BsonSerializer.Deserialize<TripInfo>(getTrip), data, activitylog_collection);
            return Ok(new ResponseData
            {
              Code = "200",
              Message = "Trip info made active"
            });
          }
          else
          {
            return BadRequest(new ResponseData
            {
              Code = "404",
              Message = "Trip info not found"
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
        SL.CreateLog("TripController", "MakeTripInfoActive", ex.Message);
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
