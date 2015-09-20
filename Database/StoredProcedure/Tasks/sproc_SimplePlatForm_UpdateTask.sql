SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

IF EXISTS (SELECT NULL FROM [SYS].[OBJECTS] WITH (NOLOCK) WHERE [OBJECT_ID] = OBJECT_ID(N'[dbo].[sproc_SimplePlatForm_UpdateTask]') AND [type] IN (N'P', N'TF'))
DROP PROCEDURE [dbo].[sproc_SimplePlatForm_UpdateTask];
GO

CREATE PROCEDURE [dbo].[sproc_SimplePlatForm_UpdateTask]
(
	@Status			INT OUTPUT,
	@ID				INT,
	@Name			VARCHAR(MAX),
	@StartDate		DATETIME,
	@EndDate		DATETIME,
	@Description	VARCHAR(MAX),
	@UserId			INT = NULL,
	@OfficeId		INT = NULL
)
AS
BEGIN
	SET NOCOUNT ON;

	SET @Status = 0;

	BEGIN TRY
		BEGIN TRANSACTION;

			IF(@UserId = 0) SET @UserId = NULL;
			IF(@OfficeId = 0) SET @OfficeId = NULL;

			IF EXISTS(SELECT 1 FROM [dbo].[Tasks] WHERE IsDeleted = 0 AND TaskId = @ID)
			BEGIN
				UPDATE dbo.Tasks SET
					Name = @Name, 
					StartDate = @StartDate, 
					EndDate = @EndDate, 
					Description = @Description,
					UpdateDate = GETDATE(), 
					UsersDetail_UserId = @UserId, 
					Office_OfficeId = @OfficeId
				WHERE TaskId = @ID;

				SET @Status = 1;
			END;

		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION;
	END CATCH;
END;
