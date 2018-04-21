using System;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Web;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Web.Http;
using connect.Models;
using System.Web.Http.Cors;

namespace connect.Controllers
{
    [RoutePrefix("Reports")]
    public class HomeController : ApiController
    {
        [System.Web.Http.HttpPost]
        [EnableCors(origins: "*", headers: "*", methods: "*")] 
        [Route("GetCustomerServiceDetails")]



        public async Task<IHttpActionResult> GetCustomerServiceDetails(ReportModel ReportModel)
        {
            List<ReportModel> ReportModelList = new List<ReportModel>();
            for (int i = 0; i < 100; i++)
            {
                ReportModelList.Add(new ReportModel()
                {
                    EndDate = ReportModel.EndDate,
                    param = i.ToString() + ReportModel.param,
                    StartDate = ReportModel.StartDate
                });
            }
            return Ok(ReportModelList);
        }
    }
}
