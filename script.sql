USE [master]
GO
/****** Object:  Table [dbo].[Account]    Script Date: 08/11/2018 5:36:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Account](
	[AccountID] [int] IDENTITY(0,1) NOT NULL,
	[Username] [varchar](15) NOT NULL,
	[Password] [varchar](50) NOT NULL,
	[Email] [varchar](50) NULL,
	[Profile] [int] NULL,
	[Type] [int] NOT NULL,
	[Locked] [tinyint] NOT NULL,
	[Security] [text] NOT NULL,
	[Answer] [text] NOT NULL,
	[Image] [text] NOT NULL,
 CONSTRAINT [PK_Account] PRIMARY KEY CLUSTERED 
(
	[AccountID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Applicant]    Script Date: 08/11/2018 5:36:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Applicant](
	[ApplicantID] [int] IDENTITY(1,1) NOT NULL,
	[Profile] [int] NOT NULL,
	[Skills] [text] NOT NULL,
	[DesiredPosition] [varchar](30) NOT NULL,
	[Status] [int] NOT NULL,
 CONSTRAINT [PK_Applicant] PRIMARY KEY CLUSTERED 
(
	[ApplicantID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Appraisal]    Script Date: 08/11/2018 5:36:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Appraisal](
	[AppraisalID] [int] IDENTITY(0,1) NOT NULL,
	[CoveredPeriod] [date] NOT NULL,
	[Criteria] [varchar](50) NOT NULL,
	[Rating] [numeric](10, 0) NOT NULL,
	[TechComp] [numeric](10, 0) NOT NULL,
	[InterSkills] [numeric](10, 0) NOT NULL,
	[CommComp] [numeric](10, 0) NOT NULL,
	[Total] [numeric](10, 0) NOT NULL,
	[Comments] [nvarchar](360) NULL,
	[Evaluator] [int] NOT NULL,
	[DatePrepared] [date] NULL,
	[NotedBy] [int] NULL,
	[DateNoted] [date] NULL,
	[DiscussedWith] [int] NULL,
	[DateDiscussed] [date] NULL,
	[Type] [tinyint] NOT NULL,
	[Status] [tinyint] NOT NULL,
 CONSTRAINT [PK_Appraisal] PRIMARY KEY CLUSTERED 
(
	[AppraisalID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AssignedTraining]    Script Date: 08/11/2018 5:36:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AssignedTraining](
	[AssignedTrainingID] [int] IDENTITY(1,1) NOT NULL,
	[Training] [int] NOT NULL,
	[Profile] [int] NOT NULL,
	[Status] [tinyint] NOT NULL,
 CONSTRAINT [PK_AssignedTraining] PRIMARY KEY CLUSTERED 
(
	[AssignedTrainingID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Attendance]    Script Date: 08/11/2018 5:36:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Attendance](
	[AttendanceID] [int] IDENTITY(0,1) NOT NULL,
	[Employee] [int] NOT NULL,
	[TotalWorkingDays] [numeric](18, 0) NOT NULL,
	[Present] [numeric](18, 0) NOT NULL,
	[Absent] [numeric](18, 0) NOT NULL,
	[Overtime] [numeric](18, 0) NOT NULL,
	[Late] [numeric](18, 0) NOT NULL,
	[Undertime] [numeric](18, 0) NOT NULL,
	[Date] [date] NOT NULL,
	[Leave] [numeric](18, 0) NOT NULL,
	[HalfDays] [numeric](18, 0) NOT NULL,
 CONSTRAINT [PK_Attendance] PRIMARY KEY CLUSTERED 
(
	[AttendanceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AttendanceTime]    Script Date: 08/11/2018 5:36:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AttendanceTime](
	[AttendanceTimeID] [int] IDENTITY(0,1) NOT NULL,
	[TimeIn] [datetime] NULL,
	[TimeOut] [datetime] NULL,
	[Date] [date] NOT NULL,
	[Attendance] [int] NOT NULL,
 CONSTRAINT [PK_AttendanceTime] PRIMARY KEY CLUSTERED 
(
	[AttendanceTimeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Branch]    Script Date: 08/11/2018 5:36:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Branch](
	[BranchID] [int] IDENTITY(0,1) NOT NULL,
	[BranchName] [varchar](20) NOT NULL,
	[BranchVP] [int] NULL,
	[Type] [tinyint] NOT NULL,
 CONSTRAINT [PK_Branch] PRIMARY KEY CLUSTERED 
(
	[BranchID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Department]    Script Date: 08/11/2018 5:36:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Department](
	[DepartmentID] [int] IDENTITY(0,1) NOT NULL,
	[DepartmentName] [varchar](50) NOT NULL,
	[DepartmentHead] [int] NULL,
	[Branch] [int] NOT NULL,
	[Type] [int] NULL,
 CONSTRAINT [PK_Department] PRIMARY KEY CLUSTERED 
(
	[DepartmentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EducationalBackground]    Script Date: 08/11/2018 5:36:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EducationalBackground](
	[EducationID] [int] IDENTITY(0,1) NOT NULL,
	[Elementary] [nvarchar](30) NULL,
	[ElemStartYear] [varchar](25) NULL,
	[ElemEndYear] [varchar](25) NULL,
	[ElemAddress] [nvarchar](50) NULL,
	[HighSchool] [nvarchar](30) NULL,
	[HSStartYear] [varchar](25) NULL,
	[HSEndYear] [varchar](25) NULL,
	[HSAddress] [nvarchar](50) NULL,
	[College] [nvarchar](30) NULL,
	[CollegeStartYear] [varchar](25) NULL,
	[CollegeEndYear] [varchar](25) NULL,
	[CollegeAddress] [nvarchar](50) NULL,
	[PostGrad] [nvarchar](50) NULL,
	[PostGradStartYear] [varchar](25) NULL,
	[PostGradEndYear] [varchar](25) NULL,
	[PostGradAddress] [nvarchar](50) NULL,
 CONSTRAINT [PK_EducationalBackground] PRIMARY KEY CLUSTERED 
(
	[EducationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Employee]    Script Date: 08/11/2018 5:36:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Employee](
	[EmployeeID] [int] IDENTITY(0,1) NOT NULL,
	[Profile] [int] NOT NULL,
	[Department] [int] NULL,
	[EmploymentDate] [date] NOT NULL,
	[DateInactive] [date] NULL,
	[Status] [tinyint] NOT NULL,
	[Branch] [int] NULL,
	[Code] [varchar](50) NULL,
	[Position] [text] NULL,
 CONSTRAINT [PK_Employee] PRIMARY KEY CLUSTERED 
(
	[EmployeeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EmploymentHistory]    Script Date: 08/11/2018 5:36:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EmploymentHistory](
	[EmploymentHistoryID] [int] IDENTITY(0,1) NOT NULL,
	[Profile] [int] NOT NULL,
	[CompanyName] [nvarchar](50) NOT NULL,
	[Address] [nvarchar](50) NOT NULL,
	[Position] [varchar](20) NOT NULL,
	[StartDate] [varchar](15) NOT NULL,
	[EndDate] [varchar](15) NOT NULL,
	[LeavingReason] [text] NOT NULL,
	[ContactName] [nvarchar](50) NOT NULL,
	[ContactNo] [numeric](18, 0) NOT NULL,
 CONSTRAINT [PK_EmploymentHistory] PRIMARY KEY CLUSTERED 
(
	[EmploymentHistoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[JobPosting]    Script Date: 08/11/2018 5:36:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[JobPosting](
	[PostingID] [int] IDENTITY(0,1) NOT NULL,
	[JobTitle] [varchar](50) NOT NULL,
	[JobDescription] [text] NOT NULL,
	[Requirements] [text] NOT NULL,
	[DatePosted] [datetime] NOT NULL,
 CONSTRAINT [PK_JobPosting] PRIMARY KEY CLUSTERED 
(
	[PostingID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Leave]    Script Date: 08/11/2018 5:36:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Leave](
	[LeaveID] [int] IDENTITY(0,1) NOT NULL,
	[Employee] [int] NOT NULL,
	[StartDate] [date] NOT NULL,
	[EndDate] [date] NOT NULL,
	[Reason] [text] NOT NULL,
	[Status] [tinyint] NOT NULL,
	[Type] [tinyint] NOT NULL,
 CONSTRAINT [PK_Leave] PRIMARY KEY CLUSTERED 
(
	[LeaveID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Notification]    Script Date: 08/11/2018 5:36:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Notification](
	[NotificationID] [int] IDENTITY(1,1) NOT NULL,
	[Account] [int] NOT NULL,
	[Message] [text] NOT NULL,
	[Timestamp] [datetime] NOT NULL,
	[Status] [tinyint] NULL,
 CONSTRAINT [PK_Notification] PRIMARY KEY CLUSTERED 
(
	[NotificationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Profile]    Script Date: 08/11/2018 5:36:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Profile](
	[ProfileID] [int] IDENTITY(0,1) NOT NULL,
	[FirstName] [varchar](50) NOT NULL,
	[LastName] [varchar](50) NOT NULL,
	[MiddleName] [varchar](50) NOT NULL,
	[Education] [int] NULL,
	[Birthdate] [date] NOT NULL,
	[CivilStatus] [tinyint] NOT NULL,
	[Sex] [tinyint] NOT NULL,
	[Contact] [numeric](18, 0) NULL,
	[ContactPerson] [varchar](50) NOT NULL,
	[CPersonNo] [numeric](18, 0) NOT NULL,
	[CPersonRel] [nvarchar](20) NOT NULL,
	[HouseNo] [nvarchar](20) NOT NULL,
	[Street] [nvarchar](20) NOT NULL,
	[City] [nvarchar](20) NOT NULL,
	[Province] [nvarchar](20) NOT NULL,
 CONSTRAINT [PK_Profile] PRIMARY KEY CLUSTERED 
(
	[ProfileID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RequisitionForm]    Script Date: 08/11/2018 5:36:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RequisitionForm](
	[RequisitionID] [int] IDENTITY(0,1) NOT NULL,
	[Date] [date] NOT NULL,
	[Department] [int] NOT NULL,
	[Position] [varchar](20) NOT NULL,
	[RequestedBy] [int] NOT NULL,
	[ReasonforVacancy] [varchar](200) NOT NULL,
	[Type] [varchar](20) NOT NULL,
	[Qualification] [varchar](200) NOT NULL,
	[ExperienceRequired] [varchar](200) NOT NULL,
	[SkillsRequired] [varchar](200) NOT NULL,
	[ExpectedJoiningDate] [date] NOT NULL,
	[UnderSupervision] [int] NOT NULL,
	[BriefDescriptionofWorks] [varchar](200) NOT NULL,
	[Status] [tinyint] NOT NULL,
 CONSTRAINT [PK_RequisitionForm] PRIMARY KEY CLUSTERED 
(
	[RequisitionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TrainingHistory]    Script Date: 08/11/2018 5:36:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TrainingHistory](
	[TrainingHistoryID] [int] IDENTITY(0,1) NOT NULL,
	[Title] [varchar](50) NOT NULL,
	[Description] [text] NOT NULL,
	[Facilitator] [varchar](50) NOT NULL,
	[StartDate] [date] NOT NULL,
	[EndDate] [date] NOT NULL,
	[Location] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_TrainingHistory] PRIMARY KEY CLUSTERED 
(
	[TrainingHistoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Account] ON 

INSERT [dbo].[Account] ([AccountID], [Username], [Password], [Email], [Profile], [Type], [Locked], [Security], [Answer], [Image]) VALUES (0, N'testCSVP', N'123', N'test@email.com', 8, 2, 0, N'What is your birthday?', N' ', N'~/Content/img/uploads/default.png')
INSERT [dbo].[Account] ([AccountID], [Username], [Password], [Email], [Profile], [Type], [Locked], [Security], [Answer], [Image]) VALUES (1, N'testAVP', N'123', N'test@email.com', 4, 2, 0, N'What is your birthday?', N' ', N'~/Content/img/uploads/default.png')
INSERT [dbo].[Account] ([AccountID], [Username], [Password], [Email], [Profile], [Type], [Locked], [Security], [Answer], [Image]) VALUES (2, N'testSMVP', N'123', N'test@email.com', 5, 2, 0, N'What is your birthday?', N' ', N'~/Content/img/uploads/default.png')
INSERT [dbo].[Account] ([AccountID], [Username], [Password], [Email], [Profile], [Type], [Locked], [Security], [Answer], [Image]) VALUES (3, N'testOVP', N'123', N'test@email.com', 6, 2, 0, N'What is your birthday?', N' ', N'~/Content/img/uploads/default.png')
INSERT [dbo].[Account] ([AccountID], [Username], [Password], [Email], [Profile], [Type], [Locked], [Security], [Answer], [Image]) VALUES (4, N'testFVP', N'123', N'test@email.com', 7, 2, 0, N'What is your birthday?', N' ', N'~/Content/img/uploads/default.png')
INSERT [dbo].[Account] ([AccountID], [Username], [Password], [Email], [Profile], [Type], [Locked], [Security], [Answer], [Image]) VALUES (5, N'testCEO', N'123', N'test@email.com', 3, 1, 0, N'What is your birthday?', N' ', N'~/Content/img/uploads/default.png')
INSERT [dbo].[Account] ([AccountID], [Username], [Password], [Email], [Profile], [Type], [Locked], [Security], [Answer], [Image]) VALUES (6, N'testHRHead', N'123', N'test@test.com', 9, 3, 0, N'What is your birthday?', N' ', N'~/Content/img/uploads/default.png')
INSERT [dbo].[Account] ([AccountID], [Username], [Password], [Email], [Profile], [Type], [Locked], [Security], [Answer], [Image]) VALUES (11, N'testEmployee', N'123', N'employee@email.com', 20, 4, 0, N'What is your birthday?', N' ', N'~/Content/img/uploads/default.png')
INSERT [dbo].[Account] ([AccountID], [Username], [Password], [Email], [Profile], [Type], [Locked], [Security], [Answer], [Image]) VALUES (12, N'testApplicant', N'123', N'applicant@app.com', 21, 4, 0, N'What is your birthday?', N' ', N'~/Content/img/uploads/default.png')
INSERT [dbo].[Account] ([AccountID], [Username], [Password], [Email], [Profile], [Type], [Locked], [Security], [Answer], [Image]) VALUES (13, N'testHREmployee', N'123', N'123@23mail.com', 23, 4, 0, N'What is your birthday?', N' ', N'~/Content/img/uploads/default.png')
INSERT [dbo].[Account] ([AccountID], [Username], [Password], [Email], [Profile], [Type], [Locked], [Security], [Answer], [Image]) VALUES (14, N'testTaxHead', N'123', N'employee@email.com', 24, 3, 0, N'What is your birthday?', N' ', N'~/Content/img/uploads/default.png')
INSERT [dbo].[Account] ([AccountID], [Username], [Password], [Email], [Profile], [Type], [Locked], [Security], [Answer], [Image]) VALUES (15, N'newApp', N'123', N'employee@email.com', 29, 4, 0, N'What is your birthday?', N' ', N'~/Content/img/uploads/default.png')
INSERT [dbo].[Account] ([AccountID], [Username], [Password], [Email], [Profile], [Type], [Locked], [Security], [Answer], [Image]) VALUES (16, N'testApplicant2', N'asd', N'asd@emai.com', 33, 4, 0, N'asd', N'asd', N'~/Content/img/uploads/default.png')
INSERT [dbo].[Account] ([AccountID], [Username], [Password], [Email], [Profile], [Type], [Locked], [Security], [Answer], [Image]) VALUES (1016, N'migq', N'123', N'employee@email.com', 1030, 4, 0, N'Test Security', N'Test Answer', N'~/Content/img/uploads/default.png')
INSERT [dbo].[Account] ([AccountID], [Username], [Password], [Email], [Profile], [Type], [Locked], [Security], [Answer], [Image]) VALUES (1017, N'applicant', N'applicant', N'applicant@email.com', 1036, 5, 0, N'applicant', N'applicant', N'~/Content/img/uploads/UXSocietyLogo_main-gradient-full-2018-11-08-01-13.png')
SET IDENTITY_INSERT [dbo].[Account] OFF
SET IDENTITY_INSERT [dbo].[Applicant] ON 

INSERT [dbo].[Applicant] ([ApplicantID], [Profile], [Skills], [DesiredPosition], [Status]) VALUES (3, 21, N'Carpentry', N'Carpenter', 2)
INSERT [dbo].[Applicant] ([ApplicantID], [Profile], [Skills], [DesiredPosition], [Status]) VALUES (4, 29, N'Sad sadder sadderson', N'Sad', 2)
INSERT [dbo].[Applicant] ([ApplicantID], [Profile], [Skills], [DesiredPosition], [Status]) VALUES (5, 33, N'asd', N'asd', 2)
INSERT [dbo].[Applicant] ([ApplicantID], [Profile], [Skills], [DesiredPosition], [Status]) VALUES (1012, 1036, N'applicant', N'applicant', 1)
SET IDENTITY_INSERT [dbo].[Applicant] OFF
SET IDENTITY_INSERT [dbo].[Appraisal] ON 

INSERT [dbo].[Appraisal] ([AppraisalID], [CoveredPeriod], [Criteria], [Rating], [TechComp], [InterSkills], [CommComp], [Total], [Comments], [Evaluator], [DatePrepared], [NotedBy], [DateNoted], [DiscussedWith], [DateDiscussed], [Type], [Status]) VALUES (0, CAST(N'2018-01-01' AS Date), N'asd', CAST(0 AS Numeric(10, 0)), CAST(20 AS Numeric(10, 0)), CAST(20 AS Numeric(10, 0)), CAST(20 AS Numeric(10, 0)), CAST(20 AS Numeric(10, 0)), N'asdfasdf', 9, CAST(N'2018-11-04' AS Date), 4, CAST(N'2018-11-04' AS Date), 20, CAST(N'2018-11-04' AS Date), 2, 2)
INSERT [dbo].[Appraisal] ([AppraisalID], [CoveredPeriod], [Criteria], [Rating], [TechComp], [InterSkills], [CommComp], [Total], [Comments], [Evaluator], [DatePrepared], [NotedBy], [DateNoted], [DiscussedWith], [DateDiscussed], [Type], [Status]) VALUES (1, CAST(N'2018-01-01' AS Date), N'Test Supervisory Appraisal Criteria', CAST(0 AS Numeric(10, 0)), CAST(40 AS Numeric(10, 0)), CAST(29 AS Numeric(10, 0)), CAST(55 AS Numeric(10, 0)), CAST(39 AS Numeric(10, 0)), N'Test Supervisory Comment', 3, CAST(N'2018-11-05' AS Date), 3, CAST(N'2018-11-05' AS Date), 4, NULL, 1, 1)
SET IDENTITY_INSERT [dbo].[Appraisal] OFF
SET IDENTITY_INSERT [dbo].[AssignedTraining] ON 

INSERT [dbo].[AssignedTraining] ([AssignedTrainingID], [Training], [Profile], [Status]) VALUES (1, 2, 20, 1)
INSERT [dbo].[AssignedTraining] ([AssignedTrainingID], [Training], [Profile], [Status]) VALUES (2, 0, 4, 1)
SET IDENTITY_INSERT [dbo].[AssignedTraining] OFF
SET IDENTITY_INSERT [dbo].[Attendance] ON 

INSERT [dbo].[Attendance] ([AttendanceID], [Employee], [TotalWorkingDays], [Present], [Absent], [Overtime], [Late], [Undertime], [Date], [Leave], [HalfDays]) VALUES (21, 9, CAST(20 AS Numeric(18, 0)), CAST(0 AS Numeric(18, 0)), CAST(0 AS Numeric(18, 0)), CAST(0 AS Numeric(18, 0)), CAST(0 AS Numeric(18, 0)), CAST(0 AS Numeric(18, 0)), CAST(N'2018-11-06' AS Date), CAST(0 AS Numeric(18, 0)), CAST(0 AS Numeric(18, 0)))
INSERT [dbo].[Attendance] ([AttendanceID], [Employee], [TotalWorkingDays], [Present], [Absent], [Overtime], [Late], [Undertime], [Date], [Leave], [HalfDays]) VALUES (22, 14, CAST(20 AS Numeric(18, 0)), CAST(0 AS Numeric(18, 0)), CAST(0 AS Numeric(18, 0)), CAST(0 AS Numeric(18, 0)), CAST(0 AS Numeric(18, 0)), CAST(0 AS Numeric(18, 0)), CAST(N'2018-11-06' AS Date), CAST(0 AS Numeric(18, 0)), CAST(0 AS Numeric(18, 0)))
INSERT [dbo].[Attendance] ([AttendanceID], [Employee], [TotalWorkingDays], [Present], [Absent], [Overtime], [Late], [Undertime], [Date], [Leave], [HalfDays]) VALUES (23, 17, CAST(20 AS Numeric(18, 0)), CAST(0 AS Numeric(18, 0)), CAST(0 AS Numeric(18, 0)), CAST(0 AS Numeric(18, 0)), CAST(1 AS Numeric(18, 0)), CAST(0 AS Numeric(18, 0)), CAST(N'2018-11-06' AS Date), CAST(0 AS Numeric(18, 0)), CAST(0 AS Numeric(18, 0)))
INSERT [dbo].[Attendance] ([AttendanceID], [Employee], [TotalWorkingDays], [Present], [Absent], [Overtime], [Late], [Undertime], [Date], [Leave], [HalfDays]) VALUES (24, 20, CAST(20 AS Numeric(18, 0)), CAST(0 AS Numeric(18, 0)), CAST(0 AS Numeric(18, 0)), CAST(0 AS Numeric(18, 0)), CAST(0 AS Numeric(18, 0)), CAST(0 AS Numeric(18, 0)), CAST(N'2018-11-06' AS Date), CAST(0 AS Numeric(18, 0)), CAST(0 AS Numeric(18, 0)))
INSERT [dbo].[Attendance] ([AttendanceID], [Employee], [TotalWorkingDays], [Present], [Absent], [Overtime], [Late], [Undertime], [Date], [Leave], [HalfDays]) VALUES (25, 21, CAST(20 AS Numeric(18, 0)), CAST(0 AS Numeric(18, 0)), CAST(0 AS Numeric(18, 0)), CAST(0 AS Numeric(18, 0)), CAST(0 AS Numeric(18, 0)), CAST(0 AS Numeric(18, 0)), CAST(N'2018-11-06' AS Date), CAST(1 AS Numeric(18, 0)), CAST(0 AS Numeric(18, 0)))
INSERT [dbo].[Attendance] ([AttendanceID], [Employee], [TotalWorkingDays], [Present], [Absent], [Overtime], [Late], [Undertime], [Date], [Leave], [HalfDays]) VALUES (26, 22, CAST(20 AS Numeric(18, 0)), CAST(0 AS Numeric(18, 0)), CAST(0 AS Numeric(18, 0)), CAST(0 AS Numeric(18, 0)), CAST(0 AS Numeric(18, 0)), CAST(0 AS Numeric(18, 0)), CAST(N'2018-11-06' AS Date), CAST(0 AS Numeric(18, 0)), CAST(0 AS Numeric(18, 0)))
INSERT [dbo].[Attendance] ([AttendanceID], [Employee], [TotalWorkingDays], [Present], [Absent], [Overtime], [Late], [Undertime], [Date], [Leave], [HalfDays]) VALUES (27, 23, CAST(20 AS Numeric(18, 0)), CAST(0 AS Numeric(18, 0)), CAST(0 AS Numeric(18, 0)), CAST(0 AS Numeric(18, 0)), CAST(0 AS Numeric(18, 0)), CAST(0 AS Numeric(18, 0)), CAST(N'2018-11-06' AS Date), CAST(0 AS Numeric(18, 0)), CAST(0 AS Numeric(18, 0)))
SET IDENTITY_INSERT [dbo].[Attendance] OFF
SET IDENTITY_INSERT [dbo].[AttendanceTime] ON 

INSERT [dbo].[AttendanceTime] ([AttendanceTimeID], [TimeIn], [TimeOut], [Date], [Attendance]) VALUES (1001, CAST(N'2018-11-07T02:00:35.790' AS DateTime), NULL, CAST(N'2018-11-07' AS Date), 22)
SET IDENTITY_INSERT [dbo].[AttendanceTime] OFF
SET IDENTITY_INSERT [dbo].[Branch] ON 

INSERT [dbo].[Branch] ([BranchID], [BranchName], [BranchVP], [Type]) VALUES (1, N'Corporate Services', 8, 1)
INSERT [dbo].[Branch] ([BranchID], [BranchName], [BranchVP], [Type]) VALUES (2, N'Administration', 4, 2)
INSERT [dbo].[Branch] ([BranchID], [BranchName], [BranchVP], [Type]) VALUES (3, N'Sales & Marketing', 5, 3)
INSERT [dbo].[Branch] ([BranchID], [BranchName], [BranchVP], [Type]) VALUES (4, N'Operations', 6, 4)
INSERT [dbo].[Branch] ([BranchID], [BranchName], [BranchVP], [Type]) VALUES (5, N'Finance', 7, 5)
INSERT [dbo].[Branch] ([BranchID], [BranchName], [BranchVP], [Type]) VALUES (6, N'None', 3, 6)
SET IDENTITY_INSERT [dbo].[Branch] OFF
SET IDENTITY_INSERT [dbo].[Department] ON 

INSERT [dbo].[Department] ([DepartmentID], [DepartmentName], [DepartmentHead], [Branch], [Type]) VALUES (1, N'Special Projects', NULL, 1, 0)
INSERT [dbo].[Department] ([DepartmentID], [DepartmentName], [DepartmentHead], [Branch], [Type]) VALUES (2, N'None', NULL, 6, 18)
INSERT [dbo].[Department] ([DepartmentID], [DepartmentName], [DepartmentHead], [Branch], [Type]) VALUES (4, N'Internal Auditing', NULL, 1, 1)
INSERT [dbo].[Department] ([DepartmentID], [DepartmentName], [DepartmentHead], [Branch], [Type]) VALUES (5, N'Tax Matters', NULL, 1, 2)
INSERT [dbo].[Department] ([DepartmentID], [DepartmentName], [DepartmentHead], [Branch], [Type]) VALUES (6, N'Paralegal', NULL, 1, 3)
INSERT [dbo].[Department] ([DepartmentID], [DepartmentName], [DepartmentHead], [Branch], [Type]) VALUES (7, N'Corporate Services Branch', 8, 1, 19)
INSERT [dbo].[Department] ([DepartmentID], [DepartmentName], [DepartmentHead], [Branch], [Type]) VALUES (8, N'Administration Branch', 4, 2, 20)
INSERT [dbo].[Department] ([DepartmentID], [DepartmentName], [DepartmentHead], [Branch], [Type]) VALUES (9, N'Sales & Marketing Branch', 5, 3, 21)
INSERT [dbo].[Department] ([DepartmentID], [DepartmentName], [DepartmentHead], [Branch], [Type]) VALUES (10, N'Operations Branch', 6, 4, 22)
INSERT [dbo].[Department] ([DepartmentID], [DepartmentName], [DepartmentHead], [Branch], [Type]) VALUES (11, N'Finance Branch', 7, 5, 23)
INSERT [dbo].[Department] ([DepartmentID], [DepartmentName], [DepartmentHead], [Branch], [Type]) VALUES (14, N'Human Resources', 9, 2, 4)
INSERT [dbo].[Department] ([DepartmentID], [DepartmentName], [DepartmentHead], [Branch], [Type]) VALUES (15, N'Social/Environment', NULL, 2, 5)
INSERT [dbo].[Department] ([DepartmentID], [DepartmentName], [DepartmentHead], [Branch], [Type]) VALUES (16, N'General Service', NULL, 2, 6)
INSERT [dbo].[Department] ([DepartmentID], [DepartmentName], [DepartmentHead], [Branch], [Type]) VALUES (17, N'IT', NULL, 2, 7)
INSERT [dbo].[Department] ([DepartmentID], [DepartmentName], [DepartmentHead], [Branch], [Type]) VALUES (18, N'Security', NULL, 2, 8)
INSERT [dbo].[Department] ([DepartmentID], [DepartmentName], [DepartmentHead], [Branch], [Type]) VALUES (19, N'Sales & Marketing', NULL, 3, 9)
INSERT [dbo].[Department] ([DepartmentID], [DepartmentName], [DepartmentHead], [Branch], [Type]) VALUES (20, N'Quality Assurance', NULL, 3, 10)
INSERT [dbo].[Department] ([DepartmentID], [DepartmentName], [DepartmentHead], [Branch], [Type]) VALUES (21, N'Shipping', NULL, 3, 11)
INSERT [dbo].[Department] ([DepartmentID], [DepartmentName], [DepartmentHead], [Branch], [Type]) VALUES (22, N'Research & Development', NULL, 3, 12)
INSERT [dbo].[Department] ([DepartmentID], [DepartmentName], [DepartmentHead], [Branch], [Type]) VALUES (23, N'Production Planning', NULL, 4, 13)
INSERT [dbo].[Department] ([DepartmentID], [DepartmentName], [DepartmentHead], [Branch], [Type]) VALUES (24, N'Production Furniture & Accessory', NULL, 4, 14)
INSERT [dbo].[Department] ([DepartmentID], [DepartmentName], [DepartmentHead], [Branch], [Type]) VALUES (25, N'Logistics', NULL, 4, 15)
INSERT [dbo].[Department] ([DepartmentID], [DepartmentName], [DepartmentHead], [Branch], [Type]) VALUES (26, N'Accounting', NULL, 5, 16)
INSERT [dbo].[Department] ([DepartmentID], [DepartmentName], [DepartmentHead], [Branch], [Type]) VALUES (27, N'Finance', NULL, 5, 17)
SET IDENTITY_INSERT [dbo].[Department] OFF
SET IDENTITY_INSERT [dbo].[EducationalBackground] ON 

INSERT [dbo].[EducationalBackground] ([EducationID], [Elementary], [ElemStartYear], [ElemEndYear], [ElemAddress], [HighSchool], [HSStartYear], [HSEndYear], [HSAddress], [College], [CollegeStartYear], [CollegeEndYear], [CollegeAddress], [PostGrad], [PostGradStartYear], [PostGradEndYear], [PostGradAddress]) VALUES (13, N'Don Bosco Technical College', N'2011', N'2008', N'Mandaluyong', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[EducationalBackground] ([EducationID], [Elementary], [ElemStartYear], [ElemEndYear], [ElemAddress], [HighSchool], [HSStartYear], [HSEndYear], [HSAddress], [College], [CollegeStartYear], [CollegeEndYear], [CollegeAddress], [PostGrad], [PostGradStartYear], [PostGradEndYear], [PostGradAddress]) VALUES (18, N'Don Bosco Technical College', N'2006', N'2010', N'Mandaluyong', N'Don Bosco Technical College', N'2011', N'2014', N'Mandaluyong', N'De La Salle University', N'2015', N'2018', N'Taft', N'', N'', N'', N'')
INSERT [dbo].[EducationalBackground] ([EducationID], [Elementary], [ElemStartYear], [ElemEndYear], [ElemAddress], [HighSchool], [HSStartYear], [HSEndYear], [HSAddress], [College], [CollegeStartYear], [CollegeEndYear], [CollegeAddress], [PostGrad], [PostGradStartYear], [PostGradEndYear], [PostGradAddress]) VALUES (19, N'Don Bosco Technical College', N'2008', N'2011', N'Mandaluyong', N'Don Bosco Technical College', N'2011', N'2014', N'Mandaluyong', N'De La Salle University', N'2014', N'2019', N'Taft', NULL, NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[EducationalBackground] OFF
SET IDENTITY_INSERT [dbo].[Employee] ON 

INSERT [dbo].[Employee] ([EmployeeID], [Profile], [Department], [EmploymentDate], [DateInactive], [Status], [Branch], [Code], [Position]) VALUES (3, 3, 2, CAST(N'1998-03-03' AS Date), CAST(N'2018-10-29' AS Date), 1, 6, N'0001', N'CEO')
INSERT [dbo].[Employee] ([EmployeeID], [Profile], [Department], [EmploymentDate], [DateInactive], [Status], [Branch], [Code], [Position]) VALUES (4, 4, 8, CAST(N'1998-03-31' AS Date), NULL, 1, 2, N'0002', N'VP - Administration')
INSERT [dbo].[Employee] ([EmployeeID], [Profile], [Department], [EmploymentDate], [DateInactive], [Status], [Branch], [Code], [Position]) VALUES (5, 5, 9, CAST(N'1998-03-31' AS Date), NULL, 1, 3, N'0003', N'VP - Sales & Marketing')
INSERT [dbo].[Employee] ([EmployeeID], [Profile], [Department], [EmploymentDate], [DateInactive], [Status], [Branch], [Code], [Position]) VALUES (6, 6, 10, CAST(N'1998-03-31' AS Date), NULL, 1, 4, N'0004', N'VP - Operations')
INSERT [dbo].[Employee] ([EmployeeID], [Profile], [Department], [EmploymentDate], [DateInactive], [Status], [Branch], [Code], [Position]) VALUES (7, 7, 11, CAST(N'1998-03-31' AS Date), NULL, 1, 5, N'0005', N'VP - Finance')
INSERT [dbo].[Employee] ([EmployeeID], [Profile], [Department], [EmploymentDate], [DateInactive], [Status], [Branch], [Code], [Position]) VALUES (8, 8, 7, CAST(N'1998-03-03' AS Date), CAST(N'9999-12-31' AS Date), 1, 1, N'0006', N'VP - Corporate Service')
INSERT [dbo].[Employee] ([EmployeeID], [Profile], [Department], [EmploymentDate], [DateInactive], [Status], [Branch], [Code], [Position]) VALUES (9, 9, 14, CAST(N'1998-03-31' AS Date), CAST(N'9999-12-31' AS Date), 1, 2, N'0007', N'HR Department Head')
INSERT [dbo].[Employee] ([EmployeeID], [Profile], [Department], [EmploymentDate], [DateInactive], [Status], [Branch], [Code], [Position]) VALUES (14, 20, 23, CAST(N'2003-03-04' AS Date), CAST(N'9999-12-31' AS Date), 1, NULL, N'E1029123', N'Carpenter')
INSERT [dbo].[Employee] ([EmployeeID], [Profile], [Department], [EmploymentDate], [DateInactive], [Status], [Branch], [Code], [Position]) VALUES (17, 21, 21, CAST(N'2018-11-01' AS Date), CAST(N'9999-12-31' AS Date), 1, NULL, N'babc46f7-74fe-4a50-be1d-0571f85a709a', N'Driver')
INSERT [dbo].[Employee] ([EmployeeID], [Profile], [Department], [EmploymentDate], [DateInactive], [Status], [Branch], [Code], [Position]) VALUES (20, 23, 14, CAST(N'1998-03-31' AS Date), CAST(N'0001-01-01' AS Date), 1, NULL, N'E102931', N'HR Employee')
INSERT [dbo].[Employee] ([EmployeeID], [Profile], [Department], [EmploymentDate], [DateInactive], [Status], [Branch], [Code], [Position]) VALUES (21, 24, 5, CAST(N'2006-01-13' AS Date), CAST(N'0001-01-01' AS Date), 1, NULL, N'DH10293', N'Tax Matters Head')
INSERT [dbo].[Employee] ([EmployeeID], [Profile], [Department], [EmploymentDate], [DateInactive], [Status], [Branch], [Code], [Position]) VALUES (22, 29, 14, CAST(N'2018-11-03' AS Date), CAST(N'9999-12-31' AS Date), 1, NULL, N'1fb7f1fe-3776-41c9-ace4-b733e091600a', N'HR Employee')
INSERT [dbo].[Employee] ([EmployeeID], [Profile], [Department], [EmploymentDate], [DateInactive], [Status], [Branch], [Code], [Position]) VALUES (23, 33, 2, CAST(N'2018-11-04' AS Date), CAST(N'0001-01-01' AS Date), 0, NULL, N'c86fca0f-c954-4c79-9799-a8f59407ca9c', N'New Employee')
INSERT [dbo].[Employee] ([EmployeeID], [Profile], [Department], [EmploymentDate], [DateInactive], [Status], [Branch], [Code], [Position]) VALUES (1022, 1030, 16, CAST(N'2018-11-07' AS Date), NULL, 1, NULL, N'E10293', N'Head Janitor')
SET IDENTITY_INSERT [dbo].[Employee] OFF
SET IDENTITY_INSERT [dbo].[EmploymentHistory] ON 

INSERT [dbo].[EmploymentHistory] ([EmploymentHistoryID], [Profile], [CompanyName], [Address], [Position], [StartDate], [EndDate], [LeavingReason], [ContactName], [ContactNo]) VALUES (0, 20, N'Sample Company', N'Sample Address', N'Sample Position', N'2018', N'2017', N'asd', N'Sample Sampleson', CAST(9174277190 AS Numeric(18, 0)))
INSERT [dbo].[EmploymentHistory] ([EmploymentHistoryID], [Profile], [CompanyName], [Address], [Position], [StartDate], [EndDate], [LeavingReason], [ContactName], [ContactNo]) VALUES (1, 20, N'Sample Company 2', N'Sample Address 2', N'Sample Position 2', N'2006', N'2010', N'asdasd', N'Sample McSampleson', CAST(9178510533 AS Numeric(18, 0)))
INSERT [dbo].[EmploymentHistory] ([EmploymentHistoryID], [Profile], [CompanyName], [Address], [Position], [StartDate], [EndDate], [LeavingReason], [ContactName], [ContactNo]) VALUES (2, 29, N'Sample Company', N'Sample Address 2', N'Sample Position', N'2017', N'2018', N'Reasons why i left', N'Sample McSampleson', CAST(9174277190 AS Numeric(18, 0)))
SET IDENTITY_INSERT [dbo].[EmploymentHistory] OFF
SET IDENTITY_INSERT [dbo].[JobPosting] ON 

INSERT [dbo].[JobPosting] ([PostingID], [JobTitle], [JobDescription], [Requirements], [DatePosted]) VALUES (1, N'Test Job Post #1', N'testing job post #1', N'no requirements', CAST(N'2018-10-30T01:13:19.053' AS DateTime))
INSERT [dbo].[JobPosting] ([PostingID], [JobTitle], [JobDescription], [Requirements], [DatePosted]) VALUES (2, N'testtest', N'asd', N'qwe', CAST(N'2005-09-13T00:00:00.000' AS DateTime))
SET IDENTITY_INSERT [dbo].[JobPosting] OFF
SET IDENTITY_INSERT [dbo].[Leave] ON 

INSERT [dbo].[Leave] ([LeaveID], [Employee], [StartDate], [EndDate], [Reason], [Status], [Type]) VALUES (1, 14, CAST(N'1998-03-31' AS Date), CAST(N'1998-04-05' AS Date), N'Need To Shit', 3, 1)
INSERT [dbo].[Leave] ([LeaveID], [Employee], [StartDate], [EndDate], [Reason], [Status], [Type]) VALUES (4, 14, CAST(N'2001-04-25' AS Date), CAST(N'2001-04-26' AS Date), N'Lol too lazy', 2, 1)
INSERT [dbo].[Leave] ([LeaveID], [Employee], [StartDate], [EndDate], [Reason], [Status], [Type]) VALUES (5, 9, CAST(N'2016-04-01' AS Date), CAST(N'2016-07-02' AS Date), N'Need to shit', 1, 1)
INSERT [dbo].[Leave] ([LeaveID], [Employee], [StartDate], [EndDate], [Reason], [Status], [Type]) VALUES (7, 17, CAST(N'2018-01-02' AS Date), CAST(N'2018-01-03' AS Date), N'Dunno', 3, 1)
INSERT [dbo].[Leave] ([LeaveID], [Employee], [StartDate], [EndDate], [Reason], [Status], [Type]) VALUES (1009, 17, CAST(N'2018-11-03' AS Date), CAST(N'2018-11-04' AS Date), N'test', 2, 1)
INSERT [dbo].[Leave] ([LeaveID], [Employee], [StartDate], [EndDate], [Reason], [Status], [Type]) VALUES (1010, 21, CAST(N'2018-11-07' AS Date), CAST(N'2018-11-08' AS Date), N'sad', 2, 1)
INSERT [dbo].[Leave] ([LeaveID], [Employee], [StartDate], [EndDate], [Reason], [Status], [Type]) VALUES (1011, 21, CAST(N'2018-11-09' AS Date), CAST(N'2018-11-17' AS Date), N'sadasd', 2, 1)
INSERT [dbo].[Leave] ([LeaveID], [Employee], [StartDate], [EndDate], [Reason], [Status], [Type]) VALUES (1012, 17, CAST(N'2018-11-03' AS Date), CAST(N'2018-11-06' AS Date), N'sasdqwe', 2, 1)
INSERT [dbo].[Leave] ([LeaveID], [Employee], [StartDate], [EndDate], [Reason], [Status], [Type]) VALUES (1013, 17, CAST(N'2018-11-06' AS Date), CAST(N'2018-11-11' AS Date), N'asdfqewr', 2, 1)
SET IDENTITY_INSERT [dbo].[Leave] OFF
SET IDENTITY_INSERT [dbo].[Notification] ON 

INSERT [dbo].[Notification] ([NotificationID], [Account], [Message], [Timestamp], [Status]) VALUES (1, 12, N'Info: You have been assigned to supervise an incoming manpower requisition.', CAST(N'2018-11-03T07:50:33.000' AS DateTime), 2)
INSERT [dbo].[Notification] ([NotificationID], [Account], [Message], [Timestamp], [Status]) VALUES (2, 6, N'A new requisition form was issued by the None department for the position: <b>Tax Person</b>', CAST(N'2018-11-03T07:50:34.000' AS DateTime), 2)
INSERT [dbo].[Notification] ([NotificationID], [Account], [Message], [Timestamp], [Status]) VALUES (3, 13, N'A new requisition form was issued by the None department for the position: <b>Tax Person</b>', CAST(N'2018-11-03T07:50:34.000' AS DateTime), 2)
INSERT [dbo].[Notification] ([NotificationID], [Account], [Message], [Timestamp], [Status]) VALUES (4, 6, N'Employee Middlename Lastname applied for a sick leave on 01/02/2018 - 04/14/2006', CAST(N'2018-11-03T08:35:34.000' AS DateTime), 2)
INSERT [dbo].[Notification] ([NotificationID], [Account], [Message], [Timestamp], [Status]) VALUES (5, 13, N'Employee Middlename Lastname applied for a sick leave on 01/02/2018 - 04/14/2006', CAST(N'2018-11-03T08:35:37.000' AS DateTime), 2)
INSERT [dbo].[Notification] ([NotificationID], [Account], [Message], [Timestamp], [Status]) VALUES (6, 15, N'Congratulations! You''ve been approved for employment, please wait for further information, you will be contacted through phone or email.', CAST(N'2018-11-03T08:44:12.000' AS DateTime), 2)
INSERT [dbo].[Notification] ([NotificationID], [Account], [Message], [Timestamp], [Status]) VALUES (7, 13, N'You have been evaluated by your superior, please check the results in My Evaluations and wait for further information...', CAST(N'2018-11-04T19:13:17.000' AS DateTime), 2)
INSERT [dbo].[Notification] ([NotificationID], [Account], [Message], [Timestamp], [Status]) VALUES (8, 12, N'<b>Info:</b> Your leave request #6 has been updated to: <i> Approved </i>', CAST(N'2018-11-04T21:16:03.493' AS DateTime), 0)
INSERT [dbo].[Notification] ([NotificationID], [Account], [Message], [Timestamp], [Status]) VALUES (9, 12, N'<b>Info:</b> Your leave request #6 has been updated to: <i> Denied </i>', CAST(N'2018-11-04T21:16:37.000' AS DateTime), 2)
INSERT [dbo].[Notification] ([NotificationID], [Account], [Message], [Timestamp], [Status]) VALUES (10, 16, N'Congratulations! You''ve been approved for employment, please wait for further information, you will be contacted through phone or email.', CAST(N'2018-11-04T23:20:34.000' AS DateTime), 2)
INSERT [dbo].[Notification] ([NotificationID], [Account], [Message], [Timestamp], [Status]) VALUES (11, 1, N'You have been evaluated by your superior, please check the results in My Evaluations and wait for further information...', CAST(N'2018-11-05T01:04:40.000' AS DateTime), 2)
INSERT [dbo].[Notification] ([NotificationID], [Account], [Message], [Timestamp], [Status]) VALUES (12, 6, N'Applicant Middlename Lastname applied for a sick leave on 11/06/2018 - 11/07/2018', CAST(N'2018-11-05T19:37:39.000' AS DateTime), 2)
INSERT [dbo].[Notification] ([NotificationID], [Account], [Message], [Timestamp], [Status]) VALUES (13, 13, N'Applicant Middlename Lastname applied for a sick leave on 11/06/2018 - 11/07/2018', CAST(N'2018-11-05T19:37:40.037' AS DateTime), 1)
INSERT [dbo].[Notification] ([NotificationID], [Account], [Message], [Timestamp], [Status]) VALUES (14, 15, N'Applicant Middlename Lastname applied for a sick leave on 11/06/2018 - 11/07/2018', CAST(N'2018-11-05T19:37:40.107' AS DateTime), 1)
INSERT [dbo].[Notification] ([NotificationID], [Account], [Message], [Timestamp], [Status]) VALUES (15, 12, N'<b>Info:</b> Your leave request #9 has been updated to: <i> Approved </i>', CAST(N'2018-11-05T19:38:04.000' AS DateTime), 2)
INSERT [dbo].[Notification] ([NotificationID], [Account], [Message], [Timestamp], [Status]) VALUES (16, 12, N'<b>Info:</b> Your leave request #6 has been updated to: <i> Denied </i>', CAST(N'2018-11-05T19:53:27.000' AS DateTime), 2)
INSERT [dbo].[Notification] ([NotificationID], [Account], [Message], [Timestamp], [Status]) VALUES (17, 12, N'<b>Info:</b> Your leave request #6 has been updated to: <i> Denied </i>', CAST(N'2018-11-05T19:57:24.000' AS DateTime), 2)
INSERT [dbo].[Notification] ([NotificationID], [Account], [Message], [Timestamp], [Status]) VALUES (18, 12, N'<b>Info:</b> Your leave request #6 has been updated to: <i> Denied </i>', CAST(N'2018-11-05T19:57:51.000' AS DateTime), 2)
INSERT [dbo].[Notification] ([NotificationID], [Account], [Message], [Timestamp], [Status]) VALUES (19, 12, N'<b>Info:</b> Your leave request #9 has been updated to: <i> Denied </i>', CAST(N'2018-11-05T20:00:50.000' AS DateTime), 2)
INSERT [dbo].[Notification] ([NotificationID], [Account], [Message], [Timestamp], [Status]) VALUES (1010, 6, N'Applicant Middlename Lastname applied for a sick leave on 11/03/2018 - 11/04/2018', CAST(N'2018-11-06T06:55:41.000' AS DateTime), 2)
INSERT [dbo].[Notification] ([NotificationID], [Account], [Message], [Timestamp], [Status]) VALUES (1011, 13, N'Applicant Middlename Lastname applied for a sick leave on 11/03/2018 - 11/04/2018', CAST(N'2018-11-06T06:55:41.983' AS DateTime), 1)
INSERT [dbo].[Notification] ([NotificationID], [Account], [Message], [Timestamp], [Status]) VALUES (1012, 15, N'Applicant Middlename Lastname applied for a sick leave on 11/03/2018 - 11/04/2018', CAST(N'2018-11-06T06:55:42.047' AS DateTime), 1)
INSERT [dbo].[Notification] ([NotificationID], [Account], [Message], [Timestamp], [Status]) VALUES (1013, 12, N'<b>Info:</b> Your leave request #1009 has been updated to: <i> Approved </i>', CAST(N'2018-11-06T06:55:58.033' AS DateTime), 1)
INSERT [dbo].[Notification] ([NotificationID], [Account], [Message], [Timestamp], [Status]) VALUES (1014, 6, N'Test Testing Testerson applied for a sick leave on 11/07/2018 - 11/08/2018', CAST(N'2018-11-06T06:59:52.000' AS DateTime), 2)
INSERT [dbo].[Notification] ([NotificationID], [Account], [Message], [Timestamp], [Status]) VALUES (1015, 13, N'Test Testing Testerson applied for a sick leave on 11/07/2018 - 11/08/2018', CAST(N'2018-11-06T06:59:52.667' AS DateTime), 1)
INSERT [dbo].[Notification] ([NotificationID], [Account], [Message], [Timestamp], [Status]) VALUES (1016, 15, N'Test Testing Testerson applied for a sick leave on 11/07/2018 - 11/08/2018', CAST(N'2018-11-06T06:59:52.700' AS DateTime), 1)
INSERT [dbo].[Notification] ([NotificationID], [Account], [Message], [Timestamp], [Status]) VALUES (1017, 14, N'<b>Info:</b> Your leave request #1010 has been updated to: <i> Approved </i>', CAST(N'2018-11-06T07:00:15.597' AS DateTime), 1)
INSERT [dbo].[Notification] ([NotificationID], [Account], [Message], [Timestamp], [Status]) VALUES (1018, 11, N'<b>Info:</b> Your leave request #1 has been updated to: <i> Denied </i>', CAST(N'2018-11-06T07:01:42.940' AS DateTime), 1)
INSERT [dbo].[Notification] ([NotificationID], [Account], [Message], [Timestamp], [Status]) VALUES (1019, 11, N'<b>Info:</b> Your leave request #8 has been updated to: <i> Denied </i>', CAST(N'2018-11-06T07:01:47.153' AS DateTime), 1)
INSERT [dbo].[Notification] ([NotificationID], [Account], [Message], [Timestamp], [Status]) VALUES (1020, 12, N'<b>Info:</b> Your leave request #7 has been updated to: <i> Denied </i>', CAST(N'2018-11-06T07:01:52.330' AS DateTime), 1)
INSERT [dbo].[Notification] ([NotificationID], [Account], [Message], [Timestamp], [Status]) VALUES (1021, 6, N'Test Testing Testerson applied for a sick leave on 11/09/2018 - 11/17/2018', CAST(N'2018-11-06T07:02:22.000' AS DateTime), 2)
INSERT [dbo].[Notification] ([NotificationID], [Account], [Message], [Timestamp], [Status]) VALUES (1022, 13, N'Test Testing Testerson applied for a sick leave on 11/09/2018 - 11/17/2018', CAST(N'2018-11-06T07:02:22.447' AS DateTime), 1)
INSERT [dbo].[Notification] ([NotificationID], [Account], [Message], [Timestamp], [Status]) VALUES (1023, 15, N'Test Testing Testerson applied for a sick leave on 11/09/2018 - 11/17/2018', CAST(N'2018-11-06T07:02:22.487' AS DateTime), 1)
INSERT [dbo].[Notification] ([NotificationID], [Account], [Message], [Timestamp], [Status]) VALUES (1024, 14, N'<b>Info:</b> Your leave request #1011 has been updated to: <i> Approved </i>', CAST(N'2018-11-06T07:02:35.267' AS DateTime), 1)
INSERT [dbo].[Notification] ([NotificationID], [Account], [Message], [Timestamp], [Status]) VALUES (1025, 6, N'Applicant Middlename Lastname applied for a sick leave on 11/03/2018 - 11/06/2018', CAST(N'2018-11-06T07:03:50.000' AS DateTime), 2)
INSERT [dbo].[Notification] ([NotificationID], [Account], [Message], [Timestamp], [Status]) VALUES (1026, 13, N'Applicant Middlename Lastname applied for a sick leave on 11/03/2018 - 11/06/2018', CAST(N'2018-11-06T07:03:50.467' AS DateTime), 1)
INSERT [dbo].[Notification] ([NotificationID], [Account], [Message], [Timestamp], [Status]) VALUES (1027, 15, N'Applicant Middlename Lastname applied for a sick leave on 11/03/2018 - 11/06/2018', CAST(N'2018-11-06T07:03:50.503' AS DateTime), 1)
INSERT [dbo].[Notification] ([NotificationID], [Account], [Message], [Timestamp], [Status]) VALUES (1028, 12, N'<b>Info:</b> Your leave request #1012 has been updated to: <i> Approved </i>', CAST(N'2018-11-06T07:04:53.910' AS DateTime), 1)
INSERT [dbo].[Notification] ([NotificationID], [Account], [Message], [Timestamp], [Status]) VALUES (1029, 6, N'Applicant Middlename Lastname applied for a sick leave on 11/06/2018 - 11/11/2018', CAST(N'2018-11-06T07:05:32.000' AS DateTime), 2)
INSERT [dbo].[Notification] ([NotificationID], [Account], [Message], [Timestamp], [Status]) VALUES (1030, 13, N'Applicant Middlename Lastname applied for a sick leave on 11/06/2018 - 11/11/2018', CAST(N'2018-11-06T07:05:32.213' AS DateTime), 1)
INSERT [dbo].[Notification] ([NotificationID], [Account], [Message], [Timestamp], [Status]) VALUES (1031, 15, N'Applicant Middlename Lastname applied for a sick leave on 11/06/2018 - 11/11/2018', CAST(N'2018-11-06T07:05:32.240' AS DateTime), 1)
INSERT [dbo].[Notification] ([NotificationID], [Account], [Message], [Timestamp], [Status]) VALUES (1032, 12, N'<b>Info:</b> Your leave request #1013 has been updated to: <i> Approved </i>', CAST(N'2018-11-06T07:05:55.967' AS DateTime), 1)
SET IDENTITY_INSERT [dbo].[Notification] OFF
SET IDENTITY_INSERT [dbo].[Profile] ON 

INSERT [dbo].[Profile] ([ProfileID], [FirstName], [LastName], [MiddleName], [Education], [Birthdate], [CivilStatus], [Sex], [Contact], [ContactPerson], [CPersonNo], [CPersonRel], [HouseNo], [Street], [City], [Province]) VALUES (3, N'CEO', N'Lastname', N'Middlename', NULL, CAST(N'1998-03-31' AS Date), 1, 1, CAST(9174726190 AS Numeric(18, 0)), N'CEO Mother', CAST(9178510533 AS Numeric(18, 0)), N'Mother', N'240', N'N Domingo', N'San Juan', N'NCR')
INSERT [dbo].[Profile] ([ProfileID], [FirstName], [LastName], [MiddleName], [Education], [Birthdate], [CivilStatus], [Sex], [Contact], [ContactPerson], [CPersonNo], [CPersonRel], [HouseNo], [Street], [City], [Province]) VALUES (4, N'AVP', N'Lastname', N'Middlename', NULL, CAST(N'1998-03-31' AS Date), 1, 1, CAST(9174726190 AS Numeric(18, 0)), N'AVP Mother', CAST(9178510533 AS Numeric(18, 0)), N'Mother', N'240', N'N Domingo', N'San Juan', N'NCR')
INSERT [dbo].[Profile] ([ProfileID], [FirstName], [LastName], [MiddleName], [Education], [Birthdate], [CivilStatus], [Sex], [Contact], [ContactPerson], [CPersonNo], [CPersonRel], [HouseNo], [Street], [City], [Province]) VALUES (5, N'SMVP', N'Lastname', N'Middlename', NULL, CAST(N'1998-03-31' AS Date), 1, 1, CAST(9174726190 AS Numeric(18, 0)), N'SMVP Mother', CAST(9178510533 AS Numeric(18, 0)), N'Mother', N'240', N'N Domingo', N'San Juan', N'NCR')
INSERT [dbo].[Profile] ([ProfileID], [FirstName], [LastName], [MiddleName], [Education], [Birthdate], [CivilStatus], [Sex], [Contact], [ContactPerson], [CPersonNo], [CPersonRel], [HouseNo], [Street], [City], [Province]) VALUES (6, N'OVP', N'Lastname', N'Middlename', NULL, CAST(N'1998-03-31' AS Date), 1, 1, CAST(9174726190 AS Numeric(18, 0)), N'OVP Mother', CAST(9178510533 AS Numeric(18, 0)), N'Mother', N'240', N'N Domingo', N'San Juan', N'NCR')
INSERT [dbo].[Profile] ([ProfileID], [FirstName], [LastName], [MiddleName], [Education], [Birthdate], [CivilStatus], [Sex], [Contact], [ContactPerson], [CPersonNo], [CPersonRel], [HouseNo], [Street], [City], [Province]) VALUES (7, N'FVP', N'Lastname', N'Middlename', NULL, CAST(N'1998-03-31' AS Date), 1, 1, CAST(9174726190 AS Numeric(18, 0)), N'FVP Mother', CAST(9178510533 AS Numeric(18, 0)), N'Mother', N'240', N'N Domingo', N'San Juan', N'NCR')
INSERT [dbo].[Profile] ([ProfileID], [FirstName], [LastName], [MiddleName], [Education], [Birthdate], [CivilStatus], [Sex], [Contact], [ContactPerson], [CPersonNo], [CPersonRel], [HouseNo], [Street], [City], [Province]) VALUES (8, N'CSVP', N'Lastname', N'Middlename', NULL, CAST(N'1998-03-31' AS Date), 1, 1, CAST(9174726190 AS Numeric(18, 0)), N'CSVP Mother', CAST(9178510533 AS Numeric(18, 0)), N'Mother', N'240', N'N Domingo', N'San Juan', N'NCR')
INSERT [dbo].[Profile] ([ProfileID], [FirstName], [LastName], [MiddleName], [Education], [Birthdate], [CivilStatus], [Sex], [Contact], [ContactPerson], [CPersonNo], [CPersonRel], [HouseNo], [Street], [City], [Province]) VALUES (9, N'HR', N'Head', N'Middlename', NULL, CAST(N'1998-03-31' AS Date), 1, 1, CAST(9174726190 AS Numeric(18, 0)), N'HR Mother', CAST(9179510533 AS Numeric(18, 0)), N'Mother', N'240', N'sTREET', N'City', N'Province')
INSERT [dbo].[Profile] ([ProfileID], [FirstName], [LastName], [MiddleName], [Education], [Birthdate], [CivilStatus], [Sex], [Contact], [ContactPerson], [CPersonNo], [CPersonRel], [HouseNo], [Street], [City], [Province]) VALUES (20, N'Employee', N'Lastname', N'Middlename', 13, CAST(N'2018-01-01' AS Date), 1, 1, CAST(91792919191 AS Numeric(18, 0)), N'Momma', CAST(91238172123 AS Numeric(18, 0)), N'Mother', N'#380', N'N. Domingo', N'San Juan City', N'NCR')
INSERT [dbo].[Profile] ([ProfileID], [FirstName], [LastName], [MiddleName], [Education], [Birthdate], [CivilStatus], [Sex], [Contact], [ContactPerson], [CPersonNo], [CPersonRel], [HouseNo], [Street], [City], [Province]) VALUES (21, N'Applicant', N'Lastname', N'Middlename', NULL, CAST(N'2018-01-01' AS Date), 1, 1, CAST(9174726190 AS Numeric(18, 0)), N'Matha', CAST(9123481283 AS Numeric(18, 0)), N'Mother', N'#123', N'N. Domingo', N'San Juan City', N'NCR')
INSERT [dbo].[Profile] ([ProfileID], [FirstName], [LastName], [MiddleName], [Education], [Birthdate], [CivilStatus], [Sex], [Contact], [ContactPerson], [CPersonNo], [CPersonRel], [HouseNo], [Street], [City], [Province]) VALUES (23, N'Sample', N'Sampleson JR.', N'Sampler', NULL, CAST(N'2018-01-01' AS Date), 5, 2, CAST(91792919191 AS Numeric(18, 0)), N'Momma', CAST(91238172123 AS Numeric(18, 0)), N'Mother', N'#380', N'N. Domingo', N'San Juan City', N'NCR')
INSERT [dbo].[Profile] ([ProfileID], [FirstName], [LastName], [MiddleName], [Education], [Birthdate], [CivilStatus], [Sex], [Contact], [ContactPerson], [CPersonNo], [CPersonRel], [HouseNo], [Street], [City], [Province]) VALUES (24, N'Test', N'Testerson', N'Testing', NULL, CAST(N'2018-01-01' AS Date), 1, 1, CAST(91792919191 AS Numeric(18, 0)), N'Momma', CAST(91238172123 AS Numeric(18, 0)), N'Mother', N'#380', N'N. Domingo', N'San Juan City', N'NCR')
INSERT [dbo].[Profile] ([ProfileID], [FirstName], [LastName], [MiddleName], [Education], [Birthdate], [CivilStatus], [Sex], [Contact], [ContactPerson], [CPersonNo], [CPersonRel], [HouseNo], [Street], [City], [Province]) VALUES (29, N'New Applicant', N'Lastname', N'Sampler', 18, CAST(N'2018-01-01' AS Date), 2, 1, CAST(91792919191 AS Numeric(18, 0)), N'Momma', CAST(91238172123 AS Numeric(18, 0)), N'Mother', N'#380', N'N. Domingo', N'San Juan City', N'NCR')
INSERT [dbo].[Profile] ([ProfileID], [FirstName], [LastName], [MiddleName], [Education], [Birthdate], [CivilStatus], [Sex], [Contact], [ContactPerson], [CPersonNo], [CPersonRel], [HouseNo], [Street], [City], [Province]) VALUES (33, N'asdgqwe', N'asdfqwer', N'asd', NULL, CAST(N'2018-01-01' AS Date), 2, 1, CAST(123 AS Numeric(18, 0)), N'341234123', CAST(91238172123 AS Numeric(18, 0)), N'123', N'asd', N'ads', N'asd', N'asd')
INSERT [dbo].[Profile] ([ProfileID], [FirstName], [LastName], [MiddleName], [Education], [Birthdate], [CivilStatus], [Sex], [Contact], [ContactPerson], [CPersonNo], [CPersonRel], [HouseNo], [Street], [City], [Province]) VALUES (1030, N'Miguel', N'Quiambao', N'Malalis', 19, CAST(N'1998-03-31' AS Date), 2, 1, CAST(9178510533 AS Numeric(18, 0)), N'Myrna Quiambao', CAST(91719238123123 AS Numeric(18, 0)), N'Mother', N'#380', N'N. Domingo', N'San Juan City', N'NCR')
INSERT [dbo].[Profile] ([ProfileID], [FirstName], [LastName], [MiddleName], [Education], [Birthdate], [CivilStatus], [Sex], [Contact], [ContactPerson], [CPersonNo], [CPersonRel], [HouseNo], [Street], [City], [Province]) VALUES (1034, N'Applicant', N'Sampleson JR.', N'Middlename', NULL, CAST(N'2021-03-02' AS Date), 1, 2, CAST(91792919191 AS Numeric(18, 0)), N'Momma', CAST(91238172123 AS Numeric(18, 0)), N'Mother', N'#380', N'N. Domingo', N'San Juan City', N'NCR')
INSERT [dbo].[Profile] ([ProfileID], [FirstName], [LastName], [MiddleName], [Education], [Birthdate], [CivilStatus], [Sex], [Contact], [ContactPerson], [CPersonNo], [CPersonRel], [HouseNo], [Street], [City], [Province]) VALUES (1035, N'Applicant', N'Sampleson JR.', N'Middlename', NULL, CAST(N'2021-03-02' AS Date), 1, 2, CAST(91792919191 AS Numeric(18, 0)), N'Momma', CAST(91238172123 AS Numeric(18, 0)), N'Mother', N'#380', N'N. Domingo', N'San Juan City', N'NCR')
INSERT [dbo].[Profile] ([ProfileID], [FirstName], [LastName], [MiddleName], [Education], [Birthdate], [CivilStatus], [Sex], [Contact], [ContactPerson], [CPersonNo], [CPersonRel], [HouseNo], [Street], [City], [Province]) VALUES (1036, N'Applicant', N'Sampleson JR.', N'Middlename', NULL, CAST(N'2021-03-02' AS Date), 1, 2, CAST(91792919191 AS Numeric(18, 0)), N'Momma', CAST(91238172123 AS Numeric(18, 0)), N'Mother', N'#380', N'N. Domingo', N'San Juan City', N'NCR')
SET IDENTITY_INSERT [dbo].[Profile] OFF
SET IDENTITY_INSERT [dbo].[RequisitionForm] ON 

INSERT [dbo].[RequisitionForm] ([RequisitionID], [Date], [Department], [Position], [RequestedBy], [ReasonforVacancy], [Type], [Qualification], [ExperienceRequired], [SkillsRequired], [ExpectedJoiningDate], [UnderSupervision], [BriefDescriptionofWorks], [Status]) VALUES (0, CAST(N'2018-11-03' AS Date), 14, N'Position', 9, N'Requisition Reason', N'Resignation', N'Qualifications', N'Experience', N'Skills', CAST(N'2018-01-02' AS Date), 9, N'Description', 2)
INSERT [dbo].[RequisitionForm] ([RequisitionID], [Date], [Department], [Position], [RequestedBy], [ReasonforVacancy], [Type], [Qualification], [ExperienceRequired], [SkillsRequired], [ExpectedJoiningDate], [UnderSupervision], [BriefDescriptionofWorks], [Status]) VALUES (1, CAST(N'2018-11-03' AS Date), 5, N'asd', 21, N'asd', N'Termination', N'asd', N'asd', N'asd', CAST(N'2005-01-13' AS Date), 17, N'asd', 1)
INSERT [dbo].[RequisitionForm] ([RequisitionID], [Date], [Department], [Position], [RequestedBy], [ReasonforVacancy], [Type], [Qualification], [ExperienceRequired], [SkillsRequired], [ExpectedJoiningDate], [UnderSupervision], [BriefDescriptionofWorks], [Status]) VALUES (3, CAST(N'2018-11-03' AS Date), 5, N'Tax Person', 21, N'Testing Notifications', N'Resignation', N'Testing Notifications', N'Testing Notifications', N'Testing Notifications', CAST(N'2007-02-03' AS Date), 17, N'Testing Notifications', 1)
SET IDENTITY_INSERT [dbo].[RequisitionForm] OFF
SET IDENTITY_INSERT [dbo].[TrainingHistory] ON 

INSERT [dbo].[TrainingHistory] ([TrainingHistoryID], [Title], [Description], [Facilitator], [StartDate], [EndDate], [Location]) VALUES (0, N'Digital Marketing Seminar', N'Something descriptiveasd', N'Laura Martinez', CAST(N'2018-01-10' AS Date), CAST(N'2018-01-20' AS Date), N'Quezon City')
INSERT [dbo].[TrainingHistory] ([TrainingHistoryID], [Title], [Description], [Facilitator], [StartDate], [EndDate], [Location]) VALUES (2, N'Test Training', N'Something', N'Mark Ocampo', CAST(N'2018-08-02' AS Date), CAST(N'2018-08-10' AS Date), N'Quezon City')
SET IDENTITY_INSERT [dbo].[TrainingHistory] OFF
SET ANSI_PADDING ON
GO
/****** Object:  Index [Username]    Script Date: 08/11/2018 5:36:50 PM ******/
ALTER TABLE [dbo].[Account] ADD  CONSTRAINT [Username] UNIQUE NONCLUSTERED 
(
	[Username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [UQ__Employee__97D5297130318DBA]    Script Date: 08/11/2018 5:36:50 PM ******/
ALTER TABLE [dbo].[Employee] ADD  CONSTRAINT [UQ__Employee__97D5297130318DBA] UNIQUE NONCLUSTERED 
(
	[Profile] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Account] ADD  CONSTRAINT [DF_Account_Locked]  DEFAULT ((0)) FOR [Locked]
GO
ALTER TABLE [dbo].[Account] ADD  CONSTRAINT [DF_Account_Security]  DEFAULT ('What is your birthday?') FOR [Security]
GO
ALTER TABLE [dbo].[Account] ADD  CONSTRAINT [DF_Account_Answer]  DEFAULT (' ') FOR [Answer]
GO
ALTER TABLE [dbo].[Account] ADD  CONSTRAINT [DF_Account_Image]  DEFAULT ('~/Content/img/uploads/default.png') FOR [Image]
GO
ALTER TABLE [dbo].[Applicant] ADD  CONSTRAINT [DF_Applicant_Status]  DEFAULT ((1)) FOR [Status]
GO
ALTER TABLE [dbo].[Appraisal] ADD  CONSTRAINT [DF_Appraisal_Rating]  DEFAULT ((0)) FOR [Rating]
GO
ALTER TABLE [dbo].[Appraisal] ADD  CONSTRAINT [DF_Appraisal_TechComp]  DEFAULT ((0)) FOR [TechComp]
GO
ALTER TABLE [dbo].[Appraisal] ADD  CONSTRAINT [DF_Appraisal_InterSkills]  DEFAULT ((0)) FOR [InterSkills]
GO
ALTER TABLE [dbo].[Appraisal] ADD  CONSTRAINT [DF_Appraisal_CommComp]  DEFAULT ((0)) FOR [CommComp]
GO
ALTER TABLE [dbo].[Appraisal] ADD  CONSTRAINT [DF_Appraisal_Total]  DEFAULT ((0)) FOR [Total]
GO
ALTER TABLE [dbo].[Appraisal] ADD  CONSTRAINT [DF_Appraisal_DatePrepared]  DEFAULT (getdate()) FOR [DatePrepared]
GO
ALTER TABLE [dbo].[Appraisal] ADD  CONSTRAINT [DF_Appraisal_Type]  DEFAULT ((0)) FOR [Type]
GO
ALTER TABLE [dbo].[Appraisal] ADD  CONSTRAINT [DF_Appraisal_Completed]  DEFAULT ((1)) FOR [Status]
GO
ALTER TABLE [dbo].[AssignedTraining] ADD  CONSTRAINT [DF_AssignedTraining_Status]  DEFAULT ((1)) FOR [Status]
GO
ALTER TABLE [dbo].[Attendance] ADD  CONSTRAINT [DF_Attendance_TotalWorkingDays]  DEFAULT ((0)) FOR [TotalWorkingDays]
GO
ALTER TABLE [dbo].[Attendance] ADD  CONSTRAINT [DF_Attendance_Present]  DEFAULT ((0)) FOR [Present]
GO
ALTER TABLE [dbo].[Attendance] ADD  CONSTRAINT [DF_Attendance_Absent]  DEFAULT ((0)) FOR [Absent]
GO
ALTER TABLE [dbo].[Attendance] ADD  CONSTRAINT [DF_Attendance_Overtime]  DEFAULT ((0)) FOR [Overtime]
GO
ALTER TABLE [dbo].[Attendance] ADD  CONSTRAINT [DF_Attendance_Late]  DEFAULT ((0)) FOR [Late]
GO
ALTER TABLE [dbo].[Attendance] ADD  CONSTRAINT [DF_Attendance_Undertime]  DEFAULT ((0)) FOR [Undertime]
GO
ALTER TABLE [dbo].[Attendance] ADD  CONSTRAINT [DF_Attendance_Date]  DEFAULT (getdate()) FOR [Date]
GO
ALTER TABLE [dbo].[Attendance] ADD  CONSTRAINT [DF_Attendance_Leave]  DEFAULT ((0)) FOR [Leave]
GO
ALTER TABLE [dbo].[Attendance] ADD  CONSTRAINT [DF_Attendance_HalfDays]  DEFAULT ((0)) FOR [HalfDays]
GO
ALTER TABLE [dbo].[AttendanceTime] ADD  CONSTRAINT [DF_AttendanceTime_TimeIn]  DEFAULT (getdate()) FOR [TimeIn]
GO
ALTER TABLE [dbo].[AttendanceTime] ADD  CONSTRAINT [DF_AttendanceTime_TimeOut]  DEFAULT (getdate()) FOR [TimeOut]
GO
ALTER TABLE [dbo].[AttendanceTime] ADD  CONSTRAINT [DF_AttendanceTime_Date]  DEFAULT (getdate()) FOR [Date]
GO
ALTER TABLE [dbo].[Employee] ADD  CONSTRAINT [DF_Employee_Status]  DEFAULT ((1)) FOR [Status]
GO
ALTER TABLE [dbo].[JobPosting] ADD  CONSTRAINT [DF_JobPosting_DatePosted]  DEFAULT (getdate()) FOR [DatePosted]
GO
ALTER TABLE [dbo].[Leave] ADD  CONSTRAINT [DF_Leave_Status]  DEFAULT ((1)) FOR [Status]
GO
ALTER TABLE [dbo].[Leave] ADD  CONSTRAINT [DF_Leave_Type]  DEFAULT ((1)) FOR [Type]
GO
ALTER TABLE [dbo].[Notification] ADD  CONSTRAINT [DF_Notification_Timestamp]  DEFAULT (getdate()) FOR [Timestamp]
GO
ALTER TABLE [dbo].[Notification] ADD  CONSTRAINT [DF_Notification_Status]  DEFAULT ((1)) FOR [Status]
GO
ALTER TABLE [dbo].[RequisitionForm] ADD  CONSTRAINT [DF_RequisitionForm_Status]  DEFAULT ((1)) FOR [Status]
GO
ALTER TABLE [dbo].[Applicant]  WITH CHECK ADD  CONSTRAINT [FK_Applicant_Profile] FOREIGN KEY([Profile])
REFERENCES [dbo].[Profile] ([ProfileID])
GO
ALTER TABLE [dbo].[Applicant] CHECK CONSTRAINT [FK_Applicant_Profile]
GO
ALTER TABLE [dbo].[Appraisal]  WITH CHECK ADD  CONSTRAINT [FK_Appraisal_Employee] FOREIGN KEY([NotedBy])
REFERENCES [dbo].[Employee] ([EmployeeID])
GO
ALTER TABLE [dbo].[Appraisal] CHECK CONSTRAINT [FK_Appraisal_Employee]
GO
ALTER TABLE [dbo].[Appraisal]  WITH CHECK ADD  CONSTRAINT [FK_Appraisal_Employee2] FOREIGN KEY([Evaluator])
REFERENCES [dbo].[Employee] ([EmployeeID])
GO
ALTER TABLE [dbo].[Appraisal] CHECK CONSTRAINT [FK_Appraisal_Employee2]
GO
ALTER TABLE [dbo].[Appraisal]  WITH CHECK ADD  CONSTRAINT [FK_Appraisal_Employee3] FOREIGN KEY([DiscussedWith])
REFERENCES [dbo].[Employee] ([EmployeeID])
GO
ALTER TABLE [dbo].[Appraisal] CHECK CONSTRAINT [FK_Appraisal_Employee3]
GO
ALTER TABLE [dbo].[AssignedTraining]  WITH CHECK ADD  CONSTRAINT [FK_AssignedTraining_Profile] FOREIGN KEY([Profile])
REFERENCES [dbo].[Profile] ([ProfileID])
GO
ALTER TABLE [dbo].[AssignedTraining] CHECK CONSTRAINT [FK_AssignedTraining_Profile]
GO
ALTER TABLE [dbo].[AssignedTraining]  WITH CHECK ADD  CONSTRAINT [FK_AssignedTraining_Training] FOREIGN KEY([Training])
REFERENCES [dbo].[TrainingHistory] ([TrainingHistoryID])
GO
ALTER TABLE [dbo].[AssignedTraining] CHECK CONSTRAINT [FK_AssignedTraining_Training]
GO
ALTER TABLE [dbo].[Attendance]  WITH CHECK ADD  CONSTRAINT [FK_Attendance_Employee] FOREIGN KEY([Employee])
REFERENCES [dbo].[Employee] ([EmployeeID])
GO
ALTER TABLE [dbo].[Attendance] CHECK CONSTRAINT [FK_Attendance_Employee]
GO
ALTER TABLE [dbo].[AttendanceTime]  WITH CHECK ADD  CONSTRAINT [FK_AttendanceTime_Attendance] FOREIGN KEY([Attendance])
REFERENCES [dbo].[Attendance] ([AttendanceID])
GO
ALTER TABLE [dbo].[AttendanceTime] CHECK CONSTRAINT [FK_AttendanceTime_Attendance]
GO
ALTER TABLE [dbo].[Branch]  WITH CHECK ADD  CONSTRAINT [FK_Branch_Profile] FOREIGN KEY([BranchVP])
REFERENCES [dbo].[Employee] ([EmployeeID])
GO
ALTER TABLE [dbo].[Branch] CHECK CONSTRAINT [FK_Branch_Profile]
GO
ALTER TABLE [dbo].[Department]  WITH CHECK ADD  CONSTRAINT [FK_Department_Branch] FOREIGN KEY([Branch])
REFERENCES [dbo].[Branch] ([BranchID])
GO
ALTER TABLE [dbo].[Department] CHECK CONSTRAINT [FK_Department_Branch]
GO
ALTER TABLE [dbo].[Department]  WITH CHECK ADD  CONSTRAINT [FK_Department_Profile] FOREIGN KEY([DepartmentHead])
REFERENCES [dbo].[Employee] ([EmployeeID])
GO
ALTER TABLE [dbo].[Department] CHECK CONSTRAINT [FK_Department_Profile]
GO
ALTER TABLE [dbo].[Employee]  WITH CHECK ADD  CONSTRAINT [FK_Employee_Branch] FOREIGN KEY([Branch])
REFERENCES [dbo].[Branch] ([BranchID])
GO
ALTER TABLE [dbo].[Employee] CHECK CONSTRAINT [FK_Employee_Branch]
GO
ALTER TABLE [dbo].[Employee]  WITH CHECK ADD  CONSTRAINT [FK_Employee_Department] FOREIGN KEY([Department])
REFERENCES [dbo].[Department] ([DepartmentID])
GO
ALTER TABLE [dbo].[Employee] CHECK CONSTRAINT [FK_Employee_Department]
GO
ALTER TABLE [dbo].[Employee]  WITH CHECK ADD  CONSTRAINT [FK_Employee_Profile] FOREIGN KEY([Profile])
REFERENCES [dbo].[Profile] ([ProfileID])
GO
ALTER TABLE [dbo].[Employee] CHECK CONSTRAINT [FK_Employee_Profile]
GO
ALTER TABLE [dbo].[EmploymentHistory]  WITH CHECK ADD  CONSTRAINT [FK_EmploymentHistory_Profile] FOREIGN KEY([Profile])
REFERENCES [dbo].[Profile] ([ProfileID])
GO
ALTER TABLE [dbo].[EmploymentHistory] CHECK CONSTRAINT [FK_EmploymentHistory_Profile]
GO
ALTER TABLE [dbo].[Leave]  WITH CHECK ADD  CONSTRAINT [FK_Leave_Employee] FOREIGN KEY([Employee])
REFERENCES [dbo].[Employee] ([EmployeeID])
GO
ALTER TABLE [dbo].[Leave] CHECK CONSTRAINT [FK_Leave_Employee]
GO
ALTER TABLE [dbo].[Notification]  WITH CHECK ADD  CONSTRAINT [FK_Notification_Account] FOREIGN KEY([Account])
REFERENCES [dbo].[Account] ([AccountID])
GO
ALTER TABLE [dbo].[Notification] CHECK CONSTRAINT [FK_Notification_Account]
GO
ALTER TABLE [dbo].[Profile]  WITH CHECK ADD  CONSTRAINT [FK_Profile_Education] FOREIGN KEY([Education])
REFERENCES [dbo].[EducationalBackground] ([EducationID])
GO
ALTER TABLE [dbo].[Profile] CHECK CONSTRAINT [FK_Profile_Education]
GO
ALTER TABLE [dbo].[RequisitionForm]  WITH CHECK ADD  CONSTRAINT [FK_RequisitionForm_Department] FOREIGN KEY([Department])
REFERENCES [dbo].[Department] ([DepartmentID])
GO
ALTER TABLE [dbo].[RequisitionForm] CHECK CONSTRAINT [FK_RequisitionForm_Department]
GO
ALTER TABLE [dbo].[RequisitionForm]  WITH CHECK ADD  CONSTRAINT [FK_RequisitionForm_Employee] FOREIGN KEY([RequestedBy])
REFERENCES [dbo].[Employee] ([EmployeeID])
GO
ALTER TABLE [dbo].[RequisitionForm] CHECK CONSTRAINT [FK_RequisitionForm_Employee]
GO
ALTER TABLE [dbo].[RequisitionForm]  WITH CHECK ADD  CONSTRAINT [FK_RequisitionForm_Employee2] FOREIGN KEY([UnderSupervision])
REFERENCES [dbo].[Employee] ([EmployeeID])
GO
ALTER TABLE [dbo].[RequisitionForm] CHECK CONSTRAINT [FK_RequisitionForm_Employee2]
GO
