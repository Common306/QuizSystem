USE [master]
GO
CREATE DATABASE [QuizSystemDatabase]
GO
USE [QuizSystemDatabase]

/* ------------------ Script create table ------------------ */
GO
CREATE TABLE [dbo].[Role](
	[RoleId] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
	[RoleName] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](250) NULL
)
GO
CREATE TABLE [dbo].[User](
	[UserId] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](250) NOT NULL,
	[RoleId] [int] NOT NULL,
	[FullName] [nvarchar](250) NULL,
	[PhoneNumber] [nvarchar](50) NULL,
	[IsEnable] [bit] NULL,
	[CreateAt] [datetime] default CURRENT_TIMESTAMP,
	[UpdateAt] [datetime] default CURRENT_TIMESTAMP,
	FOREIGN KEY (RoleId) REFERENCES [Role](RoleId)
)
GO
CREATE TABLE [dbo].[Quiz](
	[QuizId] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](250) NULL,
	[Description] [text] NULL,
	[IsPublish] [bit] NULL,
	[StartAt] [datetime] NULL,
	[EndAt] [datetime] NULL,
	[PassScore] [float] NULL,
	[QuizCode] [nvarchar](50) NULL,
	[CreatorId] [int] NULL,
	[CreateAt] [datetime] default CURRENT_TIMESTAMP,
	[UpdateAt] [datetime] default CURRENT_TIMESTAMP,
	FOREIGN KEY (CreatorId) REFERENCES [User](UserId)
)
GO
CREATE TABLE [dbo].[TakeQuiz](
	[TakeQuizId] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
	[UserId] [int] NULL,
	[QuizId] [int] NULL,
	[StartAt] [datetime] NULL,
	[EndAt] [datetime] NULL,
	[Score] [float] NULL,
	FOREIGN KEY (UserId) REFERENCES [User](UserId),
	FOREIGN KEY (QuizId) REFERENCES [Quiz](QuizId)
) 
GO
CREATE TABLE [dbo].[Question](
	[QuestionId] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
	[Content] [nvarchar](500) NULL,
	[Score] [float] NULL,
	[MultipleChoice] [bit] NULL,
	[QuizId] [int] NULL,
	[IsActive] [bit] NULL,
	FOREIGN KEY (QuizId) REFERENCES [Quiz](QuizId)
)
GO
CREATE TABLE [dbo].[Answer](
	[AnswerId] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
	[Content] [nvarchar](500) NULL,
	[IsCorrect] [bit] NULL,
	[IsActive] [bit] NULL,
	[QuestionId] [int] NULL,
	FOREIGN KEY (QuestionId) REFERENCES [Question](QuestionId)
)
GO
CREATE TABLE [dbo].[TakeAnswer](
	[TakeQuizId] [int] NOT NULL,
	[AnswerId] [int] NOT NULL,
	[QuestionId] [int] NULL,
	FOREIGN KEY (TakeQuizId) REFERENCES [TakeQuiz](TakeQuizId),
	FOREIGN KEY (AnswerId) REFERENCES [Answer](AnswerId),
	PRIMARY KEY(TakeQuizId, AnswerId)
)
/* ------------------ Script insert data ------------------ */
GO
SET IDENTITY_INSERT [dbo].[Role] ON 
INSERT [dbo].[Role] ([RoleId], [RoleName], [Description]) VALUES (1, N'Admin', NULL)
INSERT [dbo].[Role] ([RoleId], [RoleName], [Description]) VALUES (2, N'Teacher', NULL)
INSERT [dbo].[Role] ([RoleId], [RoleName], [Description]) VALUES (3, N'Student', NULL)
SET IDENTITY_INSERT [dbo].[Role] OFF 
GO
INSERT [dbo].[User] ([Username], [Password], [RoleId], [FullName], [PhoneNumber], [IsEnable]) VALUES (N'admin', N'123456', 1, N'Admin System', N'0912564788', 1)
INSERT [dbo].[User] ([Username], [Password], [RoleId], [FullName], [PhoneNumber], [IsEnable]) VALUES (N'teacher1', N'123456', 2, N'Teacher Number 1', N'0328451552', 1)
INSERT [dbo].[User] ([Username], [Password], [RoleId], [FullName], [PhoneNumber], [IsEnable]) VALUES (N'teacher2', N'123456', 2, N'Teacher Number 2', N'0315654112', 1)
INSERT [dbo].[User] ([Username], [Password], [RoleId], [FullName], [PhoneNumber], [IsEnable]) VALUES (N'student1', N'123456', 3, N'Student Number 1', N'0323454565', 1)
INSERT [dbo].[User] ([Username], [Password], [RoleId], [FullName], [PhoneNumber], [IsEnable]) VALUES (N'student2', N'123456', 3, N'Student Number 2 ', N'0212345789', 1)
GO
INSERT [dbo].[Quiz] ([Title], [Description], [IsPublish], [StartAt], [EndAt], [PassScore], [QuizCode], [CreatorId]) VALUES (N'SE1608_PRN231 PT1 ', N'Practice Test 1', 1, NULL, NULL, 4, N'1', 2)
INSERT [dbo].[Quiz] ([Title], [Description], [IsPublish], [StartAt], [EndAt], [PassScore], [QuizCode], [CreatorId]) VALUES (N'SE1617_PRU221m PT1', N'Practice Test 1', 1, NULL, NULL, 4, N'1', 3)
