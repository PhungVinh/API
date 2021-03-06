﻿
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
/****** Object:  Table [dbo].[TblAttributeConstraint]    Script Date: 6/14/2019 1:47:54 PM ******/
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
/****** Object:  Table [dbo].[tblAttributeOptions]    Script Date: 6/14/2019 1:47:54 PM ******/
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
/****** Object:  Table [dbo].[tblCategory]    Script Date: 6/14/2019 1:47:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblCategory](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CategoryCode] [nvarchar](200) NULL,
	[CategoryTypeCode] [nvarchar](200) NULL,
	[CategoryName] [nvarchar](200) NULL,
	[ExtContent] [nvarchar](500) NULL,
	[CreateBy] [nvarchar](50) NULL,
	[CreateDate] [datetime] NULL,
	[UpdateBy] [nvarchar](50) NULL,
	[UpdateDate] [datetime] NULL,
	[IsDelete] [bit] NULL,
	[IsActive] [int] NULL,
	[OrderNum] [int] NULL,
	[CategoryDescription] [nvarchar](500) NULL,
 CONSTRAINT [PK_tblCategory] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblCategoryGroup]    Script Date: 6/14/2019 1:47:54 PM ******/
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
/****** Object:  Table [dbo].[tblCIMSAttributeForm]    Script Date: 6/14/2019 1:47:54 PM ******/
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
/****** Object:  Table [dbo].[tblCIMSAttributeValue]    Script Date: 6/14/2019 1:47:54 PM ******/
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
/****** Object:  Table [dbo].[tblCIMSForm]    Script Date: 6/14/2019 1:47:54 PM ******/
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
/****** Object:  Table [dbo].[tblCimsFormHistory]    Script Date: 6/14/2019 1:47:54 PM ******/
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
 CONSTRAINT [PK_CimsFormHistory] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblConnectionConfig]    Script Date: 6/14/2019 1:47:54 PM ******/
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
/****** Object:  Table [dbo].[tblCustomer]    Script Date: 6/14/2019 1:47:54 PM ******/
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
/****** Object:  Table [dbo].[tblEncryption]    Script Date: 6/14/2019 1:47:54 PM ******/
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
	[OrgCode] [nvarchar](50) NULL,
	[Field] [nvarchar](50) NULL,
 CONSTRAINT [PK_tblEncryption] PRIMARY KEY CLUSTERED 
(
	[EncryptionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblReferenceConstraint]    Script Date: 6/14/2019 1:47:54 PM ******/
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
/****** Object:  Table [dbo].[tblVOCAttributeForm]    Script Date: 6/14/2019 1:47:54 PM ******/
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
/****** Object:  Table [dbo].[tblVOCAttributes]    Script Date: 6/14/2019 1:47:54 PM ******/
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
/****** Object:  Table [dbo].[tblVOCForm]    Script Date: 6/14/2019 1:47:54 PM ******/
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
/****** Object:  Table [dbo].[tblVOCStepAttributes]    Script Date: 6/14/2019 1:47:54 PM ******/
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
/****** Object:  Table [dbo].[tblVOCStepAttributesValue]    Script Date: 6/14/2019 1:47:54 PM ******/
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
/****** Object:  Table [dbo].[tblVOCSteps]    Script Date: 6/14/2019 1:47:54 PM ******/
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
/****** Object:  Table [dbo].[tblVOCWorkflows]    Script Date: 6/14/2019 1:47:54 PM ******/
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
/****** Object:  Table [dbo].[tblVOCWorkflowSteps]    Script Date: 6/14/2019 1:47:54 PM ******/
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
/****** Object:  View [dbo].[Encrpytion_View]    Script Date: 6/14/2019 1:47:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[Encrpytion_View] AS
SELECT * FROM tblEncryption
GO
SET IDENTITY_INSERT [dbo].[tblCategory] ON 

INSERT [dbo].[tblCategory] ([ID], [CategoryCode], [CategoryTypeCode], [CategoryName], [ExtContent], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete], [IsActive], [OrderNum], [CategoryDescription]) VALUES (1, N'DataCode-01', N'DataCode', N'Kiểu chữ', NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL)
INSERT [dbo].[tblCategory] ([ID], [CategoryCode], [CategoryTypeCode], [CategoryName], [ExtContent], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete], [IsActive], [OrderNum], [CategoryDescription]) VALUES (2, N'DataCode-02', N'DataCode', N'Kiểu số nguyên', NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL)
INSERT [dbo].[tblCategory] ([ID], [CategoryCode], [CategoryTypeCode], [CategoryName], [ExtContent], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete], [IsActive], [OrderNum], [CategoryDescription]) VALUES (3, N'DataCode-03', N'DataCode', N'Kiểu số thực', NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL)
INSERT [dbo].[tblCategory] ([ID], [CategoryCode], [CategoryTypeCode], [CategoryName], [ExtContent], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete], [IsActive], [OrderNum], [CategoryDescription]) VALUES (6, N'Control-01', N'Control', N'TEXTBOX', NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL)
INSERT [dbo].[tblCategory] ([ID], [CategoryCode], [CategoryTypeCode], [CategoryName], [ExtContent], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete], [IsActive], [OrderNum], [CategoryDescription]) VALUES (7, N'Control-02', N'Control', N'CHECKBOX', NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL)
INSERT [dbo].[tblCategory] ([ID], [CategoryCode], [CategoryTypeCode], [CategoryName], [ExtContent], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete], [IsActive], [OrderNum], [CategoryDescription]) VALUES (8, N'Control-03', N'Control', N'TEXTAREA', NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL)
INSERT [dbo].[tblCategory] ([ID], [CategoryCode], [CategoryTypeCode], [CategoryName], [ExtContent], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete], [IsActive], [OrderNum], [CategoryDescription]) VALUES (9, N'Control-04', N'Control', N'DROPDOWNLIST', NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL)
INSERT [dbo].[tblCategory] ([ID], [CategoryCode], [CategoryTypeCode], [CategoryName], [ExtContent], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete], [IsActive], [OrderNum], [CategoryDescription]) VALUES (10, N'Control-05', N'Control', N'LISTCHECKBOX', NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL)
INSERT [dbo].[tblCategory] ([ID], [CategoryCode], [CategoryTypeCode], [CategoryName], [ExtContent], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete], [IsActive], [OrderNum], [CategoryDescription]) VALUES (11, N'Control-06', N'Control', N'RADIO', NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL)
INSERT [dbo].[tblCategory] ([ID], [CategoryCode], [CategoryTypeCode], [CategoryName], [ExtContent], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete], [IsActive], [OrderNum], [CategoryDescription]) VALUES (12, N'Control-07', N'Control', N'LISTRADIO', NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL)
INSERT [dbo].[tblCategory] ([ID], [CategoryCode], [CategoryTypeCode], [CategoryName], [ExtContent], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete], [IsActive], [OrderNum], [CategoryDescription]) VALUES (21, N'Department-01', N'Department', N'Phòng Kế Toán', NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL)
INSERT [dbo].[tblCategory] ([ID], [CategoryCode], [CategoryTypeCode], [CategoryName], [ExtContent], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete], [IsActive], [OrderNum], [CategoryDescription]) VALUES (22, N'Department-02', N'Department', N'Phòng Kinh Doanh', NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL)
INSERT [dbo].[tblCategory] ([ID], [CategoryCode], [CategoryTypeCode], [CategoryName], [ExtContent], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete], [IsActive], [OrderNum], [CategoryDescription]) VALUES (23, N'Department-03', N'Department', N'Trung Tâm VAS', NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL)
INSERT [dbo].[tblCategory] ([ID], [CategoryCode], [CategoryTypeCode], [CategoryName], [ExtContent], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete], [IsActive], [OrderNum], [CategoryDescription]) VALUES (92, N'DataCode-04', N'DataCode', N'Kiểu link', NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL)
INSERT [dbo].[tblCategory] ([ID], [CategoryCode], [CategoryTypeCode], [CategoryName], [ExtContent], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete], [IsActive], [OrderNum], [CategoryDescription]) VALUES (93, N'DataCode-05', N'DataCode', N'Kiểu ảnh', NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL)
INSERT [dbo].[tblCategory] ([ID], [CategoryCode], [CategoryTypeCode], [CategoryName], [ExtContent], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete], [IsActive], [OrderNum], [CategoryDescription]) VALUES (94, N'DataCode-06', N'DataCode', N'Kiểu Email', NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL)
INSERT [dbo].[tblCategory] ([ID], [CategoryCode], [CategoryTypeCode], [CategoryName], [ExtContent], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete], [IsActive], [OrderNum], [CategoryDescription]) VALUES (95, N'DataCode-07', N'DataCode', N'Kiểu số điện thoại', NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL)
INSERT [dbo].[tblCategory] ([ID], [CategoryCode], [CategoryTypeCode], [CategoryName], [ExtContent], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete], [IsActive], [OrderNum], [CategoryDescription]) VALUES (96, N'DataCode-08', N'DataCode', N'Kiểu mật khẩu ', NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL)
INSERT [dbo].[tblCategory] ([ID], [CategoryCode], [CategoryTypeCode], [CategoryName], [ExtContent], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete], [IsActive], [OrderNum], [CategoryDescription]) VALUES (97, N'DataCode-09', N'DataCode', N'Date', NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL)
INSERT [dbo].[tblCategory] ([ID], [CategoryCode], [CategoryTypeCode], [CategoryName], [ExtContent], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete], [IsActive], [OrderNum], [CategoryDescription]) VALUES (98, N'DataCode-10', N'DataCode', N'Datetime', NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL)
INSERT [dbo].[tblCategory] ([ID], [CategoryCode], [CategoryTypeCode], [CategoryName], [ExtContent], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete], [IsActive], [OrderNum], [CategoryDescription]) VALUES (1311, N'Object-01', N'Object', N'Dropdown', NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL)
INSERT [dbo].[tblCategory] ([ID], [CategoryCode], [CategoryTypeCode], [CategoryName], [ExtContent], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete], [IsActive], [OrderNum], [CategoryDescription]) VALUES (1312, N'Object-02', N'Object', N'Textbox', NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL)
INSERT [dbo].[tblCategory] ([ID], [CategoryCode], [CategoryTypeCode], [CategoryName], [ExtContent], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete], [IsActive], [OrderNum], [CategoryDescription]) VALUES (1313, N'Object-03', N'Object', N'Checkbox', NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL)
INSERT [dbo].[tblCategory] ([ID], [CategoryCode], [CategoryTypeCode], [CategoryName], [ExtContent], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete], [IsActive], [OrderNum], [CategoryDescription]) VALUES (1314, N'Object-04', N'Object', N'Text area', NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL)
INSERT [dbo].[tblCategory] ([ID], [CategoryCode], [CategoryTypeCode], [CategoryName], [ExtContent], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete], [IsActive], [OrderNum], [CategoryDescription]) VALUES (1315, N'Object-05', N'Object', N'Text', NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL)
INSERT [dbo].[tblCategory] ([ID], [CategoryCode], [CategoryTypeCode], [CategoryName], [ExtContent], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete], [IsActive], [OrderNum], [CategoryDescription]) VALUES (1316, N'Object-06', N'Object', N'Label', NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL)
INSERT [dbo].[tblCategory] ([ID], [CategoryCode], [CategoryTypeCode], [CategoryName], [ExtContent], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete], [IsActive], [OrderNum], [CategoryDescription]) VALUES (1410, N'Position-01', N'Position', N'Giám đốc', NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL)
INSERT [dbo].[tblCategory] ([ID], [CategoryCode], [CategoryTypeCode], [CategoryName], [ExtContent], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete], [IsActive], [OrderNum], [CategoryDescription]) VALUES (1411, N'Position-02', N'Position', N'Phó giám đốc', NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL)
INSERT [dbo].[tblCategory] ([ID], [CategoryCode], [CategoryTypeCode], [CategoryName], [ExtContent], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete], [IsActive], [OrderNum], [CategoryDescription]) VALUES (1412, N'Position-03', N'Position', N'Trưởng phòng', NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL)
INSERT [dbo].[tblCategory] ([ID], [CategoryCode], [CategoryTypeCode], [CategoryName], [ExtContent], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete], [IsActive], [OrderNum], [CategoryDescription]) VALUES (1413, N'Position-04', N'Position', N'Nhân viên', NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL)
INSERT [dbo].[tblCategory] ([ID], [CategoryCode], [CategoryTypeCode], [CategoryName], [ExtContent], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete], [IsActive], [OrderNum], [CategoryDescription]) VALUES (1414, N'TEST1', N'', N'test1', NULL, N'adminFSOFT', CAST(N'2019-06-14 11:30:57.173' AS DateTime), NULL, NULL, 0, NULL, NULL, N'')
INSERT [dbo].[tblCategory] ([ID], [CategoryCode], [CategoryTypeCode], [CategoryName], [ExtContent], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete], [IsActive], [OrderNum], [CategoryDescription]) VALUES (1415, N'TEST2', N'TEST1', N'test2', N'', NULL, CAST(N'2019-06-14 11:30:57.290' AS DateTime), NULL, NULL, 0, NULL, NULL, NULL)
INSERT [dbo].[tblCategory] ([ID], [CategoryCode], [CategoryTypeCode], [CategoryName], [ExtContent], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete], [IsActive], [OrderNum], [CategoryDescription]) VALUES (1416, N'Control-08', N'Control', N'DATETIME', NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL)
INSERT [dbo].[tblCategory] ([ID], [CategoryCode], [CategoryTypeCode], [CategoryName], [ExtContent], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete], [IsActive], [OrderNum], [CategoryDescription]) VALUES (1417, N'Control-09', N'Control', N'DATETODATE ', NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL)
INSERT [dbo].[tblCategory] ([ID], [CategoryCode], [CategoryTypeCode], [CategoryName], [ExtContent], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete], [IsActive], [OrderNum], [CategoryDescription]) VALUES (1418, N'Control-10', N'Control', N'LABEL', NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[tblCategory] OFF
SET IDENTITY_INSERT [dbo].[tblVOCAttributes] ON 

INSERT [dbo].[tblVOCAttributes] ([AttributesId], [AttributeCode], [AttributeType], [AttributeWidth], [DataType], [AttributeLabel], [DefaultValue], [IsVisible], [IsRequired], [IsTableShow], [IsCategory], [ModuleParent], [IsSort], [IsDuplicate], [DetailRefer], [CreateBy], [CreateDate], [UpDateBy], [UpdateDate], [IsDelete], [IsReuse], [CategoryParentCode], [AttributeDescription], [Encyption], [EncyptWaiting], [IndexTitleTable], [DefaultValueWithTextBox], [Disabled]) VALUES (220, N'ATTRIBUTE1', N'TEXTBOX', NULL, N'DataCode-01', N'Người tạo', NULL, 1, 0, 1, NULL, N'CIMS', NULL, 0, NULL, N'admin', NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, NULL, 1, NULL, NULL)
INSERT [dbo].[tblVOCAttributes] ([AttributesId], [AttributeCode], [AttributeType], [AttributeWidth], [DataType], [AttributeLabel], [DefaultValue], [IsVisible], [IsRequired], [IsTableShow], [IsCategory], [ModuleParent], [IsSort], [IsDuplicate], [DetailRefer], [CreateBy], [CreateDate], [UpDateBy], [UpdateDate], [IsDelete], [IsReuse], [CategoryParentCode], [AttributeDescription], [Encyption], [EncyptWaiting], [IndexTitleTable], [DefaultValueWithTextBox], [Disabled]) VALUES (222, N'ATTRIBUTE2', N'TEXTBOX', NULL, N'DataCode-01', N'Ngày tạo', NULL, 1, 0, 1, NULL, N'CIMS', NULL, 0, NULL, N'admin', NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, NULL, 2, NULL, NULL)
SET IDENTITY_INSERT [dbo].[tblVOCAttributes] OFF
SET IDENTITY_INSERT [dbo].[tblVOCForm] ON 

INSERT [dbo].[tblVOCForm] ([FormId], [FormName], [FormDescription], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate], [IsDelete]) VALUES (1, N'VOC-Step1', N'step1', N'HiepPD1', NULL, NULL, NULL, 0)
SET IDENTITY_INSERT [dbo].[tblVOCForm] OFF
ALTER TABLE [dbo].[tblVOCStepAttributesValue]  WITH CHECK ADD  CONSTRAINT [FK_tblVOCStepAttributesValue_tblVOCSteps] FOREIGN KEY([StepId])
REFERENCES [dbo].[tblVOCSteps] ([StepId])
GO
ALTER TABLE [dbo].[tblVOCStepAttributesValue] CHECK CONSTRAINT [FK_tblVOCStepAttributesValue_tblVOCSteps]
GO
/****** Object:  StoredProcedure [dbo].[cims_GetCustomerList]    Script Date: 6/14/2019 1:47:54 PM ******/
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

-- cims_GetCustomerList 'CIMS_LIST', 1, 20

GO
/****** Object:  StoredProcedure [dbo].[cims_GetCustomerList_RecordId]    Script Date: 6/14/2019 1:47:54 PM ******/
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
	-- Comment by daibh
	-- and a.IsTableShow = 1
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

-- cims_GetCustomerList_RecordId '201906061728157896430'

GO
/****** Object:  StoredProcedure [dbo].[GetAllCategory]    Script Date: 6/14/2019 1:47:54 PM ******/
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

		Drop table #Temp
END


-- exec GetAllCategory '',1,15

GO
/****** Object:  StoredProcedure [dbo].[GetAllCategory_New]    Script Date: 6/14/2019 1:47:54 PM ******/
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

	-- HaiHM add TOP 1 - line 32
	Select t.*, (Select a.CategoryName from #Category a where a.CategoryCode = t.CategoryTypeCode) CategoryTypeName into #Category1 from #Category t

	Select ROW_NUMBER() OVER(ORDER BY a.CreateDate desc) [index],a.CategoryCode,ISNULL(a.CategoryTypeCode,'') as CategoryTypeCode,ISNULL(a.CategoryName,'') as CategoryName,ISNULL(a.CategoryTypeName,'') as CategoryTypeName,ISNULL(a.CreateBy,'') as CreateBy,a.CreateDate
		into #Temp
				from #Category1 a

		Select @Total = Count(*) from #Temp

	if @currPage > 1
		Begin
			Set @start = (@currPage -1)*@recodperpage + 1
			Set @end = @currPage *@recodperpage
		End
	else
		Begin
			Set @start = 1
			Set @end = @recodperpage
		End

	--Tinh page size
		--Select @PageSize = case when @recodperpage <> 0 and @Total%@recodperpage = 0 then @Total/@recodperpage else case when @recodperpage <> 0 then @Total/@recodperpage + 1 else 0 end end 
		--Select Case when @PageSize <> 0 then @Total else 0 end TotalPage, Case when @PageSize <> 0 then @currPage else 0 end CurrPage, @PageSize PageSize
		Select @PageSize = case when @Total%@recodperpage = 0 then @Total/@recodperpage else @Total/@recodperpage + 1 end 
		Select * from #Temp
		where [index] between @start and @end

		Select @Total TotalPage, @currPage CurrPage, @PageSize PageSize

		Drop table #Temp,#Category,#Category1
END


-- exec GetAllCategory_New '',1,15
GO
/****** Object:  StoredProcedure [dbo].[GetAllChildCategory]    Script Date: 6/14/2019 1:47:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[GetAllChildCategory]
@Category varchar(300)
as
begin
	select a.CategoryCode,a.CategoryName,a.CategoryTypeCode,a.ExtContent,
	Case when (((select COUNT(*) from tblVOCAttributes b where b.IsDelete = 0 and (b.DataType =a.CategoryCode or b.AttributeType =a.CategoryCode or b.CategoryParentCode=a.CategoryCode or b.DefaultValue = a.CategoryCode)) >0 ) or ((select count(*) from TblAttributeConstraint c where c.IsDelete =0 and (c.ContraintsType =a.CategoryCode or c.ContraintsValue=a.CategoryCode)) >0)) Then CONVERT(bit,1) else CONVERT(bit,0) end IsCheck
	from tblCategory a
	where a.IsDelete =0 
	and a.CategoryTypeCode =@Category
	order by a.CreateDate ASC
end



GO
/****** Object:  StoredProcedure [dbo].[GetAllConstraint]    Script Date: 6/14/2019 1:47:54 PM ******/
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
/****** Object:  StoredProcedure [dbo].[GetAllConstraint1]    Script Date: 6/14/2019 1:47:54 PM ******/
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
/****** Object:  StoredProcedure [dbo].[GetAllEncryptData]    Script Date: 6/14/2019 1:47:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[GetAllEncryptData]
AS
BEGIN
	SELECT a.AttributeCode, a.ParentCode, a.EncryptionStatus, b.AttributeValue, b.RecordId 
	FROM tblEncryption a
	INNER JOIN tblCIMSAttributeValue b on a.AttributeCode = b.AttributeCode and a.ParentCode = b.Module
	WHERE a.EncryptionStatus = 1 AND IsFirst = 1
END
GO
/****** Object:  StoredProcedure [dbo].[GetAttributeValue]    Script Date: 6/14/2019 1:47:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[GetAttributeValue](
 @EncryptionStatus bit
)
as
begin
	select * from tblCIMSAttributeValue a
	inner join tblEncryption b on a.AttributeCode = b.AttributeCode and a.Module = b.ParentCode
	where b.EncryptionStatus = @EncryptionStatus
end
GO
/****** Object:  StoredProcedure [dbo].[GetCimsvalue]    Script Date: 6/14/2019 1:47:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[GetCimsvalue]

@ModuleParent nvarchar(250)
AS 
BEGIN
Select a.*
into #Temp1
from tblCIMSAttributeValue a
where a.IsDelete = 0
--and (isnull(@TextSearch, '') = '' or a. like '%'+@TextSearch+'%' or a.CanFullName like '%'+@TextSearch+'%' or a.CanFullName like '%'+@TextSearch+'%')

Select a.*
into #Temp2
from tblVOCAttributes a
where a.IsTableShow= 1
Select t1.*, t2.AttributeName
into #Temp3
from #Temp1 t1

inner join #Temp2 t2 on t1.AttributeCode = t2.AttributeCode
where t1.Module=@ModuleParent


DECLARE @cols NVARCHAR(MAX), @query NVARCHAR(MAX);
SET @cols = STUFF(
                 (
                     SELECT DISTINCT
                            ','+QUOTENAME(c.[AttributeName])
                     FROM #Temp3 c FOR XML PATH(''), TYPE
                 ).value('.', 'nvarchar(max)'), 1, 1, '')

SET @query = 'SELECT [RecordId], '+@cols+'from (SELECT [RecordId],
           CAST([AttributeValue] AS NVARCHAR(MAX)) AS AVALUE,
           [AttributeName]
		   
    FROM #Temp3
    )x pivot (max(AVALUE) for [AttributeName] in ('+@cols+')) p';
EXECUTE (@query);
--Select * from #Temp3

Drop table #Temp1, #Temp2,#Temp3
END
-- GetCimsvalue 'CIMS'


GO
/****** Object:  StoredProcedure [dbo].[GetOrg_Page]    Script Date: 6/14/2019 1:47:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetOrg_Page]
	@DateFrom nvarchar(50),
	@DateTo nvarchar(50),
	@TextSearch nvarchar(200),
	@IsActive int,
	@currPage int,
	@recodperpage int
AS
Begin
	Declare @Total int, @PageSize int
	Declare @start int
	Declare @end int
	Declare @NumberUser int
	if @IsActive = 2
	begin
		Select ROW_NUMBER() OVER(ORDER BY a.UpdateDate desc) [index],a.*,
		case when (Select Count(*) from tblOrganizationUser where OrganizationID = a.OrganizationID) > 0 then 'false' else 'true' end chkDelete ,
		(select Count(*) from tblOrganizationUser x
		inner join tblUsers y on x.UserId = y.ID where x.OrganizationID = a.OrganizationID and y.IsDelete = 0 and y.IsLock = 0) NumberUser
		into #Temp
				from tblOrganization a
				Where a.IsDelete = 0 
				and (isnull(@TextSearch, '') = '' or a.OrganizationName like '%'+@TextSearch+'%'  or a.OrganizationCode like '%'+@TextSearch+'%' )
				and (isnull(@DateFrom,'') = '' or a.OrganizationTo between @DateFrom and @DateTo)
		Select @Total = Count(*) from #Temp
	
		if @currPage > 0
			Begin
				Set @start = (@currPage-1)*@recodperpage + 1
				Set @end = @currPage*@recodperpage

				Select @PageSize = case when @Total%@recodperpage = 0 then @Total/@recodperpage else @Total/@recodperpage + 1 end 
		Select ROW_NUMBER() OVER(ORDER BY a.OrganizationTo desc) [index],
		a.chkDelete,a.CreateBy,a.CreateDate,a.IsActive,a.IsDelete,a.IsLock,a.OrganizationAddress,a.OrganizationCode,
		a.OrganizationEmail,a.OrganizationFrom,a.OrganizationHomePage,a.OrganizationID,a.OrganizationLogo,a.OrganizationName,
		a.OrganizationNote,a.UpdateDate,a.UpdateBy,a.OrganizationTo,a.OrganizationTaxCode,a.OrganizationSphereID,a.OrganizationRemark,
		a.OrganizationPhone,a.OrganizationParentCode,a.NumberUser
		into #Temp111
		from #Temp a
		-- Goi dich vu 

		if Isnull(@DateFrom,'') = ''
			Begin
				Select t.*, dbo.GetListServiceByOrgId(t.OrganizationID) ServiceList
				
				from #Temp t
		where [index] between @start and @end
		Order by [index]
			End
		Else
			Begin
				Select t.* ,dbo.GetListServiceByOrgId(t.OrganizationID) ServiceList
				from #Temp111 t
		where [index] between @start and @end
			End


		Drop table #Temp111
			End
		else
			Begin
				Select @PageSize = case when @Total%@recodperpage = 0 then @Total/@recodperpage else @Total/@recodperpage + 1 end 
		Select ROW_NUMBER() OVER(ORDER BY a.OrganizationTo desc),
		a.* ,dbo.GetListServiceByOrgId(t.OrganizationID) ServiceList
		from #Temp a

			End

		--Tinh page size
		
		Select @Total TotalPage, @currPage CurrPage, @PageSize PageSize

		Drop table #Temp
		end
	else


	begin
	if @IsActive= 0
	begin
		Select ROW_NUMBER() OVER(ORDER BY a.UpdateDate desc) [index],a.*,
		case when (Select Count(*) from tblOrganizationUser where OrganizationID = a.OrganizationID) > 0 then 'false' else 'true' end chkDelete,
		(select Count(*) from tblOrganizationUser x
		inner join tblUsers y on x.UserId = y.ID where x.OrganizationID = a.OrganizationID and y.IsDelete = 0 and y.IsLock = 0) NumberUser
		into #Temp1
				from tblOrganization a
				Where a.IsDelete = 0 
				and a.IsActive=0
				and (isnull(@TextSearch, '') = '' or a.OrganizationName like '%'+@TextSearch+'%' or a.OrganizationCode like '%'+@TextSearch+'%')
				and (isnull(@DateFrom,'') = '' or a.OrganizationTo between @DateFrom and @DateTo)
		Select @Total = Count(*) from #Temp1
	
		if @currPage > 0
			Begin
				Set @start = (@currPage-1)*@recodperpage + 1
				Set @end = @currPage*@recodperpage

				Select @PageSize = case when @Total%@recodperpage = 0 then @Total/@recodperpage else @Total/@recodperpage + 1 end 
		--Select * from #Temp1
		--where [index] between @start and @end

		Select ROW_NUMBER() OVER(ORDER BY a.OrganizationTo desc) [index],
		a.chkDelete,a.CreateBy,a.CreateDate,a.IsActive,a.IsDelete,a.IsLock,a.OrganizationAddress,a.OrganizationCode,
		a.OrganizationEmail,a.OrganizationFrom,a.OrganizationHomePage,a.OrganizationID,a.OrganizationLogo,a.OrganizationName,
		a.OrganizationNote,a.UpdateDate,a.UpdateBy,a.OrganizationTo,a.OrganizationTaxCode,a.OrganizationSphereID,a.OrganizationRemark,
		a.OrganizationPhone,a.OrganizationParentCode,a.NumberUser
		into #Temp222
		from #Temp1 a


		if Isnull(@DateFrom,'') = ''
			Begin
				Select t.* ,dbo.GetListServiceByOrgId(t.OrganizationID) ServiceList
				from #Temp222 t
		where [index] between @start and @end
		Order by [index]
			End
		Else
			Begin
				Select t.* ,dbo.GetListServiceByOrgId(t.OrganizationID) ServiceList
				from #Temp222 t
		where [index] between @start and @end
		Order by [index]
			End


		Drop table #Temp222


			End
		else
			Begin
				Select @PageSize = case when @Total%@recodperpage = 0 then @Total/@recodperpage else @Total/@recodperpage + 1 end 
		Select * from #Temp1


			End
		Select @Total TotalPage, @currPage CurrPage, @PageSize PageSize
		Drop table #Temp1
	end
	else
	begin
	Select ROW_NUMBER() OVER(ORDER BY a.UpdateDate desc) [index],a.*,
		case when (Select Count(*) from tblOrganizationUser where OrganizationID = a.OrganizationID) > 1 then 'false' else 'true' end chkDelete ,
		(select Count(*) from tblOrganizationUser x
		inner join tblUsers y on x.UserId = y.ID where x.OrganizationID = a.OrganizationID and y.IsDelete = 0 and y.IsLock = 0) NumberUser
		into #Temp2
				from tblOrganization a
				Where a.IsDelete = 0 
				and a.IsActive=1
				and (isnull(@TextSearch, '') = '' or a.OrganizationName like '%'+@TextSearch+'%' or a.OrganizationCode like '%'+@TextSearch+'%')
				and (isnull(@DateFrom,'') = '' or a.OrganizationTo between @DateFrom and @DateTo)
		Select @Total = Count(*) from #Temp2
			if @currPage > 0
			Begin
				Set @start = (@currPage-1)*@recodperpage + 1
				Set @end = @currPage*@recodperpage

				Select @PageSize = case when @Total%@recodperpage = 0 then @Total/@recodperpage else @Total/@recodperpage + 1 end 
		--Select * from #Temp2
		--where [index] between @start and @end
		Select ROW_NUMBER() OVER(ORDER BY a.OrganizationTo desc) [index],
		a.chkDelete,a.CreateBy,a.CreateDate,a.IsActive,a.IsDelete,a.IsLock,a.OrganizationAddress,a.OrganizationCode,
		a.OrganizationEmail,a.OrganizationFrom,a.OrganizationHomePage,a.OrganizationID,a.OrganizationLogo,a.OrganizationName,
		a.OrganizationNote,a.UpdateDate,a.UpdateBy,a.OrganizationTo,a.OrganizationTaxCode,a.OrganizationSphereID,a.OrganizationRemark,
		a.OrganizationPhone,a.OrganizationParentCode,a.NumberUser
		into #Temp22
		from #Temp2 a
		
		if Isnull(@DateFrom,'') = ''
			Begin
				Select t.* ,dbo.GetListServiceByOrgId(t.OrganizationID) ServiceList
				from #Temp22 t
		where [index] between @start and @end
		Order by [index]
			End
		Else
			Begin
				Select t.* ,dbo.GetListServiceByOrgId(t.OrganizationID) ServiceList
				from #Temp22 t
		where [index] between @start and @end
		Order by [index]
			End

		Drop table #Temp22
			End
		else
			Begin
				Select @PageSize = case when @Total%@recodperpage = 0 then @Total/@recodperpage else @Total/@recodperpage + 1 end 
		Select * from #Temp2
			End

		Select @Total TotalPage, @currPage CurrPage, @PageSize PageSize
		Drop table #Temp2
	end
	end	
	End
	 
-- totalPage
-- currentPage
-- pageSize

-- GetOrg_Page  '','','',2,1,10





GO
/****** Object:  StoredProcedure [dbo].[sp_CheckHasDataEncrypted]    Script Date: 6/14/2019 1:47:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		HaiHM
-- Create date: 11/06/2019
-- Description:	Lấy ra danh sách các trường đã mã hóa xong
-- =============================================
CREATE PROCEDURE [dbo].[sp_CheckHasDataEncrypted]
	-- Add the parameters for the stored procedure here
	@AttributeLabel nvarchar(200) = '',
	@ParentCode nvarchar(200) = ''
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * FROM [tblEncryption] e
		WHERE e.IsFirst = 0  
			--AND e.IsDone = 1 
			AND e.FinalizationStatus = 1
			AND((isnull(@AttributeLabel, '') = ' ' or e.Field = @AttributeLabel))
			AND ((isnull(@ParentCode, '') = ' ' or e.ParentCode = @ParentCode) )
END

GO
/****** Object:  StoredProcedure [dbo].[sp_ExecuteEncryption]    Script Date: 6/14/2019 1:47:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[sp_ExecuteEncryption](
	@AttributeCode nvarchar(50),
	@AttributeValue nvarchar(50),
	@RecordId nvarchar(50),
	@ModuleCode nvarchar(50)
)
as
begin
	update tblCIMSAttributeValue set AttributeValue = @AttributeValue 
	where AttributeCode = @AttributeCode and RecordId = @RecordId and Module = @ModuleCode
end
GO
/****** Object:  StoredProcedure [dbo].[sp_FindAttribuiteNewByParentCode]    Script Date: 6/14/2019 1:47:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[sp_FindAttribuiteNewByParentCode]
as
begin
	select a.AttributeValue, a.RecordId from tblCIMSAttributeValue a
	inner join tblEncryption b on a.AttributeCode = b.AttributeCode and a.Module = b.ParentCode
	where a.CreatedDate > b.UpdateDate and b.FinalizationStatus = 1 and b.IsFirst = 0
end
GO
/****** Object:  StoredProcedure [dbo].[sp_FindAttributeNew]    Script Date: 6/14/2019 1:47:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[sp_FindAttributeNew]
as
begin
	select a.AttributeCode, a.AttributeLabel, a.ParentCode, a.UpdateDate from tblEncryption a
	where a.IsFirst = 0 and a.FinalizationStatus = 1
end

GO
/****** Object:  StoredProcedure [dbo].[sp_GenerateTable]    Script Date: 6/14/2019 1:47:54 PM ******/
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
/****** Object:  StoredProcedure [dbo].[sp_GetAllCategoryByTypeCode]    Script Date: 6/14/2019 1:47:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		HaiHM
-- Create date: 14/6/2019
-- Description:	get all category
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetAllCategoryByTypeCode]
	@CategoryTypeCode nvarchar(50)
	
AS
BEGIN
	SET NOCOUNT ON;

   SELECT * FROM tblCategory c
		WHERE c.IsDelete = 0 and c.CategoryTypeCode = @CategoryTypeCode
END

GO
/****** Object:  StoredProcedure [dbo].[sp_GetAttributeValueByTblEncryption]    Script Date: 6/14/2019 1:47:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[sp_GetAttributeValueByTblEncryption](
	@AttributeCode nvarchar(50),
	@ParentCode nvarchar(50)
)
as
begin
	select b.RecordId, b.AttributeValue from tblEncryption a 
	inner join tblCIMSAttributeValue b on a.AttributeCode = b.AttributeCode and a.ParentCode = b.Module
	where a.AttributeCode = @AttributeCode and a.ParentCode = @ParentCode
end
GO
/****** Object:  StoredProcedure [dbo].[sp_GetAttributeValueEncryption]    Script Date: 6/14/2019 1:47:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[sp_GetAttributeValueEncryption](
 @StartDate datetime,
 @EndDate datetime
)
as
begin
	select a.AttributeCode, a.ParentCode, a.EncryptionStatus, a.AttributeLabel from tblEncryption a	
	where a.IsDone = 0
end
GO
/****** Object:  StoredProcedure [dbo].[sp_lstAttributes]    Script Date: 6/14/2019 1:47:54 PM ******/
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
/****** Object:  StoredProcedure [dbo].[sp_UpdateAttributeEncryption]    Script Date: 6/14/2019 1:47:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[sp_UpdateAttributeEncryption](
	@RecordId NVARCHAR(100),
	@Module NVARCHAR(20),
	@AttrCode NVARCHAR(50),
	@AttrValue NVARCHAR(MAX),
	@EncryptionStatus bit,
	@UpdateDate datetime
)
as
begin
	UPDATE tblCIMSAttributeValue
	SET AttributeValue = @AttrValue
	WHERE RecordId = @RecordId
	AND AttributeCode = @AttrCode
	-----------
	UPDATE tblEncryption 
	SET IsFirst = 0, FinalizationStatus = @EncryptionStatus, IsDone = 1, UpdateDate = @UpdateDate
	WHERE AttributeCode = @AttrCode
	AND ParentCode = @Module
end
GO
/****** Object:  StoredProcedure [dbo].[sp_UpdateAttributeEncryptionNew]    Script Date: 6/14/2019 1:47:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[sp_UpdateAttributeEncryptionNew](
	@RecordId NVARCHAR(100),
	@Module NVARCHAR(20),
	@AttrCode NVARCHAR(50),
	@AttrValue NVARCHAR(MAX)
)
as
begin
	UPDATE tblCIMSAttributeValue
	SET AttributeValue = @AttrValue
	WHERE RecordId = @RecordId
	AND AttributeCode = @AttrCode
end
GO
/****** Object:  StoredProcedure [dbo].[sp_UpdateEncryptionStatus]    Script Date: 6/14/2019 1:47:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[sp_UpdateEncryptionStatus](
	@AttributeCode nvarchar(50),
	@ParentCode nvarchar(50),
	@EncryptionStatus bit,
	@UpdateDate datetime
)
as
begin
	update tblEncryption set IsFirst = 0, IsDone = 1, FinalizationStatus = @EncryptionStatus, UpdateDate = @UpdateDate
	where AttributeCode = @AttributeCode and ParentCode = @ParentCode
end
GO
/****** Object:  StoredProcedure [dbo].[UpdateEncryptionValue]    Script Date: 6/14/2019 1:47:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[UpdateEncryptionValue]
	@RecordId NVARCHAR(100),
	@Module NVARCHAR(20),
	@AttrCode NVARCHAR(50),
	@AttrValue NVARCHAR(MAX)
AS
BEGIN
	UPDATE tblCIMSAttributeValue
	SET AttributeValue = @AttrValue
	WHERE RecordId = @RecordId
	AND AttributeCode = @AttrCode
	-----------
	UPDATE tblEncryption 
	SET IsFirst = 0, FinalizationStatus = 1, IsDone = 1
	WHERE AttributeCode = @AttrCode
	AND ParentCode = @Module
END
GO
