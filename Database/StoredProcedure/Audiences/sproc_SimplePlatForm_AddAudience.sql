﻿SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

IF EXISTS (SELECT NULL FROM [SYS].[OBJECTS] WITH (NOLOCK) WHERE [OBJECT_ID] = OBJECT_ID(N'[dbo].[sproc_SimplePlatForm_AddAudience]') AND [type] IN (N'P', N'TF'))
DROP PROCEDURE [dbo].[sproc_SimplePlatForm_AddAudience];
GO

CREATE PROCEDURE [dbo].[sproc_SimplePlatForm_AddAudience]
(
	@Status			INT OUTPUT,
	@Name			VARCHAR(MAX), 
	@Contact		VARCHAR(MAX),
	@EmailAddress	VARCHAR(MAX),
	@VisitDate		DATETIME, 
	@VisitTypeID	INT, 
	@OfficeID		INT, 
	@EventID		INT,
	@FSMID			INT,
	@ConventionID	INT,
	@ServiceID		INT,
	@ArrivalDate	DATETIME = NULL,
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
			IF (@ServiceID = 0) SET @ServiceID = NULL;
			IF (@FSMID = 0) SET @FSMID = NULL;

			IF (@EventID = 0) 
			BEGIN
				SET @EventID = NULL;
			END
			ELSE
			BEGIN
				SELECT @ConventionID = convention_ConventionId FROM dbo.Events WHERE EventId = @EventID AND IsDeleted = 0;
			END;

			INSERT INTO dbo.Audiences(
					  Name ,
			          VisitDate ,
			          Contact ,
			          IsDeleted ,
			          CreateDate ,
			          UpdateDate ,
			          Convention_ConventionId ,
			          Event_EventId ,
			          Office_OfficeId,
					  Sevice_ServiceId,
					  FSMDetail_UserId,
			          VisitType_VisitTypeId,
			          GSBAmount ,
			          IsAttended,
			          Amount ,
			          BookingStatus,
					  EmailAddress,
					  ArrivalDate
			        )
			VALUES  ( @Name,
			          @VisitDate,
			          @Contact,
			          0, -- IsDeleted - bit
			          GETDATE() , -- CreateDate - datetime
			          GETDATE() , -- UpdateDate - datetime
			          @ConventionID, -- Convention_ConventionId - int
			          @EventID, -- Event_EventId - int
			          @OfficeID, -- Office_OfficeId - int
					  @ServiceID,
					  @FSMID,
			          @VisitTypeID, -- VisitType_VisitTypeId - int
			          @GSBAmount, -- GSBAmount - real
			          0, -- IsAttended - bit
			          @Amount, -- Amount - real
			          @BookingStatus,  -- IsBooked - bit
					  @EmailAddress,
					  @ArrivalDate
			        )

			SET @Status = SCOPE_IDENTITY();

		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
		SELECT ERROR_MESSAGE();
		ROLLBACK TRANSACTION;
	END CATCH;
END;
