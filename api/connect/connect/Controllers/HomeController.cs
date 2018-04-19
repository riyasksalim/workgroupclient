using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Web.Http;

namespace connect.Controllers
{
    [RoutePrefix("Reports")]
    public class HomeController : ApiController
    {
        [System.Web.Http.HttpGet]
        [Route("GetCustomerServiceDetails")]
        public async Task<IHttpActionResult> GetCustomerServiceDetails(int filterId)
        {

            return Ok("Hai");
        }
    }
}
