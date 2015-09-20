SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

IF EXISTS (SELECT NULL FROM [SYS].[OBJECTS] WITH (NOLOCK) WHERE [OBJECT_ID] = OBJECT_ID(N'[dbo].[sproc_SimplePlatForm_AddTask]') AND [type] IN (N'P', N'TF'))
DROP PROCEDURE [dbo].[sproc_SimplePlatForm_AddTask];
GO

CREATE PROCEDURE [dbo].[sproc_SimplePlatForm_AddTask]
(
	@Status			INT OUTPUT,
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

			INSERT INTO dbo.Tasks(Name, StartDate, EndDate, Description, IsDeleted, CreateDate, UpdateDate, UsersDetail_UserId, Office_OfficeId, IsCompleted)
			VALUES (@Name, @StartDate, @EndDate, @Description, 0,  GETDATE(), GETDATE(), @UserId, @OfficeId, 0);

			SET @Status = 1;

		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION;
	END CATCH;
END;
