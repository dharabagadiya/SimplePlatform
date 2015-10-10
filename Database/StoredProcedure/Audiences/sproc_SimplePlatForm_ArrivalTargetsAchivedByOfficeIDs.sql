SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

IF EXISTS (SELECT NULL FROM [SYS].[OBJECTS] WITH (NOLOCK) WHERE [OBJECT_ID] = OBJECT_ID(N'[dbo].[sproc_SimplePlatForm_ArrivalTargetsAchivedByOfficeIDs]') AND [type] IN (N'P', N'TF'))
DROP PROCEDURE [dbo].[sproc_SimplePlatForm_ArrivalTargetsAchivedByOfficeIDs];
GO

CREATE PROCEDURE [dbo].[sproc_SimplePlatForm_ArrivalTargetsAchivedByOfficeIDs]
(
	@IDs		VARCHAR(MAX),
	@StartDate	DATETIME,
	@EndDate	DATETIME
)
AS
BEGIN
	SET NOCOUNT ON;
	
	SET DATEFIRST 4;

	WITH [OfficeIDs] AS (
		SELECT [Value] AS [ID] FROM dbo.func_SimplePlatForm_GetParamsToList(@IDs)
	),
	[ArrivalTargets] AS (
		SELECT
			[dbo].[func_SimplePlatForm_StartOfWeek]([I].[VisitDate]) AS [WeekStartDate],
			DATEPART(wk, [I].[VisitDate]) AS [WeekNumber]
		FROM dbo.Audiences AS [I]
		INNER JOIN OfficeIDs AS [VI] ON [VI].[ID] = [I].[Office_OfficeId]
		-- Need to Add Convetion NULL
		WHERE [I].[IsDeleted] = 0 AND [I].[VisitDate] BETWEEN @StartDate AND @EndDate AND I.BookingStatus = 1 AND [I].[IsAttended] = 1
	)
	SELECT
		ArrivalTargets.WeekStartDate AS [WeekStartDate],
		ArrivalTargets.WeekNumber AS [WeekNumber],
		COUNT(ArrivalTargets.WeekNumber) AS [Arrival]
	FROM [ArrivalTargets]
	GROUP BY ArrivalTargets.WeekNumber, ArrivalTargets.WeekStartDate
	ORDER BY ArrivalTargets.WeekStartDate;

END;
