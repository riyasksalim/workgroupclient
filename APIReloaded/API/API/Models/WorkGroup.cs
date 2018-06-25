using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class WorkGroup
    {
        public Guid WorkGroupId { get; set; }
        public string WorkGroupName { get; set; }
    }

    public class ReviewTemplate
    {
        public Guid TemplateId { get; set; }
        public string Templatedesc { get; set; }
    }
}