SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

IF EXISTS (SELECT NULL FROM [SYS].[OBJECTS] WITH (NOLOCK) WHERE [OBJECT_ID] = OBJECT_ID(N'[dbo].[sproc_SimplePlatForm_GetActiveEventByOfficeIDs]') AND [type] IN (N'P', N'TF'))
DROP PROCEDURE [dbo].[sproc_SimplePlatForm_GetActiveEventByOfficeIDs];
GO

CREATE PROCEDURE [dbo].[sproc_SimplePlatForm_GetActiveEventByOfficeIDs]
(
	@officeIDs		VARCHAR(MAX)
)
AS
BEGIN
	SET NOCOUNT ON;

	WITH [OfficeList] AS (

		SELECT 
			[II].[OfficeId],
			[II].[Name],
			[I].[Id],
			[I].[path]
		FROM dbo.FileResources AS [I]
		RIGHT JOIN dbo.Offices AS [II] ON [II].[FileResource_Id] = [I].[Id]
		INNER JOIN dbo.func_SimplePlatForm_GetParamsToList(@officeIDs) AS [III] ON [III].[Value] = [II].[OfficeId]
	),
	[EventListWithCount] AS(
		SELECT
			[I].[EventId],
			COUNT([I].[EventId]) AS [TotalAttended]
		FROM dbo.Audiences AS [III]
		INNER JOIN [dbo].[Events] AS [I] ON I.IsDeleted = 0 AND CONVERT(DATE,I.StartDate) >= CONVERT(DATE,GETDATE()) AND [I].[EventId] = [III].[Event_EventId]
		INNER JOIN OfficeList AS [II] ON II.OfficeId = I.Office_OfficeId
		GROUP BY [I].[EventId]
	)
	SELECT
		[I].[EventId],
		[I].Name,
		[I].StartDate,
		[I].[EndDate],
		[I].[Description],
		[I].[City],
		[II].[OfficeId],
		[II].[Name] AS [OfficeName],
		[II].[Id] AS [FileResourceID],
		[II].[path] AS [FileResourcePath],
		[III].[ConventionId],
		[III].[Name] AS [ConventionName],
		ISNULL([IV].[TotalAttended], 0) AS [TotalAttended]
	FROM  dbo.Conventions AS [III]
	INNER JOIN [dbo].[Events] AS [I] ON I.IsDeleted = 0 AND CONVERT(DATE,I.StartDate) >= CONVERT(DATE,GETDATE()) AND [I].[convention_ConventionId] = [III].[ConventionId]
	INNER JOIN OfficeList AS [II] ON II.OfficeId = I.Office_OfficeId
	LEFT JOIN EventListWithCount  AS [IV] ON [IV].[EventId] = [I].[EventId];

END;
