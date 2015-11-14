SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

IF EXISTS (SELECT NULL FROM [SYS].[OBJECTS] WITH (NOLOCK) WHERE [OBJECT_ID] = OBJECT_ID(N'[dbo].[sproc_SimplePlatForm_GetFSMSelectionByCurrentWeek]') AND [type] IN (N'P', N'TF'))
DROP PROCEDURE [dbo].[sproc_SimplePlatForm_GetFSMSelectionByCurrentWeek];
GO

CREATE PROCEDURE [dbo].[sproc_SimplePlatForm_GetFSMSelectionByCurrentWeek]
AS
BEGIN
	SET NOCOUNT ON;

	SET DATEFIRST 5;
	DECLARE @StartDate AS DATETIME, @EndDate AS DATETIME;
	SELECT  @StartDate = DATEADD(DAY, 1 - DATEPART(WEEKDAY, GETDATE()), CAST(GETDATE() AS DATE)), @EndDate = DATEADD(DAY, 7 - DATEPART(WEEKDAY, GETDATE()), CAST(GETDATE() AS DATE));
	
	SELECT
		[I].[FSMDetail_UserId] AS [FSMDetailID]
	FROM dbo.Audiences AS [I]
	WHERE [I].[IsDeleted] = 0 AND [I].[FSMDetail_UserId] IS NOT NULL AND ([I].[VisitDate] BETWEEN @StartDate AND @EndDate);

END;
