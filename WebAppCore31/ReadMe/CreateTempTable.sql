USE [Sedna_FileStream]
GO

/****** Object:  Table [dbo].[tbl_Files]    Script Date: 10/27/2020 7:15:00 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tbl_Files](
	[FileName] [nvarchar](50) NOT NULL,
	[ContentGuid] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[Extension] [varchar](10) NULL,
	[PrivacyType] [bit] NOT NULL,
	[Content] [varbinary](max) FILESTREAM  NULL,
	[CreateDate] [datetime] NULL,
UNIQUE NONCLUSTERED 
(
	[ContentGuid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] FILESTREAM_ON [FileStream_01]
GO

ALTER TABLE [dbo].[tbl_Files] ADD  DEFAULT (newsequentialid()) FOR [ContentGuid]
GO


