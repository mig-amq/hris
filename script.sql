USE [master]
GO
/****** Object:  Table [dbo].[Account]    Script Date: 06/11/2018 3:06:10 AM ******/
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
 CONSTRAINT [PK_Account] PRIMARY KEY CLUSTERED 
(
	[AccountID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [Username] UNIQUE NONCLUSTERED 
(
	[Username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Applicant]    Script Date: 06/11/2018 3:06:10 AM ******/
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
/****** Object:  Table [dbo].[Appraisal]    Script Date: 06/11/2018 3:06:10 AM ******/
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
/****** Object:  Table [dbo].[AssignedTraining]    Script Date: 06/11/2018 3:06:10 AM ******/
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
/****** Object:  Table [dbo].[Attendance]    Script Date: 06/11/2018 3:06:10 AM ******/
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
 CONSTRAINT [PK_Attendance] PRIMARY KEY CLUSTERED 
(
	[AttendanceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AttendanceTime]    Script Date: 06/11/2018 3:06:10 AM ******/
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
/****** Object:  Table [dbo].[Branch]    Script Date: 06/11/2018 3:06:10 AM ******/
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
/****** Object:  Table [dbo].[Department]    Script Date: 06/11/2018 3:06:10 AM ******/
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
/****** Object:  Table [dbo].[EducationalBackground]    Script Date: 06/11/2018 3:06:10 AM ******/
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
/****** Object:  Table [dbo].[Employee]    Script Date: 06/11/2018 3:06:10 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UQ__Employee__97D5297130318DBA] UNIQUE NONCLUSTERED 
(
	[Profile] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EmploymentHistory]    Script Date: 06/11/2018 3:06:10 AM ******/
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
/****** Object:  Table [dbo].[JobPosting]    Script Date: 06/11/2018 3:06:10 AM ******/
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
/****** Object:  Table [dbo].[Leave]    Script Date: 06/11/2018 3:06:10 AM ******/
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
 CONSTRAINT [PK_Leave] PRIMARY KEY CLUSTERED 
(
	[LeaveID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Notification]    Script Date: 06/11/2018 3:06:10 AM ******/
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
/****** Object:  Table [dbo].[Profile]    Script Date: 06/11/2018 3:06:10 AM ******/
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
/****** Object:  Table [dbo].[RequisitionForm]    Script Date: 06/11/2018 3:06:10 AM ******/
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
/****** Object:  Table [dbo].[TrainingHistory]    Script Date: 06/11/2018 3:06:10 AM ******/
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
ALTER TABLE [dbo].[Account] ADD  CONSTRAINT [DF_Account_Locked]  DEFAULT ((0)) FOR [Locked]
GO
ALTER TABLE [dbo].[Account] ADD  CONSTRAINT [DF_Account_Security]  DEFAULT ('What is your birthday?') FOR [Security]
GO
ALTER TABLE [dbo].[Account] ADD  CONSTRAINT [DF_Account_Answer]  DEFAULT (' ') FOR [Answer]
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
ALTER TABLE [dbo].[Notification] ADD  CONSTRAINT [DF_Notification_Timestamp]  DEFAULT (getdate()) FOR [Timestamp]
GO
ALTER TABLE [dbo].[Notification] ADD  CONSTRAINT [DF_Notification_Status]  DEFAULT ((1)) FOR [Status]
GO
ALTER TABLE [dbo].[RequisitionForm] ADD  CONSTRAINT [DF_RequisitionForm_Status]  DEFAULT ((1)) FOR [Status]
GO
ALTER TABLE [dbo].[Applicant]  WITH CHECK ADD  CONSTRAINT [FK_Applicant_Profile] FOREIGN KEY([ApplicantID])
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
