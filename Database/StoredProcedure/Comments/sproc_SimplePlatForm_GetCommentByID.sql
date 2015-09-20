SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

IF EXISTS (SELECT NULL FROM [SYS].[OBJECTS] WITH (NOLOCK) WHERE [OBJECT_ID] = OBJECT_ID(N'[dbo].[sproc_SimplePlatForm_GetCommentByID]') AND [type] IN (N'P', N'TF'))
DROP PROCEDURE [dbo].[sproc_SimplePlatForm_GetCommentByID];
GO

CREATE PROCEDURE [dbo].[sproc_SimplePlatForm_GetCommentByID]
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
			[CommentId] AS [CommentId],
			[CommentText] AS [CommentText],
			[Task_TaskId] AS [Task_TaskId],
			[UserDetail_UserId] AS [UserDetail_UserId]
		FROM dbo.Comments
		WHERE CommentId = @ID
	END
END;
