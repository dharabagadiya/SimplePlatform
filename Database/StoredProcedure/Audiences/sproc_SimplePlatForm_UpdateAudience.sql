﻿SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

IF EXISTS (SELECT NULL FROM [SYS].[OBJECTS] WITH (NOLOCK) WHERE [OBJECT_ID] = OBJECT_ID(N'[dbo].[sproc_SimplePlatForm_UpdateAudience]') AND [type] IN (N'P', N'TF'))
DROP PROCEDURE [dbo].[sproc_SimplePlatForm_UpdateAudience];
GO

CREATE PROCEDURE [dbo].[sproc_SimplePlatForm_UpdateAudience]
(
	@Status			INT OUTPUT,
	@AudienceID		INT,
	@Name			VARCHAR(MAX), 
	@Contact		VARCHAR(MAX), 
	@VisitDate		DATETIME, 
	@VisitTypeID	INT, 
	@OfficeID		INT, 
	@EventID		INT,
	@FSMName		VARCHAR(MAX),
	@ConventionID	INT, 
	@IsBooked		BIT, 
	@GSBAmount		REAL, 
	@Amount			REAL
)
AS
BEGIN
	SET NOCOUNT ON;

	SET @Status = 0;

	BEGIN TRY
		BEGIN TRANSACTION;

			IF (@ConventionID = 0) SET @ConventionID = NULL;
			IF (@EventID = 0) SET @EventID = NULL;
			
			IF EXISTS(SELECT 1 FROM dbo.Audiences WHERE AudienceID =  @AudienceID AND IsDeleted = 0)
			BEGIN
				UPDATE dbo.Audiences SET
						  Name = @Name,
						  VisitDate = @VisitDate,
						  Contact = @Contact,
						  UpdateDate = GETDATE(),
						  Convention_ConventionId = @ConventionID,
						  Event_EventId = @EventID,
						  Office_OfficeId = @OfficeID,
						  VisitType_VisitTypeId = @VisitTypeID,
						  GSBAmount = @GSBAmount,
						  FSMName = @FSMName,
						  Amount =  @Amount,
						  IsBooked = @IsBooked
				WHERE AudienceID =  @AudienceID AND IsDeleted = 0;

				SET @Status = 1;
			END

		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION;
	END CATCH;
END;