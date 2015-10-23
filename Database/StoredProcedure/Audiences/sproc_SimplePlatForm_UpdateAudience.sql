SET ANSI_NULLS ON;
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
	@FSMID			INT,
	@ConventionID	INT,
	@ServiceID		INT,
	@BookingStatus	INT, 
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
			IF (@ServiceID = 0) SET @ServiceID = NULL;
			IF (@FSMID = 0) SET @FSMID = NULL;

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
						  Sevice_ServiceId = @ServiceID,
						  GSBAmount = @GSBAmount,
						  FSMDetail_UserId = @FSMID,
						  Amount =  @Amount,
						  BookingStatus = @BookingStatus
				WHERE AudienceID =  @AudienceID AND IsDeleted = 0;

				SET @Status = 1;
			END

		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
		SELECT ERROR_MESSAGE();
		ROLLBACK TRANSACTION;
	END CATCH;
END;
