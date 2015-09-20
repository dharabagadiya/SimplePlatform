SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

IF EXISTS (SELECT NULL FROM [SYS].[OBJECTS] WITH (NOLOCK) WHERE [OBJECT_ID] = OBJECT_ID(N'[dbo].[sproc_SimplePlatForm_GetVisitTypes]') AND [type] IN (N'P', N'TF'))
DROP PROCEDURE [dbo].[sproc_SimplePlatForm_GetVisitTypes];
GO

CREATE PROCEDURE [dbo].[sproc_SimplePlatForm_GetVisitTypes]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		[VisitTypeId] AS [VisitTypeId],
		[VisitTypeName] AS [VisitTypeName]
	FROM dbo.VisitTypes
END;
