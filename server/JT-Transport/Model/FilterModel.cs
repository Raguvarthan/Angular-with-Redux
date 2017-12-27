using System;

namespace JT_Transport.Model
{
  /// <summary>
  /// Filter model to get trip info
  /// </summary>
  public class TripInfo_FilterModel
  {
    /// <summary>
    /// Vehicle number
    /// </summary>
    public string VehicleNo { get; set; }
    /// <summary>
    /// From date
    /// </summary>
    public DateTime? FromDate { get; set; }
    /// <summary>
    /// To date
    /// </summary>
    public DateTime? ToDate { get; set; }
  }
}
