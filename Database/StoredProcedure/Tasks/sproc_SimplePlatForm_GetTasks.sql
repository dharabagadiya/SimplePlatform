SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

IF EXISTS (SELECT NULL FROM [SYS].[OBJECTS] WITH (NOLOCK) WHERE [OBJECT_ID] = OBJECT_ID(N'[dbo].[sproc_SimplePlatForm_GetTasks]') AND [type] IN (N'P', N'TF'))
DROP PROCEDURE [dbo].[sproc_SimplePlatForm_GetTasks];
GO

CREATE PROCEDURE [dbo].[sproc_SimplePlatForm_GetTasks]
AS
BEGIN
	SET NOCOUNT ON

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
		[III].[LastName] AS [LastName]
	FROM dbo.Tasks AS [I]
	LEFT JOIN dbo.Offices AS [II] ON [II].[IsDeleted] = 0 AND [II].[OfficeId] = [I].[Office_OfficeId]
	LEFT JOIN dbo.Users AS [III] ON [III].[IsDeleted] = 0 AND [III].[UserId] = [I].[UsersDetail_UserId]
	WHERE [I].[IsDeleted] = 0

END;
