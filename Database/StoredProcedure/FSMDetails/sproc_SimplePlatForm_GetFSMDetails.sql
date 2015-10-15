SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

IF EXISTS (SELECT NULL FROM [SYS].[OBJECTS] WITH (NOLOCK) WHERE [OBJECT_ID] = OBJECT_ID(N'[dbo].[sproc_SimplePlatForm_GetFSMDetails]') AND [type] IN (N'P', N'TF'))
DROP PROCEDURE [dbo].[sproc_SimplePlatForm_GetFSMDetails];
GO

CREATE PROCEDURE [dbo].[sproc_SimplePlatForm_GetFSMDetails]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		[Id] AS [Id],
		[Name] AS [Name],
		[PhoneNumber] AS [PhoneNumber]
	FROM dbo.[FSMDetails]
	WHERE IsDeleted = 0;
END;
	