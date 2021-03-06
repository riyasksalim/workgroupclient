USE [qm]
GO
/****** Object:  StoredProcedure [dbo].[GetReportDailyJob]    Script Date: 21/07/2018 15:25:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Mujeeb K S
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[GetReportDailyJob] 
	@startdate datetime,
	@endDate datetime,
	@workgroupid varchar(max) = null
AS
BEGIN

declare @formatedStartDate datetime
set  @formatedStartDate = Cast(CONVERT(CHAR(10), @startdate, 101) as datetime)

declare @formatedEndDate datetime
set  @formatedEndDate = Cast(CONVERT(CHAR(10), @endDate, 101) as datetime)

if (@workgroupid is null)

begin
    ;with CTEGetReport (starttime, endtime, mediaid, dnis, ani, updateuserid, percentscore, reviewdate, username,
	userroleid, usertypeid, workgroupname, [description], [name], sequencenumber, questiondescription, questionnumber,
	questiontext, responserequired, questionadditionalpoint, questionadditionalconditionpoint, weightedscore,
	sectionWeight, responsetext, questionWeight, questiontypedesc, questionScored, reviewTemplate, ScorecardStatus)
	as     
	(SELECT DISTINCT 
                  m.starttime, m.endtime, m.mediaid, m.dnis, m.ani, i.firstname + ' ' + i.lastname AS updateuserid, 
				  r.percentscore, r.reviewdate, i.username, i.userroleid, i.usertypeid, w.workgroupname, s.description, 
				  s.name, s.sequencenumber, q.questiondescription, q.questionnumber, q.questiontext, q.responserequired, 
				  q.questionadditionalpoint, q.questionadditionalconditionpoint, sr.weightedscore, s.weight AS sectionWeight, 
				  qr.responsetext, q.weight AS questionWeight, qt.questiontypedesc, qt.scored AS questionScored, 
				  rt.templatedesc AS reviewTemplate, rs.description as ScorecardStatus
	FROM     reviewtemplate AS rt INNER JOIN
					  review AS r ON r.templateid = rt.templateid INNER JOIN
					  reviewstatus rs ON rs.reviewstatusid = r.reviewstatusid INNER JOIN
					  media AS m ON r.mediaid = m.mediaid INNER JOIN
					  iqmuser AS i ON i.userid = m.userid INNER JOIN
					  workgroup_iqmuser AS wi ON wi.userid = i.userid INNER JOIN
					  workgroup AS w ON w.workgroupid = wi.workgroupid INNER JOIN
					  sectionresult AS sr ON sr.reviewid = r.reviewid INNER JOIN
					  section AS s ON s.sectionid = sr.sectionid INNER JOIN
					  question AS q ON q.sectionid = s.sectionid INNER JOIN
					  questionresult AS qr ON q.questionid = qr.questionid INNER JOIN
					  questiontype AS qt ON qt.questiontypeid = q.questiontypeid 
	where  
	(CAST(CONVERT(CHAR(10), m.starttime, 101) AS datetime) BETWEEN @formatedStartDate AND @formatedEndDate) AND 
    (CAST(CONVERT(CHAR(10), m.endtime, 101) AS datetime) BETWEEN @formatedStartDate AND @formatedEndDate))

	select starttime, endtime, mediaid, dnis, ani, updateuserid, percentscore, reviewdate, username,
			userroleid, usertypeid, workgroupname, [description], [name], sequencenumber, questiondescription, questionnumber,
			questiontext, responserequired, questionadditionalpoint, questionadditionalconditionpoint, weightedscore,
			sectionWeight, responsetext, questionWeight, questiontypedesc, questionScored, reviewTemplate, ScorecardStatus 
			FROM CTEGetReport

end
else 
begin

	;with CTEGetReport (starttime, endtime, mediaid, dnis, ani, updateuserid, percentscore, reviewdate, username,
	userroleid, usertypeid, workgroupname, [description], [name], sequencenumber, questiondescription, questionnumber,
	questiontext, responserequired, questionadditionalpoint, questionadditionalconditionpoint, weightedscore,
	sectionWeight, responsetext, questionWeight, questiontypedesc, questionScored, reviewTemplate, ScorecardStatus)
	as     
	(SELECT DISTINCT 
                  m.starttime, m.endtime, m.mediaid, m.dnis, m.ani, i.firstname + ' ' + i.lastname AS updateuserid, 
				  r.percentscore, r.reviewdate, i.username, i.userroleid, i.usertypeid, w.workgroupname, s.description, 
				  s.name, s.sequencenumber, q.questiondescription, q.questionnumber, q.questiontext, q.responserequired, 
				  q.questionadditionalpoint, q.questionadditionalconditionpoint, sr.weightedscore, s.weight AS sectionWeight, 
				  qr.responsetext, q.weight AS questionWeight, qt.questiontypedesc, qt.scored AS questionScored, 
				  rt.templatedesc AS reviewTemplate, rs.description as ScorecardStatus
	FROM     reviewtemplate AS rt INNER JOIN
					  review AS r ON r.templateid = rt.templateid INNER JOIN
					  reviewstatus rs ON rs.reviewstatusid = r.reviewstatusid INNER JOIN
					  media AS m ON r.mediaid = m.mediaid INNER JOIN
					  iqmuser AS i ON i.userid = m.userid INNER JOIN
					  workgroup_iqmuser AS wi ON wi.userid = i.userid INNER JOIN
					  workgroup AS w ON w.workgroupid = wi.workgroupid INNER JOIN
					  sectionresult AS sr ON sr.reviewid = r.reviewid INNER JOIN
					  section AS s ON s.sectionid = sr.sectionid INNER JOIN
					  question AS q ON q.sectionid = s.sectionid INNER JOIN
					  questionresult AS qr ON q.questionid = qr.questionid INNER JOIN
					  questiontype AS qt ON qt.questiontypeid = q.questiontypeid 
	where w.workgroupid in ( @workgroupid ) AND
	(CAST(CONVERT(CHAR(10), m.starttime, 101) AS datetime) BETWEEN @formatedStartDate AND @formatedEndDate) AND 
    (CAST(CONVERT(CHAR(10), m.endtime, 101) AS datetime) BETWEEN @formatedStartDate AND @formatedEndDate))

	select starttime, endtime, mediaid, dnis, ani, updateuserid, percentscore, reviewdate, username,
			userroleid, usertypeid, workgroupname, [description], [name], sequencenumber, questiondescription, questionnumber,
			questiontext, responserequired, questionadditionalpoint, questionadditionalconditionpoint, weightedscore,
			sectionWeight, responsetext, questionWeight, questiontypedesc, questionScored, reviewTemplate, ScorecardStatus 
			FROM CTEGetReport

end
END

