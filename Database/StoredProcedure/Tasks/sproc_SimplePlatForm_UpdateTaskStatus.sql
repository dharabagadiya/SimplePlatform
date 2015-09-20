SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

IF EXISTS (SELECT NULL FROM [SYS].[OBJECTS] WITH (NOLOCK) WHERE [OBJECT_ID] = OBJECT_ID(N'[dbo].[sproc_SimplePlatForm_UpdateTaskStatus]') AND [type] IN (N'P', N'TF'))
DROP PROCEDURE [dbo].[sproc_SimplePlatForm_UpdateTaskStatus];
GO

CREATE PROCEDURE [dbo].[sproc_SimplePlatForm_UpdateTaskStatus]
(
	@Status			INT OUTPUT,
	@ID				INT
)
AS
BEGIN
	SET NOCOUNT ON;

	SET @Status = 0;

	BEGIN TRY
		BEGIN TRANSACTION;

			IF EXISTS(SELECT 1 FROM [dbo].[Tasks] WHERE IsDeleted = 0 AND TaskId = @ID)
			BEGIN
				
				DECLARE @CurrentStatus INT;

				SELECT @CurrentStatus = IsCompleted FROM dbo.Tasks WHERE IsDeleted = 0 AND TaskId = @ID

				IF(@CurrentStatus = 0) SET @CurrentStatus = 1;
				ELSE SET @CurrentStatus = 0;

				UPDATE dbo.Tasks SET
					IsCompleted = @CurrentStatus
				WHERE TaskId = @ID;

				SET @Status = 1;
			END;

		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION;
	END CATCH;
END;
