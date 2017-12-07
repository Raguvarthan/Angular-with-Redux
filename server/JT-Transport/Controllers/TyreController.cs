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
  /// Controller for all tyre related methods
  /// </summary>
  [Produces("application/json")]
  [Route("api/[controller]")]
  public class TyreController : Controller
  {
    /// <summary>
    /// 
    /// </summary>
    public MongoClient _client;
    /// <summary></summary>
    public IMongoDatabase tyre_db;
    /// <summary>
    /// 
    /// </summary>
    public IMongoCollection<BsonDocument> tyreinfo_collection;
    /// <summary>
    /// 
    /// </summary>
    public IMongoCollection<TyreInfo> tyreinfoCollection;
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
    public bool? alreadyAdded;

    /// <summary>
    /// 
    /// </summary>
    public TyreController()
    {
      _client = MH.GetClient();
      tyre_db = _client.GetDatabase("TyreDB");
      tyreinfo_collection = tyre_db.GetCollection<BsonDocument>("TyreInfo");
      log_db = _client.GetDatabase("LogDB");
      activitylog_collection = log_db.GetCollection<ActivityLoggerModel>("ActivityLog");
      tyreinfoCollection = tyre_db.GetCollection<TyreInfo>("TyreInfo");
    }

    /// <summary>
    /// Get all the tyres and their info
    /// </summary>
    /// <response code="200">Returns all the tyres and their info</response>
    /// <response code="404">No tyres found</response>
    /// <response code="400">Process ran into an exception</response>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(ResponseData), 200)]
    public ActionResult GetAllTyres()
    {
      try
      {
        var getTyres = MH.GetListOfObjects(tyreinfo_collection, null, null, null, null).Result;
        if (getTyres != null)
        {
          List<TyreInfo> tyreInfo = new List<TyreInfo>();
          foreach (var tyre in getTyres)
          {
            tyreInfo.Add(BsonSerializer.Deserialize<TyreInfo>(tyre));
          }
          return Ok(new ResponseData
          {
            Code = "200",
            Message = "Success",
            Data = tyreInfo
          });
        }
        else
        {
          return BadRequest(new ResponseData
          {
            Code = "404",
            Message = "No tyres found"
          });
        }
      }
      catch (Exception ex)
      {
        SL.CreateLog("TyreController", "GetAllTyres", ex.Message);
        return BadRequest(new ResponseData
        {
          Code = "400",
          Message = "Failed",
          Data = ex.Message
        });
      }
    }

    /// <summary>
    /// Gets info of tyre with given tyre id
    /// </summary>
    /// <param name="tyreId">Id of tyre</param>
    /// <response code="200">Returns info of tyre with given tyre id</response>
    /// <response code="401">Bad Request</response>
    /// <response code="404">Tyres not found</response>
    /// <response code="400">Process ran into an exception</response>
    /// <returns></returns>
    [HttpGet("{tyreId}")]
    [ProducesResponseType(typeof(ResponseData), 200)]
    public ActionResult GetInfoOftyre(string tyreId)
    {
      try
      {
        if (tyreId != null)
        {
          var getTyre = MH.GetSingleObject(tyreinfo_collection, "TyreId", tyreId, null, null).Result;
          if (getTyre != null)
          {
            var tyreInfo = BsonSerializer.Deserialize<TyreInfo>(getTyre);
            return Ok(new ResponseData
            {
              Code = "200",
              Message = "Success",
              Data = tyreInfo
            });
          }
          else
          {
            return BadRequest(new ResponseData
            {
              Code = "404",
              Message = "Tyre not found"
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
        SL.CreateLog("TyreController", "GetInfoOfTyre", ex.Message);
        return BadRequest(new ResponseData
        {
          Code = "400",
          Message = "Failed",
          Data = ex.Message
        });
      }
    }

    /// <summary>
    /// Insert new tyre info
    /// </summary>
    /// <param name="data">Info of tyre</param>
    /// <param name="username">UserName of user</param>
    /// <response code="200">Tyre info inserted successfully</response>
    /// <response code="401">Tyre with same tyre name and address is already added</response>
    /// <response code="402">Tyre info with same id is already added</response>
    /// <response code="403">Bad Request</response>
    /// <response code="400">Process ran into an exception</response>
    /// <returns></returns>
    [HttpPost("{username}")]
    [SwaggerRequestExample(typeof(TyreInfo), typeof(Example_InsertTyreInfo))]
    [ProducesResponseType(typeof(ResponseData), 200)]
    public ActionResult InsertTyreInfo([FromBody]TyreInfo data, string username)
    {
      try
      {
        if (data != null && username != null)
        {
          var checkTyre = MH.GetListOfObjects(tyreinfo_collection, null, null, null, null).Result;
          foreach (var details in checkTyre)
          {
            var deserializedDetails = BsonSerializer.Deserialize<TyreInfo>(details).PurchaseDetails;
            if (deserializedDetails.TyreNo == data.PurchaseDetails.TyreNo && deserializedDetails.BrandName == data.PurchaseDetails.BrandName)
            {
              alreadyAdded = true;
            }
          }
          if (alreadyAdded == true)
          {
            return BadRequest(new ResponseData
            {
              Code = "401",
              Message = "Tyre with same brand and tyre number is already added"
            });
          }
          else
          {
            #region Calculate tyre id
            var getTyres = MH.GetListOfObjects(tyreinfo_collection, null, null, null, null).Result;
            if (getTyres.Count == 0)
            {
              data.TyreId = "TRE-1";
            }
            else
            {
              List<long> idList = new List<long>();
              foreach (var tyre in getTyres)
              {
                TyreInfo tyreInfo = BsonSerializer.Deserialize<TyreInfo>(tyre);
                long seperatedId = Convert.ToInt64(tyreInfo.TyreId.Substring(tyreInfo.TyreId.LastIndexOf('-') + 1));
                idList.Add(seperatedId);
              }
              var maxId = idList.Max();
              data.TyreId = "TRE-" + (maxId + 1);
            }
            #endregion
            data.IsActive = true;
            var insert = MH.InsertNewTyreInfo(data, tyreinfoCollection);
            if (insert == true)
            {
              AL.CreateLog(username, "InsertTyreInfo", null, data, activitylog_collection);
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
                Message = "Tyre info with same id is already added"
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
        SL.CreateLog("TyreController", "InsertTyreInfo", ex.Message);
        return BadRequest(new ResponseData
        {
          Code = "400",
          Message = "Failed",
          Data = ex.Message
        });
      }
    }

    /// <summary>
    /// Update tyre info
    /// </summary>
    /// <param name="data">Data to be updated</param>
    /// <param name="username">UserName of user</param>
    /// <param name="tyreId">Id of tyre</param>
    /// <returns></returns>
    /// <response code="200">Tyre info updated successfully </response>
    /// <response code="401">Bad Request</response>
    /// <response code="402">Update failed</response>
    /// <response code="404">Tyre not found</response>
    /// <response code="400">Process ran into an exception</response>
    [HttpPut("{username}/{tyreId}")]
    [SwaggerRequestExample(typeof(ExampleModel_TyreInfo), typeof(Example_UpdateTyreInfo))]
    [ProducesResponseType(typeof(ResponseData), 200)]
    public ActionResult UpdateTyreInfo([FromBody]ExampleModel_TyreInfo data, string username, string tyreId)
    {
      try
      {
        if (data != null && username != null && tyreId != null)
        {
          var tyreDetails = BsonSerializer.Deserialize<TyreInfo>(MH.GetSingleObject(tyreinfo_collection, "TyreId", tyreId, null, null).Result);
          if (tyreDetails != null)
          {
            if (data.TyreStatus != null)
            {
              var updateDefinition = Builders<BsonDocument>.Update.Set("TyreStatus", data.TyreStatus);
              update = MH.UpdateSingleObject(tyreinfo_collection, "TyreId", tyreId, null, null, updateDefinition);
            }
            if (data.PurchaseDetails != null)
            {
              PurchaseDetails purchaseDetails = new PurchaseDetails();
              if (tyreDetails.PurchaseDetails != null)
              {
                purchaseDetails = tyreDetails.PurchaseDetails;
              }
              if (data.PurchaseDetails.BrandName != null)
              {
                purchaseDetails.BrandName = data.PurchaseDetails.BrandName;
              }
              if (data.PurchaseDetails.TyreModel != null)
              {
                purchaseDetails.TyreModel = data.PurchaseDetails.TyreModel;
              }
              if (data.PurchaseDetails.TyreNo != null)
              {
                purchaseDetails.TyreNo = data.PurchaseDetails.TyreNo;
              }
              if (data.PurchaseDetails.VendorName != null)
              {
                purchaseDetails.VendorName = data.PurchaseDetails.VendorName;
              }
              if (data.PurchaseDetails.PurchaseDate != null)
              {
                purchaseDetails.PurchaseDate = data.PurchaseDetails.PurchaseDate;
              }
              if (data.PurchaseDetails.BillNo != null)
              {
                purchaseDetails.BillNo = data.PurchaseDetails.BillNo;
              }
              var updateDefinition = Builders<BsonDocument>.Update.Set("PurchaseDetails", purchaseDetails);
              update = MH.UpdateSingleObject(tyreinfo_collection, "TyreId", tyreId, null, null, updateDefinition);
            }
            if (data.DisposalDetails != null)
            {
              DisposalDetails disposalDetails = new DisposalDetails();
              if (tyreDetails.DisposalDetails != null)
              {
                disposalDetails = tyreDetails.DisposalDetails;
              }
              if (data.DisposalDetails.VendorName != null)
              {
                disposalDetails.VendorName = data.DisposalDetails.VendorName;
              }
              if (data.DisposalDetails.DisposalDate != null)
              {
                disposalDetails.DisposalDate = data.DisposalDetails.DisposalDate;
              }
              if (data.DisposalDetails.SoldPrice != null)
              {
                disposalDetails.SoldPrice = data.DisposalDetails.SoldPrice;
              }
              var updateDefinition = Builders<BsonDocument>.Update.Set("DisposalDetails", disposalDetails);
              update = MH.UpdateSingleObject(tyreinfo_collection, "TyreId", tyreId, null, null, updateDefinition);
            }
            if (data.TotalKMRunned != null)
            {
              var updateDefinition = Builders<BsonDocument>.Update.Set("TotalKMRunned", data.TotalKMRunned);
              update = MH.UpdateSingleObject(tyreinfo_collection, "TyreId", tyreId, null, null, updateDefinition);
            }
            if (data.IsActive != null)
            {
              var updateDefinition = Builders<BsonDocument>.Update.Set("IsActive", data.IsActive);
              update = MH.UpdateSingleObject(tyreinfo_collection, "TyreId", tyreId, null, null, updateDefinition);
            }
            if (update != null)
            {
              AL.CreateLog(username, "UpdateTyreInfo", BsonSerializer.Deserialize<TyreInfo>(update), data, activitylog_collection);
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
                Code = "402",
                Message = "Update failed"
              });
            }
          }
          else
          {
            return BadRequest(new ResponseData
            {
              Code = "404",
              Message = "Tyre info not found"
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
        SL.CreateLog("TyreController", "UpdateTyreInfo", ex.Message);
        return BadRequest(new ResponseData
        {
          Code = "400",
          Message = "Failed",
          Data = ex.Message
        });
      }
    }

    /// <summary>
    /// Update vehicle details for tyre info
    /// </summary>
    /// <param name="data">Vehicle data to be updated</param>
    /// <param name="username">username of user</param>
    /// <param name="tyreId">Id of tyre</param>
    /// <returns></returns>
    /// <response code="200">Vehicle details for tyre info updated successfully </response>
    /// <response code="401">Bad Request</response>
    /// <response code="402">Update failed</response>
    /// <response code="404">Tyre not found</response>
    /// <response code="400">Process ran into an exception</response>
    [HttpPut("vehicledetails/{username}/{tyreId}")]
    [SwaggerRequestExample(typeof(VehicleDetails), typeof(Example_UpdateVehicleInfoForTyreInfo))]
    [ProducesResponseType(typeof(ResponseData), 200)]
    public ActionResult UpdateVehicleDetails([FromBody]VehicleDetails data, string username, string tyreId)
    {
      try
      {
        if (data != null && username != null && tyreId != null)
        {
          var tyreDetails = BsonSerializer.Deserialize<TyreInfo>(MH.GetSingleObject(tyreinfo_collection, "TyreId", tyreId, null, null).Result);
          if (tyreDetails != null)
          {
            if (tyreDetails.VehicleDetails == null)
            {
              data.Id = 1;
            }
            else
            {
              data.Id = tyreDetails.VehicleDetails.Count + 1;
            }
            List<VehicleDetails> detailsList = new List<VehicleDetails>();
            if (tyreDetails.VehicleDetails != null)
            {
              foreach (var vehicle in tyreDetails.VehicleDetails)
              {
                detailsList.Add(vehicle);
              }
            }
            detailsList.Add(data);
            var updateDefinition = Builders<BsonDocument>.Update.Set("VehicleDetails", detailsList);
            update = MH.UpdateSingleObject(tyreinfo_collection, "TyreId", tyreId, null, null, updateDefinition);
            if (update != null)
            {
              AL.CreateLog(username, "UpdateVehicleDetails", BsonSerializer.Deserialize<TyreInfo>(update), data, activitylog_collection);
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
                Code = "402",
                Message = "Update failed"
              });
            }
          }
          else
          {
            return BadRequest(new ResponseData
            {
              Code = "404",
              Message = "Tyre info not found"
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
        SL.CreateLog("TyreController", "UpdateVehicleDetails", ex.Message);
        return BadRequest(new ResponseData
        {
          Code = "400",
          Message = "Failed",
          Data = ex.Message
        });
      }

    }

    /// <summary>
    /// Update rethreading details for tyre info
    /// </summary>
    /// <param name="data">Rethreading data to be updated</param>
    /// <param name="username">username of user</param>
    /// <param name="tyreId">Id of tyre</param>
    /// <returns></returns>
    /// <response code="200">Rethreading details for tyre info updated successfully </response>
    /// <response code="401">Bad Request</response>
    /// <response code="402">Update failed</response>
    /// <response code="404">Tyre not found</response>
    /// <response code="400">Process ran into an exception</response>
    [HttpPut("rethreadingdetails/{username}/{tyreId}")]
    [SwaggerRequestExample(typeof(RethreadingDetails), typeof(Example_UpdateVehicleInfoForRethreadingInfo))]
    [ProducesResponseType(typeof(ResponseData), 200)]
    public ActionResult UpdateRethreadingDetails([FromBody]RethreadingDetails data, string username, string tyreId)
    {
      try
      {
        if (data != null && username != null && tyreId != null)
        {
          var tyreDetails = BsonSerializer.Deserialize<TyreInfo>(MH.GetSingleObject(tyreinfo_collection, "TyreId", tyreId, null, null).Result);
          if (tyreDetails != null)
          {
            if (tyreDetails.RethreadingDetails == null)
            {
              data.Id = 1;
            }
            else
            {
              data.Id = tyreDetails.RethreadingDetails.Count + 1;
            }
            List<RethreadingDetails> detailsList = new List<RethreadingDetails>();
            if (tyreDetails.RethreadingDetails != null)
            {
              foreach (var rework in tyreDetails.RethreadingDetails)
              {
                detailsList.Add(rework);
              }
            }
            detailsList.Add(data);
            var updateDefinition = Builders<BsonDocument>.Update.Set("RethreadingDetails", detailsList);
            update = MH.UpdateSingleObject(tyreinfo_collection, "TyreId", tyreId, null, null, updateDefinition);
            if (update != null)
            {
              AL.CreateLog(username, "UpdateRethreadingDetails", BsonSerializer.Deserialize<TyreInfo>(update), data, activitylog_collection);
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
                Code = "402",
                Message = "Update failed"
              });
            }
          }
          else
          {
            return BadRequest(new ResponseData
            {
              Code = "404",
              Message = "Tyre info not found"
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
        SL.CreateLog("TyreController", "UpdateRethreadingDetails", ex.Message);
        return BadRequest(new ResponseData
        {
          Code = "400",
          Message = "Failed",
          Data = ex.Message
        });
      }

    }

    /// <summary>
    /// Make tyre info inactive
    /// </summary>
    /// <param name="username">UserName of user</param>
    /// <param name="tyreId">Id of tyre</param>
    /// <returns></returns>
    /// <response code="200">Tyre info made inactive</response>
    /// <response code="401">Bad Request</response>
    /// <response code="404">Tyre info not found</response>
    /// <response code="400">Process ran into an exception</response>
    [HttpDelete("{username}/{tyreId}")]
    [ProducesResponseType(typeof(ResponseData), 200)]
    public ActionResult MakeTyreInfoInActive(string username, string tyreId)
    {
      try
      {
        if (username != null && tyreId != null)
        {
          var getTyre = MH.GetSingleObject(tyreinfo_collection, "TyreId", tyreId, null, null).Result;
          if (getTyre != null)
          {
            var updateDefinition = Builders<BsonDocument>.Update.Set("IsActive", false);
            update = MH.UpdateSingleObject(tyreinfo_collection, "TyreId", tyreId, null, null, updateDefinition);
            var data = BsonSerializer.Deserialize<TyreInfo>(getTyre);
            data.IsActive = false;
            AL.CreateLog(username, "MakeTyreInfoInActive", BsonSerializer.Deserialize<TyreInfo>(getTyre), data, activitylog_collection);
            return Ok(new ResponseData
            {
              Code = "200",
              Message = "Tyre info made inactive"
            });
          }
          else
          {
            return BadRequest(new ResponseData
            {
              Code = "404",
              Message = "Tyre info not found"
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
        SL.CreateLog("tyreController", "MakeTyreInfoInActive", ex.Message);
        return BadRequest(new ResponseData
        {
          Code = "400",
          Message = "Failed",
          Data = ex.Message
        });
      }
    }

    /// <summary>
    /// Make tyre info active
    /// </summary>
    /// <param name="username">UserName of user</param>
    /// <param name="tyreId">Id of tyre</param>
    /// <returns></returns>
    /// <response code="200">Tyre info made active</response>
    /// <response code="401">Bad Request</response>
    /// <response code="404">Tyre info not found</response>
    /// <response code="400">Process ran into an exception</response>
    [HttpPut("makeactive/{username}/{tyreId}")]
    [ProducesResponseType(typeof(ResponseData), 200)]
    public ActionResult MakeTyreInfoActive(string username, string tyreId)
    {
      try
      {
        if (User != null && tyreId != null)
        {
          var getTyre = MH.GetSingleObject(tyreinfo_collection, "TyreId", tyreId, null, null).Result;
          if (getTyre != null)
          {
            var updateDefinition = Builders<BsonDocument>.Update.Set("IsActive", true);
            update = MH.UpdateSingleObject(tyreinfo_collection, "TyreId", tyreId, null, null, updateDefinition);
            var data = BsonSerializer.Deserialize<TyreInfo>(getTyre);
            data.IsActive = true;
            AL.CreateLog(username, "MakeTyreInfoActive", BsonSerializer.Deserialize<TyreInfo>(getTyre), data, activitylog_collection);
            return Ok(new ResponseData
            {
              Code = "200",
              Message = "Tyre info made active"
            });
          }
          else
          {
            return BadRequest(new ResponseData
            {
              Code = "404",
              Message = "Tyre info not found"
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
        SL.CreateLog("TyreController", "MakeTyreInfoActive", ex.Message);
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
