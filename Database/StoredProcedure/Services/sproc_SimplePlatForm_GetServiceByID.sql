SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

IF EXISTS (SELECT NULL FROM [SYS].[OBJECTS] WITH (NOLOCK) WHERE [OBJECT_ID] = OBJECT_ID(N'[dbo].[sproc_SimplePlatForm_GetServiceByID]') AND [type] IN (N'P', N'TF'))
DROP PROCEDURE [dbo].[sproc_SimplePlatForm_GetServiceByID];
GO

CREATE PROCEDURE [dbo].[sproc_SimplePlatForm_GetServiceByID]
(
	@ID		INT
)
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT
		[ServiceId] AS [ServiceId],
		[ServiceName] AS [ServiceName]
	FROM [Services]
	WHERE IsDeleted  = 0 AND [ServiceId] = @ID;
END;
