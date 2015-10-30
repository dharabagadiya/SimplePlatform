SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

IF EXISTS (SELECT NULL FROM [SYS].[OBJECTS] WITH (NOLOCK) WHERE [OBJECT_ID] = OBJECT_ID(N'[dbo].[sproc_SimplePlatForm_GSBTargetsAchivedByOfficeIDs]') AND [type] IN (N'P', N'TF'))
DROP PROCEDURE [dbo].[sproc_SimplePlatForm_GSBTargetsAchivedByOfficeIDs];
GO

CREATE PROCEDURE [dbo].[sproc_SimplePlatForm_GSBTargetsAchivedByOfficeIDs]
(
	@IDs		VARCHAR(MAX),
	@StartDate	DATETIME,
	@EndDate	DATETIME
)
AS
BEGIN
	SET NOCOUNT ON;
	
	SET DATEFIRST 5;

	WITH [OfficeIDs] AS (
		SELECT [Value] AS [ID] FROM dbo.func_SimplePlatForm_GetParamsToList(@IDs)
	),
	[GSBTargets] AS (
		SELECT
			[dbo].[func_SimplePlatForm_StartOfWeek]([I].[VisitDate]) AS [WeekStartDate],
			DATEPART(wk, [I].[VisitDate]) AS [WeekNumber],
			[I].GSBAmount
		FROM dbo.Audiences AS [I]
		INNER JOIN OfficeIDs AS [VI] ON [VI].[ID] = [I].[Office_OfficeId]
		WHERE [I].[IsDeleted] = 0 AND [I].[VisitDate] BETWEEN @StartDate AND @EndDate
	)
	SELECT
		GSBTargets.WeekStartDate AS [WeekStartDate],
		GSBTargets.WeekNumber AS [WeekNumber],
		SUM(GSBTargets.GSBAmount) AS [GSBAmount]
	FROM [GSBTargets]
	GROUP BY GSBTargets.WeekNumber, GSBTargets.WeekStartDate
	ORDER BY GSBTargets.WeekStartDate;

END;
