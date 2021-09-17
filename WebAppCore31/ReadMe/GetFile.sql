USE [Sedna_FileStream]
GO
/****** Object:  StoredProcedure [dbo].[Stp_GetFile]    Script Date: 10/27/2020 7:15:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[Stp_GetFile]
	@P_ContentGuid uniqueidentifier
AS
BEGIN 


SELECT * from dbo.tbl_Files where ContentGuid = @P_ContentGuid;


END;