using System;
using System.Collections.Generic;

namespace JT_Transport.Model
{
  /// <summary>
  /// Renewal details of vehicle
  /// </summary>
  public class RenewalDetails
  {
    /// <summary>
    /// Vehicle no
    /// </summary>
    public string VehicleNo { get; set; }
    /// <summary>
    /// Date
    /// </summary>
    public DateTime? Date { get; set; }
  }


  /// <summary>
  /// Renewal details of vehicle
  /// </summary>
  public class RenewalList
  {
    /// <summary>
    /// Insurance renewal list
    /// </summary>
    public List<RenewalDetails> InsuranceRenewalList { get; set; }
    /// <summary>
    /// FC renewal list
    /// </summary>
    public List<RenewalDetails> FCRenewalList { get; set; }
    /// <summary>
    /// NP Tax renewal list
    /// </summary>
    public List<RenewalDetails> NPTaxRenewalList { get; set; }
    /// <summary>
    /// Permit renewal list
    /// </summary>
    public List<RenewalDetails> PermitRenewlList { get; set; }
  }
}
