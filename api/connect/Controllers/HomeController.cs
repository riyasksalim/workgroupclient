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
using System.Net.Http;
using System.Net;
using System.Security.AccessControl;
using System.Security.Principal;

namespace connect.Controllers
{

    [RoutePrefix("Reports")]

    public class HomeController : ApiController
    {
        public string filename { get; set; }
         public string locationAndFile { get; set; }
         public string location { get; set; }


        string bookPath_Pdf = @"C:\MyWorkSpace\SelfDev\UserAPI\UserAPI\Books\sample.pdf";
        string bookPath_xls = @"C:\MyWorkSpace\SelfDev\UserAPI\UserAPI\Books\sample.xls";
        string bookPath_doc = @"C:\MyWorkSpace\SelfDev\UserAPI\UserAPI\Books\sample.doc";
        string bookPath_zip = @"C:\MyWorkSpace\SelfDev\UserAPI\UserAPI\Books\sample.zip";


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

                   //string location = ConfigurationManager.AppSettings["FTPLocation"];

                   // //  string formatedStartDate = ReportModel.StartDate.ToString().Replace("/", "").Replace(" ", "").Replace(":", "");
                   // //  string formatedEndDate = ReportModel.EndDate.ToString().Replace("/", "").Replace(" ", "").Replace(":", "");
                   // //  filename = formatedStartDate + "_" + formatedEndDate + ".csv";

                   
                   // var status = Check(location);
                   // if (status == "notExist")
                   // {
                   //     GrantAccess(location);
                   // }
                   // else if (status == "inaccessible")
                   // {
                   //     GrantAccess(location);
                   // }
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

            string headerText = $"\"mediaid\",\"dnis\",\"ani\",\"updateuserid\",\"percentscore\",\"overallscore\",\"reviewdate\",\"username\",\"userroleid\",\"usertypeid\"," +
                $"\"workgroupname\",\"description\",\"name\",\"sequencenumber\",\"weight\",\"questiondescription\",\"questionnumber\",\"questiontext\",\"responserequired\"," +
                $"\"questionadditionalpoint\",\"autofailpoint\",\"questionadditionalconditionpoint\"";
            sb.AppendLine(headerText);
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
           
            locationAndFile = location+"/" + filename;
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

        [System.Web.Http.HttpGet]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [Route("GetFile")]
        public IHttpActionResult GetFile(string Filename)
        {
            string location = ConfigurationManager.AppSettings["FTPLocation"];
            var a = Filename.Split('\\');
            var b = a[1].Split('/');
            string newstring = @""+a[0] + "/" + b[0] + "/" + b[1].ToString();
            var dataBytes = File.ReadAllBytes(newstring);
            var dataStream = new MemoryStream(dataBytes);
            return new eBookResult(dataStream, Request, Filename);
        }
        private static void GrantAccess(string file)
        {
            bool exists = System.IO.Directory.Exists(file);
            if (!exists)
            {
                DirectoryInfo di = System.IO.Directory.CreateDirectory(file);
          
                //DirectoryInfo dInfo = new DirectoryInfo(file);
                //DirectorySecurity dSecurity = dInfo.GetAccessControl();
                //dSecurity.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), FileSystemRights.FullControl, InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit, PropagationFlags.NoPropagateInherit, AccessControlType.Allow));
                //dInfo.SetAccessControl(dSecurity);
            }
            else
            {
                Console.WriteLine("The Folder already exists");
            }
          

        }
        public string Check(string name)
        {
            DirectoryInfo di = new DirectoryInfo(name);
            var state = "";
            if (!di.Exists)
            {
                try
                {
                    if ((int)di.Attributes == -1)
                    {
                        state = "notExist";
                    }
                }
                catch (UnauthorizedAccessException)
                {
                    state = "inaccessible";
                }
            }
            return state;
        }
        [HttpGet]
        [Route("Ebook/GetBookForHRM/{format}")]
        public HttpResponseMessage GetBookForHRM(string format)
        {
            string reqBook = format.ToLower() == "pdf" ? bookPath_Pdf : (format.ToLower() == "xls" ? bookPath_xls : (format.ToLower() == "doc" ? bookPath_doc : bookPath_zip));
            string bookName = "sample." + format.ToLower();
            //converting Pdf file into bytes array  
            var dataBytes = File.ReadAllBytes(reqBook);
            //adding bytes to memory stream   
            var dataStream = new MemoryStream(dataBytes);

            HttpResponseMessage httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK);
            httpResponseMessage.Content = new StreamContent(dataStream);
            httpResponseMessage.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            httpResponseMessage.Content.Headers.ContentDisposition.FileName = bookName;
            httpResponseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

            return httpResponseMessage;
        }

    }
    public class eBookResult : IHttpActionResult
    {
        MemoryStream bookStuff;
        string PdfFileName;
        HttpRequestMessage httpRequestMessage;
        HttpResponseMessage httpResponseMessage;
        public eBookResult(MemoryStream data, HttpRequestMessage request, string filename)
        {
            bookStuff = data;
            httpRequestMessage = request;
            PdfFileName = filename;
        }
        public System.Threading.Tasks.Task<HttpResponseMessage> ExecuteAsync(System.Threading.CancellationToken cancellationToken)
        {
            httpResponseMessage = httpRequestMessage.CreateResponse(HttpStatusCode.OK);
            httpResponseMessage.Content = new StreamContent(bookStuff);
            //httpResponseMessage.Content = new ByteArrayContent(bookStuff.ToArray());  
            httpResponseMessage.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            httpResponseMessage.Content.Headers.ContentDisposition.FileName = PdfFileName;
            httpResponseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

            return System.Threading.Tasks.Task.FromResult(httpResponseMessage);
        }
    }
}