using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using System.Configuration;

namespace ConsoleApp1
{
    public class ClassSearchCriteria
    {
        //public SearchCriteria GetSearchCriteria()
        //{
        //    SearchCriteria criterias = new SearchCriteria();
        //    try
        //    {
        //        //Querying with LINQ to Entities 
        //        using (var context = new GetValuesEntities())
        //        {
        //            var query = context.SearchCriterias
        //                               .Where(s => s.Processed == false)
        //                               .FirstOrDefault<SearchCriteria>();
        //            return criterias = query;
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}

        public List<GetReportDailyJob_Result> GetValues(DateTime? fromDate, DateTime? toDate, string workgroupID)
        {
            Guid? workGroupID = null;
            if (workgroupID != null)
                workGroupID = Guid.Parse(workgroupID);

            List<GetReportDailyJob_Result> studentInfo = new List<GetReportDailyJob_Result>();
            try
            {
                //Querying with LINQ to Entities 
                using (var context = new GetValuesEntities())
                {
                    var query = context.GetReportDailyJob(fromDate, toDate, workGroupID).ToList();
                    //                   .Where(s => s.starttime >= fromDate && s.endtime <= toDate)
                    //                   .ToList();
                    return studentInfo = query;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public void CreateCSV(List<ReportInfo> list)
        {
            var location = ConfigurationManager.AppSettings["FTPLocation"];
            string startDate = "", endDate = "";
            StringBuilder sb = new StringBuilder();
            //{Environment.NewLine}";
            string headerText = $"\"mediaid\",\"dnis\",\"ani\",\"updateuserid\",\"percentscore\",\"overallscore\",\"reviewdate\",\"username\",\"userroleid\",\"usertypeid\"," +
                $"\"workgroupname\",\"description\",\"name\",\"sequencenumber\",\"weight\",\"questiondescription\",\"questionnumber\",\"questiontext\",\"responserequired\"," +
                $"\"questionadditionalpoint\",\"autofailpoint\",\"questionadditionalconditionpoint\"";
            sb.AppendLine(headerText);
            foreach (ReportInfo student in list)
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
                sb.Append(FormatCSV(student.weight.ToString()) + ",");
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
            string filename = formatedStartDate + "_" + formatedEndDate + ".csv";
            string locationAndFile = location + filename;
            File.WriteAllText(locationAndFile, sb.ToString());
            //string locationAndFileNew = "";
            //if (!File.Exists(locationAndFile))
            //{
                
            //    File.WriteAllText(locationAndFileNew, headerText);
            //    File.WriteAllText(locationAndFileNew, locationAndFile);
            //}
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
