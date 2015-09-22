SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

IF EXISTS (SELECT NULL FROM [SYS].[OBJECTS] WITH (NOLOCK) WHERE [OBJECT_ID] = OBJECT_ID(N'[dbo].[sproc_SimplePlatForm_AddEvent]') AND [type] IN (N'P', N'TF'))
DROP PROCEDURE [dbo].[sproc_SimplePlatForm_AddEvent];
GO

CREATE PROCEDURE [dbo].[sproc_SimplePlatForm_AddEvent]
(
	@Status			INT OUTPUT,
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
			
			INSERT dbo.Events
			        ( Name ,
			          StartDate ,
			          EndDate ,
			          Description ,
			          IsDeleted ,
			          convention_ConventionId ,
			          Office_OfficeId ,
			          City
			        )
			VALUES  ( @name , -- Name - nvarchar(max)
			          @startDate, -- StartDate - datetime
			          @endDate , -- EndDate - datetime
			          @description, -- Description - nvarchar(max)
			          0, -- IsDeleted - bit
			          @officeID, -- convention_ConventionId - int
			          @conventionID, -- Office_OfficeId - int
			          @city  -- City - nvarchar(max)
			        )

			SET @Status = 1;

		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION;
	END CATCH;
END;
