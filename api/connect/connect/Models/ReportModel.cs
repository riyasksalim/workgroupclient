using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace connect.Models
{
    public class ReportModel
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string param { get; set; }
    }
}