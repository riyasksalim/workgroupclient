USE [qm]
GO
/****** Object:  StoredProcedure [dbo].[GetReport]    Script Date: 21/07/2018 15:22:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[GetReport] 
	@startdate datetime,
	@endDate datetime,
	@workgroupid varchar(max),
	@templateid varchar(max)
AS
BEGIN

	declare @formatedStartDate datetime
	set  @formatedStartDate = Cast(CONVERT(CHAR(10), @startdate, 101) as datetime)

	declare @formatedEndDate datetime
	set  @formatedEndDate = Cast(CONVERT(CHAR(10), @endDate, 101) as datetime) 

	--split WorkgroupIDs into a table
	DECLARE @workgroupIDs  table(WorkGroupId varchar(1000) NULL)
	INSERT INTO @workgroupIDs
	select * from dbo.SplitString(@workgroupid, ',') 

	--split TemplateIDs into a table
	DECLARE @templateIDs  table(TemplateId varchar(1000) NULL)
	INSERT INTO @templateIDs
	select * from dbo.SplitString(@templateid, ',') 

	DECLARE @FinResult  table(starttime datetime null, endtime datetime null, mediaid uniqueidentifier null, dnis nvarchar(50) null, ani nvarchar(50) null, 
	updateuserid nvarchar(1000) null, percentscore smallint null, reviewdate datetime null, username nvarchar(50),
	userroleid tinyint null, usertypeid tinyint null, workgroupname nvarchar(200) null, [description] nvarchar(256) null, 
	[name] nvarchar(50), sequencenumber tinyint null, questiondescription nvarchar(1024) null, questionnumber tinyint null,
	questiontext nvarchar(1024), responserequired bit null, questionadditionalpoint decimal(18,0) null, 
	questionadditionalconditionpoint decimal(18,0) null, weightedscore float null,
	sectionWeight tinyint null, responsetext nvarchar(1024), questionWeight tinyint null, questiontypedesc nvarchar(50) null, 
	questionScored bit null, reviewTemplate nvarchar(100), workgroupid uniqueidentifier NOT null, ScorecardStatus nvarchar(50) null)

	;with CTEGetReport (starttime, endtime, mediaid, dnis, ani, updateuserid, percentscore, reviewdate, username,
	userroleid, usertypeid, workgroupname, [description], [name], sequencenumber, questiondescription, questionnumber,
	questiontext, responserequired, questionadditionalpoint, questionadditionalconditionpoint, weightedscore,
	sectionWeight, responsetext, questionWeight, questiontypedesc, questionScored, reviewTemplate, workgroupid, ScorecardStatus)
	as  
	(SELECT DISTINCT 
                  m.starttime, m.endtime, m.mediaid, m.dnis, m.ani, i.firstname + ' ' + i.lastname AS updateuserid, 
				  r.percentscore, r.reviewdate, i.username, i.userroleid, i.usertypeid, w.workgroupname, s.description, 
				  s.name, s.sequencenumber, q.questiondescription, q.questionnumber, q.questiontext, q.responserequired, 
				  q.questionadditionalpoint, q.questionadditionalconditionpoint, sr.weightedscore, s.weight AS sectionWeight, 
				  qr.responsetext, q.weight AS questionWeight, qt.questiontypedesc, qt.scored AS questionScored, 
				  rt.templatedesc AS reviewTemplate, w.workgroupid, rs.description as ScorecardStatus
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
		WHERE  (rt.templateid IN (SELECT TemplateId FROM @templateIDs)) AND
		(CAST(CONVERT(CHAR(10), m.starttime, 101) AS datetime) BETWEEN @formatedStartDate AND @formatedEndDate) AND
		(CAST(CONVERT(CHAR(10), m.endtime, 101) AS datetime) BETWEEN @formatedStartDate AND @formatedEndDate) 
		AND (rt.templateid IN (SELECT TemplateId FROM @templateIDs)))

		insert into @FinResult
		select starttime, endtime, mediaid, dnis, ani, updateuserid, percentscore, reviewdate, username,
		userroleid, usertypeid, workgroupname, [description], [name], sequencenumber, questiondescription, questionnumber,
		questiontext, responserequired, questionadditionalpoint, questionadditionalconditionpoint, weightedscore,
		sectionWeight, responsetext, questionWeight, questiontypedesc, questionScored, reviewTemplate, workgroupid, ScorecardStatus from CTEGetReport
		
		IF LEN(@workgroupid) > 0
		BEGIN
			select starttime, endtime, mediaid, dnis, ani, updateuserid, percentscore, reviewdate, username,
			userroleid, usertypeid, workgroupname, [description], [name], sequencenumber, questiondescription, questionnumber,
			questiontext, responserequired, questionadditionalpoint, questionadditionalconditionpoint, weightedscore,
			sectionWeight, responsetext, questionWeight, questiontypedesc, questionScored, reviewTemplate, workgroupid, ScorecardStatus 
			FROM @FinResult WHERE (workgroupid IN (SELECT WorkGroupId FROM @workgroupIDs)) 
		END
		ELSE
		BEGIN
			select starttime, endtime, mediaid, dnis, ani, updateuserid, percentscore, reviewdate, username,
			userroleid, usertypeid, workgroupname, [description], [name], sequencenumber, questiondescription, questionnumber,
			questiontext, responserequired, questionadditionalpoint, questionadditionalconditionpoint, weightedscore,
			sectionWeight, responsetext, questionWeight, questiontypedesc, questionScored, reviewTemplate, workgroupid, ScorecardStatus 
			FROM @FinResult
		END
END


