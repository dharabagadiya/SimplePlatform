SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

IF EXISTS (SELECT NULL FROM [SYS].[OBJECTS] WITH (NOLOCK) WHERE [OBJECT_ID] = OBJECT_ID(N'[dbo].[sproc_SimplePlatForm_GetTasksByUserID]') AND [type] IN (N'P', N'TF'))
DROP PROCEDURE [dbo].[sproc_SimplePlatForm_GetTasksByUserID];
GO

CREATE PROCEDURE [dbo].[sproc_SimplePlatForm_GetTasksByUserID]
(
	@StartDate	DATETIME,
	@EndDate	DATETIME,
	@UserID		INT
)
AS
BEGIN
	SET NOCOUNT ON;

	WITH [OfficesID] AS (
		SELECT DISTINCT
			[OfficeId] AS [OfficeId]
		FROM dbo.UserOffices
		WHERE (UserId = @UserID OR @UserID = 0)
	)	
	SELECT
		[I].[TaskId] AS [TaskId],
		[I].[Name] AS [Name],
		[I].[StartDate] AS [StartDate],
		[I].[EndDate] AS [EndDate],
		[I].[Description] AS [Description],
		[I].[CreateDate] AS [CreateDate],
		[I].[UpdateDate] AS [UpdateDate],
		[I].[IsCompleted] AS [IsCompleted],
		[II].[OfficeId] AS [OfficeId],
		[II].[Name] AS [OfficeName],
		[III].[UserId] AS [UserId],
		[III].[FirstName] AS [FirstName],
		[III].[LastName] AS [LastName],
		[VI].[Id] AS [FileResourceID],
		[VI].[name] AS [FileResourceName],
		[VI].[path] AS [FileResourcePath]
	FROM dbo.Tasks AS [I]
	INNER JOIN [OfficesID] AS [IV] ON [IV].[OfficeId] = [I].[Office_OfficeId]
	LEFT JOIN dbo.Offices AS [II] ON [II].[IsDeleted] = 0 AND [II].[OfficeId] = [I].[Office_OfficeId]
	LEFT JOIN dbo.Users AS [III] ON [III].[IsDeleted] = 0 AND [III].[UserId] = [I].[UsersDetail_UserId]
	LEFT JOIN dbo.UserDetails AS [V] ON V.UserId = I.UsersDetail_UserId
	LEFT JOIN dbo.FileResources AS [VI] ON VI.Id = II.FileResource_Id OR VI.Id = V.FileResource_Id 
	WHERE [I].[IsDeleted] = 0 AND [I].[StartDate] BETWEEN @StartDate AND @EndDate;

END;
