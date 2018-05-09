using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityService
{
    public class ReportInfo
    {

        public string workgroupid { get; set; }
        public DateTime? starttime { get; set; }
        public DateTime? endtime { get; set; }
        public Guid? mediaid { get; set; }
        public string dnis { get; set; }
        public string ani { get; set; }
        public Guid? updateuserid { get; set; }
        public Int16? percentscore { get; set; }
        public string overallscore { get; set; }
        public DateTime? reviewdate { get; set; }
        public string username { get; set; }
        public Int16 userroleid { get; set; }
        public Int16 usertypeid { get; set; }
        public string workgroupname { get; set; }
        public string description { get; set; }
        public string name { get; set; }
        public Int16 sequencenumber { get; set; }
        public Int16 weight { get; set; }
        public string questiondescription { get; set; }
        public Int16 questionnumber { get; set; }
        public string questiontext { get; set; }
        public bool responserequired { get; set; }
        public decimal? questionadditionalpoint { get; set; }
        public decimal? autofailpoint { get; set; }
        public decimal? questionadditionalconditionpoint { get; set; }
        public double? weightedscore { get; set; }
        public byte sectionWeight { get; set; }
        public string responsetext { get; set; }
        public byte questionWeight { get; set; }
        public string questiontypedesc { get; set; }
        public bool questionScored { get; set; }
    }
}
