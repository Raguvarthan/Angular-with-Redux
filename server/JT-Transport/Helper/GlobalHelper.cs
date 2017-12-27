using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using JT_Transport.Logger;
using MongoDB.Driver;
using SL = JT_Transport.Logger.ServerSideLogger;
using MH = JT_Transport.Helper.MongoHelper;
using System.Collections.Generic;

namespace JT_Transport.Helper
{
  /// <summary>
  /// 
  /// </summary>
  public class GlobalHelper
  {
    /// <summary>Get current directory of project</summary>
    public static string GetCurrentDir()
    {
      return Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
    }

    /// <summary>To read XML</summary>
    public static XElement ReadXML()
    {
      try
      {
        var dir = GetCurrentDir();
        var xmlStr = File.ReadAllText(Path.Combine(dir, "Keys.xml"));
        return XElement.Parse(xmlStr);
      }
      catch (Exception ex)
      {
        SL.CreateLog("GlobalHelper", "ReadXML", ex.Message);
        return null;
      }
    }

    /// <summary>Get ip config from xml</summary>
    public static string GetIpConfig(IMongoCollection<ServerSideLoggerModel> log_collection)
    {
      try
      {
        var result = ReadXML().Elements("ipconfig").Where(x => x.Element("current").Value.Equals("Yes")).Descendants("jt_transport");
        return result.First().Value;
      }
      catch (Exception ex)
      {
        SL.CreateLog("GlobalHelper", "GetIpConfig", ex.Message);
        return null;
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="StartingDate"></param>
    /// <param name="EndingDate"></param>
    /// <returns></returns>
    public static List<DateTime> GetDateRange(DateTime StartingDate, DateTime EndingDate)
    {
      if (StartingDate > EndingDate)
      {
        return null;
      }
      List<DateTime> dateList = new List<DateTime>();
      DateTime tmpDate = StartingDate;
      do
      {
        dateList.Add(tmpDate);
        tmpDate = tmpDate.AddDays(1);
      } while (tmpDate <= EndingDate);
      return dateList;
    }
  }
}
