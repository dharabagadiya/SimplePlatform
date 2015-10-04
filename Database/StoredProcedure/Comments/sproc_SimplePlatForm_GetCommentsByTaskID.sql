SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

IF EXISTS (SELECT NULL FROM [SYS].[OBJECTS] WITH (NOLOCK) WHERE [OBJECT_ID] = OBJECT_ID(N'[dbo].[sproc_SimplePlatForm_GetCommentsByTaskID]') AND [type] IN (N'P', N'TF'))
DROP PROCEDURE [dbo].[sproc_SimplePlatForm_GetCommentsByTaskID];
GO

CREATE PROCEDURE [dbo].[sproc_SimplePlatForm_GetCommentsByTaskID]
(
	@TaskID		INT
)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @Status INT = 0;

	WITH [CommentList] AS
	(
		SELECT
			[I].[CommentId] AS [CommentId],
			[I].[CommentText] AS [CommentText],
			[I].[Task_TaskId] AS [Task_TaskId],
			[I].[CreateDate] AS [CreateDate],
			[I].[UpdateDate] AS [UpdateDate],
			[II].[UserId] AS [UserId],
			[II].[FirstName] AS [FirstName],
			[II].[LastName] AS [LastName]
		FROM dbo.Comments AS [I]
		INNER JOIN dbo.Users AS [II] ON [II].[UserId] = [I].[UserDetail_UserId]
		WHERE [I].[IsDeleted] = 0 AND [I].[Task_TaskId] = @TaskID
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
		[I].[CreateDate] AS [CreateDate],
		[I].[UpdateDate] AS [UpdateDate],
		[I].[UserId] AS [UserId],
		[I].[FirstName] AS [FirstName],
		[I].[LastName] AS [LastName],
 		ISNULL([II].[CommentAttachmentId], 0) AS [IsFileAttached]
	FROM  [CommentList] AS  [I] 
	INNER JOIN [IsAttachment] AS [II] ON [II].[CommentId] = [I].[CommentId]

END;
