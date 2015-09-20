SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

IF EXISTS (SELECT NULL FROM [SYS].[OBJECTS] WITH (NOLOCK) WHERE [OBJECT_ID] = OBJECT_ID(N'[dbo].[sproc_SimplePlatForm_AddComment]') AND [type] IN (N'P', N'TF'))
DROP PROCEDURE [dbo].[sproc_SimplePlatForm_AddComment];
GO

DROP TYPE [dbo].[Attachments];
CREATE TYPE [dbo].[Attachments] AS TABLE
(
	[Name]	VARCHAR(max),
	[Path]	VARCHAR(max)
);
GO

CREATE PROCEDURE [dbo].[sproc_SimplePlatForm_AddComment]
(
	@taksID				INT,
	@userID				INT,
	@message			VARCHAR(MAX),
	@Attachments		[dbo].[Attachments] READONLY
)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @Status INT = 0;

	IF EXISTS(SELECT 1 FROM dbo.Tasks WHERE TaskId = @taksID)
	BEGIN
		BEGIN TRY
			BEGIN TRANSACTION;

			DECLARE @commentID INT;

			INSERT dbo.Comments(CommentText, IsDeleted, CreateDate, UpdateDate, Task_TaskId, UserDetail_UserId)
			VALUES  (@message, 0, GETDATE(), GETDATE(), @taksID, @userID);

			SET @commentID = SCOPE_IDENTITY();

			IF EXISTS (SELECT 1 FROM @Attachments)
			BEGIN

				DECLARE @name VARCHAR(MAX), @path VARCHAR(MAX), @fileResourceID INT;
				
				DECLARE INSERT_CURSOR CURSOR FOR 
				SELECT [Name] AS [name], [Path] As [path] FROM @Attachments

				OPEN INSERT_CURSOR
				FETCH NEXT FROM INSERT_CURSOR INTO @name, @path

				While (@@FETCH_STATUS = 0)
				BEGIN

					INSERT INTO dbo.FileResources([name], [path]) VALUES(@name, @path);

					SET @fileResourceID = SCOPE_IDENTITY();

					INSERT INTO dbo.CommentAttachments(Comment_CommentId, FileResource_Id)
					VALUES( @commentID, @fileResourceID);

					FETCH NEXT FROM INSERT_CURSOR INTO @name, @path;
				END
			END;

			SET @Status = 1;

			COMMIT TRANSACTION;
		END TRY
		BEGIN CATCH
			ROLLBACK TRANSACTION;
		END CATCH;
	END
	RETURN @Status;
END;
