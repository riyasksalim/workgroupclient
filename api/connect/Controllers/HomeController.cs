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
using System.IO;
using System.Reflection;

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
            var FileName="";
			List < WorkGroupReportBO > WorkGroupReportBOList = new System.Collections.Generic.List < WorkGroupReportBO > ();
			var dbManager = new DBManager("DBConnection");
			IDbConnection connection = null;
			var parameters = new List < IDbDataParameter > ();
			parameters.Add(dbManager.CreateParameter("@startdate", ReportModel.StartDate, DbType.DateTime));
			parameters.Add(dbManager.CreateParameter("@endDate", ReportModel.EndDate, DbType.DateTime));
			parameters.Add(dbManager.CreateParameter("@workgroupid", "81A0EA05-1A9D-477D-A268-4D08D7AE6055", DbType.String));
			var datareader = dbManager.GetDataReader("GetReport", CommandType.StoredProcedure, parameters.ToArray(), out connection);

			try

			{

				while (datareader.Read())
				{
					WorkGroupReportBOList.Add(new WorkGroupReportBO()
					{

						workgroupid = datareader["workgroupid"]?.ToString(),
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
						sweight = null,
						questiondescription = datareader["questiondescription"]?.ToString(),
						questionnumber = datareader["questionnumber"]?.ToString(),
						questiontext = datareader["questiontext"]?.ToString(),
						qweight = null,
						responserequired = datareader["responserequired"]?.ToString(),
						questionadditionalpoint = datareader["questionadditionalpoint"]?.ToString(),
						autofailpoint = datareader["autofailpoint"]?.ToString(),
						questionadditionalconditionpoint = datareader["questionadditionalconditionpoint"]?.ToString(),
					});

				}
                if(WorkGroupReportBOList.Count>0){
                    Guid FilenameGuid = Guid.NewGuid();
                 FileName="D:\\"+ReportModel.StartDate.ToString("MMMM dd, yyyy")+ ReportModel.EndDate.ToString("MMMM dd, yyyy") + FilenameGuid.ToString()+".csv";
				CreateCSVFromGenericList(WorkGroupReportBOList, FileName);
				ReportGeneratedBO ReportGeneratedBO = new ReportGeneratedBO()
				{
					CreatedBy = "Manually",
					CreatedOn = DateTime.Today,
					ReportGenerated =FileName
				};

				int lastID = 0;
				var parameters1 = new List < IDbDataParameter > ();
				parameters1.Add(dbManager.CreateParameter("@CreatedOn", ReportGeneratedBO.CreatedOn, DbType.DateTime));
				parameters1.Add(dbManager.CreateParameter("@CreatedBy", ReportGeneratedBO.CreatedBy, DbType.String));
				parameters1.Add(dbManager.CreateParameter("@ReportGenerated",FileName.ToString(), DbType.String));
				dbManager.Insert("InsertReport", CommandType.StoredProcedure, parameters1.ToArray());
                }
				
			}

			catch(Exception ex)
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

        [System.Web.Http.HttpGet]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [Route("GetCSVList")]
        public async Task<IHttpActionResult> GetCSVList()
        {
            List<ReportGeneratedBO> ReportGeneratedBOList = new System.Collections.Generic.List<ReportGeneratedBO>();
            var dbManager = new DBManager("DBConnection");
            IDbConnection connection = null;
            var dataReader = dbManager.GetDataReader("GetReportcsvList", CommandType.StoredProcedure, null, out connection);
            try
            {
                while (dataReader.Read())
                {
                    ReportGeneratedBOList.Add(new ReportGeneratedBO()
                    {

                        ReportGenerated = dataReader["ReportGenerated"].ToString(),
                        CreatedBy = dataReader["CreatedBy"].ToString(),
                        CreatedOn = (DateTime)dataReader["CreatedOn"]
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
            return Ok(ReportGeneratedBOList);
        }

        public static void CreateCSVFromGenericList<T>(List<T> list, string csvNameWithExt)
        {

            if (list == null || list.Count == 0) return;

            //get type from 0th member 

            Type t = list[0].GetType();

            string newLine = Environment.NewLine;

            using (var sw = new StreamWriter(csvNameWithExt))
            {

                //make a new instance of the class name we figured out to get its props 

                object o = Activator.CreateInstance(t);

                //gets all properties 

                PropertyInfo[] props = o.GetType().GetProperties();

                //foreach of the properties in class above, write out properties 

                //this is the header row 

                foreach (PropertyInfo pi in props)
                {

                    sw.Write(pi.Name.ToUpper() + ",");

                }

                sw.Write(newLine);

                //this acts as datarow 

                foreach (T item in list)
                {

                    //this acts as datacolumn 

                    foreach (PropertyInfo pi in props)
                    {

                        //this is the row+col intersection (the value) 

                        string whatToWrite =

                        Convert.ToString(item.GetType()

                        .GetProperty(pi.Name)

                        .GetValue(item, null))

                        .Replace(',', ' ') + ',';

                        sw.Write(whatToWrite);

                    }

                    sw.Write(newLine);

                }

            }

        }

    }

}