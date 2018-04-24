using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace connect.Models
{
    public class WorkGroupReportBO
    {
        public string workgroupid { get; set; }

        public string starttime { get; set; }
        public string endtime { get; set; }
        public string mediaid { get; set; }
        public string dnis { get; set; }
        public string ani { get; set; }
        public string updateuserid { get; set; }
        public string percentscore { get; set; }
        public string overallscore { get; set; }
        public string reviewdate { get; set; }
        public string username { get; set; }
        public string userroleid { get; set; }
        public string usertypeid { get; set; }
        public string workgroupname { get; set; }
        public string description { get; set; }
        public string name { get; set; }
        public string sequencenumber { get; set; }
        public string sweight { get; set; }
        public string questiondescription { get; set; }
        public string questionnumber { get; set; }
        public string questiontext { get; set; }
        public string qweight { get; set; }
        public string responserequired { get; set; }
        public string questionadditionalpoint { get; set; }
        public string autofailpoint { get; set; }
        public string questionadditionalconditionpoint { get; set; }
    }
}