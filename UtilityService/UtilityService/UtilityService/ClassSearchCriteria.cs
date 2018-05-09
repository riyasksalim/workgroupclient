using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityService
{
    public class ClassSearchCriteria
    {

        public List<GetReportDailyJob_Result> GetValues(DateTime? fromDate, DateTime? toDate, string workgroupID)
        {
            Guid? workGroupID = null;
            if (workgroupID != null)
                workGroupID = Guid.Parse(workgroupID);

            List<GetReportDailyJob_Result> studentInfo = new List<GetReportDailyJob_Result>();
            try
            {
                //Querying with LINQ to Entities 
                using (var context = new qmEntities())
                {
                    var query = context.GetReportDailyJob(fromDate, toDate, workGroupID).ToList();
                    return studentInfo = query;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public int UpdateReportGeneratedDetails(string FileName)
        {
            int affectedRows = 0;
            try
            {
                var location = ConfigurationManager.AppSettings["FTPLocation"];
                string locationAndFile = location + FileName;
                //Querying with LINQ to Entities 
                using (var context = new qmEntities())
                {
                    affectedRows = context.InsertReport(FileName, DateTime.Now, "Windows Service", "Nigtly Job", locationAndFile, location);
                    return affectedRows;
                }
            }
            catch (Exception ex)
            {
                return affectedRows;
                throw ex;
            }
        }

        public string CreateCSV(List<ReportInfo> list)
        {
            var location = ConfigurationManager.AppSettings["FTPLocation"];
            string startDate = "", endDate = "";
            StringBuilder sb = new StringBuilder();
            //{Environment.NewLine}";
            string headerText = $"\"Media ID\",\"Start Date\",\"End Date\",\"DNIS\",\"ANI\",\"Update User ID\",\"Percent Score\",\"Overall Score\"" +
                    $",\"Review Date\",\"User Name\",\"User Role ID\",\"User Type ID\"," +
                    $"\"Work Group Name\",\"Description\",\"Name\",\"Sequence Number\",\"Question Description\",\"Question Number\",\"Question Text\"," +
                    $"\"Response Required\"," +
                    $"\"Question Additional Point\",\"Auto Fail Point\",\"Question Additional Condition Point\"," +
                    $"\"Weighted Score\"" +
                    $",\"Section Weight\",\"Response Text\",\"Question Weight\",\"Question Type Desc\",\"Question Scored\"";
            sb.AppendLine(headerText);
            foreach (ReportInfo student in list)
            {
                startDate = student.starttime.ToString();
                endDate = student.endtime.ToString();
                sb.Append(FormatCSV(student.mediaid.ToString()) + ",");
                sb.Append(FormatCSV(startDate) + ",");
                sb.Append(FormatCSV(endDate) + ",");
                sb.Append(FormatCSV(student.dnis) + ",");
                sb.Append(FormatCSV(student.ani) + ",");
                sb.Append(FormatCSV(student.updateuserid.ToString()) + ",");
                sb.Append(FormatCSV(student.percentscore.ToString()) + ",");
                sb.Append(FormatCSV(student.overallscore) + ",");
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
                sb.Append(FormatCSV(student.questionadditionalpoint.ToString()) + ",");
                sb.Append(FormatCSV(student.autofailpoint.ToString()) + ",");
                sb.Append(FormatCSV(student.questionadditionalconditionpoint.ToString()) + ",");
                sb.Append(FormatCSV(student.weightedscore.ToString()) + ",");
                sb.Append(FormatCSV(student.sectionWeight.ToString()) + ",");
                sb.Append(FormatCSV(student.responsetext) + ",");
                sb.Append(FormatCSV(student.questionWeight.ToString()) + ",");
                sb.Append(FormatCSV(student.questiontypedesc.ToString()) + ",");
                sb.Append(FormatCSV(student.questionScored.ToString()));
                sb.Remove(sb.Length - 1, 1);
                sb.AppendLine();
            }
            string formatedStartDate = startDate.Replace("/", "").Replace(" ", "").Replace(":", "");
            string formatedEndDate = endDate.Replace("/", "").Replace(" ", "").Replace(":", "");
            string filename = "Report_" + formatedStartDate + "_" + formatedEndDate + ".csv";
            string locationAndFile = location + filename;
            File.WriteAllText(locationAndFile, sb.ToString());
            return filename;
        }

        DataTable GetData()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("StudentID", typeof(int));
            dt.Columns.Add("StudentName", typeof(string));
            dt.Columns.Add("RollNumber", typeof(int));
            dt.Columns.Add("TotalMarks", typeof(int));
            dt.Rows.Add(1, "Jame's", 101, 900);
            dt.Rows.Add(2, "Steave, Smith", 105, 820);
            dt.Rows.Add(3, "Mark\"Waugh", 109, 850);
            dt.Rows.Add(4, "Steave,\"Waugh", 110, 950);
            dt.Rows.Add(5, "Smith", 111, 910);
            dt.Rows.Add(6, "Williams", 115, 864);
            return dt;
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

    }
}
