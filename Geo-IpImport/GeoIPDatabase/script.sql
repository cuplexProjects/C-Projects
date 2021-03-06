USE [CuplexApi]
GO
/****** Object:  Table [dbo].[ApplicationVersion]    Script Date: 2016-03-15 08:29:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ApplicationVersion](
	[ApplicationId] [int] IDENTITY(1,1) NOT NULL,
	[ApplicationGUID] [char](36) NOT NULL,
	[Name] [varchar](250) NOT NULL,
	[LatestVersionNumber] [varchar](50) NOT NULL,
	[LatestVersionGUID] [char](36) NOT NULL,
	[LastUpdated] [datetime] NULL,
 CONSTRAINT [PK_ApplicationVersion] PRIMARY KEY CLUSTERED 
(
	[ApplicationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CountryBlock-IPv4]    Script Date: 2016-03-15 08:29:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CountryBlock-IPv4](
	[IPFrom] [bigint] NOT NULL,
	[IPTo] [bigint] NOT NULL,
	[Network] [varchar](18) NOT NULL,
	[GeoNameId] [int] NULL,
	[RegisteredCountryGeonameId] [int] NULL,
	[RepresentedCountryGeonameId] [int] NULL,
	[IsAnonymousProxy] [bit] NOT NULL,
	[IsSatelliteProvider] [bit] NOT NULL,
 CONSTRAINT [PK_CountryBlock-IPv4] PRIMARY KEY CLUSTERED 
(
	[IPFrom] ASC,
	[IPTo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CountryBlock-IPv6]    Script Date: 2016-03-15 08:29:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CountryBlock-IPv6](
	[IPv6BlockId] [int] IDENTITY(1,1) NOT NULL,
	[Network] [varchar](50) NOT NULL,
	[GeonameId] [int] NULL,
	[RegisteredCountryGeonameId] [int] NULL,
	[RepresentedCountryGeonameId] [int] NULL,
	[IsAnonymousProxy] [bit] NOT NULL,
	[IsSatelliteProvider] [bit] NOT NULL,
 CONSTRAINT [PK_CountryBlock-IPv6] PRIMARY KEY CLUSTERED 
(
	[IPv6BlockId] ASC,
	[Network] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CountryLocation]    Script Date: 2016-03-15 08:29:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CountryLocation](
	[GeonameId] [int] NOT NULL,
	[LocaleCode] [char](2) NOT NULL,
	[ContinentCode] [char](2) NOT NULL,
	[ContinentName] [varchar](50) NOT NULL,
	[country_iso_code] [char](2) NULL,
	[country_name] [varchar](150) NULL,
 CONSTRAINT [PK_CountryLocation] PRIMARY KEY CLUSTERED 
(
	[GeonameId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Language]    Script Date: 2016-03-15 08:29:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Language](
	[id] [int] NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[ISOCode] [char](2) NOT NULL,
 CONSTRAINT [PK_Language] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SecureChatMessage]    Script Date: 2016-03-15 08:29:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SecureChatMessage](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[Message] [varchar](8000) NOT NULL,
	[ReceiverUserId] [int] NOT NULL,
	[SenderUserId] [int] NOT NULL,
 CONSTRAINT [PK_SecureChatMessage] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SecureChatSettings]    Script Date: 2016-03-15 08:29:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SecureChatSettings](
	[KeyType] [varchar](50) NOT NULL,
	[Value] [varchar](8000) NOT NULL,
	[Description] [varchar](150) NULL,
	[DataType] [int] NOT NULL,
 CONSTRAINT [PK_SecureChatSettings] PRIMARY KEY CLUSTERED 
(
	[KeyType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SecureChatUser]    Script Date: 2016-03-15 08:29:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SecureChatUser](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nickname] [varchar](255) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[GUID] [char](36) NOT NULL,
	[Status] [int] NOT NULL,
 CONSTRAINT [PK_SecureChatUser] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[WordDictionary]    Script Date: 2016-03-15 08:29:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[WordDictionary](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Word] [varchar](100) NOT NULL,
	[LanguageId] [int] NOT NULL,
	[Type] [int] NOT NULL,
	[WordLength] [int] NOT NULL,
 CONSTRAINT [PK_WordDictionary] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[CountryBlock-IPv4] ADD  CONSTRAINT [DF_CountryBlock-IPv4_IsAnonymousProxy]  DEFAULT ((0)) FOR [IsAnonymousProxy]
GO
ALTER TABLE [dbo].[CountryBlock-IPv4] ADD  CONSTRAINT [DF_CountryBlock-IPv4_IsSatelliteProvider]  DEFAULT ((0)) FOR [IsSatelliteProvider]
GO
ALTER TABLE [dbo].[CountryBlock-IPv6] ADD  CONSTRAINT [DF_CountryBlock-IPv6_IsAnonymousProxy]  DEFAULT ((0)) FOR [IsAnonymousProxy]
GO
ALTER TABLE [dbo].[CountryBlock-IPv6] ADD  CONSTRAINT [DF_CountryBlock-IPv6_IsSatelliteProvider]  DEFAULT ((0)) FOR [IsSatelliteProvider]
GO
ALTER TABLE [dbo].[SecureChatSettings] ADD  CONSTRAINT [DF_SecureChatSettings_DataType]  DEFAULT ((0)) FOR [DataType]
GO
ALTER TABLE [dbo].[WordDictionary] ADD  CONSTRAINT [DF_WordDictionary_Type]  DEFAULT ((0)) FOR [Type]
GO
ALTER TABLE [dbo].[SecureChatMessage]  WITH CHECK ADD  CONSTRAINT [FK_SecureChatMessage_SecureChatUser_Receiver] FOREIGN KEY([ReceiverUserId])
REFERENCES [dbo].[SecureChatUser] ([Id])
GO
ALTER TABLE [dbo].[SecureChatMessage] CHECK CONSTRAINT [FK_SecureChatMessage_SecureChatUser_Receiver]
GO
ALTER TABLE [dbo].[SecureChatMessage]  WITH CHECK ADD  CONSTRAINT [FK_SecureChatMessage_SecureChatUser_Sender] FOREIGN KEY([SenderUserId])
REFERENCES [dbo].[SecureChatUser] ([Id])
GO
ALTER TABLE [dbo].[SecureChatMessage] CHECK CONSTRAINT [FK_SecureChatMessage_SecureChatUser_Sender]
GO
ALTER TABLE [dbo].[WordDictionary]  WITH CHECK ADD  CONSTRAINT [FK_WordDictionary_Language] FOREIGN KEY([LanguageId])
REFERENCES [dbo].[Language] ([id])
GO
ALTER TABLE [dbo].[WordDictionary] CHECK CONSTRAINT [FK_WordDictionary_Language]
GO
