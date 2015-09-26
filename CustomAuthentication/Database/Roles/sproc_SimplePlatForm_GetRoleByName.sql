SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

IF EXISTS (SELECT NULL FROM [SYS].[OBJECTS] WITH (NOLOCK) WHERE [OBJECT_ID] = OBJECT_ID(N'[dbo].[sproc_SimplePlatForm_GetRoleByName]') AND [type] IN (N'P', N'TF'))
DROP PROCEDURE [dbo].[sproc_SimplePlatForm_GetRoleByName];
GO

CREATE PROCEDURE [dbo].[sproc_SimplePlatForm_GetRoleByName]
(
	@roleName	VARCHAR(MAX)
)
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT
		RoleId, 
		RoleName
	FROM [dbo].[Roles]
	WHERE IsDeleted = 0 AND RoleName = @roleName;

END;
