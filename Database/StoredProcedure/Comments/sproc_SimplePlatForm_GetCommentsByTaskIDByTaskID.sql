SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

IF EXISTS (SELECT NULL FROM [SYS].[OBJECTS] WITH (NOLOCK) WHERE [OBJECT_ID] = OBJECT_ID(N'[dbo].[sproc_SimplePlatForm_GetCommentsByTaskID]') AND [type] IN (N'P', N'TF'))
DROP PROCEDURE [dbo].[sproc_SimplePlatForm_GetCommentsByTaskIDByTaskID];
GO

CREATE PROCEDURE [dbo].[sproc_SimplePlatForm_GetCommentsByTaskID]
(
	@TaskID		INT
)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @Status INT = 0;

	IF EXISTS(SELECT 1 FROM dbo.Comments WHERE CommentId = @ID AND IsDeleted = 0)
	BEGIN

		WITH [CommentList] AS
		(
			SELECT
				[CommentId] AS [CommentId],
				[CommentText] AS [CommentText],
				[Task_TaskId] AS [Task_TaskId],
				[UserDetail_UserId] AS [UserDetail_UserId],
				[CreateDate] AS [CreateDate]
			FROM dbo.Comments
			WHERE IsDeleted = 0 AND Task_TaskId = @TaskID
		), 
		[IsAttachment] AS
		(
			SELECT
				[II].[CommentId] AS [CommentId],
				[I].[CommentAttachmentId] AS [CommentAttachmentId]
			FROM [dbo].[CommentAttachments] AS [I]
			RIGHT JOIN CommentList AS [II] ON [I].Comment_CommentId = [II].CommentId
		)
		SELECT
			[I].[CommentId] AS [CommentId],
			[I].[CommentText] AS [CommentText],
			[I].[Task_TaskId] AS [Task_TaskId],
			[I].[UserDetail_UserId] AS [UserDetail_UserId],
			[I].[CreateDate] AS [CreateDate],
			ISNULL([II].[CommentAttachmentId], 0) AS [IsFileAttached]
		FROM  [dbo].[Comments] AS  [I]
		INNER JOIN [IsAttachment] AS [II] ON [II].[CommentId] = [I].[CommentId]
	END
END;
