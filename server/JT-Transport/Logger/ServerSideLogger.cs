using System;
using MongoDB.Bson;
using MongoDB.Driver;
using MH = JT_Transport.Helper.MongoHelper;

namespace JT_Transport.Logger
{
  /// <summary>
  /// 
  /// </summary>
  public class ServerSideLoggerModel
  {
    /// <summary>
    /// 
    /// </summary>
    public ObjectId Id { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string ClassName { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string MethodName { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public DateTime? DateAndTime { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string Error { get; set; }
  }

  /// <summary>
  /// 
  /// </summary>
  public class ServerSideLogger
  {
    /// <summary>Create log for server side error</summary>
    /// <param name="className"></param>
    /// <param name="methodName"></param>
    /// <param name="errorDescription"></param>
    public static void CreateLog(string className, string methodName, string errorDescription)
    {
      ServerSideLoggerModel logger =
          new ServerSideLoggerModel
          {
            ClassName = className,
            MethodName = methodName,
            Error = errorDescription,
            DateAndTime = DateTime.UtcNow
          };
      MH.GetClient().GetDatabase("LogDB").GetCollection<ServerSideLoggerModel>("ServerLog").InsertOneAsync(logger);
    }
  }
}
