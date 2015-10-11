SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

IF EXISTS (SELECT NULL FROM [SYS].[OBJECTS] WITH (NOLOCK) WHERE [OBJECT_ID] = OBJECT_ID(N'[dbo].[sproc_SimplePlatForm_GetServices]') AND [type] IN (N'P', N'TF'))
DROP PROCEDURE [dbo].[sproc_SimplePlatForm_GetServices];
GO

CREATE PROCEDURE [dbo].[sproc_SimplePlatForm_GetServices]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		[ServiceId] AS [ServiceId],
		[ServiceName] AS [ServiceName]
	FROM dbo.[Services]
	WHERE IsDeleted = 0;
END;
