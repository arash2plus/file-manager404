USE [Sedna_FileStream]
GO
/****** Object:  StoredProcedure [dbo].[Stp_InsertFile]    Script Date: 10/27/2020 6:59:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER Procedure [dbo].[Stp_InsertFile]
@insertedGuid uniqueidentifier out,
@P_FileName NVARCHAR(MAX),
@P_Extension varchar(10),
@P_PrivacyType bit,
@P_Content VARBINARY(MAX),
@P_CreateDate datetime 
AS
begin
INSERT INTO Dbo.tbl_Files(ContentGuid ,FileName,Extension,Content,PrivacyType, CreateDate)
VALUES (@insertedGuid,@P_FileName,@P_Extension,@P_Content,@P_PrivacyType, @P_CreateDate);
end