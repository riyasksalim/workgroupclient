﻿using System.Linq;
using System;
using System.Collections.Generic;
using System.Web;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Web.Http;
using API.Models;
using System.Web.Http.Cors;
using System.Data;
using System.IO;    
using System.Reflection;
using System.Configuration;
using System.Text;
using System.Net.Http;
using System.Net;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Globalization;
using API.Helpers;

namespace API.Controllers
{
    [System.Web.Http.RoutePrefix("Reports")]
    public class HomeController : ApiController
    {
        public string filename { get; set; }
        public string locationAndFile { get; set; }
        public string location { get; set; }

        [System.Web.Http.HttpPost]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [System.Web.Http.Route("GetCustomerServiceDetails")]
        public IHttpActionResult GetCustomerServiceDetails(ReportModel reportModel)
        {
            WorkGroupReportBO student = null;
            List<WorkGroupReportBO> WorkGroupReportBOList = new List<WorkGroupReportBO>();
            try
            {
                using (var context = new qmEntities())
                {
                    context.Configuration.AutoDetectChangesEnabled = false;
                    context.SetCommandTimeOut(10000);
                    var query = context.GetReport(reportModel.StartDate, reportModel.EndDate, reportModel.WorkGroupID, reportModel.TemplateID).ToList();
                    foreach (var item in query)
                    {
                        student = new WorkGroupReportBO
                        {
                            mediaid = item.mediaid,
                            starttime = item.starttime,
                            endtime = item.endtime,
                            dnis = item.dnis,
                            ani = item.ani,
                            updateuserid = item.updateuserid,
                            percentscore = item.percentscore,
                            reviewdate = item.reviewdate,
                            username = item.username,
                            userroleid = item.userroleid,
                            usertypeid = item.usertypeid,
                            workgroupname = item.workgroupname,
                            description = item.description,
                            name = item.name,
                            sequencenumber = item.sequencenumber,
                            questiondescription = item.questiondescription,
                            questionnumber = item.questionnumber,
                            questiontext = item.questiontext,
                            responserequired = item.responserequired,
                            questionadditionalpoint = item.questionadditionalpoint,
                            questionadditionalconditionpoint = item.questionadditionalconditionpoint,
                            weightedscore = item.weightedscore,
                            sectionWeight = item.sectionWeight,
                            responsetext = item.responsetext,
                            questionWeight = item.questionWeight,
                            questiontypedesc = item.questiontypedesc,
                            questionScored = item.questionScored,
                            reviewTemplate = item.reviewTemplate,
                            ScorecardStatus = item.ScorecardStatus
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
                return Ok(ex.ToString());
                throw ex;
            }
            return Ok(WorkGroupReportBOList);
        }

        [System.Web.Http.HttpGet]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [System.Web.Http.Route("GetAllWorkGroups")]
        public IHttpActionResult GetAllWorkGroups()
        {
            WorkGroup student = null;
            List<WorkGroup> studentInfo = new List<WorkGroup>();
            try
            {
                //Querying with LINQ to Entities 
                using (var context = new qmEntities())
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
                return Ok(ex.InnerException.ToString());
                throw ex;
            }
        }

        [System.Web.Http.HttpGet]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [System.Web.Http.Route("GetAllTemplates")]
        public IHttpActionResult GetAllTemplates()
        {
            ReviewTemplate student = null;
            List<ReviewTemplate> studentInfo = new List<ReviewTemplate>();
            try
            {
                //Querying with LINQ to Entities 
                using (var context = new qmEntities())
                {
                    var query = context.GetAllTemplates().ToList();
                    foreach (var item in query)
                    {
                        student = new ReviewTemplate();
                        student.TemplateId = item.templateid;
                        student.Templatedesc = item.templatedesc;
                        studentInfo.Add(student);
                    }
                    return Ok(studentInfo);
                }
            }
            catch (Exception ex)
            {
                return Ok(ex.InnerException.ToString());
                throw ex;
            }
        }

        [System.Web.Http.HttpGet]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [System.Web.Http.Route("GetCSVList")]
        public IHttpActionResult GetCSVList()
        {
            ReportGeneratedBO student = null;
            List<ReportGeneratedBO> ReportGeneratedBOList = new List<ReportGeneratedBO>();
            try
            {
                //Querying with LINQ to Entities 
                using (var context = new qmEntities())
                {
                    var query = context.GetReportcsvList();
                    foreach (var item in query)
                    {
                        student = new ReportGeneratedBO
                        {
                            CreatedBy = item.CreatedBy,
                            CreatedOn = item.CreatedOn,
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
                return Ok(ex.InnerException.ToString());
                throw ex;
            }
            return Ok(ReportGeneratedBOList);
        }

        public void CreateCSV(List<WorkGroupReportBO> list)
        {
            try
            {
                location = ConfigurationManager.AppSettings["FTPLocation"];
                string startDate = "", endDate = "";
                string sd = "", se = "";
                StringBuilder sb = new StringBuilder();
                string replaceWith = " ";
                string responsetext = "";

                string headerText = $"\"Media ID\",\"Start Date\",\"End Date\",\"DNIS\",\"ANI\",\"Scorer\",\"Percent Score\"," +
                    $"\"Review Date\",\"User Name\",\"User Role ID\",\"User Type ID\"," +
                    $"\"Work Group Name\",\"Description\",\"Name\",\"Sequence Number\",\"Question Description\",\"Question Number\",\"Question Text\"," +
                    $"\"Response Required\"," +
                    $"\"Question Additional Point\",\"Question Additional Condition Point\"," +
                    $"\"Weighted Score\"" +
                    $",\"Section Weight\",\"Response Text\",\"Question Weight\",\"Question Type Desc\",\"Question Scored\",\"Scorecard Template\",\"Scorecard Status\"";
                sb.AppendLine(headerText);
                foreach (WorkGroupReportBO student in list)
                {
                    startDate = student.starttime.ToString();
                    endDate = student.endtime.ToString();
                    sb.Append(FormatCSV(student.mediaid.ToString()) + ",");
                    sb.Append(FormatCSV(student.starttime.ToString()) + ",");
                    sb.Append(FormatCSV(student.endtime.ToString()) + ",");
                    sb.Append(FormatCSV(student.dnis) + ",");
                    sb.Append(FormatCSV(student.ani) + ",");
                    sb.Append(FormatCSV(student.updateuserid) + ",");
                    sb.Append(FormatCSV(student.percentscore.ToString()) + ",");  //sb.Append(FormatCSV(student.overallscore) + ",");
                    sb.Append(FormatCSV(student.reviewdate.ToString()) + ",");
                    sb.Append(FormatCSV(student.username) + ",");
                    sb.Append(FormatCSV(student.userroleid.ToString()) + ",");
                    sb.Append(FormatCSV(student.usertypeid.ToString()) + ",");
                    sb.Append(FormatCSV(student.workgroupname) + ",");
                    sb.Append(FormatCSV(student.description) + ",");
                    sb.Append(FormatCSV(student.name) + ",");
                    sb.Append(FormatCSV(student.sequencenumber.ToString()) + ",");
                    sb.Append(FormatCSV(student.questiondescription) + ",");
                    sb.Append(FormatCSV(student.questionnumber.ToString()) + ",");
                    sb.Append(FormatCSV(student.questiontext) + ",");
                    sb.Append(FormatCSV(student.responserequired.ToString()) + ",");
                    sb.Append(FormatCSV(student.questionadditionalpoint.ToString()) + ","); //sb.Append(FormatCSV(student.autofailpoint.ToString()) + ",");
                    sb.Append(FormatCSV(student.questionadditionalconditionpoint.ToString()) + ",");
                    sb.Append(FormatCSV(student.weightedscore.ToString()) + ",");
                    sb.Append(FormatCSV(student.sectionWeight.ToString()) + ",");
                    if (student.responsetext.Length > 0)
                    {
                        responsetext = "";
                        responsetext = student.responsetext;
                        responsetext = responsetext.Replace("\\r\\n", replaceWith).Replace("\\n", replaceWith).Replace("\\r", replaceWith);
                    }
                    sb.Append(FormatCSV(responsetext) + ",");
                    sb.Append(FormatCSV(student.questionWeight.ToString()) + ",");
                    sb.Append(FormatCSV(student.questiontypedesc) + ",");
                    sb.Append(FormatCSV(student.questionScored.ToString()) + ",");
                    sb.Append(FormatCSV(student.reviewTemplate.ToString()) + ",");
                    sb.Append(FormatCSV(student.ScorecardStatus.ToString()));
                    sb.Append(" ");
                    sb.Remove(sb.Length - 1, 1);
                    sb.AppendLine();
                }
                string formatedStartDate = startDate.Replace("/", "").Replace(" ", "").Replace(":", "");
                string formatedEndDate = endDate.Replace("/", "").Replace(" ", "").Replace(":", "");
                filename = "Report_" + formatedStartDate + "_" + formatedEndDate + ".csv";

                locationAndFile = location + "/" + filename;
                File.WriteAllText(locationAndFile, sb.ToString());
            }
            catch (Exception ex)
            {
               // return Ok(ex.InnerException.ToString());
                throw ex;
            }
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
        [System.Web.Http.Route("GetFile")]
        public IHttpActionResult GetFile(string Filename)
        {
            string location = ConfigurationManager.AppSettings["FTPLocation"];
            var abc = Filename.Substring(location.Length + 1, Filename.Length - location.Length - 1); //file name
            var cc = location.Substring(0, 1); //directory
            char[] splitchar = { '\\' };
            var strArr = location.Split(splitchar);
            StringBuilder sb = new StringBuilder();
            foreach (var a in strArr)
            {
                sb.Append(a.ToString() + "/");
            }
            //string newstring = @"" + cc + "/" + location + "/" + abc.ToString();
            var dataBytes = File.ReadAllBytes(sb.ToString() + abc);
            var dataStream = new MemoryStream(dataBytes);
            return new eBookResult(dataStream, Request, Filename);
        }

        private static void GrantAccess(string file)
        {
            bool exists = System.IO.Directory.Exists(file);
            if (!exists)
            {
                DirectoryInfo di = System.IO.Directory.CreateDirectory(file);
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
                        state = "notExist";
                }
                catch (UnauthorizedAccessException)
                {
                    state = "inaccessible";
                }
            }
            return state;
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
