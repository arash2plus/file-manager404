--EXEC sp_configure filestream_access_level, 2  
--RECONFIGURE  

USE [master]
GO
/****** Object:  Database [Sedna_FileStream]    Script Date: 9/15/2020 8:15:48 AM ******/
CREATE DATABASE [Sedna_FileStream]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Sedna_FileStream', FILENAME = N'D:\Database\Sedna_FileStream.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB ), 
 FILEGROUP [FileStream_01] CONTAINS FILESTREAM  DEFAULT
( NAME = N'FileStream_01', FILENAME = N'D:\Database\fileStream\FileStream_01' , MAXSIZE = UNLIMITED)
 LOG ON 
( NAME = N'Sedna_FileStream_log', FILENAME = N'D:\database\Sedna_FileStream_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Sedna_FileStream].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

EXEC sys.sp_db_vardecimal_storage_format N'Sedna_FileStream', N'ON'
GO
ALTER DATABASE [Sedna_FileStream] SET QUERY_STORE = OFF
GO


USE Sedna_FileStream
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
USE [master]
GO
ALTER DATABASE [Sedna_FileStream] SET  READ_WRITE 
GO
