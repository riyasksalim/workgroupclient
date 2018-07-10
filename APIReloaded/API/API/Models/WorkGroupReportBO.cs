using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class WorkGroupReportBO
    {
        public string workgroupid { get; set; }
        public DateTime? starttime { get; set; }
        public DateTime? endtime { get; set; }
        public Guid? mediaid { get; set; }
        public string dnis { get; set; }
        public string ani { get; set; }
        public string updateuserid { get; set; }
        public short? percentscore { get; set; }
        public DateTime? reviewdate { get; set; }
        public string username { get; set; }
        public byte? userroleid { get; set; }
        public byte? usertypeid { get; set; }
        public string workgroupname { get; set; }
        public string description { get; set; }
        public string name { get; set; }
        public byte? sequencenumber { get; set; }
        public byte sweight { get; set; }
        public byte sweight1 { get; set; }
        public string questiondescription { get; set; }
        public byte? questionnumber { get; set; }
        public string questiontext { get; set; }
        public byte qweight { get; set; }
        public bool? responserequired { get; set; }
        public decimal? questionadditionalpoint { get; set; }
        public decimal? questionadditionalconditionpoint { get; set; }
        public double? weightedscore { get; set; }
        public byte? sectionWeight { get; set; }
        public string responsetext { get; set; }
        public byte? questionWeight { get; set; }
        public string questiontypedesc { get; set; }
        public bool? questionScored { get; set; }
        public string reviewTemplate { get; set; }
        public string ScorecardStatus { get; set; }
    }
}