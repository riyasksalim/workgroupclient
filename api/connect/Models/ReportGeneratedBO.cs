using System;

using System.Collections.Generic;

using System.Linq;

using System.Web;



namespace connect.Models
{

    public class ReportGeneratedBO
    {

      
            public string ReportGeneratedFileName { get; set; }
	        public DateTime CreatedOn { get; set; }
	        public string CreatedBy { get; set; }
	        public string MethodofCreation { get; set; }
	        public string ReportGeneratedFullPath { get; set; }
            public string ReportLocation { get; set; }



    }

}