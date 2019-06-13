
/****** Object:  Table [dbo].[tblAttributeAdvanced]    Script Date: 6/7/2019 5:05:42 PM ******/

GO
CREATE TABLE [dbo].[tblAttributeAdvanced](
	[AdvancedId] [int] IDENTITY(1,1) NOT NULL,
	[AdvancedName] [nvarchar](100) NULL,
	[AdvancedDescription] [nvarchar](100) NULL,
	[AdvancedDataType] [nvarchar](50) NULL,
	[AdvancedObject] [nvarchar](50) NULL,
	[CreateBy] [nvarchar](50) NULL,
	[CreateDate] [datetime] NULL,
	[UpdateBy] [nvarchar](50) NULL,
	[UpdateDate] [datetime] NULL,
	[IsDelete] [bit] NULL,
 CONSTRAINT [PK_tblAttributeAdvanced] PRIMARY KEY CLUSTERED 
(
	[AdvancedId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TblAttributeConstraint]    Script Date: 6/7/2019 5:05:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TblAttributeConstraint](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](250) NULL,
	[ControlType] [nvarchar](250) NULL,
	[ContraintsType] [nvarchar](250) NULL,
	[ContraintsValue] [nvarchar](250) NULL,
	[IsContraintType] [bit] NULL,
	[IsContraintValue] [bit] NULL,
	[LinkContraints] [nvarchar](250) NULL,
	[CreateBy] [nvarchar](250) NULL,
	[CreateDate] [datetime] NULL,
	[UpdateBy] [nvarchar](250) NULL,
	[UpdateDate] [datetime] NULL,
	[IsDelete] [bit] NULL,
 CONSTRAINT [PK_TblAttributeConstraint] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblAttributeOptions]    Script Date: 6/7/2019 5:05:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblAttributeOptions](
	[AttributeOptionsId] [int] IDENTITY(1,1) NOT NULL,
	[AttributeId] [int] NULL,
	[OptionId] [int] NULL,
	[OptionValue] [nvarchar](50) NULL,
	[OptionLabel] [nvarchar](100) NULL,
	[CreateBy] [nvarchar](50) NULL,
	[CreateDate] [datetime] NULL,
	[UpdateBy] [nvarchar](50) NULL,
	[UpdateDate] [datetime] NULL,
	[IsDelete] [bit] NULL,
 CONSTRAINT [PK_tblAttributeOptions] PRIMARY KEY CLUSTERED 
(
	[AttributeOptionsId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblCategory]    Script Date: 6/7/2019 5:05:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblCategory](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CategoryCode] [nvarchar](50) NULL,
	[CategoryTypeCode] [nvarchar](50) NULL,
	[CategoryName] [nvarchar](200) NULL,
	[CategoryDescription] [nvarchar](500) NULL,
	[ExtContent] [nvarchar](500) NULL,
	[CreateBy] [nvarchar](50) NULL,
	[CreateDate] [datetime] NULL,
	[UpdateBy] [nvarchar](50) NULL,
	[UpdateDate] [datetime] NULL,
	[IsDelete] [bit] NULL,
	[IsActive] [int] NULL,
	[OrderNum] [int] NULL,
 CONSTRAINT [PK_tblCategory] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblCategoryGroup]    Script Date: 6/7/2019 5:05:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblCategoryGroup](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CategoryCode] [nvarchar](50) NULL,
	[CategoryTypeCode] [nvarchar](50) NULL,
	[CategoryGroupName] [nvarchar](200) NULL,
	[CreateBy] [nvarchar](50) NULL,
	[CreateDate] [datetime] NULL,
	[UpdateBy] [nvarchar](50) NULL,
	[UpdateDate] [datetime] NULL,
	[IsDelete] [bit] NULL,
	[CategoryDescription] [nvarchar](500) NULL,
 CONSTRAINT [PK_tblCategoryGroup] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblCIMSAttributeForm]    Script Date: 6/7/2019 5:05:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tblCIMSAttributeForm](
	[AttributeFormId] [int] IDENTITY(1,1) NOT NULL,
	[FormId] [int] NULL,
	[AttributeId] [int] NULL,
	[AttributeCode] [varchar](100) NULL,
	[AttributeType] [nvarchar](50) NULL,
	[AttrOrder] [int] NULL,
	[AttributeColumn] [int] NULL,
	[RowIndex] [int] NULL,
	[RowTitle] [nvarchar](150) NULL,
	[ChildCode] [nvarchar](50) NULL,
	[IsShowLabel] [bit] NULL,
	[DefaultValue] [nvarchar](50) NULL,
 CONSTRAINT [PK_tblCIMSAttributeForm] PRIMARY KEY CLUSTERED 
(
	[AttributeFormId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tblCIMSAttributeValue]    Script Date: 6/7/2019 5:05:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tblCIMSAttributeValue](
	[AttributesValueId] [int] IDENTITY(1,1) NOT NULL,
	[AttributeId] [int] NULL,
	[AttributeValue] [ntext] NULL,
	[IsDelete] [bit] NULL,
	[AttributeCode] [nchar](100) NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[UpdatedDate] [datetime] NULL,
	[UpdatedBy] [nvarchar](50) NULL,
	[RecordId] [varchar](255) NULL,
	[Module] [varchar](255) NULL,
 CONSTRAINT [PK_tblCIMSAttributeValue] PRIMARY KEY CLUSTERED 
(
	[AttributesValueId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tblCIMSForm]    Script Date: 6/7/2019 5:05:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tblCIMSForm](
	[FormId] [int] IDENTITY(1,1) NOT NULL,
	[FormName] [nvarchar](50) NULL,
	[FormDescription] [nvarchar](100) NULL,
	[MenuCode] [nvarchar](50) NULL,
	[CreateBy] [nvarchar](50) NULL,
	[CreateDate] [datetime] NULL,
	[UpdateBy] [nvarchar](50) NULL,
	[UpdateDate] [datetime] NULL,
	[IsDelete] [bit] NULL,
	[FormType] [varchar](50) NULL,
	[IsContinute] [bit] NULL,
	[ChildCode] [nvarchar](50) NULL,
 CONSTRAINT [PK_tblCIMSForm] PRIMARY KEY CLUSTERED 
(
	[FormId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tblCimsFormHistory]    Script Date: 6/7/2019 5:05:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblCimsFormHistory](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](100) NULL,
	[ChildCode] [nvarchar](50) NULL,
	[CreateBy] [nvarchar](50) NULL,
	[CreateDate] [datetime] NULL,
	[UpdateBy] [nvarchar](50) NULL,
	[UpdateDate] [datetime] NULL,
 CONSTRAINT [PK_tblCimsFormHistory] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblConnectionConfig]    Script Date: 6/7/2019 5:05:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblConnectionConfig](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ConnectionKey] [nvarchar](50) NULL,
	[ConnectionValue] [nvarchar](255) NULL,
 CONSTRAINT [PK_tblConnectionConfig] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblCustomer]    Script Date: 6/7/2019 5:05:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tblCustomer](
	[ATTRIBUTE01] [varchar](1) NULL,
	[ATTRIBUTE02] [nvarchar](1) NULL,
	[ATTRIBUTE03] [datetime] NULL,
	[ATTRIBUTE04] [varchar](1) NULL,
	[ATTRIBUTE05] [varchar](1) NULL,
	[ATTRIBUTE06] [nvarchar](1) NULL,
	[ATTRIBUTE07] [varchar](1) NULL,
	[ATTRIBUTE08] [varchar](1) NULL,
	[ATTRIBUTE09] [nvarchar](1) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tblEncryption]    Script Date: 6/7/2019 5:05:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblEncryption](
	[EncryptionId] [int] IDENTITY(1,1) NOT NULL,
	[AttributeCode] [nvarchar](50) NULL,
	[ParentCode] [nvarchar](50) NULL,
	[CreateDate] [datetime] NULL,
	[EncryptionStatus] [bit] NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[UpdatedBy] [nvarchar](50) NULL,
	[AttributeLabel] [nvarchar](100) NULL,
	[UpdateDate] [datetime] NULL,
	[IsFirst] [bit] NULL,
	[IsDone] [bit] NULL,
	[ModuleName] [nvarchar](50) NULL,
	[FinalizationStatus] [bit] NULL,
 CONSTRAINT [PK_tblEncryption] PRIMARY KEY CLUSTERED 
(
	[EncryptionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblReferenceConstraint]    Script Date: 6/7/2019 5:05:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblReferenceConstraint](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AttributeCode] [nvarchar](100) NULL,
	[ConstraintId] [int] NULL,
	[MenuCode] [nvarchar](50) NULL,
 CONSTRAINT [PK_tblReferenceConstraint] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblVOCAttributeForm]    Script Date: 6/7/2019 5:05:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblVOCAttributeForm](
	[AttributeFormId] [int] IDENTITY(1,1) NOT NULL,
	[FormId] [int] NULL,
	[AttributeId] [int] NULL,
 CONSTRAINT [PK_tblVOCAttributeForm] PRIMARY KEY CLUSTERED 
(
	[AttributeFormId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblVOCAttributes]    Script Date: 6/7/2019 5:05:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tblVOCAttributes](
	[AttributesId] [int] IDENTITY(1,1) NOT NULL,
	[AttributeCode] [nvarchar](50) NULL,
	[AttributeType] [nvarchar](50) NULL,
	[AttributeWidth] [int] NULL,
	[DataType] [nvarchar](50) NULL,
	[AttributeLabel] [nvarchar](100) NULL,
	[DefaultValue] [nvarchar](200) NULL,
	[IsVisible] [bit] NULL,
	[IsRequired] [bit] NULL,
	[IsTableShow] [bit] NULL,
	[IsCategory] [bit] NULL,
	[ModuleParent] [nvarchar](100) NULL,
	[IsSort] [bit] NULL,
	[IsDuplicate] [bit] NULL,
	[DetailRefer] [nvarchar](500) NULL,
	[CreateBy] [nvarchar](50) NULL,
	[CreateDate] [datetime] NULL,
	[UpDateBy] [nvarchar](50) NULL,
	[UpdateDate] [datetime] NULL,
	[IsDelete] [bit] NULL,
	[IsReuse] [bit] NULL,
	[CategoryParentCode] [varchar](50) NULL,
	[AttributeDescription] [nvarchar](500) NULL,
	[Encyption] [bit] NULL,
	[EncyptWaiting] [bit] NULL,
	[IndexTitleTable] [int] NULL,
	[DefaultValueWithTextBox] [nvarchar](200) NULL,
	[Disabled] [bit] NULL,
 CONSTRAINT [PK_tblAttributes] PRIMARY KEY CLUSTERED 
(
	[AttributesId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tblVOCForm]    Script Date: 6/7/2019 5:05:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblVOCForm](
	[FormId] [int] IDENTITY(1,1) NOT NULL,
	[FormName] [nvarchar](100) NULL,
	[FormDescription] [nvarchar](500) NULL,
	[CreateBy] [nvarchar](50) NULL,
	[CreateDate] [datetime] NULL,
	[UpdateBy] [nvarchar](50) NULL,
	[UpdateDate] [datetime] NULL,
	[IsDelete] [bit] NULL,
 CONSTRAINT [PK_tblVOCForm] PRIMARY KEY CLUSTERED 
(
	[FormId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblVOCStepAttributes]    Script Date: 6/7/2019 5:05:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblVOCStepAttributes](
	[StepAttributesId] [int] IDENTITY(1,1) NOT NULL,
	[StepId] [int] NULL,
	[AttributeId] [int] NULL,
	[AttributeIndex] [int] NULL,
 CONSTRAINT [PK_tblStepAttributes] PRIMARY KEY CLUSTERED 
(
	[StepAttributesId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblVOCStepAttributesValue]    Script Date: 6/7/2019 5:05:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblVOCStepAttributesValue](
	[StepAttributesValueId] [int] IDENTITY(1,1) NOT NULL,
	[StepId] [int] NULL,
	[AttributeId] [int] NULL,
	[AttributeValue] [text] NULL,
	[CreateBy] [nvarchar](50) NULL,
	[CreateDate] [datetime] NULL,
	[UpdateBy] [nvarchar](50) NULL,
	[UpdateDate] [datetime] NULL,
	[IsDelete] [bit] NULL,
 CONSTRAINT [PK_tblStepAttributesValue] PRIMARY KEY CLUSTERED 
(
	[StepAttributesValueId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblVOCSteps]    Script Date: 6/7/2019 5:05:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblVOCSteps](
	[StepId] [int] IDENTITY(1,1) NOT NULL,
	[StepName] [nvarchar](200) NULL,
	[OrganizationId] [int] NULL,
	[TemplateEmailId] [int] NULL,
	[CreateBy] [nvarchar](50) NULL,
	[CreateDate] [datetime] NULL,
	[UpdateBy] [nvarchar](50) NULL,
	[UpdateDate] [datetime] NULL,
	[IsDelete] [bit] NULL,
 CONSTRAINT [PK_tblSteps] PRIMARY KEY CLUSTERED 
(
	[StepId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblVOCWorkflows]    Script Date: 6/7/2019 5:05:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblVOCWorkflows](
	[WorkflowId] [int] IDENTITY(1,1) NOT NULL,
	[WorkflowName] [nvarchar](200) NULL,
	[OrganizationId] [int] NULL,
	[CreateBy] [nvarchar](50) NULL,
	[CreateDate] [datetime] NULL,
	[UpdateBy] [nvarchar](50) NULL,
	[UpdateDate] [datetime] NULL,
	[IsDelete] [bit] NULL,
 CONSTRAINT [PK_tblWorkfolows] PRIMARY KEY CLUSTERED 
(
	[WorkflowId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblVOCWorkflowSteps]    Script Date: 6/7/2019 5:05:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblVOCWorkflowSteps](
	[WorkflowStepsId] [int] IDENTITY(1,1) NOT NULL,
	[WorkflowId] [int] NULL,
	[StepId] [int] NULL,
	[StepIndex] [int] NULL,
 CONSTRAINT [PK_tblWorkFolowSteps] PRIMARY KEY CLUSTERED 
(
	[WorkflowStepsId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TempShow]    Script Date: 6/7/2019 5:05:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TempShow](
	[index] [bigint] NULL,
	[RecordId] [varchar](255) NULL,
	[Mã khách hàng] [nvarchar](max) NULL,
	[Địa chỉ] [nvarchar](max) NULL,
	[Ngày sinh] [nvarchar](max) NULL,
	[Email] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[TblAttributeConstraint] ON 

INSERT [dbo].[TblAttributeConstraint] ([Id], [Name], [ControlType], [ContraintsType], [ContraintsValue], [IsContraintType], [IsContraintValue], [LinkContraints], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete]) VALUES (1, N'Check Trùng', NULL, N'Control-02', N'', NULL, NULL, N'', N'admin', CAST(N'2019-05-06 14:53:15.660' AS DateTime), NULL, NULL, 1)
INSERT [dbo].[TblAttributeConstraint] ([Id], [Name], [ControlType], [ContraintsType], [ContraintsValue], [IsContraintType], [IsContraintValue], [LinkContraints], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete]) VALUES (2, N'Phòng ban', NULL, N'Control-04', N'', NULL, NULL, N'DepartmentCode', N'admin', CAST(N'2019-05-06 15:08:43.010' AS DateTime), N'admin', CAST(N'2019-05-06 15:41:37.950' AS DateTime), 1)
INSERT [dbo].[TblAttributeConstraint] ([Id], [Name], [ControlType], [ContraintsType], [ContraintsValue], [IsContraintType], [IsContraintValue], [LinkContraints], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete]) VALUES (3, N'bắt buộc', NULL, N'Control-04', N'Trung Tâm VAS', NULL, NULL, N'DepartmentCode', N'admin', CAST(N'2019-05-06 15:09:58.277' AS DateTime), N'admin', CAST(N'2019-05-07 09:08:33.910' AS DateTime), 1)
INSERT [dbo].[TblAttributeConstraint] ([Id], [Name], [ControlType], [ContraintsType], [ContraintsValue], [IsContraintType], [IsContraintValue], [LinkContraints], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete]) VALUES (4, N'regex', NULL, N'Control-02', N'', NULL, NULL, N'', N'admin', CAST(N'2019-05-06 15:50:37.583' AS DateTime), NULL, NULL, 1)
INSERT [dbo].[TblAttributeConstraint] ([Id], [Name], [ControlType], [ContraintsType], [ContraintsValue], [IsContraintType], [IsContraintValue], [LinkContraints], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete]) VALUES (6, N'test', NULL, N'Control-02', N'', NULL, NULL, N'', N'admin', CAST(N'2019-05-06 15:52:39.303' AS DateTime), NULL, NULL, 1)
INSERT [dbo].[TblAttributeConstraint] ([Id], [Name], [ControlType], [ContraintsType], [ContraintsValue], [IsContraintType], [IsContraintValue], [LinkContraints], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete]) VALUES (7, N'test 23542', NULL, N'Control-01', N'', NULL, NULL, N'', N'admin', CAST(N'2019-05-06 15:54:48.820' AS DateTime), NULL, NULL, 1)
INSERT [dbo].[TblAttributeConstraint] ([Id], [Name], [ControlType], [ContraintsType], [ContraintsValue], [IsContraintType], [IsContraintValue], [LinkContraints], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete]) VALUES (8, N'dat', NULL, N'Control-02', N'', NULL, NULL, N'', N'admin', CAST(N'2019-05-06 15:55:47.633' AS DateTime), NULL, NULL, 1)
INSERT [dbo].[TblAttributeConstraint] ([Id], [Name], [ControlType], [ContraintsType], [ContraintsValue], [IsContraintType], [IsContraintValue], [LinkContraints], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete]) VALUES (9, N'test2', NULL, N'Control-02', N'Nam', NULL, NULL, N'GENDER', N'admin', CAST(N'2019-05-07 10:31:09.153' AS DateTime), N'admin', CAST(N'2019-05-07 10:31:26.570' AS DateTime), 1)
INSERT [dbo].[TblAttributeConstraint] ([Id], [Name], [ControlType], [ContraintsType], [ContraintsValue], [IsContraintType], [IsContraintValue], [LinkContraints], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete]) VALUES (10, N'dattest', NULL, N'Control-02', N'Nam', NULL, NULL, N'GENDER', N'admin', CAST(N'2019-05-07 10:49:06.380' AS DateTime), N'admin', CAST(N'2019-05-07 10:51:52.337' AS DateTime), 1)
INSERT [dbo].[TblAttributeConstraint] ([Id], [Name], [ControlType], [ContraintsType], [ContraintsValue], [IsContraintType], [IsContraintValue], [LinkContraints], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete]) VALUES (11, N'datole1', NULL, N'Control-02', N'Nam', NULL, NULL, N'GENDER', N'admin', CAST(N'2019-05-07 10:57:00.587' AS DateTime), NULL, NULL, 1)
INSERT [dbo].[TblAttributeConstraint] ([Id], [Name], [ControlType], [ContraintsType], [ContraintsValue], [IsContraintType], [IsContraintValue], [LinkContraints], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete]) VALUES (12, N'datole1', NULL, N'Control-02', N'Nam', NULL, NULL, N'GENDER', N'admin', CAST(N'2019-05-07 10:57:24.303' AS DateTime), NULL, NULL, 1)
INSERT [dbo].[TblAttributeConstraint] ([Id], [Name], [ControlType], [ContraintsType], [ContraintsValue], [IsContraintType], [IsContraintValue], [LinkContraints], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete]) VALUES (13, N'ádfasdfasdf', NULL, N'Control-02', N'Nam', NULL, NULL, N'GENDER', N'admin', CAST(N'2019-05-07 10:58:43.367' AS DateTime), NULL, NULL, 1)
INSERT [dbo].[TblAttributeConstraint] ([Id], [Name], [ControlType], [ContraintsType], [ContraintsValue], [IsContraintType], [IsContraintValue], [LinkContraints], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete]) VALUES (14, N'1', NULL, N'Control-02', N'Nam', NULL, NULL, N'GENDER', N'admin', CAST(N'2019-05-07 10:59:59.537' AS DateTime), NULL, NULL, 1)
INSERT [dbo].[TblAttributeConstraint] ([Id], [Name], [ControlType], [ContraintsType], [ContraintsValue], [IsContraintType], [IsContraintValue], [LinkContraints], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete]) VALUES (15, N'Hiển thị', NULL, N'Control-02', N'', NULL, NULL, N'', N'admin', CAST(N'2019-05-07 13:43:06.177' AS DateTime), N'admin', CAST(N'2019-05-07 13:43:15.507' AS DateTime), 1)
INSERT [dbo].[TblAttributeConstraint] ([Id], [Name], [ControlType], [ContraintsType], [ContraintsValue], [IsContraintType], [IsContraintValue], [LinkContraints], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete]) VALUES (16, N'dat', NULL, N'Control-02', N'', NULL, NULL, N'', N'admin', CAST(N'2019-05-07 14:11:21.327' AS DateTime), NULL, NULL, 1)
INSERT [dbo].[TblAttributeConstraint] ([Id], [Name], [ControlType], [ContraintsType], [ContraintsValue], [IsContraintType], [IsContraintValue], [LinkContraints], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete]) VALUES (17, N'dat1', NULL, N'Control-02', N'', NULL, NULL, N'', N'admin', CAST(N'2019-05-07 14:12:33.537' AS DateTime), NULL, NULL, 1)
INSERT [dbo].[TblAttributeConstraint] ([Id], [Name], [ControlType], [ContraintsType], [ContraintsValue], [IsContraintType], [IsContraintValue], [LinkContraints], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete]) VALUES (18, N'dat2121212121213', NULL, N'Control-01', N'', NULL, NULL, N'', N'admin', CAST(N'2019-05-07 14:12:47.047' AS DateTime), N'admin', CAST(N'2019-05-07 16:43:36.550' AS DateTime), 1)
INSERT [dbo].[TblAttributeConstraint] ([Id], [Name], [ControlType], [ContraintsType], [ContraintsValue], [IsContraintType], [IsContraintValue], [LinkContraints], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete]) VALUES (19, N'dat3', NULL, N'Control-01', N'Nam', NULL, NULL, N'GENDER', N'admin', CAST(N'2019-05-07 14:16:57.480' AS DateTime), N'admin', CAST(N'2019-05-07 14:18:21.367' AS DateTime), 1)
INSERT [dbo].[TblAttributeConstraint] ([Id], [Name], [ControlType], [ContraintsType], [ContraintsValue], [IsContraintType], [IsContraintValue], [LinkContraints], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete]) VALUES (20, N'dat100455', NULL, N'Control-02', N'', NULL, NULL, N'GENDER', N'admin', CAST(N'2019-05-08 10:08:43.887' AS DateTime), N'agent1', CAST(N'2019-05-21 10:13:08.207' AS DateTime), 1)
INSERT [dbo].[TblAttributeConstraint] ([Id], [Name], [ControlType], [ContraintsType], [ContraintsValue], [IsContraintType], [IsContraintValue], [LinkContraints], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete]) VALUES (21, N'Validate trùng', NULL, N'Object-02', N'', NULL, NULL, N'', N'adminMP', CAST(N'2019-06-03 17:31:06.183' AS DateTime), NULL, NULL, 1)
INSERT [dbo].[TblAttributeConstraint] ([Id], [Name], [ControlType], [ContraintsType], [ContraintsValue], [IsContraintType], [IsContraintValue], [LinkContraints], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete]) VALUES (22, N'Validate bắt buộc MP', NULL, N'Object-01', N'', NULL, NULL, N'', N'adminMP', CAST(N'2019-06-03 17:31:30.097' AS DateTime), NULL, NULL, 1)
INSERT [dbo].[TblAttributeConstraint] ([Id], [Name], [ControlType], [ContraintsType], [ContraintsValue], [IsContraintType], [IsContraintValue], [LinkContraints], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete]) VALUES (23, N'fsdfd', NULL, N'Object-01', N'', NULL, NULL, N'', N'agent1', CAST(N'2019-06-04 20:09:10.857' AS DateTime), NULL, NULL, 1)
INSERT [dbo].[TblAttributeConstraint] ([Id], [Name], [ControlType], [ContraintsType], [ContraintsValue], [IsContraintType], [IsContraintValue], [LinkContraints], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete]) VALUES (24, N'dfgfdg`', NULL, N'Object-03', N'', NULL, NULL, N'', N'agent1', CAST(N'2019-06-04 20:10:21.500' AS DateTime), NULL, NULL, 1)
INSERT [dbo].[TblAttributeConstraint] ([Id], [Name], [ControlType], [ContraintsType], [ContraintsValue], [IsContraintType], [IsContraintValue], [LinkContraints], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete]) VALUES (25, N'fdfgfd', NULL, N'Object-02', N'', NULL, NULL, N'', N'agent1', CAST(N'2019-06-04 20:11:43.127' AS DateTime), NULL, NULL, 1)
SET IDENTITY_INSERT [dbo].[TblAttributeConstraint] OFF
SET IDENTITY_INSERT [dbo].[tblCategory] ON 

INSERT [dbo].[tblCategory] ([ID], [CategoryCode], [CategoryTypeCode], [CategoryName], [CategoryDescription], [ExtContent], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete], [IsActive], [OrderNum]) VALUES (1, N'DataCode-01', N'DataCode', N'NVARCHAR', NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL)
INSERT [dbo].[tblCategory] ([ID], [CategoryCode], [CategoryTypeCode], [CategoryName], [CategoryDescription], [ExtContent], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete], [IsActive], [OrderNum]) VALUES (2, N'DataCode-02', N'DataCode', N'INT', NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL)
INSERT [dbo].[tblCategory] ([ID], [CategoryCode], [CategoryTypeCode], [CategoryName], [CategoryDescription], [ExtContent], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete], [IsActive], [OrderNum]) VALUES (3, N'DataCode-03', N'DataCode', N'BIT', NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL)
INSERT [dbo].[tblCategory] ([ID], [CategoryCode], [CategoryTypeCode], [CategoryName], [CategoryDescription], [ExtContent], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete], [IsActive], [OrderNum]) VALUES (4, N'DataCode-04', N'DataCode', N'DATETIME', NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL)
INSERT [dbo].[tblCategory] ([ID], [CategoryCode], [CategoryTypeCode], [CategoryName], [CategoryDescription], [ExtContent], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete], [IsActive], [OrderNum]) VALUES (5, N'DataCode-05', N'DataCode', N'VARCHAR', NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL)
INSERT [dbo].[tblCategory] ([ID], [CategoryCode], [CategoryTypeCode], [CategoryName], [CategoryDescription], [ExtContent], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete], [IsActive], [OrderNum]) VALUES (6, N'Control-01', N'Control', N'TEXTBOX', NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL)
INSERT [dbo].[tblCategory] ([ID], [CategoryCode], [CategoryTypeCode], [CategoryName], [CategoryDescription], [ExtContent], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete], [IsActive], [OrderNum]) VALUES (7, N'Control-02', N'Control', N'CHECKBOX', NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL)
INSERT [dbo].[tblCategory] ([ID], [CategoryCode], [CategoryTypeCode], [CategoryName], [CategoryDescription], [ExtContent], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete], [IsActive], [OrderNum]) VALUES (8, N'Control-03', N'Control', N'TEXTAREA', NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL)
INSERT [dbo].[tblCategory] ([ID], [CategoryCode], [CategoryTypeCode], [CategoryName], [CategoryDescription], [ExtContent], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete], [IsActive], [OrderNum]) VALUES (9, N'Control-04', N'Control', N'DROPDOWNLIST', NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL)
INSERT [dbo].[tblCategory] ([ID], [CategoryCode], [CategoryTypeCode], [CategoryName], [CategoryDescription], [ExtContent], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete], [IsActive], [OrderNum]) VALUES (10, N'Control-05', N'Control', N'LISTCHECKBOX', NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL)
INSERT [dbo].[tblCategory] ([ID], [CategoryCode], [CategoryTypeCode], [CategoryName], [CategoryDescription], [ExtContent], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete], [IsActive], [OrderNum]) VALUES (11, N'Control-06', N'Control', N'RADIO', NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL)
INSERT [dbo].[tblCategory] ([ID], [CategoryCode], [CategoryTypeCode], [CategoryName], [CategoryDescription], [ExtContent], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete], [IsActive], [OrderNum]) VALUES (12, N'Control-07', N'Control', N'LISTRADIO', NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL)
INSERT [dbo].[tblCategory] ([ID], [CategoryCode], [CategoryTypeCode], [CategoryName], [CategoryDescription], [ExtContent], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete], [IsActive], [OrderNum]) VALUES (13, N'FormType-01', N'FormType', N'Thêm mới Form', NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL)
INSERT [dbo].[tblCategory] ([ID], [CategoryCode], [CategoryTypeCode], [CategoryName], [CategoryDescription], [ExtContent], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete], [IsActive], [OrderNum]) VALUES (14, N'FormType-02', N'FormType', N'Chỉnh sửa Form', NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL)
INSERT [dbo].[tblCategory] ([ID], [CategoryCode], [CategoryTypeCode], [CategoryName], [CategoryDescription], [ExtContent], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete], [IsActive], [OrderNum]) VALUES (15, N'FormType-03', N'FormType', N'Form tìm kiếm', NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL)
INSERT [dbo].[tblCategory] ([ID], [CategoryCode], [CategoryTypeCode], [CategoryName], [CategoryDescription], [ExtContent], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete], [IsActive], [OrderNum]) VALUES (16, N'GENDER-01', N'GENDER', N'Nam', NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL)
INSERT [dbo].[tblCategory] ([ID], [CategoryCode], [CategoryTypeCode], [CategoryName], [CategoryDescription], [ExtContent], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete], [IsActive], [OrderNum]) VALUES (17, N'GENDER-02', N'GENDER', N'Nữ', NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL)
INSERT [dbo].[tblCategory] ([ID], [CategoryCode], [CategoryTypeCode], [CategoryName], [CategoryDescription], [ExtContent], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete], [IsActive], [OrderNum]) VALUES (18, N'GENDER', NULL, N'Chọn giới tính', NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL)
INSERT [dbo].[tblCategory] ([ID], [CategoryCode], [CategoryTypeCode], [CategoryName], [CategoryDescription], [ExtContent], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete], [IsActive], [OrderNum]) VALUES (19, N'GENDER-03', N'GENDER', N'khác', NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL)
INSERT [dbo].[tblCategory] ([ID], [CategoryCode], [CategoryTypeCode], [CategoryName], [CategoryDescription], [ExtContent], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete], [IsActive], [OrderNum]) VALUES (20, N'DepartmentCode', NULL, N'Phòng ban', NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL)
INSERT [dbo].[tblCategory] ([ID], [CategoryCode], [CategoryTypeCode], [CategoryName], [CategoryDescription], [ExtContent], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete], [IsActive], [OrderNum]) VALUES (21, N'Department-01', N'DepartmentCode', N'Phòng Kế Toán', NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL)
INSERT [dbo].[tblCategory] ([ID], [CategoryCode], [CategoryTypeCode], [CategoryName], [CategoryDescription], [ExtContent], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete], [IsActive], [OrderNum]) VALUES (22, N'Department-02', N'DepartmentCode', N'Phòng Kinh Doanh', NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL)
INSERT [dbo].[tblCategory] ([ID], [CategoryCode], [CategoryTypeCode], [CategoryName], [CategoryDescription], [ExtContent], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete], [IsActive], [OrderNum]) VALUES (23, N'Department-03', N'DepartmentCode', N'Trung Tâm VAS', NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL)
INSERT [dbo].[tblCategory] ([ID], [CategoryCode], [CategoryTypeCode], [CategoryName], [CategoryDescription], [ExtContent], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete], [IsActive], [OrderNum]) VALUES (34, N'DistrictCode-1', N'CityCode-1', N'Quận thanh xuân', NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL)
INSERT [dbo].[tblCategory] ([ID], [CategoryCode], [CategoryTypeCode], [CategoryName], [CategoryDescription], [ExtContent], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete], [IsActive], [OrderNum]) VALUES (35, N'DistrictCode-2', N'CityCode-1', N'Quận cầu giấy', NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL)
INSERT [dbo].[tblCategory] ([ID], [CategoryCode], [CategoryTypeCode], [CategoryName], [CategoryDescription], [ExtContent], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete], [IsActive], [OrderNum]) VALUES (195, N'THANHPHO', N'', N'Thành phố', N'', NULL, N'adminMP', CAST(N'2019-06-06 15:11:36.900' AS DateTime), NULL, NULL, 0, NULL, NULL)
INSERT [dbo].[tblCategory] ([ID], [CategoryCode], [CategoryTypeCode], [CategoryName], [CategoryDescription], [ExtContent], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete], [IsActive], [OrderNum]) VALUES (196, N'HANOI1', N'THANHPHO', N'Hà Nội', NULL, N'', NULL, CAST(N'2019-06-06 15:11:36.917' AS DateTime), NULL, NULL, 0, NULL, NULL)
INSERT [dbo].[tblCategory] ([ID], [CategoryCode], [CategoryTypeCode], [CategoryName], [CategoryDescription], [ExtContent], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete], [IsActive], [OrderNum]) VALUES (214, N'56', N'', N'56', N'', NULL, N'adminMP', CAST(N'2019-06-06 16:33:59.890' AS DateTime), NULL, NULL, 0, NULL, NULL)
INSERT [dbo].[tblCategory] ([ID], [CategoryCode], [CategoryTypeCode], [CategoryName], [CategoryDescription], [ExtContent], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete], [IsActive], [OrderNum]) VALUES (215, N'556', N'56', N'556', NULL, N'', NULL, CAST(N'2019-06-06 16:33:59.910' AS DateTime), NULL, NULL, 0, NULL, NULL)
INSERT [dbo].[tblCategory] ([ID], [CategoryCode], [CategoryTypeCode], [CategoryName], [CategoryDescription], [ExtContent], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete], [IsActive], [OrderNum]) VALUES (222, N'4523', N'', N'4523', N'', NULL, N'adminMP', CAST(N'2019-06-06 16:35:30.147' AS DateTime), NULL, NULL, 0, NULL, NULL)
INSERT [dbo].[tblCategory] ([ID], [CategoryCode], [CategoryTypeCode], [CategoryName], [CategoryDescription], [ExtContent], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete], [IsActive], [OrderNum]) VALUES (223, N'11', N'4523', N'1', NULL, N'', NULL, CAST(N'2019-06-06 16:35:30.153' AS DateTime), NULL, NULL, 0, NULL, NULL)
INSERT [dbo].[tblCategory] ([ID], [CategoryCode], [CategoryTypeCode], [CategoryName], [CategoryDescription], [ExtContent], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete], [IsActive], [OrderNum]) VALUES (224, N'21', N'4523', N'2', NULL, N'', NULL, CAST(N'2019-06-06 16:35:30.157' AS DateTime), NULL, NULL, 0, NULL, NULL)
INSERT [dbo].[tblCategory] ([ID], [CategoryCode], [CategoryTypeCode], [CategoryName], [CategoryDescription], [ExtContent], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete], [IsActive], [OrderNum]) VALUES (225, N'31', N'4523', N'3', NULL, N'', NULL, CAST(N'2019-06-06 16:35:30.160' AS DateTime), NULL, NULL, 0, NULL, NULL)
INSERT [dbo].[tblCategory] ([ID], [CategoryCode], [CategoryTypeCode], [CategoryName], [CategoryDescription], [ExtContent], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete], [IsActive], [OrderNum]) VALUES (226, N'41', N'4523', N'4', NULL, N'', NULL, CAST(N'2019-06-06 16:35:30.163' AS DateTime), NULL, NULL, 0, NULL, NULL)
INSERT [dbo].[tblCategory] ([ID], [CategoryCode], [CategoryTypeCode], [CategoryName], [CategoryDescription], [ExtContent], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete], [IsActive], [OrderNum]) VALUES (227, N'51', N'4523', N'5', NULL, N'', NULL, CAST(N'2019-06-06 16:35:30.163' AS DateTime), NULL, NULL, 0, NULL, NULL)
INSERT [dbo].[tblCategory] ([ID], [CategoryCode], [CategoryTypeCode], [CategoryName], [CategoryDescription], [ExtContent], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete], [IsActive], [OrderNum]) VALUES (228, N'6', N'4523', N'6', NULL, N'', NULL, CAST(N'2019-06-06 16:35:30.167' AS DateTime), NULL, NULL, 0, NULL, NULL)
INSERT [dbo].[tblCategory] ([ID], [CategoryCode], [CategoryTypeCode], [CategoryName], [CategoryDescription], [ExtContent], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete], [IsActive], [OrderNum]) VALUES (229, N'7', N'4523', N'7', NULL, N'', NULL, CAST(N'2019-06-06 16:35:30.170' AS DateTime), NULL, NULL, 0, NULL, NULL)
INSERT [dbo].[tblCategory] ([ID], [CategoryCode], [CategoryTypeCode], [CategoryName], [CategoryDescription], [ExtContent], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete], [IsActive], [OrderNum]) VALUES (230, N'8', N'4523', N'8', NULL, N'', NULL, CAST(N'2019-06-06 16:35:30.173' AS DateTime), NULL, NULL, 0, NULL, NULL)
INSERT [dbo].[tblCategory] ([ID], [CategoryCode], [CategoryTypeCode], [CategoryName], [CategoryDescription], [ExtContent], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete], [IsActive], [OrderNum]) VALUES (231, N'9', N'4523', N'9', NULL, N'', NULL, CAST(N'2019-06-06 16:35:30.177' AS DateTime), NULL, NULL, 0, NULL, NULL)
INSERT [dbo].[tblCategory] ([ID], [CategoryCode], [CategoryTypeCode], [CategoryName], [CategoryDescription], [ExtContent], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete], [IsActive], [OrderNum]) VALUES (232, N'10', N'4523', N'10', NULL, N'', NULL, CAST(N'2019-06-06 16:35:30.180' AS DateTime), NULL, NULL, 0, NULL, NULL)
INSERT [dbo].[tblCategory] ([ID], [CategoryCode], [CategoryTypeCode], [CategoryName], [CategoryDescription], [ExtContent], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete], [IsActive], [OrderNum]) VALUES (233, N'111', N'4523', N'11', NULL, N'', NULL, CAST(N'2019-06-06 16:35:30.183' AS DateTime), NULL, NULL, 0, NULL, NULL)
INSERT [dbo].[tblCategory] ([ID], [CategoryCode], [CategoryTypeCode], [CategoryName], [CategoryDescription], [ExtContent], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete], [IsActive], [OrderNum]) VALUES (234, N'12', N'4523', N'12', NULL, N'', NULL, CAST(N'2019-06-06 16:35:30.187' AS DateTime), NULL, NULL, 0, NULL, NULL)
INSERT [dbo].[tblCategory] ([ID], [CategoryCode], [CategoryTypeCode], [CategoryName], [CategoryDescription], [ExtContent], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete], [IsActive], [OrderNum]) VALUES (235, N'13', N'4523', N'13', NULL, N'', NULL, CAST(N'2019-06-06 16:35:30.190' AS DateTime), NULL, NULL, 0, NULL, NULL)
INSERT [dbo].[tblCategory] ([ID], [CategoryCode], [CategoryTypeCode], [CategoryName], [CategoryDescription], [ExtContent], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete], [IsActive], [OrderNum]) VALUES (236, N'14', N'4523', N'14', NULL, N'', NULL, CAST(N'2019-06-06 16:35:30.193' AS DateTime), NULL, NULL, 0, NULL, NULL)
INSERT [dbo].[tblCategory] ([ID], [CategoryCode], [CategoryTypeCode], [CategoryName], [CategoryDescription], [ExtContent], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete], [IsActive], [OrderNum]) VALUES (238, N'4454', N'', N'4454', N'', NULL, N'adminMP', CAST(N'2019-06-06 16:43:20.330' AS DateTime), NULL, NULL, 0, NULL, NULL)
INSERT [dbo].[tblCategory] ([ID], [CategoryCode], [CategoryTypeCode], [CategoryName], [CategoryDescription], [ExtContent], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete], [IsActive], [OrderNum]) VALUES (239, N'3242', N'4454', N'3242', NULL, N'', NULL, CAST(N'2019-06-06 16:43:20.337' AS DateTime), NULL, NULL, 0, NULL, NULL)
INSERT [dbo].[tblCategory] ([ID], [CategoryCode], [CategoryTypeCode], [CategoryName], [CategoryDescription], [ExtContent], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete], [IsActive], [OrderNum]) VALUES (240, N'34532', N'', N'34532', N'4534', NULL, N'adminMP', CAST(N'2019-06-06 16:43:26.397' AS DateTime), NULL, NULL, 0, NULL, NULL)
INSERT [dbo].[tblCategory] ([ID], [CategoryCode], [CategoryTypeCode], [CategoryName], [CategoryDescription], [ExtContent], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete], [IsActive], [OrderNum]) VALUES (241, N'3525', N'34532', N'3525', NULL, N'', NULL, CAST(N'2019-06-06 16:43:26.403' AS DateTime), NULL, NULL, 0, NULL, NULL)
INSERT [dbo].[tblCategory] ([ID], [CategoryCode], [CategoryTypeCode], [CategoryName], [CategoryDescription], [ExtContent], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete], [IsActive], [OrderNum]) VALUES (242, N'43254', N'', N'43254', N'Nhập text vào ô bên dưới để biết được đoạn text của bạn có bao nhiêu ký tự.

', NULL, N'adminMP', CAST(N'2019-06-06 16:43:39.367' AS DateTime), NULL, NULL, 0, NULL, NULL)
INSERT [dbo].[tblCategory] ([ID], [CategoryCode], [CategoryTypeCode], [CategoryName], [CategoryDescription], [ExtContent], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete], [IsActive], [OrderNum]) VALUES (243, N'452345', N'43254', N'452345', NULL, N'', NULL, CAST(N'2019-06-06 16:43:39.370' AS DateTime), NULL, NULL, 0, NULL, NULL)
INSERT [dbo].[tblCategory] ([ID], [CategoryCode], [CategoryTypeCode], [CategoryName], [CategoryDescription], [ExtContent], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete], [IsActive], [OrderNum]) VALUES (263, N'1', N'DANHMUC', N'1', NULL, N'', NULL, CAST(N'2019-06-06 17:52:04.553' AS DateTime), NULL, NULL, 0, NULL, NULL)
INSERT [dbo].[tblCategory] ([ID], [CategoryCode], [CategoryTypeCode], [CategoryName], [CategoryDescription], [ExtContent], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete], [IsActive], [OrderNum]) VALUES (268, N'TAOMOIDM', N'', N'tạo mới DM', N'', NULL, N'adminMP', CAST(N'2019-06-07 09:13:33.910' AS DateTime), NULL, NULL, 0, NULL, NULL)
INSERT [dbo].[tblCategory] ([ID], [CategoryCode], [CategoryTypeCode], [CategoryName], [CategoryDescription], [ExtContent], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete], [IsActive], [OrderNum]) VALUES (269, N'1111', N'TAOMOIDM', N'111', NULL, N'', NULL, CAST(N'2019-06-07 09:13:33.917' AS DateTime), NULL, NULL, 0, NULL, NULL)
INSERT [dbo].[tblCategory] ([ID], [CategoryCode], [CategoryTypeCode], [CategoryName], [CategoryDescription], [ExtContent], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete], [IsActive], [OrderNum]) VALUES (270, N'TESTBA', N'', N'Test bA', N'Test', NULL, N'adminMP', CAST(N'2019-06-07 16:00:33.857' AS DateTime), NULL, NULL, 0, NULL, NULL)
INSERT [dbo].[tblCategory] ([ID], [CategoryCode], [CategoryTypeCode], [CategoryName], [CategoryDescription], [ExtContent], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete], [IsActive], [OrderNum]) VALUES (271, N'TEST', N'TESTBA', N'Test', N'Test', N'', NULL, CAST(N'2019-06-07 16:00:33.977' AS DateTime), NULL, NULL, 0, NULL, NULL)
SET IDENTITY_INSERT [dbo].[tblCategory] OFF
SET IDENTITY_INSERT [dbo].[tblCategoryGroup] ON 

INSERT [dbo].[tblCategoryGroup] ([ID], [CategoryCode], [CategoryTypeCode], [CategoryGroupName], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete], [CategoryDescription]) VALUES (40, N'DANHMUC', N'GENDER', N'danh mục', N'adminMP', CAST(N'2019-06-06 17:52:04.473' AS DateTime), NULL, NULL, 0, N'')
SET IDENTITY_INSERT [dbo].[tblCategoryGroup] OFF
SET IDENTITY_INSERT [dbo].[tblCIMSAttributeForm] ON 

INSERT [dbo].[tblCIMSAttributeForm] ([AttributeFormId], [FormId], [AttributeId], [AttributeCode], [AttributeType], [AttrOrder], [AttributeColumn], [RowIndex], [RowTitle], [ChildCode], [IsShowLabel], [DefaultValue]) VALUES (173, 27, NULL, N'ATTRIBUTE1', N'TEXTBOX', 1, 6, 1, N'', N'CIMS_ADD', NULL, N'0')
INSERT [dbo].[tblCIMSAttributeForm] ([AttributeFormId], [FormId], [AttributeId], [AttributeCode], [AttributeType], [AttrOrder], [AttributeColumn], [RowIndex], [RowTitle], [ChildCode], [IsShowLabel], [DefaultValue]) VALUES (174, 27, NULL, N'ATTRIBUTE2', N'TEXTBOX', 2, 6, 1, N'', N'CIMS_ADD', NULL, NULL)
INSERT [dbo].[tblCIMSAttributeForm] ([AttributeFormId], [FormId], [AttributeId], [AttributeCode], [AttributeType], [AttrOrder], [AttributeColumn], [RowIndex], [RowTitle], [ChildCode], [IsShowLabel], [DefaultValue]) VALUES (175, 27, NULL, N'ATTRIBUTE6', N'LISTRADIO', 1, 6, 2, N'', N'CIMS_ADD', NULL, N'GENDER-01')
INSERT [dbo].[tblCIMSAttributeForm] ([AttributeFormId], [FormId], [AttributeId], [AttributeCode], [AttributeType], [AttrOrder], [AttributeColumn], [RowIndex], [RowTitle], [ChildCode], [IsShowLabel], [DefaultValue]) VALUES (176, 27, NULL, N'ATTRIBUTE3', N'TEXTBOX', 2, 6, 2, N'', N'CIMS_ADD', NULL, NULL)
INSERT [dbo].[tblCIMSAttributeForm] ([AttributeFormId], [FormId], [AttributeId], [AttributeCode], [AttributeType], [AttrOrder], [AttributeColumn], [RowIndex], [RowTitle], [ChildCode], [IsShowLabel], [DefaultValue]) VALUES (177, 27, NULL, N'ATTRIBUTE4', N'TEXTAREA', 1, 12, 3, N'', N'CIMS_ADD', NULL, NULL)
INSERT [dbo].[tblCIMSAttributeForm] ([AttributeFormId], [FormId], [AttributeId], [AttributeCode], [AttributeType], [AttrOrder], [AttributeColumn], [RowIndex], [RowTitle], [ChildCode], [IsShowLabel], [DefaultValue]) VALUES (178, 27, NULL, N'ATTRIBUTE5', N'DROPDOWNLIST', 1, 6, 4, N'', N'CIMS_ADD', NULL, N'0')
SET IDENTITY_INSERT [dbo].[tblCIMSAttributeForm] OFF
SET IDENTITY_INSERT [dbo].[tblCIMSAttributeValue] ON 

INSERT [dbo].[tblCIMSAttributeValue] ([AttributesValueId], [AttributeId], [AttributeValue], [IsDelete], [AttributeCode], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [RecordId], [Module]) VALUES (294, NULL, N'', 0, N'ATTRIBUTE1                                                                                          ', CAST(N'2019-06-06 16:22:30.867' AS DateTime), NULL, CAST(N'2019-06-07 16:11:47.387' AS DateTime), NULL, N'201906061622307519483', N'CIMS')
INSERT [dbo].[tblCIMSAttributeValue] ([AttributesValueId], [AttributeId], [AttributeValue], [IsDelete], [AttributeCode], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [RecordId], [Module]) VALUES (295, NULL, N'Bùi Hữu Đại', 0, N'ATTRIBUTE2                                                                                          ', CAST(N'2019-06-06 16:22:30.870' AS DateTime), NULL, CAST(N'2019-06-07 16:11:47.440' AS DateTime), NULL, N'201906061622307519483', N'CIMS')
INSERT [dbo].[tblCIMSAttributeValue] ([AttributesValueId], [AttributeId], [AttributeValue], [IsDelete], [AttributeCode], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [RecordId], [Module]) VALUES (296, NULL, N'1234567895', 0, N'ATTRIBUTE3                                                                                          ', CAST(N'2019-06-06 16:22:30.873' AS DateTime), NULL, CAST(N'2019-06-07 15:42:14.330' AS DateTime), NULL, N'201906061622307519483', N'CIMS')
INSERT [dbo].[tblCIMSAttributeValue] ([AttributesValueId], [AttributeId], [AttributeValue], [IsDelete], [AttributeCode], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [RecordId], [Module]) VALUES (297, NULL, N'aaaaaaYYYbbbbbbb', 0, N'ATTRIBUTE4                                                                                          ', CAST(N'2019-06-06 16:22:30.877' AS DateTime), NULL, CAST(N'2019-06-07 16:14:25.407' AS DateTime), NULL, N'201906061622307519483', N'CIMS')
INSERT [dbo].[tblCIMSAttributeValue] ([AttributesValueId], [AttributeId], [AttributeValue], [IsDelete], [AttributeCode], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [RecordId], [Module]) VALUES (298, NULL, N'Department-01', 0, N'ATTRIBUTE5                                                                                          ', CAST(N'2019-06-06 16:22:30.880' AS DateTime), NULL, CAST(N'2019-06-07 16:14:25.420' AS DateTime), NULL, N'201906061622307519483', N'CIMS')
INSERT [dbo].[tblCIMSAttributeValue] ([AttributesValueId], [AttributeId], [AttributeValue], [IsDelete], [AttributeCode], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [RecordId], [Module]) VALUES (299, NULL, N'', 0, N'ATTRIBUTE6                                                                                          ', CAST(N'2019-06-06 16:22:30.710' AS DateTime), NULL, NULL, NULL, N'201906061622307519483', N'CIMS')
INSERT [dbo].[tblCIMSAttributeValue] ([AttributesValueId], [AttributeId], [AttributeValue], [IsDelete], [AttributeCode], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [RecordId], [Module]) VALUES (300, NULL, N'CUS.201906061725196913331', 0, N'ATTRIBUTE1                                                                                          ', CAST(N'2019-06-06 17:24:22.423' AS DateTime), NULL, CAST(N'2019-06-07 14:58:29.470' AS DateTime), NULL, N'201906061724222980559', N'CIMS')
INSERT [dbo].[tblCIMSAttributeValue] ([AttributesValueId], [AttributeId], [AttributeValue], [IsDelete], [AttributeCode], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [RecordId], [Module]) VALUES (301, NULL, N'Vương Thị Xuân Nụ', 0, N'ATTRIBUTE2                                                                                          ', CAST(N'2019-06-06 17:24:22.430' AS DateTime), NULL, CAST(N'2019-06-07 14:58:29.480' AS DateTime), NULL, N'201906061724222980559', N'CIMS')
INSERT [dbo].[tblCIMSAttributeValue] ([AttributesValueId], [AttributeId], [AttributeValue], [IsDelete], [AttributeCode], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [RecordId], [Module]) VALUES (302, NULL, N'1234567890', 0, N'ATTRIBUTE3                                                                                          ', CAST(N'2019-06-06 17:24:22.433' AS DateTime), NULL, CAST(N'2019-06-07 14:58:29.493' AS DateTime), NULL, N'201906061724222980559', N'CIMS')
INSERT [dbo].[tblCIMSAttributeValue] ([AttributesValueId], [AttributeId], [AttributeValue], [IsDelete], [AttributeCode], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [RecordId], [Module]) VALUES (303, NULL, N'', 0, N'ATTRIBUTE4                                                                                          ', CAST(N'2019-06-06 17:24:22.437' AS DateTime), NULL, CAST(N'2019-06-07 14:58:29.503' AS DateTime), NULL, N'201906061724222980559', N'CIMS')
INSERT [dbo].[tblCIMSAttributeValue] ([AttributesValueId], [AttributeId], [AttributeValue], [IsDelete], [AttributeCode], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [RecordId], [Module]) VALUES (304, NULL, N'Department-01', 0, N'ATTRIBUTE5                                                                                          ', CAST(N'2019-06-06 17:24:22.443' AS DateTime), NULL, CAST(N'2019-06-07 14:58:29.513' AS DateTime), NULL, N'201906061724222980559', N'CIMS')
INSERT [dbo].[tblCIMSAttributeValue] ([AttributesValueId], [AttributeId], [AttributeValue], [IsDelete], [AttributeCode], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [RecordId], [Module]) VALUES (305, NULL, N'201906061622307519483', 0, N'RecordId                                                                                            ', CAST(N'2019-06-06 17:24:22.447' AS DateTime), NULL, NULL, NULL, N'201906061724222980559', N'CIMS')
INSERT [dbo].[tblCIMSAttributeValue] ([AttributesValueId], [AttributeId], [AttributeValue], [IsDelete], [AttributeCode], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [RecordId], [Module]) VALUES (306, NULL, N'', 0, N'ATTRIBUTE6                                                                                          ', CAST(N'2019-06-06 17:24:22.303' AS DateTime), NULL, NULL, NULL, N'201906061724222980559', N'CIMS')
INSERT [dbo].[tblCIMSAttributeValue] ([AttributesValueId], [AttributeId], [AttributeValue], [IsDelete], [AttributeCode], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [RecordId], [Module]) VALUES (307, NULL, N'CUS.201906061724224232602', 0, N'ATTRIBUTE1                                                                                          ', CAST(N'2019-06-06 17:24:33.203' AS DateTime), NULL, CAST(N'2019-06-07 12:27:45.017' AS DateTime), NULL, N'201906061724331908426', N'CIMS')
INSERT [dbo].[tblCIMSAttributeValue] ([AttributesValueId], [AttributeId], [AttributeValue], [IsDelete], [AttributeCode], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [RecordId], [Module]) VALUES (308, NULL, N'Lâm Đức Đạt', 0, N'ATTRIBUTE2                                                                                          ', CAST(N'2019-06-06 17:24:33.213' AS DateTime), NULL, CAST(N'2019-06-07 12:27:45.027' AS DateTime), NULL, N'201906061724331908426', N'CIMS')
INSERT [dbo].[tblCIMSAttributeValue] ([AttributesValueId], [AttributeId], [AttributeValue], [IsDelete], [AttributeCode], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [RecordId], [Module]) VALUES (309, NULL, N'1234567893', 0, N'ATTRIBUTE3                                                                                          ', CAST(N'2019-06-06 17:24:33.217' AS DateTime), NULL, CAST(N'2019-06-07 12:27:45.040' AS DateTime), NULL, N'201906061724331908426', N'CIMS')
INSERT [dbo].[tblCIMSAttributeValue] ([AttributesValueId], [AttributeId], [AttributeValue], [IsDelete], [AttributeCode], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [RecordId], [Module]) VALUES (310, NULL, N'', 0, N'ATTRIBUTE4                                                                                          ', CAST(N'2019-06-06 17:24:33.223' AS DateTime), NULL, CAST(N'2019-06-07 12:27:45.053' AS DateTime), NULL, N'201906061724331908426', N'CIMS')
INSERT [dbo].[tblCIMSAttributeValue] ([AttributesValueId], [AttributeId], [AttributeValue], [IsDelete], [AttributeCode], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [RecordId], [Module]) VALUES (311, NULL, N'Department-01', 0, N'ATTRIBUTE5                                                                                          ', CAST(N'2019-06-06 17:24:33.227' AS DateTime), NULL, CAST(N'2019-06-07 12:27:45.067' AS DateTime), NULL, N'201906061724331908426', N'CIMS')
INSERT [dbo].[tblCIMSAttributeValue] ([AttributesValueId], [AttributeId], [AttributeValue], [IsDelete], [AttributeCode], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [RecordId], [Module]) VALUES (312, NULL, N'201906061622307519483', 0, N'RecordId                                                                                            ', CAST(N'2019-06-06 17:24:33.233' AS DateTime), NULL, NULL, NULL, N'201906061724331908426', N'CIMS')
INSERT [dbo].[tblCIMSAttributeValue] ([AttributesValueId], [AttributeId], [AttributeValue], [IsDelete], [AttributeCode], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [RecordId], [Module]) VALUES (313, NULL, N'', 0, N'ATTRIBUTE6                                                                                          ', CAST(N'2019-06-06 17:24:33.070' AS DateTime), NULL, NULL, NULL, N'201906061724331908426', N'CIMS')
INSERT [dbo].[tblCIMSAttributeValue] ([AttributesValueId], [AttributeId], [AttributeValue], [IsDelete], [AttributeCode], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [RecordId], [Module]) VALUES (314, NULL, N'CUS.201906061724332042564', 0, N'ATTRIBUTE1                                                                                          ', CAST(N'2019-06-06 17:25:19.690' AS DateTime), NULL, CAST(N'2019-06-07 12:27:55.260' AS DateTime), NULL, N'201906061725196827380', N'CIMS')
INSERT [dbo].[tblCIMSAttributeValue] ([AttributesValueId], [AttributeId], [AttributeValue], [IsDelete], [AttributeCode], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [RecordId], [Module]) VALUES (315, NULL, N'Phùng Văn Vinh', 0, N'ATTRIBUTE2                                                                                          ', CAST(N'2019-06-06 17:25:19.700' AS DateTime), NULL, CAST(N'2019-06-07 12:27:55.273' AS DateTime), NULL, N'201906061725196827380', N'CIMS')
INSERT [dbo].[tblCIMSAttributeValue] ([AttributesValueId], [AttributeId], [AttributeValue], [IsDelete], [AttributeCode], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [RecordId], [Module]) VALUES (316, NULL, N'1234567895', 0, N'ATTRIBUTE3                                                                                          ', CAST(N'2019-06-06 17:25:19.707' AS DateTime), NULL, CAST(N'2019-06-07 12:27:55.287' AS DateTime), NULL, N'201906061725196827380', N'CIMS')
INSERT [dbo].[tblCIMSAttributeValue] ([AttributesValueId], [AttributeId], [AttributeValue], [IsDelete], [AttributeCode], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [RecordId], [Module]) VALUES (317, NULL, N'', 0, N'ATTRIBUTE4                                                                                          ', CAST(N'2019-06-06 17:25:19.717' AS DateTime), NULL, CAST(N'2019-06-07 12:27:55.297' AS DateTime), NULL, N'201906061725196827380', N'CIMS')
INSERT [dbo].[tblCIMSAttributeValue] ([AttributesValueId], [AttributeId], [AttributeValue], [IsDelete], [AttributeCode], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [RecordId], [Module]) VALUES (318, NULL, N'Department-01', 0, N'ATTRIBUTE5                                                                                          ', CAST(N'2019-06-06 17:25:19.720' AS DateTime), NULL, CAST(N'2019-06-07 12:27:55.307' AS DateTime), NULL, N'201906061725196827380', N'CIMS')
INSERT [dbo].[tblCIMSAttributeValue] ([AttributesValueId], [AttributeId], [AttributeValue], [IsDelete], [AttributeCode], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [RecordId], [Module]) VALUES (319, NULL, N'201906061622307519483', 0, N'RecordId                                                                                            ', CAST(N'2019-06-06 17:25:19.727' AS DateTime), NULL, NULL, NULL, N'201906061725196827380', N'CIMS')
INSERT [dbo].[tblCIMSAttributeValue] ([AttributesValueId], [AttributeId], [AttributeValue], [IsDelete], [AttributeCode], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [RecordId], [Module]) VALUES (320, NULL, N'', 0, N'ATTRIBUTE6                                                                                          ', CAST(N'2019-06-06 17:25:19.563' AS DateTime), NULL, NULL, NULL, N'201906061725196827380', N'CIMS')
INSERT [dbo].[tblCIMSAttributeValue] ([AttributesValueId], [AttributeId], [AttributeValue], [IsDelete], [AttributeCode], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [RecordId], [Module]) VALUES (321, NULL, N'CUS.201906061725196913331', 0, N'ATTRIBUTE1                                                                                          ', CAST(N'2019-06-06 17:25:25.133' AS DateTime), NULL, CAST(N'2019-06-07 12:30:26.813' AS DateTime), NULL, N'201906061725251253740', N'CIMS')
INSERT [dbo].[tblCIMSAttributeValue] ([AttributesValueId], [AttributeId], [AttributeValue], [IsDelete], [AttributeCode], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [RecordId], [Module]) VALUES (322, NULL, N'Phạm Đức Hiệp', 0, N'ATTRIBUTE2                                                                                          ', CAST(N'2019-06-06 17:25:25.137' AS DateTime), NULL, CAST(N'2019-06-07 12:30:26.823' AS DateTime), NULL, N'201906061725251253740', N'CIMS')
INSERT [dbo].[tblCIMSAttributeValue] ([AttributesValueId], [AttributeId], [AttributeValue], [IsDelete], [AttributeCode], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [RecordId], [Module]) VALUES (323, NULL, N'1234567890', 0, N'ATTRIBUTE3                                                                                          ', CAST(N'2019-06-06 17:25:25.143' AS DateTime), NULL, CAST(N'2019-06-07 12:30:26.840' AS DateTime), NULL, N'201906061725251253740', N'CIMS')
INSERT [dbo].[tblCIMSAttributeValue] ([AttributesValueId], [AttributeId], [AttributeValue], [IsDelete], [AttributeCode], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [RecordId], [Module]) VALUES (324, NULL, N'', 0, N'ATTRIBUTE4                                                                                          ', CAST(N'2019-06-06 17:25:25.147' AS DateTime), NULL, CAST(N'2019-06-07 12:30:26.847' AS DateTime), NULL, N'201906061725251253740', N'CIMS')
INSERT [dbo].[tblCIMSAttributeValue] ([AttributesValueId], [AttributeId], [AttributeValue], [IsDelete], [AttributeCode], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [RecordId], [Module]) VALUES (325, NULL, N'Department-01', 0, N'ATTRIBUTE5                                                                                          ', CAST(N'2019-06-06 17:25:25.153' AS DateTime), NULL, CAST(N'2019-06-07 12:30:26.857' AS DateTime), NULL, N'201906061725251253740', N'CIMS')
INSERT [dbo].[tblCIMSAttributeValue] ([AttributesValueId], [AttributeId], [AttributeValue], [IsDelete], [AttributeCode], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [RecordId], [Module]) VALUES (326, NULL, N'201906061622307519483', 0, N'RecordId                                                                                            ', CAST(N'2019-06-06 17:25:25.157' AS DateTime), NULL, NULL, NULL, N'201906061725251253740', N'CIMS')
INSERT [dbo].[tblCIMSAttributeValue] ([AttributesValueId], [AttributeId], [AttributeValue], [IsDelete], [AttributeCode], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [RecordId], [Module]) VALUES (327, NULL, N'', 0, N'ATTRIBUTE6                                                                                          ', CAST(N'2019-06-06 17:25:25.000' AS DateTime), NULL, NULL, NULL, N'201906061725251253740', N'CIMS')
INSERT [dbo].[tblCIMSAttributeValue] ([AttributesValueId], [AttributeId], [AttributeValue], [IsDelete], [AttributeCode], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [RecordId], [Module]) VALUES (328, NULL, N'CUS.201906061725251317152', 0, N'ATTRIBUTE1                                                                                          ', CAST(N'2019-06-06 17:25:47.133' AS DateTime), NULL, CAST(N'2019-06-07 12:31:08.973' AS DateTime), NULL, N'201906061725471250219', N'CIMS')
INSERT [dbo].[tblCIMSAttributeValue] ([AttributesValueId], [AttributeId], [AttributeValue], [IsDelete], [AttributeCode], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [RecordId], [Module]) VALUES (329, NULL, N'Nguyễn Trung Cương', 0, N'ATTRIBUTE2                                                                                          ', CAST(N'2019-06-06 17:25:47.137' AS DateTime), NULL, CAST(N'2019-06-07 12:31:08.983' AS DateTime), NULL, N'201906061725471250219', N'CIMS')
INSERT [dbo].[tblCIMSAttributeValue] ([AttributesValueId], [AttributeId], [AttributeValue], [IsDelete], [AttributeCode], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [RecordId], [Module]) VALUES (330, NULL, N'1234567896', 0, N'ATTRIBUTE3                                                                                          ', CAST(N'2019-06-06 17:25:47.143' AS DateTime), NULL, CAST(N'2019-06-07 12:31:08.997' AS DateTime), NULL, N'201906061725471250219', N'CIMS')
INSERT [dbo].[tblCIMSAttributeValue] ([AttributesValueId], [AttributeId], [AttributeValue], [IsDelete], [AttributeCode], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [RecordId], [Module]) VALUES (331, NULL, N'', 0, N'ATTRIBUTE4                                                                                          ', CAST(N'2019-06-06 17:25:47.147' AS DateTime), NULL, CAST(N'2019-06-07 12:31:09.007' AS DateTime), NULL, N'201906061725471250219', N'CIMS')
INSERT [dbo].[tblCIMSAttributeValue] ([AttributesValueId], [AttributeId], [AttributeValue], [IsDelete], [AttributeCode], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [RecordId], [Module]) VALUES (332, NULL, N'Department-01', 0, N'ATTRIBUTE5                                                                                          ', CAST(N'2019-06-06 17:25:47.153' AS DateTime), NULL, CAST(N'2019-06-07 12:31:09.017' AS DateTime), NULL, N'201906061725471250219', N'CIMS')
INSERT [dbo].[tblCIMSAttributeValue] ([AttributesValueId], [AttributeId], [AttributeValue], [IsDelete], [AttributeCode], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [RecordId], [Module]) VALUES (333, NULL, N'201906061622307519483', 0, N'RecordId                                                                                            ', CAST(N'2019-06-06 17:25:47.157' AS DateTime), NULL, NULL, NULL, N'201906061725471250219', N'CIMS')
INSERT [dbo].[tblCIMSAttributeValue] ([AttributesValueId], [AttributeId], [AttributeValue], [IsDelete], [AttributeCode], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [RecordId], [Module]) VALUES (334, NULL, N'', 0, N'ATTRIBUTE6                                                                                          ', CAST(N'2019-06-06 17:25:46.983' AS DateTime), NULL, NULL, NULL, N'201906061725471250219', N'CIMS')
INSERT [dbo].[tblCIMSAttributeValue] ([AttributesValueId], [AttributeId], [AttributeValue], [IsDelete], [AttributeCode], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [RecordId], [Module]) VALUES (335, NULL, N'CUS.201906061728310233647', 0, N'ATTRIBUTE1                                                                                          ', CAST(N'2019-06-06 17:28:31.023' AS DateTime), NULL, CAST(N'2019-06-07 12:31:25.457' AS DateTime), NULL, N'201906061728310168822', N'CIMS')
INSERT [dbo].[tblCIMSAttributeValue] ([AttributesValueId], [AttributeId], [AttributeValue], [IsDelete], [AttributeCode], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [RecordId], [Module]) VALUES (336, NULL, N'Bùi Hữu Đại', 0, N'ATTRIBUTE2                                                                                          ', CAST(N'2019-06-06 17:28:31.030' AS DateTime), NULL, CAST(N'2019-06-07 12:31:25.467' AS DateTime), NULL, N'201906061728310168822', N'CIMS')
INSERT [dbo].[tblCIMSAttributeValue] ([AttributesValueId], [AttributeId], [AttributeValue], [IsDelete], [AttributeCode], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [RecordId], [Module]) VALUES (337, NULL, N'1234567897', 0, N'ATTRIBUTE3                                                                                          ', CAST(N'2019-06-06 17:28:31.033' AS DateTime), NULL, CAST(N'2019-06-07 12:31:25.480' AS DateTime), NULL, N'201906061728310168822', N'CIMS')
INSERT [dbo].[tblCIMSAttributeValue] ([AttributesValueId], [AttributeId], [AttributeValue], [IsDelete], [AttributeCode], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [RecordId], [Module]) VALUES (338, NULL, N'', 0, N'ATTRIBUTE4                                                                                          ', CAST(N'2019-06-06 17:28:31.037' AS DateTime), NULL, CAST(N'2019-06-07 12:31:25.490' AS DateTime), NULL, N'201906061728310168822', N'CIMS')
INSERT [dbo].[tblCIMSAttributeValue] ([AttributesValueId], [AttributeId], [AttributeValue], [IsDelete], [AttributeCode], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [RecordId], [Module]) VALUES (339, NULL, N'Department-02', 0, N'ATTRIBUTE5                                                                                          ', CAST(N'2019-06-06 17:28:31.043' AS DateTime), NULL, CAST(N'2019-06-07 12:31:25.500' AS DateTime), NULL, N'201906061728310168822', N'CIMS')
INSERT [dbo].[tblCIMSAttributeValue] ([AttributesValueId], [AttributeId], [AttributeValue], [IsDelete], [AttributeCode], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [RecordId], [Module]) VALUES (340, NULL, N'201906061622307519483', 0, N'RecordId                                                                                            ', CAST(N'2019-06-06 17:28:31.047' AS DateTime), NULL, NULL, NULL, N'201906061728310168822', N'CIMS')
INSERT [dbo].[tblCIMSAttributeValue] ([AttributesValueId], [AttributeId], [AttributeValue], [IsDelete], [AttributeCode], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [RecordId], [Module]) VALUES (341, NULL, N'', 0, N'ATTRIBUTE6                                                                                          ', CAST(N'2019-06-06 17:28:30.890' AS DateTime), NULL, NULL, NULL, N'201906061728310168822', N'CIMS')
INSERT [dbo].[tblCIMSAttributeValue] ([AttributesValueId], [AttributeId], [AttributeValue], [IsDelete], [AttributeCode], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [RecordId], [Module]) VALUES (342, NULL, N'CUS.201906061800264079895', 0, N'ATTRIBUTE1                                                                                          ', CAST(N'2019-06-06 18:00:26.407' AS DateTime), NULL, CAST(N'2019-06-07 10:16:02.573' AS DateTime), NULL, N'201906061800263938992', N'CIMS')
INSERT [dbo].[tblCIMSAttributeValue] ([AttributesValueId], [AttributeId], [AttributeValue], [IsDelete], [AttributeCode], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [RecordId], [Module]) VALUES (343, NULL, N'Mạc Anh Quân', 0, N'ATTRIBUTE2                                                                                          ', CAST(N'2019-06-06 18:00:26.413' AS DateTime), NULL, CAST(N'2019-06-07 10:16:02.583' AS DateTime), NULL, N'201906061800263938992', N'CIMS')
INSERT [dbo].[tblCIMSAttributeValue] ([AttributesValueId], [AttributeId], [AttributeValue], [IsDelete], [AttributeCode], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [RecordId], [Module]) VALUES (344, NULL, N'3412312312312312', 0, N'ATTRIBUTE3                                                                                          ', CAST(N'2019-06-06 18:00:26.420' AS DateTime), NULL, CAST(N'2019-06-07 10:16:02.593' AS DateTime), NULL, N'201906061800263938992', N'CIMS')
INSERT [dbo].[tblCIMSAttributeValue] ([AttributesValueId], [AttributeId], [AttributeValue], [IsDelete], [AttributeCode], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [RecordId], [Module]) VALUES (345, NULL, N'', 0, N'ATTRIBUTE4                                                                                          ', CAST(N'2019-06-06 18:00:26.427' AS DateTime), NULL, CAST(N'2019-06-07 10:16:02.603' AS DateTime), NULL, N'201906061800263938992', N'CIMS')
INSERT [dbo].[tblCIMSAttributeValue] ([AttributesValueId], [AttributeId], [AttributeValue], [IsDelete], [AttributeCode], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [RecordId], [Module]) VALUES (346, NULL, N'Department-03', 0, N'ATTRIBUTE5                                                                                          ', CAST(N'2019-06-06 18:00:26.433' AS DateTime), NULL, CAST(N'2019-06-07 10:16:02.613' AS DateTime), NULL, N'201906061800263938992', N'CIMS')
INSERT [dbo].[tblCIMSAttributeValue] ([AttributesValueId], [AttributeId], [AttributeValue], [IsDelete], [AttributeCode], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [RecordId], [Module]) VALUES (347, NULL, N'', 0, N'ATTRIBUTE6                                                                                          ', CAST(N'2019-06-06 18:00:26.287' AS DateTime), NULL, NULL, NULL, N'201906061800263938992', N'CIMS')
INSERT [dbo].[tblCIMSAttributeValue] ([AttributesValueId], [AttributeId], [AttributeValue], [IsDelete], [AttributeCode], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [RecordId], [Module]) VALUES (348, NULL, N'CUS.201906071019342659809', 0, N'ATTRIBUTE1                                                                                          ', CAST(N'2019-06-07 10:19:34.267' AS DateTime), NULL, NULL, NULL, N'201906071019338319824', N'CIMS')
INSERT [dbo].[tblCIMSAttributeValue] ([AttributesValueId], [AttributeId], [AttributeValue], [IsDelete], [AttributeCode], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [RecordId], [Module]) VALUES (349, NULL, N'Trần Thị Hồng Phong', 0, N'ATTRIBUTE2                                                                                          ', CAST(N'2019-06-07 10:19:34.493' AS DateTime), NULL, NULL, NULL, N'201906071019338319824', N'CIMS')
INSERT [dbo].[tblCIMSAttributeValue] ([AttributesValueId], [AttributeId], [AttributeValue], [IsDelete], [AttributeCode], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [RecordId], [Module]) VALUES (350, NULL, N'123456789000', 0, N'ATTRIBUTE3                                                                                          ', CAST(N'2019-06-07 10:19:34.720' AS DateTime), NULL, NULL, NULL, N'201906071019338319824', N'CIMS')
INSERT [dbo].[tblCIMSAttributeValue] ([AttributesValueId], [AttributeId], [AttributeValue], [IsDelete], [AttributeCode], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [RecordId], [Module]) VALUES (351, NULL, N'Gầm cầu đuống', 0, N'ATTRIBUTE4                                                                                          ', CAST(N'2019-06-07 10:19:34.950' AS DateTime), NULL, NULL, NULL, N'201906071019338319824', N'CIMS')
INSERT [dbo].[tblCIMSAttributeValue] ([AttributesValueId], [AttributeId], [AttributeValue], [IsDelete], [AttributeCode], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [RecordId], [Module]) VALUES (352, NULL, N'Department-03', 0, N'ATTRIBUTE5                                                                                          ', CAST(N'2019-06-07 10:19:35.183' AS DateTime), NULL, NULL, NULL, N'201906071019338319824', N'CIMS')
INSERT [dbo].[tblCIMSAttributeValue] ([AttributesValueId], [AttributeId], [AttributeValue], [IsDelete], [AttributeCode], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [RecordId], [Module]) VALUES (353, NULL, N'', 0, N'ATTRIBUTE6                                                                                          ', CAST(N'2019-06-07 10:19:35.243' AS DateTime), NULL, NULL, NULL, N'201906071019338319824', N'CIMS')
SET IDENTITY_INSERT [dbo].[tblCIMSAttributeValue] OFF
SET IDENTITY_INSERT [dbo].[tblCIMSForm] ON 

INSERT [dbo].[tblCIMSForm] ([FormId], [FormName], [FormDescription], [MenuCode], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete], [FormType], [IsContinute], [ChildCode]) VALUES (26, NULL, NULL, N'CIMS', N'Admin', NULL, N'adminMP', CAST(N'2019-06-06 17:41:47.167' AS DateTime), 0, N'FormType-02', NULL, N'CIMS_LIST')
INSERT [dbo].[tblCIMSForm] ([FormId], [FormName], [FormDescription], [MenuCode], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete], [FormType], [IsContinute], [ChildCode]) VALUES (27, N'Thêm mới khách hàng dat', N'Thêm mới khách hàng dat', N'CIMS', N'Admin', NULL, NULL, CAST(N'2019-06-07 16:58:58.380' AS DateTime), 0, N'FormType-01', NULL, N'CIMS_ADD')
SET IDENTITY_INSERT [dbo].[tblCIMSForm] OFF
SET IDENTITY_INSERT [dbo].[tblCimsFormHistory] ON 

INSERT [dbo].[tblCimsFormHistory] ([Id], [Description], [ChildCode], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate]) VALUES (1, N'Sửa form', N'CIMS_ADD', NULL, NULL, N'adminMP', CAST(N'2019-06-06 15:44:18.213' AS DateTime))
INSERT [dbo].[tblCimsFormHistory] ([Id], [Description], [ChildCode], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate]) VALUES (2, N'Sửa form', N'CIMS_ADD', NULL, NULL, N'adminMP', CAST(N'2019-06-06 15:44:23.700' AS DateTime))
INSERT [dbo].[tblCimsFormHistory] ([Id], [Description], [ChildCode], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate]) VALUES (3, N'Sửa form', N'CIMS_ADD', NULL, NULL, N'adminMP', CAST(N'2019-06-06 15:44:30.017' AS DateTime))
INSERT [dbo].[tblCimsFormHistory] ([Id], [Description], [ChildCode], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate]) VALUES (4, N'Sửa form', N'CIMS_ADD', NULL, NULL, N'adminMP', CAST(N'2019-06-06 15:44:35.577' AS DateTime))
INSERT [dbo].[tblCimsFormHistory] ([Id], [Description], [ChildCode], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate]) VALUES (5, N'Sửa form', N'CIMS_ADD', NULL, NULL, N'adminMP', CAST(N'2019-06-06 15:44:40.930' AS DateTime))
INSERT [dbo].[tblCimsFormHistory] ([Id], [Description], [ChildCode], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate]) VALUES (6, N'Sửa form', N'CIMS_ADD', NULL, NULL, N'adminMP', CAST(N'2019-06-06 15:45:35.163' AS DateTime))
INSERT [dbo].[tblCimsFormHistory] ([Id], [Description], [ChildCode], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate]) VALUES (7, N'Sửa form', N'CIMS_ADD', NULL, NULL, N'adminMP', CAST(N'2019-06-06 15:46:28.043' AS DateTime))
INSERT [dbo].[tblCimsFormHistory] ([Id], [Description], [ChildCode], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate]) VALUES (8, N'Sửa form', N'CIMS_ADD', NULL, NULL, N'adminMP', CAST(N'2019-06-06 15:46:48.707' AS DateTime))
INSERT [dbo].[tblCimsFormHistory] ([Id], [Description], [ChildCode], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate]) VALUES (9, N'Sửa form', N'CIMS_ADD', NULL, NULL, N'adminMP', CAST(N'2019-06-06 15:47:02.787' AS DateTime))
INSERT [dbo].[tblCimsFormHistory] ([Id], [Description], [ChildCode], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate]) VALUES (10, N'Sửa form', N'CIMS_ADD', NULL, NULL, N'adminMP', CAST(N'2019-06-06 15:48:08.113' AS DateTime))
INSERT [dbo].[tblCimsFormHistory] ([Id], [Description], [ChildCode], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate]) VALUES (11, N'Sửa form', N'CIMS_ADD', NULL, NULL, N'adminMP', CAST(N'2019-06-06 15:48:18.433' AS DateTime))
INSERT [dbo].[tblCimsFormHistory] ([Id], [Description], [ChildCode], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate]) VALUES (12, N'Sửa form', N'CIMS_LIST', NULL, NULL, N'adminMP', CAST(N'2019-06-06 15:48:43.447' AS DateTime))
INSERT [dbo].[tblCimsFormHistory] ([Id], [Description], [ChildCode], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate]) VALUES (13, N'Sửa form', N'CIMS_LIST', NULL, NULL, N'adminMP', CAST(N'2019-06-06 17:41:47.167' AS DateTime))
INSERT [dbo].[tblCimsFormHistory] ([Id], [Description], [ChildCode], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate]) VALUES (14, N'Sửa form', N'CIMS_ADD', NULL, NULL, N'adminMP', CAST(N'2019-06-07 16:58:58.377' AS DateTime))
SET IDENTITY_INSERT [dbo].[tblCimsFormHistory] OFF
SET IDENTITY_INSERT [dbo].[tblVOCAttributes] ON 

INSERT [dbo].[tblVOCAttributes] ([AttributesId], [AttributeCode], [AttributeType], [AttributeWidth], [DataType], [AttributeLabel], [DefaultValue], [IsVisible], [IsRequired], [IsTableShow], [IsCategory], [ModuleParent], [IsSort], [IsDuplicate], [DetailRefer], [CreateBy], [CreateDate], [UpDateBy], [UpdateDate], [IsDelete], [IsReuse], [CategoryParentCode], [AttributeDescription], [Encyption], [EncyptWaiting], [IndexTitleTable], [DefaultValueWithTextBox], [Disabled]) VALUES (122, N'ATTRIBUTE1', N'CHECKBOX', 50, N'DataCode-05', N'Mã khách hàng', N'0', 1, 0, 1, NULL, N'CIMS', NULL, 0, NULL, N'adminMP', CAST(N'2019-06-06 15:40:48.213' AS DateTime), N'adminMP', CAST(N'2019-06-07 09:14:06.830' AS DateTime), 0, 1, N'TAOMOIDM', N'Mã khách hàng', 0, 0, 1, NULL, NULL)
INSERT [dbo].[tblVOCAttributes] ([AttributesId], [AttributeCode], [AttributeType], [AttributeWidth], [DataType], [AttributeLabel], [DefaultValue], [IsVisible], [IsRequired], [IsTableShow], [IsCategory], [ModuleParent], [IsSort], [IsDuplicate], [DetailRefer], [CreateBy], [CreateDate], [UpDateBy], [UpdateDate], [IsDelete], [IsReuse], [CategoryParentCode], [AttributeDescription], [Encyption], [EncyptWaiting], [IndexTitleTable], [DefaultValueWithTextBox], [Disabled]) VALUES (123, N'ATTRIBUTE2', N'TEXTBOX', 60, N'DataCode-01', N'Tên khách hàng', NULL, 1, 1, 1, NULL, N'CIMS', NULL, NULL, NULL, N'adminMP', CAST(N'2019-06-06 15:42:47.727' AS DateTime), N'adminMP', CAST(N'2019-06-07 16:58:55.973' AS DateTime), 0, 0, NULL, NULL, 0, 0, 2, N'', NULL)
INSERT [dbo].[tblVOCAttributes] ([AttributesId], [AttributeCode], [AttributeType], [AttributeWidth], [DataType], [AttributeLabel], [DefaultValue], [IsVisible], [IsRequired], [IsTableShow], [IsCategory], [ModuleParent], [IsSort], [IsDuplicate], [DetailRefer], [CreateBy], [CreateDate], [UpDateBy], [UpdateDate], [IsDelete], [IsReuse], [CategoryParentCode], [AttributeDescription], [Encyption], [EncyptWaiting], [IndexTitleTable], [DefaultValueWithTextBox], [Disabled]) VALUES (124, N'ATTRIBUTE3', N'TEXTBOX', NULL, N'DataCode-05', N'Số điện thoại', NULL, 1, 1, 1, NULL, N'CIMS', NULL, 1, NULL, N'adminMP', CAST(N'2019-06-06 15:43:19.583' AS DateTime), N'adminMP', CAST(N'2019-06-07 12:27:25.630' AS DateTime), 0, NULL, NULL, NULL, 0, 0, 3, N'', NULL)
INSERT [dbo].[tblVOCAttributes] ([AttributesId], [AttributeCode], [AttributeType], [AttributeWidth], [DataType], [AttributeLabel], [DefaultValue], [IsVisible], [IsRequired], [IsTableShow], [IsCategory], [ModuleParent], [IsSort], [IsDuplicate], [DetailRefer], [CreateBy], [CreateDate], [UpDateBy], [UpdateDate], [IsDelete], [IsReuse], [CategoryParentCode], [AttributeDescription], [Encyption], [EncyptWaiting], [IndexTitleTable], [DefaultValueWithTextBox], [Disabled]) VALUES (125, N'ATTRIBUTE4', N'TEXTAREA', NULL, N'DataCode-01', N'Địa chỉ', NULL, 1, 0, 0, NULL, N'CIMS', NULL, 0, NULL, N'adminMP', CAST(N'2019-06-06 15:45:27.117' AS DateTime), NULL, NULL, 0, 0, NULL, N'', 0, 0, NULL, N'', NULL)
INSERT [dbo].[tblVOCAttributes] ([AttributesId], [AttributeCode], [AttributeType], [AttributeWidth], [DataType], [AttributeLabel], [DefaultValue], [IsVisible], [IsRequired], [IsTableShow], [IsCategory], [ModuleParent], [IsSort], [IsDuplicate], [DetailRefer], [CreateBy], [CreateDate], [UpDateBy], [UpdateDate], [IsDelete], [IsReuse], [CategoryParentCode], [AttributeDescription], [Encyption], [EncyptWaiting], [IndexTitleTable], [DefaultValueWithTextBox], [Disabled]) VALUES (126, N'ATTRIBUTE5', N'DROPDOWNLIST', NULL, N'DataCode-05', N'Phòng ban', N'0', 1, NULL, 1, NULL, N'CIMS', NULL, NULL, NULL, N'adminMP', CAST(N'2019-06-06 15:46:15.993' AS DateTime), NULL, NULL, 0, NULL, N'DepartmentCode', NULL, 0, 0, 4, NULL, NULL)
INSERT [dbo].[tblVOCAttributes] ([AttributesId], [AttributeCode], [AttributeType], [AttributeWidth], [DataType], [AttributeLabel], [DefaultValue], [IsVisible], [IsRequired], [IsTableShow], [IsCategory], [ModuleParent], [IsSort], [IsDuplicate], [DetailRefer], [CreateBy], [CreateDate], [UpDateBy], [UpdateDate], [IsDelete], [IsReuse], [CategoryParentCode], [AttributeDescription], [Encyption], [EncyptWaiting], [IndexTitleTable], [DefaultValueWithTextBox], [Disabled]) VALUES (127, N'ATTRIBUTE6', N'LISTRADIO', NULL, N'DataCode-05', N'Giới tính', N'GENDER-01', 1, 0, 1, NULL, N'CIMS', NULL, 0, NULL, N'adminMP', CAST(N'2019-06-06 15:47:50.750' AS DateTime), NULL, NULL, 0, 0, N'GENDER', N'', 0, 0, 5, NULL, NULL)
SET IDENTITY_INSERT [dbo].[tblVOCAttributes] OFF
SET IDENTITY_INSERT [dbo].[tblVOCForm] ON 

INSERT [dbo].[tblVOCForm] ([FormId], [FormName], [FormDescription], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete]) VALUES (1, N'VOC-Step1', N'step1', N'HiepPD1', NULL, NULL, NULL, 0)
SET IDENTITY_INSERT [dbo].[tblVOCForm] OFF
INSERT [dbo].[TempShow] ([index], [RecordId], [Mã khách hàng], [Địa chỉ], [Ngày sinh], [Email]) VALUES (1, N'201905041725486611223', N'abc', N'hn', N'29/91', N'dsa@gmail.com')
INSERT [dbo].[TempShow] ([index], [RecordId], [Mã khách hàng], [Địa chỉ], [Ngày sinh], [Email]) VALUES (2, N'201905051620273183057', N'TVP', N'Ha noi', N'29/01/1991', NULL)
INSERT [dbo].[TempShow] ([index], [RecordId], [Mã khách hàng], [Địa chỉ], [Ngày sinh], [Email]) VALUES (3, N'201905061155025464246', N'ppp2', N'nam dinh', N'1991', NULL)
ALTER TABLE [dbo].[tblVOCStepAttributesValue]  WITH CHECK ADD  CONSTRAINT [FK_tblVOCStepAttributesValue_tblVOCSteps] FOREIGN KEY([StepId])
REFERENCES [dbo].[tblVOCSteps] ([StepId])
GO
ALTER TABLE [dbo].[tblVOCStepAttributesValue] CHECK CONSTRAINT [FK_tblVOCStepAttributesValue_tblVOCSteps]
GO
/****** Object:  StoredProcedure [dbo].[cims_GetCustomerList]    Script Date: 6/7/2019 5:05:43 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[cims_GetCustomerList]
	@Module nvarchar(50),
	@CurrPage int,
	@PageSize int
AS
BEGIN
	--Lay danh sach truong thong tin-----------------
	SELECT a.AttributeCode, b.AttributeLabel, a.AttrOrder,a.RowIndex
	INTO #Temp1 
	FROM tblCIMSAttributeForm a 
	INNER JOIN tblVOCAttributes b ON a.AttributeCode = b.AttributeCode
	WHERE a.ChildCode = 'CIMS_LIST'
	ORDER BY a.AttrOrder
	--Lay danh sach tieu chi tim kiem
	SELECT * FROM #Temp1 a ORDER BY a.[AttrOrder]

	--Lay gia tri---------------------
	SELECT a.AttributeCode, a.AttributeLabel, CASE WHEN a.AttributeType in ('DROPDOWNLIST','LISTRADIO', 'LISTCHECKBOX')  
	THEN (SELECT CategoryName FROM tblCategory a WHERE a.CategoryCode = CONVERT(NVARCHAR(MAX),b.AttributeValue)) ELSE
	CONVERT(NVARCHAR(MAX),b.AttributeValue) END AttributeValue, b.RecordId, IndexTitleTable 
	INTO #Temp2
	FROM tblVOCAttributes a
	INNER JOIN tblCIMSAttributeValue b ON a.AttributeCode = b.AttributeCode
	WHERE a.ModuleParent = @Module
	and a.IsTableShow = 1
	ORDER BY a.IndexTitleTable
	------------------
	SELECT a.AttributeCode, AttributeLabel, IndexTitleTable 
	INTO #TempList
	FROM tblVOCAttributes a 
	
	WHERE a.IsTableShow = 1 
	--Lay danh sach cac truong thong tin table
	SELECT AttributeCode, AttributeLabel, IndexTitleTable 
	INTO #TempCol 
	FROM #Temp2 GROUP BY AttributeLabel,AttributeCode, IndexTitleTable

	SELECT * FROM #TempList ORDER BY IndexTitleTable

	--Lay ten truong thong tin
	DECLARE @cols NVARCHAR(MAX), @query NVARCHAR(MAX);
	SET @cols = STUFF(
                 (
                     SELECT 
                            ','+ QUOTENAME(c.AttributeCode)
                     FROM #TempList c ORDER BY IndexTitleTable FOR XML PATH(''), TYPE
                 ).value('.', 'nvarchar(max)'), 1, 1, '')
	DECLARE @start INT;
	DECLARE @end INT;
	SET @start = (@currPage-1) * @PageSize + 1 
	SET @end = @currPage*@PageSize
	-----------------
	SET @query = N'SELECT RecordId, '+@cols+' into #TempAll from 
            (
                SELECT RecordId,
						AttributeValue,
						AttributeCode
						FROM #Temp2
            ) x
            pivot 
            (
                max(AttributeValue)
                for AttributeCode IN(' + @cols + ')
            )a  
			SELECT ROW_NUMBER() OVER(ORDER BY t.RecordId) [Index], t.* 
			INTO #TempShow
			FROM #TempAll t 
			SELECT * FROM #TempShow t
			WHERE t.[index] between '+ Convert(NVARCHAR,@start) +' and '+convert(NVARCHAR,@end)+ ' 
			Select COUNT (*)TotalRecord ,'+Convert(NVARCHAR,@CurrPage)+'CurrPage,'+Convert(NVARCHAR,@PageSize)+'PageSize
			from  #TempShow
			DROP TABLE #TempAll, #TempShow'
	EXEC(@query)
	DROP TABLE #Temp1, #Temp2,#TempList
END

-- cims_GetCustomerList 'CIMS', 1, 20

GO
/****** Object:  StoredProcedure [dbo].[cims_GetCustomerList_RecordId]    Script Date: 6/7/2019 5:05:43 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[cims_GetCustomerList_RecordId]
	@RecordId nvarchar(100)
AS
BEGIN
	
	--Lay gia tri---------------------
	SELECT a.AttributeCode, a.AttributeLabel, CONVERT(NVARCHAR(MAX),b.AttributeValue) AttributeValue, b.RecordId, IndexTitleTable 
	INTO #Temp2
	FROM tblVOCAttributes a
	INNER JOIN tblCIMSAttributeValue b ON a.AttributeCode = b.AttributeCode
	WHERE a.ModuleParent = 'CIMS'
	and a.IsTableShow = 1
	ORDER BY a.IndexTitleTable

	--Lay danh sach cac truong thong tin table
	SELECT AttributeCode, AttributeLabel, IndexTitleTable 
	INTO #TempCol 
	FROM #Temp2 GROUP BY AttributeLabel,AttributeCode, IndexTitleTable
	--SELECT * FROM #TempCol ORDER BY IndexTitleTable
	--Lay ten truong thong tin

	

	DECLARE @cols NVARCHAR(MAX), @query NVARCHAR(MAX);
	SET @cols = STUFF(
                 (
                     SELECT 
                            ','+ QUOTENAME(c.AttributeCode)
                     FROM #TempCol c ORDER BY IndexTitleTable FOR XML PATH(''), TYPE
                 ).value('.', 'nvarchar(max)'), 1, 1, '')
	
	-----------------
	SET @query = N'SELECT RecordId, '+@cols+' into #TempAll from 
            (
                SELECT RecordId,
						AttributeValue,
						AttributeCode
						FROM #Temp2
            ) x
            pivot 
            (
                max(AttributeValue)
                for AttributeCode IN(' + @cols + ')
            )a  
			SELECT ROW_NUMBER() OVER(ORDER BY t.RecordId) [Index], t.* 
			
			FROM #TempAll t 
			where t.RecordId = '+ Convert(NVARCHAR,@RecordId) +'
			DROP TABLE #TempAll'
	EXEC(@query)
	DROP TABLE #Temp2
END

-- cims_GetCustomerList_RecordId '201906061059135837125'

GO
/****** Object:  StoredProcedure [dbo].[GetAllCategory]    Script Date: 6/7/2019 5:05:43 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetAllCategory]
@TextSearch nvarchar(200)='',
@currPage int,
@recodperpage int
AS
BEGIN
	Declare @Total int, @PageSize int
	Declare @start int
	Declare @end int

	Select  a.ID,a.CategoryCode, a.CategoryTypeCode, a.CategoryName ,a.CreateDate,a.CreateBy
	into #Category
	from tblCategory a
	where IsDelete = 0 
	and ISNULL(a.CategoryTypeCode,'') = ''
	and (isnull(@TextSearch, '') = '' or a.CategoryName like '%'+@TextSearch+'%')
	Union all
	Select b.ID,b.CategoryCode, b.CategoryTypeCode, b.CategoryGroupName,b.CreateDate,b.CreateBy
	from tblCategoryGroup b
	where IsDelete = 0 
	and (isnull(@TextSearch, '') = '' or b.CategoryGroupName like '%'+@TextSearch+'%')

	Select ROW_NUMBER() OVER(ORDER BY a.CreateDate desc) [index],a.Id,a.CategoryCode,a.CategoryTypeCode,a.CategoryName,a.CreateBy,a.CreateDate
		into #Temp
				from #Category a

		Select @Total = Count(*) from #Temp

	if @currPage > 1
		Begin
			Set @start = @currPage*@recodperpage + 1
			Set @end = (@currPage + 1)*@recodperpage
		End
	else
		Begin
			Set @start = 1
			Set @end = @recodperpage
		End

	--Tinh page size
		Select @PageSize = case when @Total%@recodperpage = 0 then @Total/@recodperpage else @Total/@recodperpage + 1 end 
		Select * from #Temp
		where [index] between @start and @end

		Select @Total TotalPage, @currPage CurrPage, @PageSize PageSize

		Drop table #Temp,#Category
END


-- exec GetAllConstraint '',1,15
GO
/****** Object:  StoredProcedure [dbo].[GetAllCategory_New]    Script Date: 6/7/2019 5:05:43 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetAllCategory_New]
@TextSearch nvarchar(200)='',
@currPage int,
@recodperpage int
AS
BEGIN
	Declare @Total int, @PageSize int
	Declare @start int
	Declare @end int

	Select  a.ID,a.CategoryCode, a.CategoryTypeCode, a.CategoryName ,a.CreateDate,a.CreateBy
	into #Category
	from tblCategory a
	where IsDelete = 0 
	and ISNULL(a.CategoryTypeCode,'') = ''
	and (isnull(@TextSearch, '') = '' or a.CategoryName like '%'+@TextSearch+'%')
	Union all
	Select b.ID,b.CategoryCode, b.CategoryTypeCode, b.CategoryGroupName,b.CreateDate,b.CreateBy
	from tblCategoryGroup b
	where IsDelete = 0 
	and (isnull(@TextSearch, '') = '' or b.CategoryGroupName like '%'+@TextSearch+'%')

	Select t.*, (Select a.CategoryName from #Category a where a.CategoryCode = t.CategoryTypeCode) CategoryTypeName into #Category1 from #Category t

	Select ROW_NUMBER() OVER(ORDER BY a.CreateDate desc) [index],a.CategoryCode,a.CategoryTypeCode,a.CategoryName,a.CategoryTypeName,a.CreateBy,a.CreateDate
		into #Temp
				from #Category1 a

		Select @Total = Count(*) from #Temp

	if @currPage > 1
		Begin
			Set @start = (@currPage -1)*@recodperpage + 1
			Set @end = (@currPage )*@recodperpage
		End
	else
		Begin
			Set @start = 1
			Set @end = @recodperpage
		End

	--Tinh page size
		Select @PageSize = case when @Total%@recodperpage = 0 then @Total/@recodperpage else @Total/@recodperpage + 1 end 
		Select * from #Temp
		where [index] between @start and @end

		Select @Total TotalPage, @currPage CurrPage, @PageSize PageSize

		Drop table #Temp,#Category,#Category1
END


-- GetAllCategory_New '',1,15



GO
/****** Object:  StoredProcedure [dbo].[GetAllConstraint]    Script Date: 6/7/2019 5:05:43 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetAllConstraint]
@TextSearch nvarchar(200)='',
@currPage int,
@recodperpage int
AS
BEGIN
	Declare @Total int, @PageSize int
	Declare @start int
	Declare @end int

	Select ROW_NUMBER() OVER(ORDER BY a.CreateDate desc) [index],a.Id,a.Name,c.CategoryName,a.LinkContraints,a.ContraintsValue,c.CategoryCode,
		--case when a.IsContraintType =1 then 'Đối Tượng' else 'Trường thông tin' end ConstraintType,
		case when (Select Count(*) from tblVOCAttributes c where c.AttributeType = a.ContraintsType) > 0 then 'false' else 'true' end chkDelete
		into #Temp
				from tblAttributeConstraint a
				left join tblCategory c on a.ContraintsType = c.CategoryCode 
				Where a.IsDelete = 0 
				and (isnull(@TextSearch, '') = '' or a.Name like '%'+@TextSearch+'%')

		Select @Total = Count(*) from #Temp

	if @currPage > 1
		Begin
			Set @start = (@currPage -1)*@recodperpage + 1
			Set @end = (@currPage )*@recodperpage
		End
	else
		Begin
			Set @start = 1
			Set @end = @recodperpage
		End

	--Tinh page size
		Select @PageSize = case when @Total%@recodperpage = 0 then @Total/@recodperpage else @Total/@recodperpage + 1 end 
		Select * from #Temp
		where [index] between @start and @end

		Select @Total TotalPage, @currPage CurrPage, @PageSize PageSize

		Drop table #Temp
END


-- exec GetAllConstraint '',1,15



GO
/****** Object:  StoredProcedure [dbo].[GetAllConstraint1]    Script Date: 6/7/2019 5:05:43 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetAllConstraint1]
AS
BEGIN
	
	Select ROW_NUMBER() OVER(ORDER BY a.CreateDate desc) [index],a.Name,c.CategoryName
		--case when a.IsContraintType =1 then 'Đối Tượng' else 'Trường thông tin' end ConstraintType,
		into #Temp
				from tblAttributeConstraint a
				left join tblCategory c on a.ContraintsType = c.CategoryCode 
				Where a.IsDelete = 0 


		Select * from #Temp

		Drop table #Temp
END


-- exec GetAllConstraint1 
GO
/****** Object:  StoredProcedure [dbo].[GetAttributeValue]    Script Date: 6/7/2019 5:05:43 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetAttributeValue]
	@FormId int,
	@currPage int,
	@recodperpage int
AS
Begin
	Declare @Total int, @PageSize int,@indexPage int,@TotalAtt int
	Declare @start int
	Declare @end int
		Select ROW_NUMBER() OVER(ORDER BY a.RecordId ) [index],a.AttributeValue,a.AttributeCode,a.RecordId,a.FormId
		
		into #Temp
				from tblCIMSAttributeValue a
				Where a.IsDelete = 0 				
			
		Select @Total = Count(*) from #Temp
		Select @TotalAtt = Count(*) 
		from tblCIMSAttributeValue b
		where b.FormId= @FormId
		group by RecordId
	
		if @currPage > 1
			Begin
				Set @start = @currPage*@recodperpage*@TotalAtt + 1
				Set @end = (@currPage + 1)*@recodperpage*@TotalAtt
			End
		else
			Begin
				Set @start = 1
				Set @end = @recodperpage*@TotalAtt
			End
			


			
		
		--Tinh page size
		 
		Select * from #Temp
		where [index] between @start and @end

		Select @Total TotalPage, @currPage CurrPage, @PageSize PageSize,@TotalAtt TotalAtt


		
		Drop table #Temp
	End
	 
-- totalPage
-- currentPage
-- pageSize

-- GetAttributeValue 23,1,15




GO
/****** Object:  StoredProcedure [dbo].[GetCimsvalue]    Script Date: 6/7/2019 5:05:43 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[GetCimsvalue]
	@ModuleParent nvarchar(250),
	@currPage int,
	@recodperpage int

AS 
BEGIN
Set fmtonly off
Select a.*
into #Temp1
from tblCIMSAttributeValue a
where a.IsDelete = 0
--and (isnull(@TextSearch, '') = '' or a. like '%'+@TextSearch+'%' or a.CanFullName like '%'+@TextSearch+'%' or a.CanFullName like '%'+@TextSearch+'%')


Select a.*
into #Temp2
from tblVOCAttributes a
where a.IsTableShow= 1
Select t1.RecordId,t1.AttributeCode, convert(nvarchar, AttributeValue) AttributeValue, t2.AttributeLabel,t2.IndexTitleTable
into #Temp3
from #Temp1 t1

inner join #Temp2 t2 on t1.AttributeCode = t2.AttributeCode
where t1.Module = @ModuleParent
and t2.IsTableShow = 1


--------------
Select DISTINCT a.AttributeCode,a.IndexTitleTable from #Temp3 a 

Select a.AttributeCode,b.RowIndex,b.AttrOrder
from tblVOCAttributes a
inner join tblCIMSAttributeForm b on a.AttributeCode = b.AttributeCode 
where b.ChildCode = 'CIMS_LIST'

	-----------------			

DECLARE @cols NVARCHAR(MAX), @query NVARCHAR(MAX);
SET @cols = STUFF(
                 (
                     SELECT DISTINCT
                            ','+ QUOTENAME(c.AttributeLabel)
                     FROM #Temp3 c FOR XML PATH(''), TYPE
                 ).value('.', 'nvarchar(max)'), 1, 1, '')

DECLARE @start INT;
DECLARE @end INT;
SET @start = (@currPage-1) * @recodperpage + 1 
SET @end = @currPage*@recodperpage
print @start
--SET @query = '
--WITH dbData AS (			
--				SELECT [RecordId], '+@cols+' from (SELECT [RecordId],
--						CAST([AttributeValue] AS NVARCHAR()) AS AVALUE,
--						AttributeLabel
--					FROM #Temp3
--				)x pivot (max(AVALUE) for AttributeLabel in ('+@cols+')) p
--			)
--			SELECT ROW_NUMBER() OVER(ORDER BY dbData.RecordId) [index], *
--			into #Temp22
--			 FROM dbData
--			 SELECT * 
--			 from  #Temp22 a
--			where a.[index] between '+ Convert(nvarchar,@start) +' and '+convert(nvarchar,@end)+' Drop table #Temp22';

	set @query = N'SELECT RecordId, '+@cols+' into #TempAll from 
            (
                SELECT RecordId,
						AttributeValue,
						AttributeLabel
						FROM #Temp3
            ) x
            pivot 
            (
                max(AttributeValue)
                for AttributeLabel IN(' + @cols + ')
            )a  
			Select * from #TempAll Drop table #TempAll
			'

		--set @query=N'Select a.*, 90 as N''Việt Nam'' from tblCategory a '	
EXECUTE (@query);

--Select * from #Temp3

Drop table #Temp1, #Temp2,#Temp3

END
-- GetCimsvalue 'CIMS',1,20


GO
/****** Object:  StoredProcedure [dbo].[sp_Generate_Database_Schema]    Script Date: 6/7/2019 5:05:43 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[sp_Generate_Database_Schema]
	@DatabaseName VARCHAR(100)
AS
BEGIN
	DECLARE @SqlDB nvarchar(500) = ''
	SET @SqlDB = N'CREATE DATABASE '+@DatabaseName+''
	EXEC(@SqlDB)
	DECLARE @SqlSchema nvarchar(max) = ''
	SET @SqlSchema = N'USE '+@DatabaseName+' GO '
	SET @SqlSchema += N'SET ANSI_NULLS ON
						GO
						SET QUOTED_IDENTIFIER ON
						GO
						CREATE TABLE [dbo].[tblAttributeAdvanced](
							[AdvancedId] [int] IDENTITY(1,1) NOT NULL,
							[AdvancedName] [nvarchar](100) NULL,
							[AdvancedDescription] [nvarchar](100) NULL,
							[AdvancedDataType] [nvarchar](50) NULL,
							[AdvancedObject] [nvarchar](50) NULL,
							[CreateBy] [nvarchar](50) NULL,
							[CreateDate] [datetime] NULL,
							[UpdateBy] [nvarchar](50) NULL,
							[UpdateDate] [datetime] NULL,
							[IsDelete] [bit] NULL,
						 CONSTRAINT [PK_tblAttributeAdvanced] PRIMARY KEY CLUSTERED 
						(
							[AdvancedId] ASC
						)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
						) ON [PRIMARY]

						GO
						/****** Object:  Table [dbo].[tblAttributeConstraint]    Script Date: 5/5/2019 12:44:45 PM ******/
						SET ANSI_NULLS ON
						GO
						SET QUOTED_IDENTIFIER ON
						GO
						SET ANSI_PADDING ON
						GO
						CREATE TABLE [dbo].[tblAttributeConstraint](
							[Id] [int] IDENTITY(1,1) NOT NULL,
							[Name] [nvarchar](100) NULL,
							[ControlType] [nvarchar](100) NULL,
							[ContraintsType] [nvarchar](100) NULL,
							[ContraintsValue] [nvarchar](100) NULL,
							[IsContraintType] [bit] NULL,
							[IsContraintValue] [bit] NULL,
							[LinkContraints] [varchar](250) NULL,
							[CreateBy] [nvarchar](50) NULL,
							[CreateDate] [datetime] NULL,
							[UpdateBy] [nvarchar](50) NULL,
							[UpdateDate] [datetime] NULL,
							[IsDelete] [bit] NULL,
						 CONSTRAINT [PK_tblAttributeConstraint] PRIMARY KEY CLUSTERED 
						(
							[Id] ASC
						)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
						) ON [PRIMARY]

						GO
						SET ANSI_PADDING OFF
						GO
						/****** Object:  Table [dbo].[tblAttributeOptions]    Script Date: 5/5/2019 12:44:45 PM ******/
						SET ANSI_NULLS ON
						GO
						SET QUOTED_IDENTIFIER ON
						GO
						CREATE TABLE [dbo].[tblAttributeOptions](
							[AttributeOptionsId] [int] IDENTITY(1,1) NOT NULL,
							[AttributeId] [int] NULL,
							[OptionId] [int] NULL,
							[OptionValue] [nvarchar](50) NULL,
							[OptionLabel] [nvarchar](100) NULL,
							[CreateBy] [nvarchar](50) NULL,
							[CreateDate] [datetime] NULL,
							[UpdateBy] [nvarchar](50) NULL,
							[UpdateDate] [datetime] NULL,
							[IsDelete] [bit] NULL,
						 CONSTRAINT [PK_tblAttributeOptions] PRIMARY KEY CLUSTERED 
						(
							[AttributeOptionsId] ASC
						)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
						) ON [PRIMARY]

						GO
						/****** Object:  Table [dbo].[tblCategory]    Script Date: 5/5/2019 12:44:45 PM ******/
						SET ANSI_NULLS ON
						GO
						SET QUOTED_IDENTIFIER ON
						GO
						CREATE TABLE [dbo].[tblCategory](
							[ID] [int] IDENTITY(1,1) NOT NULL,
							[CategoryCode] [nvarchar](50) NULL,
							[CategoryTypeCode] [nvarchar](50) NULL,
							[CategoryName] [nvarchar](200) NULL,
							[CreateBy] [nvarchar](50) NULL,
							[CreateDate] [datetime] NULL,
							[UpdateBy] [nvarchar](50) NULL,
							[UpdateDate] [datetime] NULL,
							[IsDelete] [bit] NULL,
							[IsActive] [int] NULL,
							[OrderNum] [int] NULL,
						 CONSTRAINT [PK_tblCategory] PRIMARY KEY CLUSTERED 
						(
							[ID] ASC
						)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
						) ON [PRIMARY]

						GO
						/****** Object:  Table [dbo].[tblCIMSAttributeForm]    Script Date: 5/5/2019 12:44:45 PM ******/
						SET ANSI_NULLS ON
						GO
						SET QUOTED_IDENTIFIER ON
						GO
						SET ANSI_PADDING ON
						GO
						CREATE TABLE [dbo].[tblCIMSAttributeForm](
							[AttributeFormId] [int] IDENTITY(1,1) NOT NULL,
							[FormId] [int] NULL,
							[AttributeId] [int] NULL,
							[RowIndex] [int] NULL,
							[AttrOrder] [int] NULL,
							[AttributeColumn] [int] NULL,
							[AttributeCode] [varchar](100) NULL,
						 CONSTRAINT [PK_tblCIMSAttributeForm] PRIMARY KEY CLUSTERED 
						(
							[AttributeFormId] ASC
						)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
						) ON [PRIMARY]

						GO
						SET ANSI_PADDING OFF
						GO
						/****** Object:  Table [dbo].[tblCIMSAttributeValue]    Script Date: 5/5/2019 12:44:45 PM ******/
						SET ANSI_NULLS ON
						GO
						SET QUOTED_IDENTIFIER ON
						GO
						SET ANSI_PADDING ON
						GO
						CREATE TABLE [dbo].[tblCIMSAttributeValue](
							[AttributesValueId] [int] IDENTITY(1,1) NOT NULL,
							[FormId] [int] NULL,
							[AttributeId] [int] NULL,
							[AttributeValue] [ntext] NULL,
							[IsDelete] [bit] NULL,
							[AttributeCode] [nchar](100) NULL,
							[CreatedDate] [datetime] NULL,
							[CreatedBy] [nvarchar](50) NULL,
							[UpdatedDate] [datetime] NULL,
							[UpdatedBy] [nvarchar](50) NULL,
							[RecordId] [varchar](255) NULL,
						 CONSTRAINT [PK_tblCIMSAttributeValue] PRIMARY KEY CLUSTERED 
						(
							[AttributesValueId] ASC
						)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
						) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

						GO
						SET ANSI_PADDING OFF
						GO
						/****** Object:  Table [dbo].[tblCIMSForm]    Script Date: 5/5/2019 12:44:45 PM ******/
						SET ANSI_NULLS ON
						GO
						SET QUOTED_IDENTIFIER ON
						GO
						SET ANSI_PADDING ON
						GO
						CREATE TABLE [dbo].[tblCIMSForm](
							[FormId] [int] IDENTITY(1,1) NOT NULL,
							[FormName] [nvarchar](50) NULL,
							[FormDescription] [nvarchar](100) NULL,
							[MenuCode] [nvarchar](50) NULL,
							[CreateBy] [nvarchar](50) NULL,
							[CreateDate] [datetime] NULL,
							[UpdateBy] [nvarchar](50) NULL,
							[UpdateDate] [datetime] NULL,
							[IsDelete] [bit] NULL,
							[FormType] [varchar](50) NULL,
							[IsContinute] [bit] NULL,
						 CONSTRAINT [PK_tblCIMSForm] PRIMARY KEY CLUSTERED 
						(
							[FormId] ASC
						)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
						) ON [PRIMARY]

						GO
						SET ANSI_PADDING OFF
						GO
						/****** Object:  Table [dbo].[tblCustomer]    Script Date: 5/5/2019 12:44:45 PM ******/
						SET ANSI_NULLS ON
						GO
						SET QUOTED_IDENTIFIER ON
						GO
						SET ANSI_PADDING ON
						GO
						CREATE TABLE [dbo].[tblCustomer](
							[ATTRIBUTE01] [varchar](1) NULL,
							[ATTRIBUTE02] [nvarchar](1) NULL,
							[ATTRIBUTE03] [datetime] NULL,
							[ATTRIBUTE04] [varchar](1) NULL,
							[ATTRIBUTE05] [varchar](1) NULL,
							[ATTRIBUTE06] [nvarchar](1) NULL,
							[ATTRIBUTE07] [varchar](1) NULL,
							[ATTRIBUTE08] [varchar](1) NULL,
							[ATTRIBUTE09] [nvarchar](1) NULL
						) ON [PRIMARY]

						GO
						SET ANSI_PADDING OFF
						GO
						/****** Object:  Table [dbo].[tblReferenceConstraint]    Script Date: 5/5/2019 12:44:45 PM ******/
						SET ANSI_NULLS ON
						GO
						SET QUOTED_IDENTIFIER ON
						GO
						CREATE TABLE [dbo].[tblReferenceConstraint](
							[Id] [int] IDENTITY(1,1) NOT NULL,
							[AttributeCode] [nvarchar](100) NULL,
							[ConstraintId] [int] NULL,
						 CONSTRAINT [PK_tblReferenceConstraint] PRIMARY KEY CLUSTERED 
						(
							[Id] ASC
						)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
						) ON [PRIMARY]

						GO
						/****** Object:  Table [dbo].[tblVOCAttributeForm]    Script Date: 5/5/2019 12:44:45 PM ******/
						SET ANSI_NULLS ON
						GO
						SET QUOTED_IDENTIFIER ON
						GO
						CREATE TABLE [dbo].[tblVOCAttributeForm](
							[AttributeFormId] [int] IDENTITY(1,1) NOT NULL,
							[FormId] [int] NULL,
							[AttributeId] [int] NULL,
						 CONSTRAINT [PK_tblVOCAttributeForm] PRIMARY KEY CLUSTERED 
						(
							[AttributeFormId] ASC
						)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
						) ON [PRIMARY]

						GO
						/****** Object:  Table [dbo].[tblVOCAttributes]    Script Date: 5/5/2019 12:44:45 PM ******/
						SET ANSI_NULLS ON
						GO
						SET QUOTED_IDENTIFIER ON
						GO
						SET ANSI_PADDING ON
						GO
						CREATE TABLE [dbo].[tblVOCAttributes](
							[AttributesId] [int] IDENTITY(1,1) NOT NULL,
							[AttributeCode] [nvarchar](50) NULL,
							[AttributeName] [nvarchar](300) NULL,
							[AttributeType] [nvarchar](50) NULL,
							[AttributeWidth] [int] NULL,
							[DataType] [nvarchar](50) NULL,
							[AttributeLabel] [nvarchar](100) NULL,
							[AttributeClass] [nvarchar](50) NULL,
							[DefaultValue] [text] NULL,
							[IsVisible] [bit] NULL,
							[IsRequired] [bit] NULL,
							[IsTableShow] [bit] NULL,
							[IsCategory] [bit] NULL,
							[ModuleParent] [nvarchar](50) NULL,
							[IsShort] [bit] NULL,
							[ShortType] [nvarchar](50) NULL,
							[IsRefer] [bit] NULL,
							[DetailRefer] [nvarchar](500) NULL,
							[CreateBy] [nvarchar](50) NULL,
							[CreateDate] [datetime] NULL,
							[UpDateBy] [nvarchar](50) NULL,
							[UpdateDate] [datetime] NULL,
							[IsDelete] [bit] NULL,
							[LabelControlForm] [varchar](100) NULL,
							[IsReuse] [bit] NULL,
							[CategoryParentCode] [varchar](50) NULL,
							[AttributeDescription] [nvarchar](500) NULL,
						 CONSTRAINT [PK_tblAttributes] PRIMARY KEY CLUSTERED 
						(
							[AttributesId] ASC
						)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
						) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

						GO
						SET ANSI_PADDING OFF
						GO
						/****** Object:  Table [dbo].[tblVOCForm]    Script Date: 5/5/2019 12:44:45 PM ******/
						SET ANSI_NULLS ON
						GO
						SET QUOTED_IDENTIFIER ON
						GO
						CREATE TABLE [dbo].[tblVOCForm](
							[FormId] [int] IDENTITY(1,1) NOT NULL,
							[FormName] [nvarchar](100) NULL,
							[FormDescription] [nvarchar](500) NULL,
							[CreateBy] [nvarchar](50) NULL,
							[CreateDate] [datetime] NULL,
							[UpdateBy] [nvarchar](50) NULL,
							[UpdateDate] [datetime] NULL,
							[IsDelete] [bit] NULL,
						 CONSTRAINT [PK_tblVOCForm] PRIMARY KEY CLUSTERED 
						(
							[FormId] ASC
						)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
						) ON [PRIMARY]

						GO
						/****** Object:  Table [dbo].[tblVOCStepAttributes]    Script Date: 5/5/2019 12:44:45 PM ******/
						SET ANSI_NULLS ON
						GO
						SET QUOTED_IDENTIFIER ON
						GO
						CREATE TABLE [dbo].[tblVOCStepAttributes](
							[StepAttributesId] [int] IDENTITY(1,1) NOT NULL,
							[StepId] [int] NULL,
							[AttributeId] [int] NULL,
							[AttributeIndex] [int] NULL,
						 CONSTRAINT [PK_tblStepAttributes] PRIMARY KEY CLUSTERED 
						(
							[StepAttributesId] ASC
						)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
						) ON [PRIMARY]

						GO
						/****** Object:  Table [dbo].[tblVOCStepAttributesValue]    Script Date: 5/5/2019 12:44:45 PM ******/
						SET ANSI_NULLS ON
						GO
						SET QUOTED_IDENTIFIER ON
						GO
						CREATE TABLE [dbo].[tblVOCStepAttributesValue](
							[StepAttributesValueId] [int] IDENTITY(1,1) NOT NULL,
							[StepId] [int] NULL,
							[AttributeId] [int] NULL,
							[AttributeValue] [text] NULL,
							[CreateBy] [nvarchar](50) NULL,
							[CreateDate] [datetime] NULL,
							[UpdateBy] [nvarchar](50) NULL,
							[UpdateDate] [datetime] NULL,
							[IsDelete] [bit] NULL,
						 CONSTRAINT [PK_tblStepAttributesValue] PRIMARY KEY CLUSTERED 
						(
							[StepAttributesValueId] ASC
						)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
						) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

						GO
						/****** Object:  Table [dbo].[tblVOCSteps]    Script Date: 5/5/2019 12:44:45 PM ******/
						SET ANSI_NULLS ON
						GO
						SET QUOTED_IDENTIFIER ON
						GO
						CREATE TABLE [dbo].[tblVOCSteps](
							[StepId] [int] IDENTITY(1,1) NOT NULL,
							[StepName] [nvarchar](200) NULL,
							[OrganizationId] [int] NULL,
							[TemplateEmailId] [int] NULL,
							[CreateBy] [nvarchar](50) NULL,
							[CreateDate] [datetime] NULL,
							[UpdateBy] [nvarchar](50) NULL,
							[UpdateDate] [datetime] NULL,
							[IsDelete] [bit] NULL,
						 CONSTRAINT [PK_tblSteps] PRIMARY KEY CLUSTERED 
						(
							[StepId] ASC
						)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
						) ON [PRIMARY]

						GO
						/****** Object:  Table [dbo].[tblVOCWorkflows]    Script Date: 5/5/2019 12:44:45 PM ******/
						SET ANSI_NULLS ON
						GO
						SET QUOTED_IDENTIFIER ON
						GO
						CREATE TABLE [dbo].[tblVOCWorkflows](
							[WorkflowId] [int] IDENTITY(1,1) NOT NULL,
							[WorkflowName] [nvarchar](200) NULL,
							[OrganizationId] [int] NULL,
							[CreateBy] [nvarchar](50) NULL,
							[CreateDate] [datetime] NULL,
							[UpdateBy] [nvarchar](50) NULL,
							[UpdateDate] [datetime] NULL,
							[IsDelete] [bit] NULL,
						 CONSTRAINT [PK_tblWorkfolows] PRIMARY KEY CLUSTERED 
						(
							[WorkflowId] ASC
						)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
						) ON [PRIMARY]

						GO
						/****** Object:  Table [dbo].[tblVOCWorkflowSteps]    Script Date: 5/5/2019 12:44:45 PM ******/
						SET ANSI_NULLS ON
						GO
						SET QUOTED_IDENTIFIER ON
						GO
						CREATE TABLE [dbo].[tblVOCWorkflowSteps](
							[WorkflowStepsId] [int] IDENTITY(1,1) NOT NULL,
							[WorkflowId] [int] NULL,
							[StepId] [int] NULL,
							[StepIndex] [int] NULL,
						 CONSTRAINT [PK_tblWorkFolowSteps] PRIMARY KEY CLUSTERED 
						(
							[WorkflowStepsId] ASC
						)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
						) ON [PRIMARY]

						GO
						ALTER TABLE [dbo].[tblVOCStepAttributesValue]  WITH CHECK ADD  CONSTRAINT [FK_tblVOCStepAttributesValue_tblVOCSteps] FOREIGN KEY([StepId])
						REFERENCES [dbo].[tblVOCSteps] ([StepId])
						GO
						ALTER TABLE [dbo].[tblVOCStepAttributesValue] CHECK CONSTRAINT [FK_tblVOCStepAttributesValue_tblVOCSteps]
						GO'
						Select LEN(@SqlSchema)
	DECLARE @SqlStore nvarchar(max) = N''
	SET @SqlStore = N'USE '+@DatabaseName+' GO '
	SET @SqlStore += N'SET ANSI_NULLS ON
						GO
						SET QUOTED_IDENTIFIER ON
						GO
						CREATE PROCEDURE [dbo].[GetAllConstraint]
						@TextSearch nvarchar(200),
						@currPage int,
						@recodperpage int
						AS
						BEGIN
							Declare @Total int, @PageSize int
							Declare @start int
							Declare @end int

							Select ROW_NUMBER() OVER(ORDER BY a.CreateDate desc) [index],a.Name,c.CategoryName,
								--case when a.IsContraintType =1 then ''Đối Tượng'' else ''Trường thông tin'' end ConstraintType,
								case when (Select Count(*) from tblVOCAttributes c where c.AttributeType = a.ContraintsType) > 0 then ''false'' else ''true'' end chkDelete
								into #Temp
										from tblAttributeConstraint a
										left join tblCategory c on a.ContraintsType = c.CategoryCode 
										Where a.IsDelete = 0 
										and (isnull(@TextSearch, '') = '' or a.Name like ''%''+@TextSearch+''%'')

								Select @Total = Count(*) from #Temp

							if @currPage > 1
								Begin
									Set @start = @currPage*@recodperpage + 1
									Set @end = (@currPage + 1)*@recodperpage
								End
							else
								Begin
									Set @start = 1
									Set @end = @recodperpage
								End

							--Tinh page size
								Select @PageSize = case when @Total%@recodperpage = 0 then @Total/@recodperpage else @Total/@recodperpage + 1 end 
								Select * from #Temp
								where [index] between @start and @end

								Select @Total TotalPage, @currPage CurrPage, @PageSize PageSize

								Drop table #Temp
						END
						-- exec GetAllConstraint '',1,15
						GO
						/****** Object:  StoredProcedure [dbo].[GetAllConstraint1]    Script Date: 5/5/2019 12:44:45 PM ******/
						SET ANSI_NULLS ON
						GO
						SET QUOTED_IDENTIFIER ON
						GO

						CREATE PROCEDURE [dbo].[GetAllConstraint1]
						AS
						BEGIN
	
							Select ROW_NUMBER() OVER(ORDER BY a.CreateDate desc) [index],a.Name,c.CategoryName
								--case when a.IsContraintType =1 then ''Đối Tượng'' else ''Trường thông tin'' end ConstraintType,
								into #Temp
										from tblAttributeConstraint a
										left join tblCategory c on a.ContraintsType = c.CategoryCode 
										Where a.IsDelete = 0 
								Select * from #Temp
								Drop table #Temp
						END
						-- exec GetAllConstraint1 
						GO
						/****** Object:  StoredProcedure [dbo].[GetAttributeValue]    Script Date: 5/5/2019 12:44:45 PM ******/
						SET ANSI_NULLS ON
						GO
						SET QUOTED_IDENTIFIER ON
						GO
						CREATE PROCEDURE [dbo].[GetAttributeValue]
							@FormId int,
							@currPage int,
							@recodperpage int
						AS
						Begin
							Declare @Total int, @PageSize int,@indexPage int,@TotalAtt int
							Declare @start int
							Declare @end int
								Select ROW_NUMBER() OVER(ORDER BY a.RecordId ) [index],a.AttributeValue,a.AttributeCode,a.RecordId,a.FormId
		
								into #Temp
										from tblCIMSAttributeValue a
										Where a.IsDelete = 0 				
			
								Select @Total = Count(*) from #Temp
								Select @TotalAtt = Count(*) 
								from tblCIMSAttributeValue b
								where b.FormId= @FormId
								group by RecordId
	
								if @currPage > 1
									Begin
										Set @start = @currPage*@recodperpage*@TotalAtt + 1
										Set @end = (@currPage + 1)*@recodperpage*@TotalAtt
									End
								else
									Begin
										Set @start = 1
										Set @end = @recodperpage*@TotalAtt
									End
								--Tinh page size
		 
								Select * from #Temp
								where [index] between @start and @end
								Select @Total TotalPage, @currPage CurrPage, @PageSize PageSize,@TotalAtt TotalAtt
								Drop table #Temp
							End
	 
						-- totalPage
						-- currentPage
						-- pageSize
						-- GetAttributeValue 23,1,15
						GO
						/****** Object:  StoredProcedure [dbo].[sp_GenerateTable]    Script Date: 5/5/2019 12:44:45 PM ******/
						SET ANSI_NULLS ON
						GO
						SET QUOTED_IDENTIFIER ON
						GO
						CREATE PROC [dbo].[sp_GenerateTable]
							@FormId int, 
							@TableName varchar(200)
						AS
						BEGIN
							IF (EXISTS (SELECT * 
										 FROM INFORMATION_SCHEMA.TABLES 
										 WHERE TABLE_NAME = @TableName))
							BEGIN
								EXEC(''DROP TABLE ''+@TableName+'')
							END
							SELECT AttributeCode 
							INTO #Temp1
							FROM tblCIMSAttributeForm
							WHERE FormId = @FormId
							GROUP BY AttributeCode
							ORDER BY AttributeCode

							SELECT Replace(t1.AttributeCode,''-'','') AttributeCode, t2.DataType 
							INTO #Temp2
							FROM #Temp1 t1
							INNER JOIN tblVOCAttributes t2 on t1.AttributeCode = t2.AttributeCode

							DECLARE @cols nvarchar(max) = ''
							SET @cols = STUFF((SELECT '','' + QUOTENAME(c.AttributeCode) + '' '' +c.DataType
									FROM #Temp2 c
									FOR XML PATH(''), TYPE
									).value(''.'', ''NVARCHAR(MAX)'') 
								,1,1,'''')
							SET @cols = N''Create table ''+@TableName+ ''('' + @cols + '')''
							SELECT @cols AS Cols
							EXEC(@cols)
							DROP TABLE #Temp1, #Temp2
						END
						-- sp_GenerateTable 23, ''tblCustomer''
						GO
						/****** Object:  StoredProcedure [dbo].[test]    Script Date: 5/5/2019 12:44:45 PM ******/
						SET ANSI_NULLS ON
						GO
						SET QUOTED_IDENTIFIER ON
						GO

						CREATE PROC [dbo].[test]
						AS 
						BEGIN
						DECLARE @cols NVARCHAR(MAX), @query NVARCHAR(MAX);
						SET @cols = STUFF(
										 (
											 SELECT DISTINCT
													'',''+QUOTENAME(c.[AttributeCode])
											 FROM [dbo].[tblCIMSAttributeValue] c FOR XML PATH(''), TYPE
										 ).value(''.'', ''nvarchar(max)''), 1, 1, '''');
						SET @query = ''SELECT [RecordId], ''+@cols+''from (SELECT [RecordId],
								   CAST([AttributeValue] AS NVARCHAR(MAX)) AS AVALUE,
								   [AttributeCode]
							FROM [dbo].[tblCIMSAttributeValue]
							)x pivot (max(AVALUE) for [AttributeCode] in (''+@cols+'')) p'';
						EXECUTE (@query);
						END'
	PRINT(@SqlSchema)
	--PRINT(@SqlStore)
	EXEC(@SqlSchema)
	--EXEC(@SqlStore)
END

-- sp_Generate_Database_Schema 'CRM_CMC'

GO
/****** Object:  StoredProcedure [dbo].[sp_GenerateTable]    Script Date: 6/7/2019 5:05:43 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[sp_GenerateTable]
	@FormId int, 
	@TableName varchar(200)
AS
BEGIN
	IF (EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_NAME = @TableName))
	BEGIN
		EXEC('DROP TABLE '+@TableName+'')
	END
	SELECT AttributeCode 
	INTO #Temp1
	FROM tblCIMSAttributeForm
	WHERE FormId = @FormId
	GROUP BY AttributeCode
	ORDER BY AttributeCode

	SELECT Replace(t1.AttributeCode,'-','') AttributeCode, t2.DataType 
	INTO #Temp2
	FROM #Temp1 t1
	INNER JOIN tblVOCAttributes t2 on t1.AttributeCode = t2.AttributeCode

	DECLARE @cols nvarchar(max) = ''
	SET @cols = STUFF((SELECT ',' + QUOTENAME(c.AttributeCode) + ' ' +c.DataType
            FROM #Temp2 c
            FOR XML PATH(''), TYPE
            ).value('.', 'NVARCHAR(MAX)') 
        ,1,1,'')
	SET @cols = N'Create table '+@TableName+ '(' + @cols + ')'
	SELECT @cols AS Cols
	EXEC(@cols)
	DROP TABLE #Temp1, #Temp2
END
-- sp_GenerateTable 23, 'tblCustomer'




GO
/****** Object:  StoredProcedure [dbo].[sp_GetAllCategory]    Script Date: 6/7/2019 5:05:43 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[sp_GetAllCategory]
As
Begin
	Select CategoryCode, CategoryTypeCode, CategoryName 
	from tblCategory 
	where IsDelete = 0 and ISNULL(CategoryTypeCode,'') = ''
	Union all
	Select CategoryCode, CategoryTypeCode, CategoryGroupName 
	from tblCategoryGroup a
	where IsDelete = 0 
End

-- sp_GetAllCategory 

GO
/****** Object:  StoredProcedure [dbo].[sp_GetCIMSValue]    Script Date: 6/7/2019 5:05:43 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[sp_GetCIMSValue]
	@ModuleParent nvarchar(250),
	@currPage int,
	@recodperpage int
As
Begin
	exec GetCimsvalue @ModuleParent,@currPage,@recodperpage
End

-- -- sp_GetCIMSValue 'CIMS',1,20


GO
/****** Object:  StoredProcedure [dbo].[sp_lstAttributes]    Script Date: 6/7/2019 5:05:43 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_lstAttributes] (
	@param int
)
AS
BEGIN
select * from tblVOCAttributes as x where x.AttributeCode in 
	(
		select c.AttributeCode from tblCIMSAttributeForm as c
		where c.FormId = @param
	)
END	
GO
