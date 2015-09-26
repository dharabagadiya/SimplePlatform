SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

IF EXISTS (SELECT NULL FROM [SYS].[OBJECTS] WITH (NOLOCK) WHERE [OBJECT_ID] = OBJECT_ID(N'[dbo].[sproc_SimplePlatForm_GetUsers]') AND [type] IN (N'P', N'TF'))
DROP PROCEDURE [dbo].[sproc_SimplePlatForm_GetUsers];
GO

CREATE PROCEDURE [dbo].[sproc_SimplePlatForm_GetUsers]
(
	@UserID		INT
)
AS
BEGIN
	SET NOCOUNT ON;
	
	WITH [OfficeIDs] AS
	(
		SELECT
			[I].[OfficeId]
		FROM dbo.UserOffices AS [I]
		INNER JOIN dbo.Offices AS [II] ON [II].[IsDeleted] = 0 AND [II].[OfficeId] = [I].[OfficeId]
		WHERE I.UserId = @UserID OR @UserID <> 0
		GROUP BY [I].[OfficeId]
	),
	[OfficeUserMapping] AS
	(
		SELECT
			[I].[UserId]
		FROM dbo.UserOffices AS [I]
		INNER JOIN [OfficeIDs] AS [II] ON II.OfficeId = I.OfficeId
		GROUP BY [I].[UserId]
	),
	[UserList] AS
	(
		SELECT 
			[I].UserId, 
			[I].UserName, 
			[I].Email, 
			[I].Password, 
			[I].FirstName, 
			[I].LastName,
			[II].[RoleId],
			[III].[RoleName]
		FROM dbo.Users AS [I]
		INNER JOIN dbo.UserRoles AS [II] ON II.UserId = I.UserId
		INNER JOIN dbo.Roles AS [III] ON [III].[IsDeleted] = 0 AND [III].[RoleId] = [II].[RoleId]
		INNER JOIN OfficeUserMapping AS [IV] ON [IV].[UserId] = [II].[UserId]
		WHERE I.IsDeleted = 0
	)
	SELECT
			[III].UserId, 
			[III].UserName, 
			[III].Email, 
			[III].Password, 
			[III].FirstName, 
			[III].LastName,
			[III].[RoleId],
			[III].[RoleName],
			[I].[Id] AS [FileResourceID],
			[I].[name] AS [FileResourceName],
			[I].[path] AS [FileResourcePath]
	FROM [dbo].[FileResources] AS [I]
	INNER JOIN [dbo].[UserDetails] AS [II] ON II.FileResource_Id = I.Id
	INNER JOIN UserList AS  [III] ON III.UserId = II.UserId;

END;
