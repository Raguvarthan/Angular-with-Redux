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
  /// Controller for all office related methods
  /// </summary>
  [Produces("application/json")]
  [Route("api/[controller]")]
  public class OfficeController : Controller
  {
    /// <summary>
    /// 
    /// </summary>
    public MongoClient _client;
    /// <summary></summary>
    public IMongoDatabase office_db;
    /// <summary>
    /// 
    /// </summary>
    public IMongoCollection<BsonDocument> officeinfo_collection;
    /// <summary>
    /// 
    /// </summary>
    public IMongoCollection<OfficeInfo> officeinfoCollection;
    /// <summary>
    /// 
    /// </summary>
    public BsonDocument update;
    /// <summary></summary>
    public IMongoDatabase log_db;
    /// <summary>
    /// 
    /// </summary>
    public IMongoCollection<ActivityLoggerModel> activitylog_collection;

    /// <summary>
    /// 
    /// </summary>
    public OfficeController()
    {
      _client = MH.GetClient();
      office_db = _client.GetDatabase("OfficeDB");
      officeinfo_collection = office_db.GetCollection<BsonDocument>("OfficeInfo");
      log_db = _client.GetDatabase("LogDB");
      activitylog_collection = log_db.GetCollection<ActivityLoggerModel>("ActivityLog");
      officeinfoCollection = office_db.GetCollection<OfficeInfo>("OfficeInfo");
    }

    /// <summary>
    /// Get all the offices and their info
    /// </summary>
    /// <response code="200">Returns all the offices and their info</response>
    /// <response code="404">No offices found</response>
    /// <response code="400">Process ran into an exception</response>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(ResponseData), 200)]
    public ActionResult GetAllOffices()
    {
      try
      {
        var getOffices = MH.GetListOfObjects(officeinfo_collection, null, null, null, null).Result;
        if (getOffices != null)
        {
          List<OfficeInfo> officeInfo = new List<OfficeInfo>();
          foreach (var office in getOffices)
          {
            officeInfo.Add(BsonSerializer.Deserialize<OfficeInfo>(office));
          }
          return Ok(new ResponseData
          {
            Code = "200",
            Message = "Success",
            Data = officeInfo
          });
        }
        else
        {
          return BadRequest(new ResponseData
          {
            Code = "404",
            Message = "No offices found"
          });
        }
      }
      catch (Exception ex)
      {
        SL.CreateLog("OfficeController", "GetAllOffices", ex.Message);
        return BadRequest(new ResponseData
        {
          Code = "400",
          Message = "Failed",
          Data = ex.Message
        });
      }
    }

    /// <summary>
    /// Gets info of office with given office id
    /// </summary>
    /// <param name="officeId">Id of office</param>
    /// <response code="200">Returns info of office with given office id</response>
    /// <response code="401">Bad Request</response>
    /// <response code="404">Offices not found</response>
    /// <response code="400">Process ran into an exception</response>
    /// <returns></returns>
    [HttpGet("{officeId}")]
    [ProducesResponseType(typeof(ResponseData), 200)]
    public ActionResult GetInfoOfOffice(string officeId)
    {
      try
      {
        if (officeId != null)
        {
          var getOffice = MH.GetSingleObject(officeinfo_collection, "OfficeId", officeId, null, null).Result;
          if (getOffice != null)
          {
            var officeInfo = BsonSerializer.Deserialize<OfficeInfo>(getOffice);
            return Ok(new ResponseData
            {
              Code = "200",
              Message = "Success",
              Data = officeInfo
            });
          }
          else
          {
            return BadRequest(new ResponseData
            {
              Code = "404",
              Message = "Office not found"
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
        SL.CreateLog("OfficeController", "GetInfoOfOffice", ex.Message);
        return BadRequest(new ResponseData
        {
          Code = "400",
          Message = "Failed",
          Data = ex.Message
        });
      }
    }

    /// <summary>
    /// Insert new office info
    /// </summary>
    /// <param name="data">Info of office</param>
    /// <param name="username">UserName of user</param>
    /// <response code="200">Office info inserted successfully</response>
    /// <response code="401">Office with same office name and address is already added</response>
    /// <response code="402">Office already added and is made active</response>
    /// <response code="403">Office info with same id is already added</response>
    /// <response code="405">Bad Request</response>
    /// <response code="400">Process ran into an exception</response>
    /// <returns></returns>
    [HttpPost("{username}")]
    [SwaggerRequestExample(typeof(OfficeInfo), typeof(Example_InsertOfficeInfo))]
    [ProducesResponseType(typeof(ResponseData), 200)]
    public ActionResult InsertOfficeInfo([FromBody]OfficeInfo data, string username)
    {
      try
      {
        if (data != null && username != null)
        {
          var checkOffice = MH.GetSingleObject(officeinfo_collection, "OfficeName", data.OfficeName, "Address", data.Address).Result;
          if (checkOffice != null)
          {
            var officeInfo = BsonSerializer.Deserialize<OfficeInfo>(checkOffice);
            if (officeInfo.IsActive == true)
            {
              return BadRequest(new ResponseData
              {
                Code = "401",
                Message = "Office with same office name and address is already added"
              });
            }
            else
            {
              var updateDefinition = Builders<BsonDocument>.Update.Set("IsActive", true);
              update = MH.UpdateSingleObject(officeinfo_collection, "OfficeName", data.OfficeName, "Address", data.Address, updateDefinition);
              var detail = officeInfo;
              detail.IsActive = true;
              AL.CreateLog(username, "InsertOfficeInfo", officeInfo, detail, activitylog_collection);
              return BadRequest(new ResponseData
              {
                Code = "402",
                Message = "Office already added and its made active"
              });
            }
          }
          else
          {
            #region Calculate office id
            var getOffices = MH.GetListOfObjects(officeinfo_collection, null, null, null, null).Result;
            if (getOffices.Count == 0)
            {
              data.OfficeId = "OF-1";
            }
            else
            {
              List<long> idList = new List<long>();
              foreach (var office in getOffices)
              {
                OfficeInfo officeInfo = BsonSerializer.Deserialize<OfficeInfo>(office);
                long seperatedId = Convert.ToInt64(officeInfo.OfficeId.Substring(officeInfo.OfficeId.LastIndexOf('-') + 1));
                idList.Add(seperatedId);
              }
              var maxId = idList.Max();
              data.OfficeId = "OF-" + (maxId + 1);
              #endregion
            }
            data.IsActive = true;
            var insert = MH.InsertNewOfficeInfo(data, officeinfoCollection);
            if (insert == true)
            {
              AL.CreateLog(username, "InsertOfficeInfo", null, data, activitylog_collection);
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
                Message = "Office info with same id is already added"
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
        }
        else
        {
          return BadRequest(new ResponseData
          {
            Code = "405",
            Message = "Bad Request"
          });
        }
      }
      catch (Exception ex)
      {
        SL.CreateLog("OfficeController", "InsertOfficeInfo", ex.Message);
        return BadRequest(new ResponseData
        {
          Code = "400",
          Message = "Failed",
          Data = ex.Message
        });
      }
    }

    /// <summary>
    /// Update office info
    /// </summary>
    /// <param name="data">Data to be updated</param>
    /// <param name="username">UserName of user</param>
    /// <param name="officeId">Id of office</param>
    /// <returns></returns>
    /// <response code="200">Office info updated successfully </response>
    /// <response code="401">Bad Request</response>
    /// <response code="404">Office not found</response>
    /// <response code="400">Process ran into an exception</response>
    [HttpPut("{username}/{officeId}")]
    [SwaggerRequestExample(typeof(ExampleModel_OfficeInfo), typeof(Example_UpdateOfficeInfo))]
    [ProducesResponseType(typeof(ResponseData), 200)]
    public ActionResult UpdateOfficeInfo([FromBody]ExampleModel_OfficeInfo data, string username, string officeId)
    {
      try
      {
        if (data != null && username != null && officeId != null)
        {
          var getOffice = MH.GetSingleObject(officeinfo_collection, "OfficeId", officeId, null, null).Result;
          if (getOffice != null)
          {
            if (data.OfficeName != null)
            {
              var updateDefinition = Builders<BsonDocument>.Update.Set("OfficeName", data.OfficeName);
              update = MH.UpdateSingleObject(officeinfo_collection, "OfficeId", officeId, null, null, updateDefinition);
            }
            if (data.ContactName != null)
            {
              var updateDefinition = Builders<BsonDocument>.Update.Set("ContactName", data.ContactName);
              update = MH.UpdateSingleObject(officeinfo_collection, "OfficeId", officeId, null, null, updateDefinition);
            }
            if (data.ContactNo != 0)
            {
              var updateDefinition = Builders<BsonDocument>.Update.Set("ContactNo", data.ContactNo);
              update = MH.UpdateSingleObject(officeinfo_collection, "OfficeId", officeId, null, null, updateDefinition);
            }
            if (data.Address != null)
            {
              var updateDefinition = Builders<BsonDocument>.Update.Set("Address", data.Address);
              update = MH.UpdateSingleObject(officeinfo_collection, "OfficeId", officeId, null, null, updateDefinition);
            }
            if (data.IsActive != null)
            {
              var updateDefinition = Builders<BsonDocument>.Update.Set("IsActive", data.IsActive);
              update = MH.UpdateSingleObject(officeinfo_collection, "OfficeId", officeId, null, null, updateDefinition);
            }
            AL.CreateLog(username, "UpdateOfficeInfo", BsonSerializer.Deserialize<OfficeInfo>(getOffice), data, activitylog_collection);
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
              Message = "Office info not found"
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
        SL.CreateLog("OfficeController", "UpdateOfficeInfo", ex.Message);
        return BadRequest(new ResponseData
        {
          Code = "400",
          Message = "Failed",
          Data = ex.Message
        });
      }
    }

    /// <summary>
    /// Make office info inactive
    /// </summary>
    /// <param name="username">UserName of user</param>
    /// <param name="officeId">Id of office</param>
    /// <returns></returns>
    /// <response code="200">Office info made inactive</response>
    /// <response code="401">Bad Request</response>
    /// <response code="404">Office not found</response>
    /// <response code="400">Process ran into an exception</response>
    [HttpDelete("{username}/{officeId}")]
    [ProducesResponseType(typeof(ResponseData), 200)]
    public ActionResult MakeUserInfoInActive(string username, string officeId)
    {
      try
      {
        if (username != null && officeId != null)
        {
          var getOffice = MH.GetSingleObject(officeinfo_collection, "OfficeId", officeId, null, null).Result;
          if (getOffice != null)
          {
            var updateDefinition = Builders<BsonDocument>.Update.Set("IsActive", false);
            update = MH.UpdateSingleObject(officeinfo_collection, "OfficeId", officeId, null, null, updateDefinition);
            var data = BsonSerializer.Deserialize<OfficeInfo>(getOffice);
            data.IsActive = false;
            AL.CreateLog(username, "MakeUserInfoInActive", BsonSerializer.Deserialize<OfficeInfo>(getOffice), data, activitylog_collection);
            return Ok(new ResponseData
            {
              Code = "200",
              Message = "UserInfo made inactive"
            });
          }
          else
          {
            return BadRequest(new ResponseData
            {
              Code = "404",
              Message = "Office info not found"
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
        SL.CreateLog("OfficeController", "MakeUserInfoInActive", ex.Message);
        return BadRequest(new ResponseData
        {
          Code = "400",
          Message = "Failed",
          Data = ex.Message
        });
      }
    }

    /// <summary>
    /// Make office info active
    /// </summary>
    /// <param name="username">UserName of user</param>
    /// <param name="officeId">Id of office</param>
    /// <returns></returns>
    /// <response code="200">Office info made active </response>
    /// <response code="401">Bad Request</response>
    /// <response code="404">Office not found</response>
    /// <response code="400">Process ran into an exception</response>
    [HttpPut("makeactive/{username}/{officeId}")]
    [ProducesResponseType(typeof(ResponseData), 200)]
    public ActionResult DeleteOfficeInfo(string username, string officeId)
    {
      try
      {
        if (username != null && officeId != null)
        {
          var getOffice = MH.GetSingleObject(officeinfo_collection, "OfficeId", officeId, null, null).Result;
          if (getOffice != null)
          {
            var updateDefinition = Builders<BsonDocument>.Update.Set("IsActive", true);
            update = MH.UpdateSingleObject(officeinfo_collection, "OfficeId", officeId, null, null, updateDefinition);
            var data = BsonSerializer.Deserialize<OfficeInfo>(getOffice);
            data.IsActive = true;
            AL.CreateLog(username, "DeleteOfficeInfo", BsonSerializer.Deserialize<OfficeInfo>(getOffice), data, activitylog_collection);
            return Ok(new ResponseData
            {
              Code = "200",
              Message = "UserInfo made active"
            });
          }
          else
          {
            return BadRequest(new ResponseData
            {
              Code = "404",
              Message = "Office info not found"
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
        SL.CreateLog("OfficeController", "InsertOfficeInfo", ex.Message);
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
