SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

IF EXISTS (SELECT NULL FROM [SYS].[OBJECTS] WITH (NOLOCK) WHERE [OBJECT_ID] = OBJECT_ID(N'[dbo].[sproc_SimplePlatForm_GetCommentAttachmentByID]') AND [type] IN (N'P', N'TF'))
DROP PROCEDURE [dbo].[sproc_SimplePlatForm_GetCommentAttachmentByID];
GO

CREATE PROCEDURE [dbo].[sproc_SimplePlatForm_GetCommentAttachmentByID]
(
	@ID				INT
)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @Status INT = 0;

	IF EXISTS(SELECT 1 FROM dbo.Comments WHERE CommentId = @ID AND IsDeleted = 0)
	BEGIN
		SELECT
			[II].[Comment_CommentId] AS [CommentId],
			[I].[Id] AS [FileResourceID],
			[I].[name] AS [Name],
			[I].[path] AS [Path]
		FROM dbo.FileResources AS [I]
		INNER JOIN dbo.CommentAttachments AS [II] ON II.Comment_CommentId = @ID AND II.FileResource_Id = [I].Id
	END
END;
