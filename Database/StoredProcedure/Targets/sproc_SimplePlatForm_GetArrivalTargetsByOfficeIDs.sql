SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

IF EXISTS (SELECT NULL FROM [SYS].[OBJECTS] WITH (NOLOCK) WHERE [OBJECT_ID] = OBJECT_ID(N'[dbo].[sproc_SimplePlatForm_GetArrivalTargetsByOfficeIDs]') AND [type] IN (N'P', N'TF'))
DROP PROCEDURE [dbo].[sproc_SimplePlatForm_GetArrivalTargetsByOfficeIDs];
GO

CREATE PROCEDURE [dbo].[sproc_SimplePlatForm_GetArrivalTargetsByOfficeIDs]
(
	@OfficeIDs	VARCHAR(MAX),
	@StartDate	DATETIME,
	@EndDate	DATETIME
)
AS
BEGIN
	SET NOCOUNT ON;

	SET DATEFIRST 5;

	WITH [OfficeIDs] AS
	(	
		SELECT [Value] AS [ID] FROM dbo.func_SimplePlatForm_GetParamsToList(@OfficeIDs)
	),
	[ArrivalTargets] AS (
		SELECT 
			[I].[Arrivals] AS [Arrivals],
			[I].[DueDate] AS [DueDate],
			[dbo].[func_SimplePlatForm_StartOfWeek]([I].[DueDate]) AS [WeekStartDate],
			DATEPART(wk, [I].[DueDate]) AS [WeekNumber],
			[I].[Office_OfficeId] AS [OfficeId],
			[II].[Name] AS [OfficeName]
		FROM [dbo].[Targets] AS [I]
		INNER JOIN [dbo].[Offices] AS [II] ON [II].[IsDeleted] = 0 AND [II].[OfficeId] = [I].[Office_OfficeId]
		INNER JOIN [OfficeIDs] AS [III] ON [III].[ID] = [II].[OfficeId]
		WHERE [I].[IsDeleted] = 0 AND [I].DueDate BETWEEN @StartDate AND @EndDate
		GROUP BY [III].[ID], [I].[DueDate], [I].[Arrivals], [II].[Name], [I].[Office_OfficeId]
	)
	SELECT 
		[ArrivalTargets].[WeekNumber] AS [WeekNumber],
		[ArrivalTargets].[WeekStartDate] AS [WeekStartDate],
		SUM([ArrivalTargets].[Arrivals]) AS [Arrivals]
	FROM [ArrivalTargets]
	GROUP BY [ArrivalTargets].[WeekNumber], [ArrivalTargets].[WeekStartDate];

END;
