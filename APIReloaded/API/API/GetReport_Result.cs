//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace API
{
    using System;
    
    public partial class GetReport_Result
    {
        public Nullable<System.DateTime> starttime { get; set; }
        public Nullable<System.DateTime> endtime { get; set; }
        public Nullable<System.Guid> mediaid { get; set; }
        public string dnis { get; set; }
        public string ani { get; set; }
        public string updateuserid { get; set; }
        public Nullable<short> percentscore { get; set; }
        public Nullable<System.DateTime> reviewdate { get; set; }
        public string username { get; set; }
        public Nullable<byte> userroleid { get; set; }
        public Nullable<byte> usertypeid { get; set; }
        public string workgroupname { get; set; }
        public string description { get; set; }
        public string name { get; set; }
        public Nullable<byte> sequencenumber { get; set; }
        public string questiondescription { get; set; }
        public Nullable<byte> questionnumber { get; set; }
        public string questiontext { get; set; }
        public Nullable<bool> responserequired { get; set; }
        public Nullable<decimal> questionadditionalpoint { get; set; }
        public Nullable<decimal> questionadditionalconditionpoint { get; set; }
        public Nullable<double> weightedscore { get; set; }
        public Nullable<byte> sectionWeight { get; set; }
        public string responsetext { get; set; }
        public Nullable<byte> questionWeight { get; set; }
        public string questiontypedesc { get; set; }
        public Nullable<bool> questionScored { get; set; }
        public string reviewTemplate { get; set; }
        public System.Guid workgroupid { get; set; }
        public string ScorecardStatus { get; set; }
    }
}
