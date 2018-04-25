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
            List<WorkGroupReportBO> WorkGroupReportBOList = new System.Collections.Generic.List<WorkGroupReportBO>();
            var dbManager = new DBManager("DBConnection");
            IDbConnection connection = null;
            var parameters = new List<IDbDataParameter>();
            parameters.Add(dbManager.CreateParameter("@startdate", 50, ReportModel.StartDate, DbType.DateTime));
            parameters.Add(dbManager.CreateParameter("@endDate", ReportModel.EndDate, DbType.DateTime));
            parameters.Add(dbManager.CreateParameter("@workgroupid", ReportModel.WorkGroupID, DbType.String));


            var datareader = dbManager.GetDataReader("GetReport", CommandType.StoredProcedure, parameters.ToArray(), out connection);
            try
            {

                while (datareader.Read())
                {
                    WorkGroupReportBOList.Add(new WorkGroupReportBO()
                    {
                           workgroupid= datareader["workgroupid"]?.ToString(),
                           starttime = datareader["starttime"]?.ToString(),
                           endtime = datareader["endtime"]?.ToString(),
                           mediaid = datareader["mediaid"]?.ToString(),
                           dnis = datareader["dnis"]?.ToString(),
                           ani = datareader["ani"]?.ToString(),
                           updateuserid = datareader["updateuserid"]?.ToString(),
                           percentscore = datareader["percentscore"]?.ToString(),
                           overallscore = datareader["overallscore"]?.ToString(),
                           reviewdate = datareader["reviewdate"]?.ToString(),
                           username = datareader["username"]?.ToString(),
                           userroleid = datareader["userroleid"]?.ToString(),
                           usertypeid = datareader["usertypeid"]?.ToString(),
                           workgroupname = datareader["workgroupname"]?.ToString(),
                           description = datareader["description"]?.ToString(),
                           name = datareader["name"]?.ToString(),
                           sequencenumber = datareader["sequencenumber"]?.ToString(),
                           sweight =datareader["sweight"]?.ToString(),
                           questiondescription = datareader["questiondescription"]?.ToString(),
                           questionnumber = datareader["questionnumber"]?.ToString(),
                           questiontext = datareader["questiontext"]?.ToString(),
                           qweight = datareader["qweight"]?.ToString(),
                           responserequired = datareader["responserequired"]?.ToString(),
                           questionadditionalpoint = datareader["questionadditionalpoint"]?.ToString(),
                           autofailpoint = datareader["autofailpoint"]?.ToString(),
                           questionadditionalconditionpoint = datareader["questionadditionalconditionpoint"]?.ToString(),
                    });      
                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
            finally
            {
                datareader.Close();
                dbManager.CloseConnection(connection);
            }
            return Ok(WorkGroupReportBOList);
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
