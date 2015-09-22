SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

IF EXISTS (SELECT NULL FROM [SYS].[OBJECTS] WITH (NOLOCK) WHERE [OBJECT_ID] = OBJECT_ID(N'[dbo].[sproc_SimplePlatForm_UpdateEvent]') AND [type] IN (N'P', N'TF'))
DROP PROCEDURE [dbo].[sproc_SimplePlatForm_UpdateEvent];
GO

CREATE PROCEDURE [dbo].[sproc_SimplePlatForm_UpdateEvent]
(
	@Status			INT OUTPUT,
	@eventID		INT,
	@name			VARCHAR(MAX),
	@startDate		DATETIME, 
	@endDate		DATETIME, 
	@description	VARCHAR(MAX), 
	@officeID		INT, 
	@conventionID	INT, 
	@city			VARCHAR(MAX)
)
AS
BEGIN
	SET NOCOUNT ON;

	SET @Status = 0;

	BEGIN TRY
		BEGIN TRANSACTION;

		IF EXISTS (SELECT 1 FROM dbo.Events WHERE EventId = @eventID AND IsDeleted = 0)
		BEGIN

			UPDATE dbo.Events SET
					Name = @name,
					StartDate =  @startDate,
					EndDate = @endDate,
					[Description] = @description,
					convention_ConventionId = @conventionID ,
					Office_OfficeId = @officeID,
					City = @city
			WHERE EventId = @eventID AND IsDeleted = 0;

		END;

		SET @Status = 1;

		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION;
	END CATCH;
END;
