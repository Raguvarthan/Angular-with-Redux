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
  /// Controller for all vendor related methods
  /// </summary>
  [Produces("application/json")]
  [Route("api/[controller]")]
  public class VendorController : Controller
  {
    /// <summary>
    /// 
    /// </summary>
    public MongoClient _client;
    /// <summary></summary>
    public IMongoDatabase vendor_db;
    /// <summary>
    /// 
    /// </summary>
    public IMongoCollection<BsonDocument> vendorinfo_collection;
    /// <summary>
    /// 
    /// </summary>
    public IMongoCollection<VendorInfo> vendorinfoCollection;
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
    public VendorController()
    {
      _client = MH.GetClient();
      vendor_db = _client.GetDatabase("VendorDB");
      vendorinfo_collection = vendor_db.GetCollection<BsonDocument>("VendorInfo");
      log_db = _client.GetDatabase("LogDB");
      activitylog_collection = log_db.GetCollection<ActivityLoggerModel>("ActivityLog");
      vendorinfoCollection = vendor_db.GetCollection<VendorInfo>("VendorInfo");
    }

    /// <summary>
    /// Get all the vendors and their info
    /// </summary>
    /// <response code="200">Returns all the vendors and their info</response>
    /// <response code="404">No vendors found</response>
    /// <response code="400">Process ran into an exception</response>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(ResponseData), 200)]
    public ActionResult GetAllVendors()
    {
      try
      {
        var getVendors = MH.GetListOfObjects(vendorinfo_collection, null, null, null, null).Result;
        if (getVendors != null)
        {
          List<VendorInfo> vendorInfo = new List<VendorInfo>();
          foreach (var vendor in getVendors)
          {
            vendorInfo.Add(BsonSerializer.Deserialize<VendorInfo>(vendor));
          }
          return Ok(new ResponseData
          {
            Code = "200",
            Message = "Success",
            Data = vendorInfo
          });
        }
        else
        {
          return BadRequest(new ResponseData
          {
            Code = "404",
            Message = "No vendors found"
          });
        }
      }
      catch (Exception ex)
      {
        SL.CreateLog("VendorController", "GetAllVendors", ex.Message);
        return BadRequest(new ResponseData
        {
          Code = "400",
          Message = "Failed",
          Data = ex.Message
        });
      }
    }

    /// <summary>
    /// Gets info of vendor with given vendor id
    /// </summary>
    /// <param name="vendorId">Id of vendor</param>
    /// <response code="200">Returns info of vendor with given vendor id</response>
    /// <response code="401">Bad Request</response>
    /// <response code="404">Vendors not found</response>
    /// <response code="400">Process ran into an exception</response>
    /// <returns></returns>
    [HttpGet("{vendorId}")]
    [ProducesResponseType(typeof(ResponseData), 200)]
    public ActionResult GetInfoOfVendor(string vendorId)
    {
      try
      {
        if (vendorId != null)
        {
          var getVendor = MH.GetSingleObject(vendorinfo_collection, "VendorId", vendorId, null, null).Result;
          if (getVendor != null)
          {
            var vendorInfo = BsonSerializer.Deserialize<VendorInfo>(getVendor);
            return Ok(new ResponseData
            {
              Code = "200",
              Message = "Success",
              Data = vendorInfo
            });
          }
          else
          {
            return BadRequest(new ResponseData
            {
              Code = "404",
              Message = "Vendor not found"
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
        SL.CreateLog("VendorController", "GetInfoOfVendor", ex.Message);
        return BadRequest(new ResponseData
        {
          Code = "400",
          Message = "Failed",
          Data = ex.Message
        });
      }
    }

    /// <summary>
    /// Insert new vendor info
    /// </summary>
    /// <param name="data">Info of vendor</param>
    /// <param name="username">UserName of user</param>
    /// <response code="200">Vendor info inserted successfully</response>
    /// <response code="401">Vendor with same vendor name and address is already added</response>
    /// <response code="402">Vendor info with same id is already added</response>
    /// <response code="403">Bad Request</response>
    /// <response code="400">Process ran into an exception</response>
    /// <returns></returns>
    [HttpPost("{username}")]
    [SwaggerRequestExample(typeof(VendorInfo), typeof(Example_InsertVendorInfo))]
    [ProducesResponseType(typeof(ResponseData), 200)]
    public ActionResult InsertVendorInfo([FromBody]VendorInfo data, string username)
    {
      try
      {
        if (data != null && username != null)
        {
          var checkVendor = MH.GetSingleObject(vendorinfo_collection, "VendorName", data.VendorName, "Address", data.Address).Result;
          if (checkVendor != null)
          {
            return BadRequest(new ResponseData
            {
              Code = "401",
              Message = "Vendor with same vendor name and address is already added"
            });
          }
          else
          {
            #region Calculate vendor id
            var getVendors = MH.GetListOfObjects(vendorinfo_collection, null, null, null, null).Result;
            if (getVendors.Count == 0)
            {
              data.VendorId = "VD-1";
            }
            else
            {
              List<long> idList = new List<long>();
              foreach (var vendor in getVendors)
              {
                VendorInfo vendorInfo = BsonSerializer.Deserialize<VendorInfo>(vendor);
                long seperatedId = Convert.ToInt64(vendorInfo.VendorId.Substring(vendorInfo.VendorId.LastIndexOf('-') + 1));
                idList.Add(seperatedId);
              }
              var maxId = idList.Max();
              data.VendorId = "VD-" + (maxId + 1);
            }
            #endregion
            data.IsActive = true;
            var insert = MH.InsertNewVendorInfo(data, vendorinfoCollection);
            if (insert == true)
            {
              AL.CreateLog(username, "InsertVendorInfo", null, data, activitylog_collection);
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
                Message = "Vendor info with same id is already added"
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
        SL.CreateLog("VendorController", "InsertVendorInfo", ex.Message);
        return BadRequest(new ResponseData
        {
          Code = "400",
          Message = "Failed",
          Data = ex.Message
        });
      }
    }

    /// <summary>
    /// Update vendor info
    /// </summary>
    /// <param name="data">Data to be updated</param>
    /// <param name="username">UserName of user</param>
    /// <param name="vendorId">Id of vendor</param>
    /// <returns></returns>
    /// <response code="200">Vendor info updated successfully </response>
    /// <response code="401">Update failed</response>
    /// <response code="402">Bad Request</response>
    /// <response code="404">Vendor not found</response>
    /// <response code="400">Process ran into an exception</response>
    [HttpPut("{username}/{vendorId}")]
    [SwaggerRequestExample(typeof(ExampleModel_VendorInfo), typeof(Example_UpdateVendorInfo))]
    [ProducesResponseType(typeof(ResponseData), 200)]
    public ActionResult UpdateVendorInfo([FromBody]ExampleModel_VendorInfo data, string username, string vendorId)
    {
      try
      {
        if (data != null && username != null && vendorId != null)
        {
          var getVendor = MH.GetSingleObject(vendorinfo_collection, "VendorId", vendorId, null, null).Result;
          if (getVendor != null)
          {
            if (data.VendorName != null)
            {
              var updateDefinition = Builders<BsonDocument>.Update.Set("VendorName", data.VendorName);
              update = MH.UpdateSingleObject(vendorinfo_collection, "VendorId", vendorId, null, null, updateDefinition);
            }
            if (data.ContactName != null)
            {
              var updateDefinition = Builders<BsonDocument>.Update.Set("ContactName", data.ContactName);
              update = MH.UpdateSingleObject(vendorinfo_collection, "VendorId", vendorId, null, null, updateDefinition);
            }
            if (data.ContactNo != 0)
            {
              var updateDefinition = Builders<BsonDocument>.Update.Set("ContactNo", data.ContactNo);
              update = MH.UpdateSingleObject(vendorinfo_collection, "VendorId", vendorId, null, null, updateDefinition);
            }
            if (data.Address != null)
            {
              var updateDefinition = Builders<BsonDocument>.Update.Set("Address", data.Address);
              update = MH.UpdateSingleObject(vendorinfo_collection, "VendorId", vendorId, null, null, updateDefinition);
            }
            if (data.IsActive != null)
            {
              var updateDefinition = Builders<BsonDocument>.Update.Set("IsActive", data.IsActive);
              update = MH.UpdateSingleObject(vendorinfo_collection, "VendorId", vendorId, null, null, updateDefinition);
            }
            if (update != null)
            {
              AL.CreateLog(username, "UpdateVendorInfo", BsonSerializer.Deserialize<VendorInfo>(update), data, activitylog_collection);
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
              Message = "Vendor info not found"
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
        SL.CreateLog("VendorController", "UpdateVendorInfo", ex.Message);
        return BadRequest(new ResponseData
        {
          Code = "400",
          Message = "Failed",
          Data = ex.Message
        });
      }
    }

    /// <summary>
    /// Make vendor info inactive
    /// </summary>
    /// <param name="username">UserName of user</param>
    /// <param name="vendorId">Id of vendor</param>
    /// <returns></returns>
    /// <response code="200">Vendor info made inactive</response>
    /// <response code="401">Bad Request</response>
    /// <response code="404">Vendor info not found</response>
    /// <response code="400">Process ran into an exception</response>
    [HttpDelete("{username}/{vendorId}")]
    [ProducesResponseType(typeof(ResponseData), 200)]
    public ActionResult MakeVendorInfoInActive(string username, string vendorId)
    {
      try
      {
        if (username != null && vendorId != null)
        {
          var getVendor = MH.GetSingleObject(vendorinfo_collection, "VendorId", vendorId, null, null).Result;
          if (getVendor != null)
          {
            var updateDefinition = Builders<BsonDocument>.Update.Set("IsActive", false);
            update = MH.UpdateSingleObject(vendorinfo_collection, "VendorId", vendorId, null, null, updateDefinition);
            var data = BsonSerializer.Deserialize<VendorInfo>(getVendor);
            data.IsActive = false;
            AL.CreateLog(username, "MakeVendorInfoInActive", BsonSerializer.Deserialize<VendorInfo>(getVendor), data, activitylog_collection);
            return Ok(new ResponseData
            {
              Code = "200",
              Message = "Vendor info made inactive"
            });
          }
          else
          {
            return BadRequest(new ResponseData
            {
              Code = "404",
              Message = "Vendor info not found"
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
        SL.CreateLog("VendorController", "MakeVendorInfoInActive", ex.Message);
        return BadRequest(new ResponseData
        {
          Code = "400",
          Message = "Failed",
          Data = ex.Message
        });
      }
    }

    /// <summary>
    /// Make vendor info active
    /// </summary>
    /// <param name="username">UserName of user</param>
    /// <param name="vendorId">Id of vendor</param>
    /// <returns></returns>
    /// <response code="200">Vendor info made active</response>
    /// <response code="401">Bad Request</response>
    /// <response code="404">Vendor info not found</response>
    /// <response code="400">Process ran into an exception</response>
    [HttpPut("makeactive/{username}/{vendorId}")]
    [ProducesResponseType(typeof(ResponseData), 200)]
    public ActionResult MakeVendorInfoActive(string username, string vendorId)
    {
      try
      {
        if (vendorId != null && username != null)
        {
          var getVendor = MH.GetSingleObject(vendorinfo_collection, "VendorId", vendorId, null, null).Result;
          if (getVendor != null)
          {
            var updateDefinition = Builders<BsonDocument>.Update.Set("IsActive", true);
            update = MH.UpdateSingleObject(vendorinfo_collection, "VendorId", vendorId, null, null, updateDefinition);
            var data = BsonSerializer.Deserialize<VendorInfo>(getVendor);
            data.IsActive = true;
            AL.CreateLog(username, "MakeVendorInfoActive", BsonSerializer.Deserialize<VendorInfo>(getVendor), data, activitylog_collection);
            return Ok(new ResponseData
            {
              Code = "200",
              Message = "Vendor info made active"
            });
          }
          else
          {
            return BadRequest(new ResponseData
            {
              Code = "404",
              Message = "Vendor info not found"
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
        SL.CreateLog("VendorController", "MakeVendorInfoActive", ex.Message);
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
