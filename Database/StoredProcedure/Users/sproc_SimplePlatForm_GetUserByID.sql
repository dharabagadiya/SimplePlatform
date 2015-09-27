SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

IF EXISTS (SELECT NULL FROM [SYS].[OBJECTS] WITH (NOLOCK) WHERE [OBJECT_ID] = OBJECT_ID(N'[dbo].[sproc_SimplePlatForm_GetUserByID]') AND [type] IN (N'P', N'TF'))
DROP PROCEDURE [dbo].[sproc_SimplePlatForm_GetUserByID];
GO

CREATE PROCEDURE [dbo].[sproc_SimplePlatForm_GetUserByID]
(
	@ID		INT
)
AS
BEGIN
	SET NOCOUNT ON;
	
	WITH [UserList] AS
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
		WHERE I.IsDeleted = 0 AND I.UserId = @ID
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
	RIGHT JOIN [dbo].[UserDetails] AS [II] ON II.FileResource_Id = I.Id
	INNER JOIN UserList AS  [III] ON III.UserId = II.UserId;

END;
