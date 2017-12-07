using System;
using System.Linq;
using MongoDB.Bson;
using System.Xml.Linq;
using MongoDB.Driver;
using SL = JT_Transport.Logger.ServerSideLogger;
using System.Threading.Tasks;
using System.Collections.Generic;
using JT_Transport.Logger;
using JT_Transport.Model;
using MongoDB.Driver.Builders;

namespace JT_Transport.Helper
{
  /// <summary>
  /// 
  /// </summary>
  public class MongoHelper
  {
    /// <summary>
    /// 
    /// </summary>
    public static FilterDefinition<BsonDocument> filter;

    /// <summary>Get Mongo Client</summary>
    public static MongoClient GetClient()
    {
      try
      {
        var ip = GlobalHelper.ReadXML().Elements("mongo").Where(x => x.Element("current").Value.Equals("Yes")).Descendants("ip").First().Value;
        var port = GlobalHelper.ReadXML().Elements("mongo").Where(x => x.Element("current").Value.Equals("Yes")).Descendants("port").First().Value;
        var user = GlobalHelper.ReadXML().Elements("mongo").Where(x => x.Element("current").Value.Equals("Yes")).Descendants("user").First().Value;
        var password = GlobalHelper.ReadXML().Elements("mongo").Where(x => x.Element("current").Value.Equals("Yes")).Descendants("password").First().Value;
        var db = GlobalHelper.ReadXML().Elements("mongo").Where(x => x.Element("current").Value.Equals("Yes")).Descendants("db").First().Value;
        var connectionString = "mongodb://" + user + ":" + password + "@" + ip + ":27018/" + db;
        //var connectionString = "mongodb://" + ip + ":" + port + "/" + db;
        var mongoClient = new MongoClient(connectionString);
        return mongoClient;
      }
      catch (Exception ex)
      {
        SL.CreateLog("MongoHelper", "GetClient", ex.Message);
        return null;
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="collection"></param>
    /// <param name="filterField1"></param>
    /// <param name="filterData1"></param>
    /// <param name="filterField2"></param>
    /// <param name="filterData2"></param>
    /// <returns></returns>
    public static async Task<bool?> CheckForData(IMongoCollection<BsonDocument> collection, string filterField1, dynamic filterData1, string filterField2, dynamic filterData2)
    {
      try
      {
        if (filterField1 == null & filterField2 == null)
        {
          filter = FilterDefinition<BsonDocument>.Empty;
        }
        else if (filterField1 != null & filterField2 == null)
        {
          filter = Builders<BsonDocument>.Filter.Eq(filterField1, filterData1);
        }
        else if (filterField1 != null & filterField2 != null)
        {
          filter = Builders<BsonDocument>.Filter.Eq(filterField1, filterData1) & Builders<BsonDocument>.Filter.Eq(filterField2, filterData2);
        }
        IAsyncCursor<BsonDocument> cursor = await collection.FindAsync(filter);
        if (cursor.FirstOrDefault() != null)
        {
          return true;
        }
        else
        {
          return false;
        }
      }
      catch (Exception ex)
      {
        SL.CreateLog("MongoHelper", "GetSingleObject", ex.Message);
        return null;
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="collection"></param>
    /// <param name="filterField1"></param>
    /// <param name="filterData1"></param>
    /// <param name="filterField2"></param>
    /// <param name="filterData2"></param>
    /// <returns></returns>
    public static async Task<List<BsonDocument>> GetListOfObjects(IMongoCollection<BsonDocument> collection, string filterField1, dynamic filterData1, string filterField2, dynamic filterData2)
    {
      try
      {
        if (filterField1 == null & filterField2 == null)
        {
          filter = FilterDefinition<BsonDocument>.Empty;
        }
        else if (filterField1 != null & filterField2 == null)
        {
          filter = Builders<BsonDocument>.Filter.Eq(filterField1, filterData1);
        }
        else if (filterField1 != null & filterField2 != null)
        {
          filter = Builders<BsonDocument>.Filter.Eq(filterField1, filterData1) & Builders<BsonDocument>.Filter.Eq(filterField2, filterData2);
        }
        IAsyncCursor<BsonDocument> cursor = await collection.FindAsync(filter);
        return cursor.ToList();
      }
      catch (Exception ex)
      {
        SL.CreateLog("MongoHelper", "GetListOfObjects", ex.Message);
        return null;
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="collection"></param>
    /// <param name="filterField1"></param>
    /// <param name="filterData1"></param>
    /// <param name="filterField2"></param>
    /// <param name="filterData2"></param>
    /// <returns></returns>
    public static async Task<BsonDocument> GetSingleObject(IMongoCollection<BsonDocument> collection, string filterField1, dynamic filterData1, string filterField2, dynamic filterData2)
    {
      try
      {
        if (filterField1 == null & filterField2 == null)
        {
          filter = FilterDefinition<BsonDocument>.Empty;
        }
        else if (filterField1 != null & filterField2 == null)
        {
          filter = Builders<BsonDocument>.Filter.Eq(filterField1, filterData1);
        }
        else if (filterField1 != null & filterField2 != null)
        {
          filter = Builders<BsonDocument>.Filter.Eq(filterField1, filterData1) & Builders<BsonDocument>.Filter.Eq(filterField2, filterData2);
        }
        IAsyncCursor<BsonDocument> cursor = await collection.FindAsync(filter);
        return cursor.FirstOrDefault();
      }
      catch (Exception ex)
      {
        SL.CreateLog("MongoHelper", "GetSingleObject", ex.Message);
        return null;
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="collection"></param>
    /// <param name="filterField1"></param>
    /// <param name="filterData1"></param>
    /// <param name="filterField2"></param>
    /// <param name="filterData2"></param>
    /// <param name="update"></param>
    /// <returns></returns>
    public static BsonDocument UpdateSingleObject(IMongoCollection<BsonDocument> collection, string filterField1, dynamic filterData1, string filterField2, dynamic filterData2, UpdateDefinition<BsonDocument> update)
    {
      try
      {
        if (filterField1 == null & filterField2 == null)
        {
          filter = FilterDefinition<BsonDocument>.Empty;
        }
        else if (filterField1 != null & filterField2 == null)
        {
          filter = Builders<BsonDocument>.Filter.Eq(filterField1, filterData1);
        }
        else if (filterField1 != null & filterField2 != null)
        {
          filter = Builders<BsonDocument>.Filter.Eq(filterField1, filterData1) & Builders<BsonDocument>.Filter.Eq(filterField2, filterData2);
        }
        BsonDocument cursor = collection.FindOneAndUpdate(filter, update);
        return cursor;
      }
      catch (Exception ex)
      {
        SL.CreateLog("MongoHelper", "GetSingleObject", ex.Message);
        return null;
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="collection"></param>
    /// <param name="filterField1"></param>
    /// <param name="filterData1"></param>
    /// <param name="filterField2"></param>
    /// <param name="filterData2"></param>
    /// <returns></returns>
    public static async Task<bool?> DeleteSingleObject(IMongoCollection<BsonDocument> collection, string filterField1, dynamic filterData1, string filterField2, dynamic filterData2)
    {
      try
      {
        if (filterField1 == null & filterField2 == null)
        {
          filter = FilterDefinition<BsonDocument>.Empty;
        }
        else if (filterField1 != null & filterField2 == null)
        {
          filter = Builders<BsonDocument>.Filter.Eq(filterField1, filterData1);
        }
        else if (filterField1 != null & filterField2 != null)
        {
          filter = Builders<BsonDocument>.Filter.Eq(filterField1, filterData1) & Builders<BsonDocument>.Filter.Eq(filterField2, filterData2);
        }
        var cursor = await collection.DeleteOneAsync(filter);
        return true;
      }
      catch (Exception ex)
      {
        SL.CreateLog("MongoHelper", "DeleteSingleObject", ex.Message);
        return null;
      }
    }

    #region Create unique index

    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    /// <param name="collection"></param>
    /// <returns></returns>
    public static dynamic InsertNewRole(Roles data, IMongoCollection<Roles> collection)
    {
      try
      {
        collection.Indexes.CreateOne("{ RoleId: " + data.RoleId + " }");
        collection.InsertOne(data);
        return true;
      }
      catch (Exception ex)
      {
        if (ex.Message.Contains("duplicate key error"))
        {
          return ex.Message;
        }
        else
        {
          SL.CreateLog("MongoHelper", "InsertNewRole", ex.Message);
          return false;
        }
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    /// <param name="collection"></param>
    /// <returns></returns>
    public static dynamic InsertNewOfficeInfo(OfficeInfo data, IMongoCollection<OfficeInfo> collection)
    {
      try
      {
        #region Create index
        //collection.Indexes.CreateOne("{ OfficeId: " + data.OfficeId + " }");
        #endregion
        collection.InsertOne(data);
        return true;
      }
      catch (Exception ex)
      {
        if (ex.Message.Contains("duplicate key error"))
        {
          return ex.Message;
        }
        else
        {
          SL.CreateLog("MongoHelper", "InsertNewOfficeInfo", ex.Message);
          return false;
        }
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    /// <param name="collection"></param>
    /// <returns></returns>
    public static dynamic InsertNewTripInfo(TripInfo data, IMongoCollection<TripInfo> collection)
    {
      try
      {
        #region Create index
        //collection.Indexes.CreateOne("{ TripId: " + data.TripId + " }");
        #endregion
        collection.InsertOne(data);
        return true;
      }
      catch (Exception ex)
      {
        if (ex.Message.Contains("duplicate key error"))
        {
          return ex.Message;
        }
        else
        {
          SL.CreateLog("MongoHelper", "InsertNewTripInfo", ex.Message);
          return false;
        }
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    /// <param name="collection"></param>
    /// <returns></returns>
    public static dynamic InsertNewTripExpenseInfo(TripExpenceInfo data, IMongoCollection<TripExpenceInfo> collection)
    {
      try
      {
        #region Create index
        //collection.Indexes.CreateOne("{ TripExpenseId: " + data.TripExpenseId + " }");
        #endregion
        collection.InsertOne(data);
        return true;
      }
      catch (Exception ex)
      {
        if (ex.Message.Contains("duplicate key error"))
        {
          return ex.Message;
        }
        else
        {
          SL.CreateLog("MongoHelper", "InsertNewTripExpenseInfo", ex.Message);
          return false;
        }
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    /// <param name="collection"></param>
    /// <returns></returns>
    public static dynamic InsertNewVendorInfo(VendorInfo data, IMongoCollection<VendorInfo> collection)
    {
      try
      {
        #region Create index
        //collection.Indexes.CreateOne("{ VendorId: " + data.VendorId + " }");
        #endregion
        collection.InsertOne(data);
        return true;
      }
      catch (Exception ex)
      {
        if (ex.Message.Contains("duplicate key error"))
        {
          return ex.Message;
        }
        else
        {
          SL.CreateLog("MongoHelper", "InsertNewVendorInfo", ex.Message);
          return false;
        }
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    /// <param name="collection"></param>
    /// <returns></returns>
    public static dynamic InsertNewVehicleInfo(VehicleInfo data, IMongoCollection<VehicleInfo> collection)
    {
      try
      {
        #region Create index
        //collection.Indexes.CreateOne("{ VehicleId: " + data.VehicleId + " }");
        #endregion
        collection.InsertOne(data);
        return true;
      }
      catch (Exception ex)
      {
        if (ex.Message.Contains("duplicate key error"))
        {
          return ex.Message;
        }
        else
        {
          SL.CreateLog("MongoHelper", "InsertNewVehicleInfo", ex.Message);
          return false;
        }
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    /// <param name="collection"></param>
    /// <returns></returns>
    public static dynamic InsertNewTyreInfo(TyreInfo data, IMongoCollection<TyreInfo> collection)
    {
      try
      {
        #region Create index
        //collection.Indexes.CreateOne("{ TyreId: " + data.TyreId + " }");
        #endregion
        collection.InsertOne(data);
        return true;
      }
      catch (Exception ex)
      {
        if (ex.Message.Contains("duplicate key error"))
        {
          return ex.Message;
        }
        else
        {
          SL.CreateLog("MongoHelper", "InsertNewTyreInfo", ex.Message);
          return false;
        }
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    /// <param name="collection"></param>
    /// <returns></returns>
    public static dynamic InsertNewDriverInfo(DriverInfo data, IMongoCollection<DriverInfo> collection)
    {
      try
      {
        #region Create index
        //collection.Indexes.CreateOne("{ DriverId: " + data.DriverId + " }");
        #endregion
        collection.InsertOne(data);
        return true;
      }
      catch (Exception ex)
      {
        if (ex.Message.Contains("duplicate key error"))
        {
          return ex.Message;
        }
        else
        {
          SL.CreateLog("MongoHelper", "InsertNewDriverInfo", ex.Message);
          return false;
        }
      }
    }

    #endregion
  }
}
