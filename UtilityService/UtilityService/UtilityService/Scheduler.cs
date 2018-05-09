using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace UtilityService
{
    public partial class Scheduler : ServiceBase
    {
        private Timer timer1 = null;

        public Scheduler()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            timer1 = new Timer();
            this.timer1.Interval = 30000 * 2; //every 30 secs
            this.timer1.Elapsed += new System.Timers.ElapsedEventHandler(this.timer1_Tick);
            timer1.Enabled = true;
            Library.WriteErrorLog("Utility window service started");
        }

        private void timer1_Tick(object sender, ElapsedEventArgs e)
        {
            Library.WriteErrorLog("Utility Tick Entry");
            ClassSearchCriteria sc = null;
            try
            {
                sc = new ClassSearchCriteria();
                var fromDateFromConfig = ConfigurationManager.AppSettings["FromDate"];
                var toDateFromConfig = ConfigurationManager.AppSettings["ToDate"];
                DateTime fromDate = DateTime.Now;
                if (fromDateFromConfig.Trim().Length > 0)
                    fromDate = Convert.ToDateTime(fromDateFromConfig);

                DateTime toDate = DateTime.Now.AddDays(1);
                if (toDateFromConfig.Trim().Length > 0)
                    toDate = Convert.ToDateTime(toDateFromConfig);

                var WorkGroupID = ConfigurationManager.AppSettings["WorkGroupID"];
                var dateFrom = Convert.ToDateTime(fromDate);
                var dateTo = Convert.ToDateTime(toDate);

                string workgroupid = null;
                if (WorkGroupID.Trim() == "")
                    workgroupid = null;
                else workgroupid = WorkGroupID.Trim();

                Library.WriteErrorLog("Utility Tick -- Getting Values -- Started on : " + DateTime.Now.ToString());
                Library.WriteErrorLog("Utility Tick Getting Values Parameters : From Date : " + dateFrom + " To Date : " + dateTo.ToString() + " Work Group : " + workgroupid);
                var values = sc.GetValues(dateFrom, dateTo, workgroupid);
                List<GetReportDailyJob_Result> studentInfos = null;
                if (values != null)
                {
                    studentInfos = new List<GetReportDailyJob_Result>();
                    studentInfos = sc.GetValues(dateFrom, dateTo, workgroupid);
                    Library.WriteErrorLog("Utility Tick Getting Values Stopped, Total number of records is : " + studentInfos.Count);
                    List<ReportInfo> lstReportInfo = new List<ReportInfo>();
                    ReportInfo reportInfo = null;
                    Library.WriteErrorLog("Utility Tick Mapping Values Started");
                    foreach (var item in studentInfos)
                    {
                        reportInfo = new ReportInfo
                        {
                            mediaid = item.mediaid,
                            starttime = item.starttime,
                            endtime = item.endtime,
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
                            questiondescription = item.questiondescription,
                            questionnumber = item.questionnumber,
                            questiontext = item.questiontext,
                            responserequired = item.responserequired,
                            questionadditionalpoint = item.questionadditionalpoint,
                            autofailpoint = item.autofailpoint,
                            questionadditionalconditionpoint = item.questionadditionalconditionpoint,
                            weightedscore = item.weightedscore,
                            sectionWeight = item.sectionWeight,
                            responsetext = item.responsetext,
                            questionWeight = item.questionWeight,
                            questiontypedesc = item.questiontypedesc,
                            questionScored = item.questionScored
                        };
                        lstReportInfo.Add(reportInfo);
                    }
                    Library.WriteErrorLog("Utility Tick Mapping Values Finished");
                    Library.WriteErrorLog("Utility Tick Create CSV Started");
                    string locAndFileName = sc.CreateCSV(lstReportInfo);
                    sc.UpdateReportGeneratedDetails(locAndFileName);
                }
                else
                    Library.WriteErrorLog("Utility Tick : Fetching with the parameters From Date : " + dateFrom + " & Date To : " + dateTo + " returns null values");

                Library.WriteErrorLog("Utility Tick Create CSV Finished");
            }
            catch (Exception ex)
            {
                Library.WriteErrorLog("Error Inside Ticker " + ex.ToString());
                Library.WriteErrorLog("Utility Tick Error Occured " + ex.InnerException.ToString());
            }

            //Write code here to do some job depends on your requirement
            Library.WriteErrorLog("Utility ticked and some job has been done successfully");
            Library.WriteErrorLog("Utility Tick Exit");
        }

        protected override void OnStop()
        {
            timer1.Enabled = false;
            Library.WriteErrorLog("Utility window service stopped");
        }

    }
}
