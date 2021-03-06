﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace UtilityService
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class qmEntities : DbContext
    {
        public qmEntities()
            : base("name=qmEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
    
        public virtual int InsertReport(string reportGeneratedFileName, Nullable<System.DateTime> createdOn, string createdBy, string methodofCreation, string reportGeneratedFullPath, string reportLocation)
        {
            var reportGeneratedFileNameParameter = reportGeneratedFileName != null ?
                new ObjectParameter("ReportGeneratedFileName", reportGeneratedFileName) :
                new ObjectParameter("ReportGeneratedFileName", typeof(string));
    
            var createdOnParameter = createdOn.HasValue ?
                new ObjectParameter("CreatedOn", createdOn) :
                new ObjectParameter("CreatedOn", typeof(System.DateTime));
    
            var createdByParameter = createdBy != null ?
                new ObjectParameter("CreatedBy", createdBy) :
                new ObjectParameter("CreatedBy", typeof(string));
    
            var methodofCreationParameter = methodofCreation != null ?
                new ObjectParameter("MethodofCreation", methodofCreation) :
                new ObjectParameter("MethodofCreation", typeof(string));
    
            var reportGeneratedFullPathParameter = reportGeneratedFullPath != null ?
                new ObjectParameter("ReportGeneratedFullPath", reportGeneratedFullPath) :
                new ObjectParameter("ReportGeneratedFullPath", typeof(string));
    
            var reportLocationParameter = reportLocation != null ?
                new ObjectParameter("ReportLocation", reportLocation) :
                new ObjectParameter("ReportLocation", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("InsertReport", reportGeneratedFileNameParameter, createdOnParameter, createdByParameter, methodofCreationParameter, reportGeneratedFullPathParameter, reportLocationParameter);
        }
    
        public virtual ObjectResult<GetReportDailyJob_Result> GetReportDailyJob(Nullable<System.DateTime> startdate, Nullable<System.DateTime> endDate, string workgroupid)
        {
            var startdateParameter = startdate.HasValue ?
                new ObjectParameter("startdate", startdate) :
                new ObjectParameter("startdate", typeof(System.DateTime));
    
            var endDateParameter = endDate.HasValue ?
                new ObjectParameter("endDate", endDate) :
                new ObjectParameter("endDate", typeof(System.DateTime));
    
            var workgroupidParameter = workgroupid != null ?
                new ObjectParameter("workgroupid", workgroupid) :
                new ObjectParameter("workgroupid", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetReportDailyJob_Result>("GetReportDailyJob", startdateParameter, endDateParameter, workgroupidParameter);
        }
    }
}
