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
using System.Configuration;
using System.Text;

namespace connect.Controllers
{

    [RoutePrefix("Reports")]

    public class HomeController : ApiController
    {
        public string filename { get; set; }
         public string locationAndFile { get; set; }
         public string location { get; set; }
        
        
        [System.Web.Http.HttpPost]

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [Route("GetCustomerServiceDetails")]

        public async Task<IHttpActionResult> GetCustomerServiceDetails(ReportModel ReportModel)

		{
          
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

						workgroupid = datareader["workgroupid"].ToString(),
						starttime = datareader["starttime"].ToString(),
						endtime = datareader["endtime"].ToString(),
						mediaid = datareader["mediaid"].ToString(),
						dnis = datareader["dnis"].ToString(),
						ani = datareader["ani"].ToString(),
						updateuserid = datareader["updateuserid"].ToString(),
						percentscore = datareader["percentscore"].ToString(),
						overallscore = datareader["overallscore"].ToString(),
						reviewdate = datareader["reviewdate"].ToString(),
						username = datareader["username"].ToString(),
						userroleid = datareader["userroleid"].ToString(),
						usertypeid = datareader["usertypeid"].ToString(),
						workgroupname = datareader["workgroupname"].ToString(),
						description = datareader["description"].ToString(),
						name = datareader["name"].ToString(),
						sequencenumber = datareader["sequencenumber"].ToString(),
						sweight = null,
						questiondescription = datareader["questiondescription"].ToString(),
						questionnumber = datareader["questionnumber"].ToString(),
						questiontext = datareader["questiontext"].ToString(),
						qweight = null,
						responserequired = datareader["responserequired"].ToString(),
						questionadditionalpoint = datareader["questionadditionalpoint"].ToString(),
						autofailpoint = datareader["autofailpoint"].ToString(),
						questionadditionalconditionpoint = datareader["questionadditionalconditionpoint"].ToString(),
					});

				}
                if(WorkGroupReportBOList.Count>0){
                 
				   CreateCSV(WorkGroupReportBOList);


				ReportGeneratedBO ReportGeneratedBO = new ReportGeneratedBO()
				{
					CreatedBy = "User",
					CreatedOn = DateTime.Today,
                    MethodofCreation="Manually",
                    ReportGeneratedFileName=filename,
                    ReportGeneratedFullPath=locationAndFile,
                    ReportLocation=location
                  
				};

				
				var parameters1 = new List < IDbDataParameter > ();
                parameters1.Add(dbManager.CreateParameter("@CreatedBy", ReportGeneratedBO.CreatedBy, DbType.String));
                parameters1.Add(dbManager.CreateParameter("@CreatedOn", ReportGeneratedBO.CreatedOn, DbType.DateTime));
                parameters1.Add(dbManager.CreateParameter("@MethodofCreation", ReportGeneratedBO.MethodofCreation.ToString(), DbType.String));
                parameters1.Add(dbManager.CreateParameter("@ReportGeneratedFileName", ReportGeneratedBO.ReportGeneratedFileName, DbType.String));
                parameters1.Add(dbManager.CreateParameter("@ReportGeneratedFullPath", ReportGeneratedBO.ReportGeneratedFullPath, DbType.String));
                parameters1.Add(dbManager.CreateParameter("@ReportLocation", ReportGeneratedBO.ReportLocation.ToString(), DbType.String));
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
                        CreatedBy = dataReader["CreatedBy"].ToString(),
                        CreatedOn = (DateTime)dataReader["CreatedOn"],
                        MethodofCreation = dataReader["MethodofCreation"].ToString(),
                        ReportGeneratedFileName = dataReader["ReportGeneratedFileName"].ToString(),
                        ReportGeneratedFullPath = dataReader["ReportGeneratedFullPath"].ToString(),
                        ReportLocation = dataReader["ReportLocation"].ToString()
                      
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


           public void CreateCSV(List<WorkGroupReportBO> list)
        {
            location = ConfigurationManager.AppSettings["FTPLocation"];
            string startDate = "", endDate = "";
            StringBuilder sb = new StringBuilder();
           
            //string headerText = $"\"mediaid\",\"dnis\",\"ani\",\"updateuserid\",\"percentscore\",\"overallscore\",\"reviewdate\",\"username\",\"userroleid\",\"usertypeid\"," +
            //    $"\"workgroupname\",\"description\",\"name\",\"sequencenumber\",\"weight\",\"questiondescription\",\"questionnumber\",\"questiontext\",\"responserequired\"," +
            //    $"\"questionadditionalpoint\",\"autofailpoint\",\"questionadditionalconditionpoint\"";
            //sb.AppendLine(headerText);
            foreach (WorkGroupReportBO student in list)
            {
                startDate = student.starttime.ToString();
                endDate = student.endtime.ToString();
                sb.Append(FormatCSV(student.mediaid) + ",");
                sb.Append(FormatCSV(student.dnis) + ",");
                sb.Append(FormatCSV(student.ani) + ",");
                sb.Append(FormatCSV(student.updateuserid) + ",");
                sb.Append(FormatCSV(student.percentscore.ToString()) + ",");
                sb.Append(FormatCSV(student.overallscore) + ",");
                sb.Append(FormatCSV(student.reviewdate.ToString()) + ",");
                sb.Append(FormatCSV(student.username) + ",");
                sb.Append(FormatCSV(student.userroleid.ToString()) + ",");
                sb.Append(FormatCSV(student.name) + ",");
                sb.Append(FormatCSV(student.sequencenumber.ToString()) + ",");
                //sb.Append(FormatCSV(student.weight.ToString()) + ",");
                sb.Append(FormatCSV(student.questiondescription) + ",");
                sb.Append(FormatCSV(student.questionnumber.ToString()) + ",");
                sb.Append(FormatCSV(student.questiontext) + ",");
                sb.Append(FormatCSV(student.responserequired.ToString()) + ",");
                sb.Append(FormatCSV(student.questionadditionalpoint.ToString()) + ",");
                sb.Append(FormatCSV(student.autofailpoint.ToString()) + ",");
                sb.Append(FormatCSV(student.questionadditionalconditionpoint.ToString()) + ",");
                sb.Remove(sb.Length - 1, 1);
                sb.AppendLine();
            }
            string formatedStartDate = startDate.Replace("/", "").Replace(" ","").Replace(":","");
            string formatedEndDate = endDate.Replace("/", "").Replace(" ", "").Replace(":", "");
             filename = formatedStartDate + "_" + formatedEndDate + ".csv";
             locationAndFile = location + filename;
            File.WriteAllText(locationAndFile, sb.ToString());
          
        }
        public static string FormatCSV(string input)
        {
            try
            {
                if (input == null)
                    return string.Empty;

                bool containsQuote = false;
                bool containsComma = false;
                int len = input.Length;
                for (int i = 0; i < len && (containsComma == false || containsQuote == false); i++)
                {
                    char ch = input[i];
                    if (ch == '"')
                        containsQuote = true;
                    else if (ch == ',')
                        containsComma = true;
                }

                if (containsQuote && containsComma)
                    input = input.Replace("\"", "\"\"");

                if (containsComma)
                    return "\"" + input + "\"";
                else
                    return input;
            }
            catch
            {
                throw;
            }
        }
        //public static void CreateCSVFromGenericList<T>(List<T> list)
        //{

        //    if (list == null || list.Count == 0) return;

        //    //get type from 0th member 

        //    Type t = list[0].GetType();

        //    string newLine = Environment.NewLine;

        //    using (var sw = new StreamWriter(csvNameWithExt))
        //    {

        //        //make a new instance of the class name we figured out to get its props 

        //        object o = Activator.CreateInstance(t);

        //        //gets all properties 

        //        PropertyInfo[] props = o.GetType().GetProperties();

        //        //foreach of the properties in class above, write out properties 

        //        //this is the header row 

        //        foreach (PropertyInfo pi in props)
        //        {

        //            sw.Write(pi.Name.ToUpper() + ",");

        //        }

        //        sw.Write(newLine);

        //        //this acts as datarow 

        //        foreach (T item in list)
        //        {

        //            //this acts as datacolumn 

        //            foreach (PropertyInfo pi in props)
        //            {

        //                //this is the row+col intersection (the value) 

        //                string whatToWrite =

        //                Convert.ToString(item.GetType()

        //                .GetProperty(pi.Name)

        //                .GetValue(item, null))

        //                .Replace(',', ' ') + ',';

        //                sw.Write(whatToWrite);

        //            }

        //            sw.Write(newLine);

        //        }

        //    }

        //}

    }

}