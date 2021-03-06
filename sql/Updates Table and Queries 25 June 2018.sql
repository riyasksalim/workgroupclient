USE [QM]
GO
/****** Object:  StoredProcedure [dbo].[GetAllTemplates]    Script Date: 6/25/2018 9:40:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetAllTemplates] 
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT templateid, templatedesc
	FROM     reviewtemplate where status = 1 order by templatedesc asc

END

GO
/****** Object:  StoredProcedure [dbo].[GetAllWorkGroups]    Script Date: 6/25/2018 9:40:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetAllWorkGroups] 
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Select workgroupid, workgroupname from workgroup where status = 1 order by workgroupname
END


GO
/****** Object:  StoredProcedure [dbo].[GetReport]    Script Date: 6/25/2018 9:40:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetReport] 
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
	questionScored bit null, reviewTemplate nvarchar(100), workgroupid uniqueidentifier NOT null)

	;with CTEGetReport (starttime, endtime, mediaid, dnis, ani, updateuserid, percentscore, reviewdate, username,
	userroleid, usertypeid, workgroupname, [description], [name], sequencenumber, questiondescription, questionnumber,
	questiontext, responserequired, questionadditionalpoint, questionadditionalconditionpoint, weightedscore,
	sectionWeight, responsetext, questionWeight, questiontypedesc, questionScored, reviewTemplate, workgroupid)
	as  
	(SELECT DISTINCT 
                  m.starttime, m.endtime, m.mediaid, m.dnis, m.ani, i.firstname + ' ' + i.lastname AS updateuserid, 
				  r.percentscore, r.reviewdate, i.username, i.userroleid, i.usertypeid, w.workgroupname, s.description, 
				  s.name, s.sequencenumber, q.questiondescription, q.questionnumber, q.questiontext, q.responserequired, 
				  q.questionadditionalpoint, q.questionadditionalconditionpoint, sr.weightedscore, s.weight AS sectionWeight, 
				  qr.responsetext, q.weight AS questionWeight, qt.questiontypedesc, qt.scored AS questionScored, 
				  rt.templatedesc AS reviewTemplate, w.workgroupid
	FROM     reviewtemplate AS rt INNER JOIN
					  review AS r ON r.templateid = rt.templateid INNER JOIN
					  media AS m ON r.mediaid = m.mediaid INNER JOIN
					  iqmuser AS i ON i.userid = r.updateuserid INNER JOIN
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
		AND (w.workgroupid IN (SELECT WorkGroupId FROM @workgroupIDs)))

		insert into @FinResult
		select starttime, endtime, mediaid, dnis, ani, updateuserid, percentscore, reviewdate, username,
		userroleid, usertypeid, workgroupname, [description], [name], sequencenumber, questiondescription, questionnumber,
		questiontext, responserequired, questionadditionalpoint, questionadditionalconditionpoint, weightedscore,
		sectionWeight, responsetext, questionWeight, questiontypedesc, questionScored, reviewTemplate, workgroupid from CTEGetReport
		
		IF LEN(@workgroupid) > 0
		BEGIN
			select starttime, endtime, mediaid, dnis, ani, updateuserid, percentscore, reviewdate, username,
			userroleid, usertypeid, workgroupname, [description], [name], sequencenumber, questiondescription, questionnumber,
			questiontext, responserequired, questionadditionalpoint, questionadditionalconditionpoint, weightedscore,
			sectionWeight, responsetext, questionWeight, questiontypedesc, questionScored, reviewTemplate, workgroupid 
			FROM @FinResult WHERE (workgroupid IN (SELECT WorkGroupId FROM @workgroupIDs)) 
		END
		ELSE
		BEGIN
			select starttime, endtime, mediaid, dnis, ani, updateuserid, percentscore, reviewdate, username,
			userroleid, usertypeid, workgroupname, [description], [name], sequencenumber, questiondescription, questionnumber,
			questiontext, responserequired, questionadditionalpoint, questionadditionalconditionpoint, weightedscore,
			sectionWeight, responsetext, questionWeight, questiontypedesc, questionScored, reviewTemplate, workgroupid 
			FROM @FinResult
		END
END


GO
/****** Object:  StoredProcedure [dbo].[GetReportcsvList]    Script Date: 6/25/2018 9:40:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetReportcsvList] 
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	--SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT ID, ReportGeneratedFileName, CreatedOn, CreatedBy, MethodofCreation, ReportGeneratedFullPath, ReportLocation
	FROM     ReportsGenerated order by ID desc
END

GO
/****** Object:  StoredProcedure [dbo].[GetReportDailyJob]    Script Date: 6/25/2018 9:40:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Mujeeb K S
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetReportDailyJob] 
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
	sectionWeight, responsetext, questionWeight, questiontypedesc, questionScored, reviewTemplate)
	as     
	(SELECT DISTINCT 
                         m.starttime, m.endtime, m.mediaid, m.dnis, m.ani, i.firstname + ' ' + i.lastname as updateuserid, r.percentscore, r.reviewdate, i.username, i.userroleid, i.usertypeid, w.workgroupname, s.description, s.name, s.sequencenumber, 
                         q.questiondescription, q.questionnumber, q.questiontext, q.responserequired, q.questionadditionalpoint, q.questionadditionalconditionpoint, sr.weightedscore, s.weight AS sectionWeight, 
                         qr.responsetext, q.weight AS questionWeight, qt.questiontypedesc, qt.scored AS questionScored, rt.templatedesc AS reviewTemplate
	FROM     reviewtemplate AS rt INNER JOIN
					  review AS r ON r.templateid = rt.templateid INNER JOIN
					  media AS m ON r.mediaid = m.mediaid INNER JOIN
					  iqmuser AS i ON i.userid = r.updateuserid INNER JOIN
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
	sectionWeight, responsetext, questionWeight, questiontypedesc, questionScored from CTEGetReport

end
else 
begin

	;with CTEGetReport (starttime, endtime, mediaid, dnis, ani, updateuserid, percentscore, reviewdate, username,
	userroleid, usertypeid, workgroupname, [description], [name], sequencenumber, questiondescription, questionnumber,
	questiontext, responserequired, questionadditionalpoint, questionadditionalconditionpoint, weightedscore,
	sectionWeight, responsetext, questionWeight, questiontypedesc, questionScored, reviewTemplate)
	as     
	(SELECT DISTINCT 
                         m.starttime, m.endtime, m.mediaid, m.dnis, m.ani, i.firstname + ' ' + i.lastname as updateuserid, r.percentscore, r.reviewdate, i.username, i.userroleid, i.usertypeid, w.workgroupname, s.description, s.name, s.sequencenumber, 
                         q.questiondescription, q.questionnumber, q.questiontext, q.responserequired, q.questionadditionalpoint, q.questionadditionalconditionpoint, sr.weightedscore, s.weight AS sectionWeight, 
                         qr.responsetext, q.weight AS questionWeight, qt.questiontypedesc, qt.scored AS questionScored, rt.templatedesc AS reviewTemplate
	FROM     reviewtemplate AS rt INNER JOIN
					  review AS r ON r.templateid = rt.templateid INNER JOIN
					  media AS m ON r.mediaid = m.mediaid INNER JOIN
					  iqmuser AS i ON i.userid = r.updateuserid INNER JOIN
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
	questiontext, responserequired, questionadditionalpoint,  questionadditionalconditionpoint, weightedscore,
	sectionWeight, responsetext, questionWeight, questiontypedesc, questionScored, reviewTemplate from CTEGetReport

end
END


GO
/****** Object:  StoredProcedure [dbo].[InsertReport]    Script Date: 6/25/2018 9:40:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[InsertReport]  

    @ReportGeneratedFileName varchar(MAX),
	@CreatedOn datetime,
	@CreatedBy varchar(MAX),
	@MethodofCreation varchar(MAX),
	@ReportGeneratedFullPath varchar(MAX),
	@ReportLocation varchar(MAX)

AS 
BEGIN 
    --SET NOCOUNT ON; 

      INSERT INTO [dbo].[ReportsGenerated] 
	  (
	   ReportGeneratedFileName,
	   CreatedOn, 
	   CreatedBy,
	   MethodofCreation,
	   ReportGeneratedFullPath,
	   ReportLocation
	  
	  ) 

VALUES (

    @ReportGeneratedFileName,
	@CreatedOn,
	@CreatedBy,
	@MethodofCreation,
	@ReportGeneratedFullPath,
	@ReportLocation
); 


END 

GO
/****** Object:  UserDefinedFunction [dbo].[SplitString]    Script Date: 6/25/2018 9:40:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[SplitString]
(    
      @Input NVARCHAR(MAX),
      @Character CHAR(1)
)
RETURNS @Output TABLE (
      Item NVARCHAR(1000)
)
AS
BEGIN
      DECLARE @StartIndex INT, @EndIndex INT
 
      SET @StartIndex = 1
      IF SUBSTRING(@Input, LEN(@Input) - 1, LEN(@Input)) <> @Character
      BEGIN
            SET @Input = @Input + @Character
      END
 
      WHILE CHARINDEX(@Character, @Input) > 0
      BEGIN
            SET @EndIndex = CHARINDEX(@Character, @Input)
           
            INSERT INTO @Output(Item)
            SELECT SUBSTRING(@Input, @StartIndex, @EndIndex - 1)
           
            SET @Input = SUBSTRING(@Input, @EndIndex + 1, LEN(@Input))
      END
 
      RETURN
END

GO
/****** Object:  Table [dbo].[iqmuser]    Script Date: 6/25/2018 9:40:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[iqmuser](
	[userid] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[status] [tinyint] NOT NULL,
	[username] [nvarchar](64) NOT NULL,
	[firstname] [nvarchar](64) NULL,
	[lastname] [nvarchar](64) NULL,
	[password] [varbinary](64) NULL,
	[userroleid] [tinyint] NOT NULL,
	[siteid] [uniqueidentifier] NOT NULL,
	[switchid] [uniqueidentifier] NULL,
	[positionid] [nvarchar](50) NULL,
	[extension] [nvarchar](50) NULL,
	[email] [nvarchar](128) NULL,
	[hostid] [bigint] NULL,
	[audiomonitoring] [bit] NOT NULL,
	[screenmonitoring] [bit] NOT NULL,
	[usertypeid] [tinyint] NOT NULL,
	[erroralertemails] [bit] NOT NULL,
	[windowslogondomain] [nvarchar](50) NULL,
	[windowslogonaccount] [nvarchar](50) NULL,
	[profileid] [uniqueidentifier] NULL,
	[duplicateldapidflg] [tinyint] NOT NULL,
	[webclientldapinfoupdate] [bit] NOT NULL,
	[allowlogin] [bit] NOT NULL,
 CONSTRAINT [PK_iqmuser] PRIMARY KEY CLUSTERED 
(
	[userid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[media]    Script Date: 6/25/2018 9:40:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[media](
	[mediaint] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[mediaid] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[mediatypeid] [tinyint] NOT NULL,
	[status] [tinyint] NOT NULL,
	[filename] [nvarchar](100) NULL,
	[scstatus] [tinyint] NOT NULL,
	[scfilename] [nvarchar](100) NULL,
	[switchid] [uniqueidentifier] NULL,
	[starttime] [datetime] NULL,
	[endtime] [datetime] NULL,
	[userid] [uniqueidentifier] NULL,
	[initialqueueid] [uniqueidentifier] NULL,
	[takenqueueid] [uniqueidentifier] NULL,
	[portid] [uniqueidentifier] NULL,
	[positionid] [nvarchar](50) NULL,
	[terminalnumber] [nvarchar](50) NULL,
	[extension] [nvarchar](50) NULL,
	[archiveexempt] [bit] NOT NULL,
	[monitorreasonid] [tinyint] NULL,
	[ani] [nvarchar](50) NULL,
	[dnis] [nvarchar](50) NULL,
	[holdcount] [int] NOT NULL,
	[maxholdtime] [int] NOT NULL,
	[totalholdtime] [int] NOT NULL,
	[applicationid] [uniqueidentifier] NULL,
	[callduration] [int] NOT NULL,
	[archivedate] [datetime] NULL,
	[deleteonarchive] [bit] NULL,
	[parentmediaid] [uniqueidentifier] NOT NULL,
	[hostid] [nvarchar](50) NULL,
	[updatetimestamp] [datetime] NULL,
	[audioscreensync] [bit] NOT NULL,
	[deletesconarchive] [bit] NULL,
	[burntmediaid] [uniqueidentifier] NULL,
	[audiofilekeyid] [uniqueidentifier] NULL,
	[scfilekeyid] [uniqueidentifier] NULL,
	[historicaldate] [datetime] NULL,
	[deleteonhistorical] [bit] NULL,
	[deletesconhistorical] [bit] NULL,
	[filestatus] [tinyint] NOT NULL,
	[storagegroupid] [uniqueidentifier] NULL,
	[scfilestatus] [tinyint] NOT NULL,
	[scstoragegroupid] [uniqueidentifier] NULL,
	[customreview] [bit] NOT NULL,
	[ucid] [nvarchar](50) NULL,
	[mirrorhostid] [nvarchar](50) NULL,
	[externrefid] [int] NULL,
	[externlotid] [int] NULL,
	[externrefstateid] [int] NOT NULL,
	[datetimeid] [datetime2](0) NULL,
 CONSTRAINT [PK_media] PRIMARY KEY CLUSTERED 
(
	[mediaint] ASC,
	[mediaid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY],
 CONSTRAINT [UK_media_mediaid] UNIQUE NONCLUSTERED 
(
	[mediaid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[note]    Script Date: 6/25/2018 9:40:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[note](
	[noteid] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[reviewid] [uniqueidentifier] NULL,
	[note] [nvarchar](1024) NULL,
 CONSTRAINT [PK_note] PRIMARY KEY CLUSTERED 
(
	[noteid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[question]    Script Date: 6/25/2018 9:40:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[question](
	[questionid] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[sectionid] [uniqueidentifier] NULL,
	[questiontypeid] [tinyint] NOT NULL,
	[responserequired] [bit] NOT NULL,
	[questionnumber] [tinyint] NOT NULL,
	[questiontext] [nvarchar](1024) NOT NULL,
	[weight] [tinyint] NOT NULL,
	[optionsorientation] [smallint] NULL,
	[questiondescription] [nvarchar](1024) NULL,
	[questionadditionaltypeid] [tinyint] NULL,
	[questionadditionalconditiontypeid] [tinyint] NULL,
	[questionadditionalconditionpoint] [decimal](18, 0) NULL,
	[questionadditionalpoint] [decimal](18, 0) NULL,
 CONSTRAINT [PK_templatequestion] PRIMARY KEY CLUSTERED 
(
	[questionid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[questionoption]    Script Date: 6/25/2018 9:40:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[questionoption](
	[questionoptionid] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[questionid] [uniqueidentifier] NULL,
	[optionnumber] [tinyint] NOT NULL,
	[optiontext] [nvarchar](200) NOT NULL,
	[optionpoint] [decimal](18, 2) NULL,
 CONSTRAINT [PK_questionoption] PRIMARY KEY CLUSTERED 
(
	[questionoptionid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[questionresult]    Script Date: 6/25/2018 9:40:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[questionresult](
	[questionresultid] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[sectionresultid] [uniqueidentifier] NULL,
	[questionid] [uniqueidentifier] NULL,
	[responsetext] [nvarchar](1024) NULL,
	[weightedscore] [float] NULL,
	[failed] [bit] NOT NULL,
 CONSTRAINT [PK_questionresults] PRIMARY KEY CLUSTERED 
(
	[questionresultid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[questionresultoption]    Script Date: 6/25/2018 9:40:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[questionresultoption](
	[questionresultoptionid] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[questionresultid] [uniqueidentifier] NULL,
	[questionoptionid] [uniqueidentifier] NULL,
 CONSTRAINT [PK_questionresultoption] PRIMARY KEY CLUSTERED 
(
	[questionresultoptionid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[questiontype]    Script Date: 6/25/2018 9:40:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[questiontype](
	[questiontypeid] [tinyint] NOT NULL,
	[questiontypedesc] [nvarchar](50) NOT NULL,
	[scored] [bit] NOT NULL,
 CONSTRAINT [PK_questiontype] PRIMARY KEY CLUSTERED 
(
	[questiontypeid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ReportsGenerated]    Script Date: 6/25/2018 9:40:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ReportsGenerated](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ReportGeneratedFileName] [varchar](max) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [varchar](max) NULL,
	[MethodofCreation] [varchar](max) NULL,
	[ReportGeneratedFullPath] [varchar](max) NULL,
	[ReportLocation] [varchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[review]    Script Date: 6/25/2018 9:40:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[review](
	[reviewid] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[mediaid] [uniqueidentifier] NULL,
	[templateid] [uniqueidentifier] NULL,
	[percentscore] [smallint] NULL,
	[owneruserid] [uniqueidentifier] NULL,
	[reviewdate] [datetime] NULL,
	[updateuserid] [uniqueidentifier] NULL,
	[reviewstatusid] [tinyint] NOT NULL,
	[training] [bit] NOT NULL,
	[reviewedbyrecordedagent] [bit] NOT NULL,
	[agentreview] [bit] NOT NULL,
	[teamreview] [bit] NOT NULL,
	[iscreatedduringtransitoryaccess] [bit] NOT NULL,
	[peerreview] [bit] NOT NULL,
	[failed] [bit] NOT NULL,
	[pointscore] [float] NULL,
	[calibrationreview] [bit] NOT NULL,
 CONSTRAINT [PK_review] PRIMARY KEY CLUSTERED 
(
	[reviewid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[reviewattachment]    Script Date: 6/25/2018 9:40:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[reviewattachment](
	[reviewattachmentid] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[reviewid] [uniqueidentifier] NOT NULL,
	[userid] [uniqueidentifier] NOT NULL,
	[sno] [int] NOT NULL,
	[filename] [nvarchar](100) NOT NULL,
	[description] [nvarchar](100) NULL,
	[attachmentdate] [datetime] NULL,
 CONSTRAINT [PK_reviewattachment] PRIMARY KEY CLUSTERED 
(
	[reviewattachmentid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[reviewcomment]    Script Date: 6/25/2018 9:40:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[reviewcomment](
	[reviewcommentid] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[reviewid] [uniqueidentifier] NOT NULL,
	[userid] [uniqueidentifier] NOT NULL,
	[eventindex] [int] NOT NULL,
	[comment] [nvarchar](1024) NULL,
	[commentfilename] [nvarchar](50) NULL,
	[modifieddate] [datetime] NOT NULL,
	[mediafileassociationid] [tinyint] NOT NULL,
 CONSTRAINT [PK_reviewcomment] PRIMARY KEY CLUSTERED 
(
	[reviewcommentid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[reviewstatus]    Script Date: 6/25/2018 9:40:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[reviewstatus](
	[reviewstatusid] [tinyint] NOT NULL,
	[description] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_reviewstatus] PRIMARY KEY CLUSTERED 
(
	[reviewstatusid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[reviewtemplate]    Script Date: 6/25/2018 9:40:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[reviewtemplate](
	[templateid] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[status] [tinyint] NOT NULL,
	[templatedesc] [nvarchar](100) NOT NULL,
	[siteid] [uniqueidentifier] NOT NULL,
	[defaultresponseid] [tinyint] NOT NULL,
	[displayscoretype] [tinyint] NULL,
	[displayscoreconditionid] [tinyint] NULL,
	[displayscoreconditionpoint] [decimal](18, 0) NULL,
 CONSTRAINT [PK_reviewtemplate] PRIMARY KEY CLUSTERED 
(
	[templateid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[reviewuser]    Script Date: 6/25/2018 9:40:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[reviewuser](
	[reviewuserid] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[reviewid] [uniqueidentifier] NOT NULL,
	[userid] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_reviewuser] PRIMARY KEY CLUSTERED 
(
	[reviewid] ASC,
	[userid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[section]    Script Date: 6/25/2018 9:40:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[section](
	[sectionid] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[templateid] [uniqueidentifier] NULL,
	[sequencenumber] [tinyint] NOT NULL,
	[name] [nvarchar](50) NOT NULL,
	[description] [nvarchar](256) NOT NULL,
	[weight] [tinyint] NOT NULL,
 CONSTRAINT [PK_section] PRIMARY KEY CLUSTERED 
(
	[sectionid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[sectionresult]    Script Date: 6/25/2018 9:40:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sectionresult](
	[sectionresultid] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[reviewid] [uniqueidentifier] NULL,
	[sectionid] [uniqueidentifier] NULL,
	[weightedscore] [float] NULL,
	[failed] [bit] NOT NULL,
 CONSTRAINT [PK_sectionresult] PRIMARY KEY CLUSTERED 
(
	[sectionresultid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[userprofile]    Script Date: 6/25/2018 9:40:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[userprofile](
	[profileid] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[profilename] [nvarchar](64) NOT NULL,
	[description] [nvarchar](64) NULL,
	[status] [tinyint] NOT NULL,
	[createrules] [bit] NOT NULL,
	[createtemplates] [bit] NOT NULL,
	[createreports] [bit] NOT NULL,
	[reviewinteractions] [bit] NOT NULL,
	[selfevaluate] [bit] NOT NULL,
	[accesscmq] [bit] NOT NULL,
	[createsurveys] [bit] NOT NULL,
	[createinvitationrules] [bit] NOT NULL,
	[erroralertemails] [bit] NOT NULL,
	[viewreports] [bit] NOT NULL,
	[downloadmedia] [bit] NOT NULL,
	[exportmedia] [bit] NOT NULL,
	[createeditpublicsearches] [bit] NOT NULL,
	[manageusers] [bit] NULL,
	[manageteams] [bit] NULL,
	[managesystems] [bit] NULL,
	[grantpeerreview] [bit] NOT NULL,
	[viewpeerreview] [bit] NOT NULL,
	[viewallevaluations] [bit] NOT NULL,
	[assignevaluations] [bit] NOT NULL,
	[assignrecordings] [bit] NOT NULL,
	[createtasks] [bit] NOT NULL,
	[siteid] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_userprofile] PRIMARY KEY CLUSTERED 
(
	[profileid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[workgroup]    Script Date: 6/25/2018 9:40:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[workgroup](
	[workgroupid] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[workgroupname] [nvarchar](128) NOT NULL,
	[status] [tinyint] NOT NULL,
	[hostid] [int] NULL,
	[switchid] [uniqueidentifier] NULL,
	[entitysourcetypeid] [tinyint] NOT NULL,
 CONSTRAINT [PK_workgroup] PRIMARY KEY CLUSTERED 
(
	[workgroupid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[workgroup_iqmuser]    Script Date: 6/25/2018 9:40:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[workgroup_iqmuser](
	[userid] [uniqueidentifier] NOT NULL,
	[workgroupid] [uniqueidentifier] NOT NULL,
	[rowguid] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
 CONSTRAINT [PK_workgroup_iqmuser] PRIMARY KEY CLUSTERED 
(
	[userid] ASC,
	[workgroupid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[iqmuser] ADD  CONSTRAINT [DF_iqmuser_userid]  DEFAULT (newid()) FOR [userid]
GO
ALTER TABLE [dbo].[iqmuser] ADD  CONSTRAINT [DF_iqmuser_userstatus]  DEFAULT ((1)) FOR [status]
GO
ALTER TABLE [dbo].[iqmuser] ADD  CONSTRAINT [DF_iqmuser_duplicateldapidflg]  DEFAULT ((0)) FOR [duplicateldapidflg]
GO
ALTER TABLE [dbo].[iqmuser] ADD  CONSTRAINT [DF_iqmuser_webclientldapinfoupdate]  DEFAULT ((0)) FOR [webclientldapinfoupdate]
GO
ALTER TABLE [dbo].[iqmuser] ADD  DEFAULT ((0)) FOR [allowlogin]
GO
ALTER TABLE [dbo].[media] ADD  CONSTRAINT [DF_media_mediaid]  DEFAULT (newid()) FOR [mediaid]
GO
ALTER TABLE [dbo].[media] ADD  CONSTRAINT [DF_media_status]  DEFAULT ((1)) FOR [status]
GO
ALTER TABLE [dbo].[media] ADD  CONSTRAINT [DF_media_scstatus]  DEFAULT ((4)) FOR [scstatus]
GO
ALTER TABLE [dbo].[media] ADD  CONSTRAINT [DF_media_archiveexempt]  DEFAULT ((0)) FOR [archiveexempt]
GO
ALTER TABLE [dbo].[media] ADD  CONSTRAINT [DF_media_holdcount]  DEFAULT ((0)) FOR [holdcount]
GO
ALTER TABLE [dbo].[media] ADD  CONSTRAINT [DF_media_maxholdtime]  DEFAULT ((0)) FOR [maxholdtime]
GO
ALTER TABLE [dbo].[media] ADD  CONSTRAINT [DF_media_totalholdtime]  DEFAULT ((0)) FOR [totalholdtime]
GO
ALTER TABLE [dbo].[media] ADD  CONSTRAINT [DF_media_updatetimestamp]  DEFAULT (getutcdate()) FOR [updatetimestamp]
GO
ALTER TABLE [dbo].[media] ADD  DEFAULT ((0)) FOR [filestatus]
GO
ALTER TABLE [dbo].[media] ADD  DEFAULT ((0)) FOR [scfilestatus]
GO
ALTER TABLE [dbo].[media] ADD  DEFAULT ((0)) FOR [customreview]
GO
ALTER TABLE [dbo].[media] ADD  DEFAULT ((0)) FOR [externrefstateid]
GO
ALTER TABLE [dbo].[note] ADD  CONSTRAINT [DF_note_noteid]  DEFAULT (newid()) FOR [noteid]
GO
ALTER TABLE [dbo].[question] ADD  CONSTRAINT [DF_question_questionid]  DEFAULT (newid()) FOR [questionid]
GO
ALTER TABLE [dbo].[questionoption] ADD  CONSTRAINT [DF_questionoption_questionoptionid]  DEFAULT (newid()) FOR [questionoptionid]
GO
ALTER TABLE [dbo].[questionresult] ADD  CONSTRAINT [DF_questionresult_questionresultid]  DEFAULT (newid()) FOR [questionresultid]
GO
ALTER TABLE [dbo].[questionresult] ADD  DEFAULT ((0)) FOR [failed]
GO
ALTER TABLE [dbo].[questionresultoption] ADD  CONSTRAINT [DF_questionresultoption_questionresultoptionid]  DEFAULT (newid()) FOR [questionresultoptionid]
GO
ALTER TABLE [dbo].[review] ADD  CONSTRAINT [DF_review_reviewid]  DEFAULT (newid()) FOR [reviewid]
GO
ALTER TABLE [dbo].[review] ADD  CONSTRAINT [DF_review_reviewedbyrecordedagent]  DEFAULT ((0)) FOR [reviewedbyrecordedagent]
GO
ALTER TABLE [dbo].[review] ADD  CONSTRAINT [DF_review_agentreview]  DEFAULT ((0)) FOR [agentreview]
GO
ALTER TABLE [dbo].[review] ADD  CONSTRAINT [DF_review_teamreview]  DEFAULT ((0)) FOR [teamreview]
GO
ALTER TABLE [dbo].[review] ADD  CONSTRAINT [DF_review_iscreatedduringtransitoryaccess]  DEFAULT ((0)) FOR [iscreatedduringtransitoryaccess]
GO
ALTER TABLE [dbo].[review] ADD  CONSTRAINT [DF_review_peerreview]  DEFAULT ((0)) FOR [peerreview]
GO
ALTER TABLE [dbo].[review] ADD  DEFAULT ((0)) FOR [failed]
GO
ALTER TABLE [dbo].[review] ADD  DEFAULT ((0)) FOR [calibrationreview]
GO
ALTER TABLE [dbo].[reviewattachment] ADD  CONSTRAINT [DF_reviewattachment_reviewattachmentid]  DEFAULT (newid()) FOR [reviewattachmentid]
GO
ALTER TABLE [dbo].[reviewcomment] ADD  CONSTRAINT [DF_reviewcomment_reviewcommentid]  DEFAULT (newid()) FOR [reviewcommentid]
GO
ALTER TABLE [dbo].[reviewcomment] ADD  CONSTRAINT [DF_reviewcomment_modifieddate]  DEFAULT (getutcdate()) FOR [modifieddate]
GO
ALTER TABLE [dbo].[reviewtemplate] ADD  CONSTRAINT [DF_reviewtemplate_templateid]  DEFAULT (newid()) FOR [templateid]
GO
ALTER TABLE [dbo].[reviewtemplate] ADD  CONSTRAINT [DF_reviewtemplate_status]  DEFAULT ((1)) FOR [status]
GO
ALTER TABLE [dbo].[reviewtemplate] ADD  CONSTRAINT [DF_reviewtemplate_defaultresponseid]  DEFAULT ((1)) FOR [defaultresponseid]
GO
ALTER TABLE [dbo].[reviewtemplate] ADD  CONSTRAINT [DF_reviewtemplate_displayscoretype]  DEFAULT ((2)) FOR [displayscoretype]
GO
ALTER TABLE [dbo].[reviewuser] ADD  CONSTRAINT [DF_reviewuser_reviewuserid]  DEFAULT (newid()) FOR [reviewuserid]
GO
ALTER TABLE [dbo].[section] ADD  CONSTRAINT [DF_section_sectionid]  DEFAULT (newid()) FOR [sectionid]
GO
ALTER TABLE [dbo].[sectionresult] ADD  CONSTRAINT [DF_sectionresult_sectionresultid]  DEFAULT (newid()) FOR [sectionresultid]
GO
ALTER TABLE [dbo].[sectionresult] ADD  DEFAULT ((0)) FOR [failed]
GO
ALTER TABLE [dbo].[userprofile] ADD  CONSTRAINT [DF_userprofile_profileid]  DEFAULT (newid()) FOR [profileid]
GO
ALTER TABLE [dbo].[workgroup] ADD  CONSTRAINT [DF_workgroup_workgroupid]  DEFAULT (newid()) FOR [workgroupid]
GO
ALTER TABLE [dbo].[workgroup] ADD  CONSTRAINT [DF_workgroup_hostid]  DEFAULT ((0)) FOR [hostid]
GO
ALTER TABLE [dbo].[workgroup_iqmuser] ADD  CONSTRAINT [DF_workgroup_iqmuser_rowguid]  DEFAULT (newid()) FOR [rowguid]
GO
ALTER TABLE [dbo].[iqmuser]  WITH CHECK ADD  CONSTRAINT [FK_user_userprofile] FOREIGN KEY([profileid])
REFERENCES [dbo].[userprofile] ([profileid])
GO
ALTER TABLE [dbo].[iqmuser] CHECK CONSTRAINT [FK_user_userprofile]
GO
ALTER TABLE [dbo].[media]  WITH CHECK ADD  CONSTRAINT [FK_media_iqmuser] FOREIGN KEY([userid])
REFERENCES [dbo].[iqmuser] ([userid])
GO
ALTER TABLE [dbo].[media] CHECK CONSTRAINT [FK_media_iqmuser]
GO
ALTER TABLE [dbo].[note]  WITH NOCHECK ADD  CONSTRAINT [FK_note_review] FOREIGN KEY([reviewid])
REFERENCES [dbo].[review] ([reviewid])
ON DELETE CASCADE
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[note] CHECK CONSTRAINT [FK_note_review]
GO
ALTER TABLE [dbo].[question]  WITH CHECK ADD  CONSTRAINT [FK_question_questiontype] FOREIGN KEY([questiontypeid])
REFERENCES [dbo].[questiontype] ([questiontypeid])
GO
ALTER TABLE [dbo].[question] CHECK CONSTRAINT [FK_question_questiontype]
GO
ALTER TABLE [dbo].[question]  WITH NOCHECK ADD  CONSTRAINT [FK_question_section] FOREIGN KEY([sectionid])
REFERENCES [dbo].[section] ([sectionid])
ON DELETE CASCADE
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[question] CHECK CONSTRAINT [FK_question_section]
GO
ALTER TABLE [dbo].[questionoption]  WITH NOCHECK ADD  CONSTRAINT [FK_questionoption_question] FOREIGN KEY([questionid])
REFERENCES [dbo].[question] ([questionid])
ON DELETE CASCADE
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[questionoption] CHECK CONSTRAINT [FK_questionoption_question]
GO
ALTER TABLE [dbo].[questionresult]  WITH CHECK ADD  CONSTRAINT [FK_questionresults_question] FOREIGN KEY([questionid])
REFERENCES [dbo].[question] ([questionid])
GO
ALTER TABLE [dbo].[questionresult] CHECK CONSTRAINT [FK_questionresults_question]
GO
ALTER TABLE [dbo].[questionresult]  WITH NOCHECK ADD  CONSTRAINT [FK_questionresults_sectionresults] FOREIGN KEY([sectionresultid])
REFERENCES [dbo].[sectionresult] ([sectionresultid])
ON DELETE CASCADE
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[questionresult] CHECK CONSTRAINT [FK_questionresults_sectionresults]
GO
ALTER TABLE [dbo].[questionresultoption]  WITH CHECK ADD  CONSTRAINT [FK_questionresultoption_questionoption] FOREIGN KEY([questionoptionid])
REFERENCES [dbo].[questionoption] ([questionoptionid])
GO
ALTER TABLE [dbo].[questionresultoption] CHECK CONSTRAINT [FK_questionresultoption_questionoption]
GO
ALTER TABLE [dbo].[questionresultoption]  WITH NOCHECK ADD  CONSTRAINT [FK_questionresultoption_questionresult] FOREIGN KEY([questionresultid])
REFERENCES [dbo].[questionresult] ([questionresultid])
ON DELETE CASCADE
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[questionresultoption] CHECK CONSTRAINT [FK_questionresultoption_questionresult]
GO
ALTER TABLE [dbo].[review]  WITH CHECK ADD  CONSTRAINT [FK_review_iqmuser] FOREIGN KEY([owneruserid])
REFERENCES [dbo].[iqmuser] ([userid])
GO
ALTER TABLE [dbo].[review] CHECK CONSTRAINT [FK_review_iqmuser]
GO
ALTER TABLE [dbo].[review]  WITH CHECK ADD  CONSTRAINT [FK_review_iqmuser1] FOREIGN KEY([updateuserid])
REFERENCES [dbo].[iqmuser] ([userid])
GO
ALTER TABLE [dbo].[review] CHECK CONSTRAINT [FK_review_iqmuser1]
GO
ALTER TABLE [dbo].[review]  WITH NOCHECK ADD  CONSTRAINT [FK_review_media] FOREIGN KEY([mediaid])
REFERENCES [dbo].[media] ([mediaid])
ON DELETE CASCADE
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[review] CHECK CONSTRAINT [FK_review_media]
GO
ALTER TABLE [dbo].[review]  WITH CHECK ADD  CONSTRAINT [FK_review_reviewstatus] FOREIGN KEY([reviewstatusid])
REFERENCES [dbo].[reviewstatus] ([reviewstatusid])
GO
ALTER TABLE [dbo].[review] CHECK CONSTRAINT [FK_review_reviewstatus]
GO
ALTER TABLE [dbo].[review]  WITH CHECK ADD  CONSTRAINT [FK_review_reviewtemplate] FOREIGN KEY([templateid])
REFERENCES [dbo].[reviewtemplate] ([templateid])
GO
ALTER TABLE [dbo].[review] CHECK CONSTRAINT [FK_review_reviewtemplate]
GO
ALTER TABLE [dbo].[reviewattachment]  WITH CHECK ADD  CONSTRAINT [FK_reviewattachment_iqmuser] FOREIGN KEY([userid])
REFERENCES [dbo].[iqmuser] ([userid])
GO
ALTER TABLE [dbo].[reviewattachment] CHECK CONSTRAINT [FK_reviewattachment_iqmuser]
GO
ALTER TABLE [dbo].[reviewattachment]  WITH NOCHECK ADD  CONSTRAINT [FK_reviewattachment_review] FOREIGN KEY([reviewid])
REFERENCES [dbo].[review] ([reviewid])
ON DELETE CASCADE
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[reviewattachment] CHECK CONSTRAINT [FK_reviewattachment_review]
GO
ALTER TABLE [dbo].[reviewcomment]  WITH CHECK ADD  CONSTRAINT [FK_reviewcomment_iqmuser] FOREIGN KEY([userid])
REFERENCES [dbo].[iqmuser] ([userid])
GO
ALTER TABLE [dbo].[reviewcomment] CHECK CONSTRAINT [FK_reviewcomment_iqmuser]
GO
ALTER TABLE [dbo].[reviewcomment]  WITH NOCHECK ADD  CONSTRAINT [FK_reviewcomment_review] FOREIGN KEY([reviewid])
REFERENCES [dbo].[review] ([reviewid])
ON DELETE CASCADE
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[reviewcomment] CHECK CONSTRAINT [FK_reviewcomment_review]
GO
ALTER TABLE [dbo].[reviewuser]  WITH CHECK ADD  CONSTRAINT [FK_reviewuser_iqmuser] FOREIGN KEY([userid])
REFERENCES [dbo].[iqmuser] ([userid])
GO
ALTER TABLE [dbo].[reviewuser] CHECK CONSTRAINT [FK_reviewuser_iqmuser]
GO
ALTER TABLE [dbo].[reviewuser]  WITH CHECK ADD  CONSTRAINT [FK_reviewuser_review] FOREIGN KEY([reviewid])
REFERENCES [dbo].[review] ([reviewid])
GO
ALTER TABLE [dbo].[reviewuser] CHECK CONSTRAINT [FK_reviewuser_review]
GO
ALTER TABLE [dbo].[section]  WITH NOCHECK ADD  CONSTRAINT [FK_section_reviewtemplate] FOREIGN KEY([templateid])
REFERENCES [dbo].[reviewtemplate] ([templateid])
ON DELETE CASCADE
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[section] CHECK CONSTRAINT [FK_section_reviewtemplate]
GO
ALTER TABLE [dbo].[sectionresult]  WITH NOCHECK ADD  CONSTRAINT [FK_sectionresults_review] FOREIGN KEY([reviewid])
REFERENCES [dbo].[review] ([reviewid])
ON DELETE CASCADE
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[sectionresult] CHECK CONSTRAINT [FK_sectionresults_review]
GO
ALTER TABLE [dbo].[sectionresult]  WITH CHECK ADD  CONSTRAINT [FK_sectionresults_section] FOREIGN KEY([sectionid])
REFERENCES [dbo].[section] ([sectionid])
GO
ALTER TABLE [dbo].[sectionresult] CHECK CONSTRAINT [FK_sectionresults_section]
GO
ALTER TABLE [dbo].[workgroup_iqmuser]  WITH NOCHECK ADD  CONSTRAINT [FK_workgroup_iqmuser_iqmuser] FOREIGN KEY([userid])
REFERENCES [dbo].[iqmuser] ([userid])
ON DELETE CASCADE
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[workgroup_iqmuser] CHECK CONSTRAINT [FK_workgroup_iqmuser_iqmuser]
GO
ALTER TABLE [dbo].[workgroup_iqmuser]  WITH NOCHECK ADD  CONSTRAINT [FK_workgroup_iqmuser_workgroup] FOREIGN KEY([workgroupid])
REFERENCES [dbo].[workgroup] ([workgroupid])
ON DELETE CASCADE
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[workgroup_iqmuser] CHECK CONSTRAINT [FK_workgroup_iqmuser_workgroup]
GO
