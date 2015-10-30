SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

IF EXISTS (SELECT NULL FROM [SYS].[OBJECTS] WITH (NOLOCK) WHERE [OBJECT_ID] = OBJECT_ID(N'[dbo].[sproc_SimplePlatForm_FundingTargetsAchivedByOfficeIDs]') AND [type] IN (N'P', N'TF'))
DROP PROCEDURE [dbo].[sproc_SimplePlatForm_FundingTargetsAchivedByOfficeIDs];
GO

CREATE PROCEDURE [dbo].[sproc_SimplePlatForm_FundingTargetsAchivedByOfficeIDs]
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
	[FundTargets] AS (
		SELECT
			[dbo].[func_SimplePlatForm_StartOfWeek]([I].[VisitDate]) AS [WeekStartDate],
			DATEPART(wk, [I].[VisitDate]) AS [WeekNumber],
			[I].Amount
		FROM dbo.Audiences AS [I]
		INNER JOIN OfficeIDs AS [VI] ON [VI].[ID] = [I].[Office_OfficeId]
		WHERE [I].[IsDeleted] = 0 AND [I].[VisitDate] BETWEEN @StartDate AND @EndDate
	)
	SELECT
		FundTargets.WeekStartDate AS [WeekStartDate],
		FundTargets.WeekNumber AS [WeekNumber],
		SUM(FundTargets.Amount) AS [FundRaised]
	FROM [FundTargets]
	GROUP BY FundTargets.WeekNumber, FundTargets.WeekStartDate
	ORDER BY FundTargets.WeekStartDate;

END;
