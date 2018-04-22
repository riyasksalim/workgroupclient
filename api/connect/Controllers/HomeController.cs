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
using DataAccessHandler;
using System.Data;

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
            var dbManager = new DBManager("DBConnection");
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

        [System.Web.Http.HttpGet]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [Route("GetAllWorkGroups")]
        public async Task<IHttpActionResult> GetAllWorkGroups()
        {
            List<WorkGroup> WorkGroupList = new System.Collections.Generic.List<WorkGroup>();
            var dbManager = new DBManager("DBConnection");
            IDbConnection connection = null;
            var dataReader = dbManager.GetDataReader("GetAllWorkGroups", CommandType.StoredProcedure, null, out connection);
            try
            {
                while (dataReader.Read())
                {
                    WorkGroupList.Add(new WorkGroup()
                    {
                        WorkGroupId = dataReader["workgroupid"].ToString(),
                        WorkGroupName = dataReader["workgroupname"].ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
            finally
            {
                dataReader.Close();
                dbManager.CloseConnection(connection);
            }
            return Ok(WorkGroupList);
        }
    }
}
