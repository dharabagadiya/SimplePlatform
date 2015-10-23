SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

IF EXISTS (SELECT NULL FROM [SYS].[OBJECTS] WITH (NOLOCK) WHERE [OBJECT_ID] = OBJECT_ID(N'[dbo].[sproc_SimplePlatForm_GetArrivalsByOfficeIDs]') AND [type] IN (N'P', N'TF'))
DROP PROCEDURE [dbo].[sproc_SimplePlatForm_GetArrivalsByOfficeIDs];
GO

CREATE PROCEDURE [dbo].[sproc_SimplePlatForm_GetArrivalsByOfficeIDs]
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
		[VIII].[Name] AS [FSMDetailName],
		[I].Name AS [PeopleName],
		[IV].[Name] AS [OfficeName],
		ISNULL([II].[Name],[VII].[ServiceName]) AS [ServiceName],
		[I].VisitDate AS [VisitDate],
		[I].IsAttended AS [ArrivalStatus],
		[I].GSBAmount AS [GSBAmount],
		[I].Amount AS [Amount]
	FROM dbo.Audiences AS [I]
	INNER JOIN OfficeIDs AS [VI] ON [VI].[ID] = [I].[Office_OfficeId]
	LEFT JOIN dbo.Conventions AS  [II] ON [II].[ConventionId] = [I].[Convention_ConventionId]
	LEFT JOIN dbo.Events AS  [III] ON [III].[EventId] = [I].[Event_EventId]
	LEFT JOIN dbo.Offices AS [IV] ON IV.OfficeId = I.Office_OfficeId
	LEFT JOIN dbo.VisitTypes AS [V] ON V.VisitTypeId = I.VisitType_VisitTypeId
	LEFT JOIN dbo.[Services] AS [VII] ON [VII].ServiceId = I.Sevice_ServiceId
	LEFT JOIN dbo.[FSMDetails] AS [VIII] ON [VIII].Id = I.[FSMDetail_UserId]
	WHERE [I].[IsDeleted] = 0 AND [I].[VisitType_VisitTypeId] IN (3, 4) AND [I].[VisitDate] BETWEEN @StartDate AND @EndDate;

END;
