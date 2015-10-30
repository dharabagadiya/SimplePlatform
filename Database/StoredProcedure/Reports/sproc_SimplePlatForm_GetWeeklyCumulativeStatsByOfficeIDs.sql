SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

IF EXISTS (SELECT NULL FROM [SYS].[OBJECTS] WITH (NOLOCK) WHERE [OBJECT_ID] = OBJECT_ID(N'[dbo].[sproc_SimplePlatForm_GetWeeklyCumulativeStatsByOfficeIDs]') AND [type] IN (N'P', N'TF'))
DROP PROCEDURE [dbo].[sproc_SimplePlatForm_GetWeeklyCumulativeStatsByOfficeIDs];
GO

CREATE PROCEDURE [dbo].[sproc_SimplePlatForm_GetWeeklyCumulativeStatsByOfficeIDs]
(
	@IDs		VARCHAR(MAX),
	@StartDate	DATETIME,
	@EndDate	DATETIME
)
AS
BEGIN
	SET NOCOUNT ON;
	
	WITH [OfficeIDs] AS (
		SELECT [Value] AS [ID] FROM dbo.func_SimplePlatForm_GetParamsToList(@IDs)
	)
	SELECT
		CONVERT(DATE, @EndDate) AS [EndDate],
		[VIII].[Id] AS [FSMDetailID],
		[VIII].[Name] AS [FSMDetailName],
		[I].[AudienceID] AS [AudienceID],
		[I].Name AS [PeopleName],
		[IV].[Name] AS [OfficeName],
		ISNULL([II].[Name],[VII].[ServiceName]) AS [ServiceName],
		ISNULL([II].[StartDate], [I].[ArrivalDate])  AS [VisitDate],
		(CASE WHEN [I].[IsAttended] = 1 THEN 'Arrived' ELSE '' END) AS [ArrivalStatus],
		[I].GSBAmount AS [GSBAmount],
		[I].Amount AS [Amount],
		[I].[BookingStatus] AS [BookingStatus],
		[I].[IsAttended] AS [IsAttended]
	INTO [#AudienceList]
	FROM dbo.Audiences AS [I]
	INNER JOIN OfficeIDs AS [VI] ON [VI].[ID] = [I].[Office_OfficeId]
	LEFT JOIN dbo.Conventions AS  [II] ON [II].[ConventionId] = [I].[Convention_ConventionId]
	LEFT JOIN dbo.Events AS  [III] ON [III].[EventId] = [I].[Event_EventId]
	LEFT JOIN dbo.Offices AS [IV] ON IV.OfficeId = I.Office_OfficeId
	LEFT JOIN dbo.VisitTypes AS [V] ON V.VisitTypeId = I.VisitType_VisitTypeId
	LEFT JOIN dbo.[Services] AS [VII] ON [VII].ServiceId = I.Sevice_ServiceId
	LEFT JOIN dbo.[FSMDetails] AS [VIII] ON [VIII].Id = I.[FSMDetail_UserId]
	WHERE [I].[IsDeleted] = 0 AND [I].[VisitType_VisitTypeId] IN (3, 4) AND [I].[FSMDetail_UserId] IS NOT NULL AND ([I].[VisitDate] BETWEEN @StartDate AND @EndDate);

	WITH [AudienceList] AS
	(
		SELECT
			[FSMDetailID] AS [FSMDetailID]
		FROM [#AudienceList]
		GROUP BY [FSMDetailID]
	),
	[FundRaised] AS(
		SELECT
			[FSMDetailID] AS [FSMDetailID],
			SUM([Amount]) AS [FundRaised]
		FROM [#AudienceList]
		GROUP BY [FSMDetailID]
	),
	[GBSAmount] AS (
		SELECT
			[FSMDetailID] AS [FSMDetailID],
			SUM([GSBAmount]) AS [GSBAmount]
		FROM [#AudienceList]
		GROUP BY [FSMDetailID]
	),
	[ReachesCount] AS (
		SELECT
			[FSMDetailID] AS [FSMDetailID],
			COUNT([AudienceID]) AS [ReachesCount]
		FROM [#AudienceList]
		WHERE BookingStatus = 3
		GROUP BY [FSMDetailID]
	),
	[InProcessCount] AS (
		SELECT
			[FSMDetailID] AS [FSMDetailID],
			COUNT([AudienceID]) AS [InProcessCount]
		FROM [#AudienceList]
		WHERE BookingStatus = 1
		GROUP BY [FSMDetailID]
	),
	[BookedCount] AS (
		SELECT
			[FSMDetailID] AS [FSMDetailID],
			COUNT([AudienceID]) AS [BookedCount]
		FROM [#AudienceList]
		WHERE BookingStatus = 2
		GROUP BY [FSMDetailID]
	),
	[ArrivalCount] AS (
		SELECT
			[FSMDetailID] AS [FSMDetailID],
			COUNT([AudienceID]) AS [ArrivalCount]
		FROM [#AudienceList]
		WHERE IsAttended = 1
		GROUP BY [FSMDetailID]
	)
	SELECT	
		 CONVERT(DATE, @EndDate) AS [EndDate],
		 [I].[FSMDetailID] AS [FSMDetailID],
		 [I].[FSMDetailName] AS [FSMDetailName],
		 ISNULL([III].[FundRaised], 0) AS [FundRaised],
		 ISNULL([V].[GSBAmount], 0) AS [GSBAmount],
		 ISNULL([IV].[ReachesCount], 0) AS [ReachesCount],
		 ISNULL([IIV].[InProcessCount], 0) AS [InProcessCount],
		 ISNULL([VI].[BookedCount], 0) AS [BookedCount],
		 ISNULL([VII].[ArrivalCount], 0) AS [ArrivalCount]
	FROM [#AudienceList] AS [I]
	INNER JOIN [AudienceList] AS [II] ON [II].[FSMDetailID] = [I].[FSMDetailID]
	LEFT JOIN [FundRaised] AS [III] ON [III].[FSMDetailID] = [I].[FSMDetailID]
	LEFT JOIN [GBSAmount] AS [V] ON [V].[FSMDetailID] = [I].[FSMDetailID]
	LEFT JOIN [ReachesCount] AS [IV] ON [IV].[FSMDetailID] = [I].[FSMDetailID]
	LEFT JOIN [InProcessCount] AS [IIV] ON [IIV].[FSMDetailID] = [I].[FSMDetailID]
	LEFT JOIN [BookedCount] AS [VI] ON [VI].[FSMDetailID] = [I].[FSMDetailID]
	LEFT JOIN [ArrivalCount] AS [VII] ON [VII].[FSMDetailID] = [I].[FSMDetailID]
	GROUP BY [I].[FSMDetailID], [FSMDetailName], [III].[FundRaised], [V].[GSBAmount], [IV].[ReachesCount], [IIV].[InProcessCount], [VI].[BookedCount], [VII].[ArrivalCount];

	-- BOOKED
	SELECT
		CONVERT(DATE, @EndDate) AS [EndDate],
		[FSMDetailID] AS [FSMDetailID],
		[FSMDetailName] AS [FSMDetailName],
		[AudienceID] AS [AudienceID],
		[PeopleName] AS [PeopleName],
		[OfficeName] AS [OfficeName],
		[ServiceName] AS [ServiceName],
		[VisitDate] AS [VisitDate],
		[ArrivalStatus] AS [ArrivalStatus],
		[GSBAmount] AS [GSBAmount],
		[Amount] AS [Amount],
		[BookingStatus] AS [BookingStatus],
		[IsAttended] AS [IsAttended]
	FROM #AudienceList
	WHERE BookingStatus = 2;

	-- In Process
	SELECT
		CONVERT(DATE, @EndDate) AS [EndDate],
		[FSMDetailID] AS [FSMDetailID],
		[FSMDetailName] AS [FSMDetailName],
		[AudienceID] AS [AudienceID],
		[PeopleName] AS [PeopleName],
		[OfficeName] AS [OfficeName],
		[ServiceName] AS [ServiceName],
		[VisitDate] AS [VisitDate],
		[ArrivalStatus] AS [ArrivalStatus],
		[GSBAmount] AS [GSBAmount],
		[Amount] AS [Amount],
		[BookingStatus] AS [BookingStatus],
		[IsAttended] AS [IsAttended]
	FROM #AudienceList
	WHERE BookingStatus = 1;

	-- Reachies
	SELECT
		CONVERT(DATE, @EndDate) AS [EndDate],
		[FSMDetailID] AS [FSMDetailID],
		[FSMDetailName] AS [FSMDetailName],
		[AudienceID] AS [AudienceID],
		[PeopleName] AS [PeopleName],
		[OfficeName] AS [OfficeName],
		[ServiceName] AS [ServiceName],
		[VisitDate] AS [VisitDate],
		[ArrivalStatus] AS [ArrivalStatus],
		[GSBAmount] AS [GSBAmount],
		[Amount] AS [Amount],
		[BookingStatus] AS [BookingStatus],
		[IsAttended] AS [IsAttended]
	FROM #AudienceList
	WHERE BookingStatus = 3;

		-- Arrivals
	SELECT
		CONVERT(DATE, @EndDate) AS [EndDate],
		[FSMDetailID] AS [FSMDetailID],
		[FSMDetailName] AS [FSMDetailName],
		[AudienceID] AS [AudienceID],
		[PeopleName] AS [PeopleName],
		[OfficeName] AS [OfficeName],
		[ServiceName] AS [ServiceName],
		[VisitDate] AS [VisitDate],
		[ArrivalStatus] AS [ArrivalStatus],
		[GSBAmount] AS [GSBAmount],
		[Amount] AS [Amount],
		[BookingStatus] AS [BookingStatus],
		[IsAttended] AS [IsAttended]
	FROM #AudienceList
	WHERE IsAttended = 1;

END;
