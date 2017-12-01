using System;
using MongoDB.Bson;
using MongoDB.Driver;
using MH = JT_Transport.Helper.MongoHelper;

namespace JT_Transport.Logger
{
  /// <summary>
  /// 
  /// </summary>
  public class ActivityLoggerModel
  {
    /// <summary>
    /// 
    /// </summary>
    public ObjectId Id { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string MethodName { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public DateTime DateAndTime { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string UserName { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public dynamic PreviousData { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public dynamic CurrentData { get; set; }
  }

  /// <summary>
  /// 
  /// </summary>
  public class ActivityLogger
  {
    /// <summary>Create log for server side error</summary>
    /// <param name="username"></param>
    /// <param name="methodName"></param>
    /// <param name="previousData"></param>
    /// <param name="currentData"></param>
    /// <param name="collection"></param>
    public static void CreateLog(string username, string methodName, dynamic previousData,dynamic currentData,IMongoCollection<ActivityLoggerModel> collection)
    {
      ActivityLoggerModel logger =
          new ActivityLoggerModel
          {
            UserName = username,
            MethodName = methodName,
            CurrentData = currentData,
            PreviousData = previousData,
            DateAndTime = DateTime.UtcNow
          };
      collection.InsertOneAsync(logger);
    }
  }
}
