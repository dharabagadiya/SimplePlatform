﻿SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

IF EXISTS (SELECT NULL FROM [SYS].[OBJECTS] WITH (NOLOCK) WHERE [OBJECT_ID] = OBJECT_ID(N'[dbo].[sproc_SimplePlatForm_GetFSMDetailByID]') AND [type] IN (N'P', N'TF'))
DROP PROCEDURE [dbo].[sproc_SimplePlatForm_GetFSMDetailByID];
GO

CREATE PROCEDURE [dbo].[sproc_SimplePlatForm_GetFSMDetailByID]
(
	@ID		INT
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		[Id] AS [Id],
		[Name] AS [Name],
		[EmailId] AS [EmailId],
		[PhoneNumber] AS [PhoneNumber]
	FROM dbo.[FSMDetails]
	WHERE IsDeleted = 0 AND [Id] = @ID;
END;
