using System.Linq;
using System;
using System.Collections.Generic;
using System.Web;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Web.Http;
using connect.Models;
using System.Web.Http.Cors;
//using DataAccessHandler;
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
        public IHttpActionResult GetCustomerServiceDetails(ReportModel reportModel)
        {
            WorkGroupReportBO student = null;
            List<WorkGroupReportBO> WorkGroupReportBOList = new List<WorkGroupReportBO>();
            try
            {
                //Querying with LINQ to Entities 
                using (var context = new QMEntities())
                {
                    var query = context.GetReport(reportModel.StartDate, reportModel.EndDate, reportModel.WorkGroupID).ToList();
                    foreach (var item in query)
                    {
                        student = new WorkGroupReportBO
                        {
                            starttime = item.starttime,
                            endtime = item.endtime,
                            mediaid = item.mediaid,
                            dnis = item.dnis,
                            ani = item.ani,
                            updateuserid = item.updateuserid,
                            percentscore = item.percentscore,
                            overallscore = item.overallscore,
                            reviewdate = item.reviewdate,
                            username = item.username,
                            userroleid = item.userroleid,
                            usertypeid = item.usertypeid,
                            workgroupname = item.workgroupname,
                            description = item.description,
                            name = item.name,
                            sequencenumber = item.sequencenumber,
                            sweight = item.sweight,
                            questiondescription = item.questiondescription,
                            questionnumber = item.questionnumber,
                            questiontext = item.questiontext,
                            qweight = item.qweight,
                            sweight1 = item.sweight,
                            responserequired = item.responserequired,
                            questionadditionalpoint = item.questionadditionalpoint,
                            autofailpoint = item.autofailpoint,
                            questionadditionalconditionpoint = item.questionadditionalconditionpoint
                        };
                        WorkGroupReportBOList.Add(student);
                    }
                    if (WorkGroupReportBOList.Count > 0)
                    {
                        CreateCSV(WorkGroupReportBOList);

                        ReportGeneratedBO rgBO = new ReportGeneratedBO()
                        {
                            CreatedBy = "User",
                            //CreatedOn = DateTime.Today,
                            MethodofCreation = "Manually",
                            ReportGeneratedFileName = filename,
                            ReportGeneratedFullPath = locationAndFile,
                            ReportLocation = location
                        };

                        var insertQuery = context.InsertReport(rgBO.ReportGeneratedFileName, DateTime.Now, rgBO.CreatedBy, rgBO.MethodofCreation, rgBO.ReportGeneratedFullPath, rgBO.ReportLocation);
                        return Ok(WorkGroupReportBOList);
                    }
                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
                throw ex;
            }
            return Ok(WorkGroupReportBOList);
        }

        [System.Web.Http.HttpGet]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [Route("GetAllWorkGroups")]
        public IHttpActionResult GetAllWorkGroups()
       {
            WorkGroup student = null;
            List<WorkGroup> studentInfo = new List<WorkGroup>();
            try
            {
                //Querying with LINQ to Entities 
                using (var context = new QMEntities())
                {
                    var query = context.GetAllWorkGroups().ToList();
                    foreach (var item in query)
                    {
                        student = new WorkGroup();
                        student.WorkGroupId = item.workgroupid;
                        student.WorkGroupName = item.workgroupname;
                        studentInfo.Add(student);
                    }

                    return Ok(studentInfo);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [System.Web.Http.HttpGet]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [Route("GetCSVList")]
        public IHttpActionResult GetCSVList()
        {
            ReportGeneratedBO student = null;
            List<ReportGeneratedBO> ReportGeneratedBOList = new List<ReportGeneratedBO>();
            try
            {
                //Querying with LINQ to Entities 
                using (var context = new QMEntities())
                {
                    var query = context.GetReportcsvList();
                    foreach (var item in query)
                    {
                        student = new ReportGeneratedBO
                        {
                            CreatedBy = item.CreatedBy,
                            MethodofCreation = item.MethodofCreation,
                            ReportGeneratedFileName = item.ReportGeneratedFileName,
                            ReportGeneratedFullPath = item.ReportGeneratedFullPath,
                            ReportLocation = item.ReportLocation
                        };
                        ReportGeneratedBOList.Add(student);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
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
                sb.Append(FormatCSV(student.mediaid.ToString()) + ",");
                sb.Append(FormatCSV(student.dnis) + ",");
                sb.Append(FormatCSV(student.ani) + ",");
                sb.Append(FormatCSV(student.updateuserid.ToString()) + ",");
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
            string formatedStartDate = startDate.Replace("/", "").Replace(" ", "").Replace(":", "");
            string formatedEndDate = endDate.Replace("/", "").Replace(" ", "").Replace(":", "");
            filename = formatedStartDate + "_" + formatedEndDate + ".csv";

            locationAndFile = location + "/" + filename;
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
            string newstring = @"" + a[0] + "/" + b[0] + "/" + b[1].ToString();
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