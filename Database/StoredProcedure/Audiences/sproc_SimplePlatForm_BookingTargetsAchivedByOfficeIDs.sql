SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

IF EXISTS (SELECT NULL FROM [SYS].[OBJECTS] WITH (NOLOCK) WHERE [OBJECT_ID] = OBJECT_ID(N'[dbo].[sproc_SimplePlatForm_BookingTargetsAchivedByOfficeIDs]') AND [type] IN (N'P', N'TF'))
DROP PROCEDURE [dbo].[sproc_SimplePlatForm_BookingTargetsAchivedByOfficeIDs];
GO

CREATE PROCEDURE [dbo].[sproc_SimplePlatForm_BookingTargetsAchivedByOfficeIDs]
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
	[BookingTargets] AS (
		SELECT
			[dbo].[func_SimplePlatForm_StartOfWeek]([I].[VisitDate]) AS [WeekStartDate],
			DATEPART(wk, [I].[VisitDate]) AS [WeekNumber]
		FROM dbo.Audiences AS [I]
		-- Need to Add Convetion NULL
		INNER JOIN OfficeIDs AS [VI] ON [VI].[ID] = [I].[Office_OfficeId]
		WHERE [I].[IsDeleted] = 0 AND [I].[VisitDate] BETWEEN @StartDate AND @EndDate AND I.IsBooked = 1
	)
	SELECT
		BookingTargets.WeekStartDate AS [WeekStartDate],
		BookingTargets.WeekNumber AS [WeekNumber],
		COUNT(BookingTargets.WeekNumber) AS [Booking]
	FROM [BookingTargets]
	GROUP BY BookingTargets.WeekNumber, BookingTargets.WeekStartDate
	ORDER BY BookingTargets.WeekStartDate;

END;
