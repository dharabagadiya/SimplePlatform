SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

IF EXISTS (SELECT NULL FROM [SYS].[OBJECTS] WITH (NOLOCK) WHERE [OBJECT_ID] = OBJECT_ID(N'[dbo].[sproc_SimplePlatForm_GetConventionAttachmentsByID]') AND [type] IN (N'P', N'TF'))
DROP PROCEDURE [dbo].[sproc_SimplePlatForm_GetConventionAttachmentsByID];
GO

CREATE PROCEDURE [dbo].[sproc_SimplePlatForm_GetConventionAttachmentsByID]
(
	@ConventionID	INT
)
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT
		[II].[ConventionAttachmentId] AS [ConventionAttachmentId],
		[II].[Convention_ConventionId] AS [Convention_ConventionId],
		[I].[Id] AS [FileResourceID],
		[I].[name] AS [FileResourceName],
		[I].[path] AS [FileResourcePath]
	FROM dbo.FileResources AS [I]
	INNER JOIN dbo.ConventionAttachments AS [II] ON II.FileResource_Id = I.Id
	INNER JOIN dbo.Conventions AS [III] ON III.IsDeleted = 0 AND III.ConventionId = @ConventionID AND III.ConventionId = II.Convention_ConventionId;
END;
