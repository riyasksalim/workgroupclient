using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            ClassSearchCriteria sc = new ClassSearchCriteria();
            //var searchCriteria = sc.GetSearchCriteria();
            var dateFrom = Convert.ToDateTime("2013-06-06 13:47:45.000");
            var dateTo = Convert.ToDateTime("2013-06-06 13:55:24.000");
            string workgroupid = null;
            List<GetReportDailyJob_Result> studentInfos = sc.GetValues(dateFrom, dateTo, workgroupid);
            List<ReportInfo> lstReportInfo = new List<ReportInfo>();
            ReportInfo reportInfo = null;
            foreach (var s in studentInfos)
            {
                reportInfo = new ReportInfo
                {
                    starttime = s.starttime,
                    endtime = s.endtime,
                    workgroupid = s.workgroupid.ToString(),
                    mediaid = s.mediaid.ToString(),
                    dnis = s.dnis,
                    ani = s.ani,
                    updateuserid = s.updateuserid.ToString(),
                    percentscore = s.percentscore,
                    overallscore = s.overallscore,
                    reviewdate = s.reviewdate,
                    username = s.username,
                    userroleid = s.userroleid,
                    usertypeid = s.usertypeid,
                    workgroupname = s.workgroupname,
                    description = s.description,
                    name = s.name,
                    sequencenumber = s.sequencenumber,
                    weight = s.weight,
                    questiondescription = s.questiondescription,
                    questionnumber = s.questionnumber,
                    questiontext = s.questiontext,
                    responserequired = s.responserequired,
                    questionadditionalpoint = s.questionadditionalpoint,
                    autofailpoint = s.autofailpoint,
                    questionadditionalconditionpoint = s.questionadditionalconditionpoint
                };
                lstReportInfo.Add(reportInfo);
            }
            sc.CreateCSV(lstReportInfo);
            //UpdateSearchCriteria();
        }
    }
}
