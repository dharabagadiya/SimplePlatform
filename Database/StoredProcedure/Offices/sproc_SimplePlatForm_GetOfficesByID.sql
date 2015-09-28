SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

IF EXISTS (SELECT NULL FROM [SYS].[OBJECTS] WITH (NOLOCK) WHERE [OBJECT_ID] = OBJECT_ID(N'[dbo].[sproc_SimplePlatForm_GetOfficesByID]') AND [type] IN (N'P', N'TF'))
DROP PROCEDURE [dbo].[sproc_SimplePlatForm_GetOfficesByID];
GO

CREATE PROCEDURE [dbo].[sproc_SimplePlatForm_GetOfficesByID]
(
	@ID			INT
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		[I].[OfficeId], 
		[I].[Name],
		[I].[ContactNo],
		[I].[City],
		[II].[UserId],
		[III].[Id],
		[III].[name],
		[III].[path]
	FROM [dbo].[Offices] AS [I]
	INNER JOIN dbo.UserOffices AS [II] ON II.OfficeId = I.OfficeId
	INNER JOIN dbo.UserRoles AS  [IV] ON IV.RoleId = 2 AND IV.UserId = II.UserId
	RIGHT JOIN dbo.FileResources AS [III] ON III.Id = I.FileResource_Id
	WHERE I.IsDeleted = 0 AND I.OfficeId = @ID;
END;
