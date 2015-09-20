SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

IF EXISTS (SELECT NULL FROM [SYS].[OBJECTS] WITH (NOLOCK) WHERE [OBJECT_ID] = OBJECT_ID(N'[dbo].[sproc_SimplePlatForm_GetBookingTargetsByOfficeIDs]') AND [type] IN (N'P', N'TF'))
DROP PROCEDURE [dbo].[sproc_SimplePlatForm_GetBookingTargetsByOfficeIDs];
GO

CREATE PROCEDURE [dbo].[sproc_SimplePlatForm_GetBookingTargetsByOfficeIDs]
(
	@OfficeIDs	VARCHAR(MAX),
	@StartDate	DATETIME,
	@EndDate	DATETIME
)
AS
BEGIN
	SET NOCOUNT ON;

	SET DATEFIRST 4;

	WITH [OfficeIDs] AS
	(	
		SELECT [Value] AS [ID] FROM dbo.func_SimplePlatForm_GetParamsToList(@OfficeIDs)
	),
	[BookingTargets] AS (
		SELECT 
			[I].[Booking] AS [Booking],
			[I].[DueDate] AS [DueDate],
			[dbo].[func_SimplePlatForm_StartOfWeek]([I].[DueDate]) AS [WeekStartDate],
			DATEPART(wk, [I].[DueDate]) AS [WeekNumber],
			[I].[Office_OfficeId] AS [OfficeId],
			[II].[Name] AS [OfficeName]
		FROM [dbo].[Targets] AS [I]
		INNER JOIN [dbo].[Offices] AS [II] ON [II].[IsDeleted] = 0 AND [II].[OfficeId] = [I].[Office_OfficeId]
		INNER JOIN [OfficeIDs] AS [III] ON [III].[ID] = [II].[OfficeId]
		WHERE [I].[IsDeleted] = 0 AND [I].DueDate BETWEEN @StartDate AND @EndDate
		GROUP BY [III].[ID], [I].[DueDate], [I].[Booking], [II].[Name], [I].[Office_OfficeId]
	)
	SELECT 
		[BookingTargets].[WeekNumber] AS [WeekNumber],
		[BookingTargets].[WeekStartDate] AS [WeekStartDate],
		SUM([BookingTargets].[Booking]) AS [Booking]
	FROM [BookingTargets]
	GROUP BY [BookingTargets].[WeekNumber], [BookingTargets].[WeekStartDate];

END;
