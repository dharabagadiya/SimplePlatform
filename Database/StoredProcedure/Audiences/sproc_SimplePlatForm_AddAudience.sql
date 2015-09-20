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
			          VisitType_VisitTypeId,
			          GSBAmount ,
			          IsAttended ,
			          FSMName ,
			          Amount ,
			          IsBooked
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
			          @VisitTypeID, -- VisitType_VisitTypeId - int
			          @GSBAmount, -- GSBAmount - real
			          0, -- IsAttended - bit
			          @FSMName, -- FSMName - nvarchar(max)
			          @Amount, -- Amount - real
			          @IsBooked  -- IsBooked - bit
			        )

			SET @Status = 1;

		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION;
	END CATCH;
END;
