SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

IF EXISTS (SELECT NULL FROM [SYS].[OBJECTS] WITH (NOLOCK) WHERE [OBJECT_ID] = OBJECT_ID(N'[dbo].[sproc_SimplePlatForm_UpdateTarget]') AND [type] IN (N'P', N'TF'))
DROP PROCEDURE [dbo].[sproc_SimplePlatForm_UpdateTarget];
GO

CREATE PROCEDURE [dbo].[sproc_SimplePlatForm_UpdateTarget]
(
	@Status			INT OUTPUT,
	@ID				INT,
	@Booking		INT,
	@FundRaising	REAL,
	@GSB			REAL,
	@Arrivals		INT,		
	@DueDate		DATETIME,
	@OfficeId		INT
)
AS
BEGIN
	SET NOCOUNT ON;

	SET @Status = 0;

	BEGIN TRY
		BEGIN TRANSACTION;
			IF EXISTS(SELECT 1 FROM dbo.Targets WHERE TargetId = @ID AND IsDeleted = 0)
			BEGIN
				
				UPDATE dbo.Targets SET 
					Booking = @Booking, 
					FundRaising = @FundRaising, 
					GSB =  @GSB, 
					Arrivals = @Arrivals, 
					DueDate = @DueDate, 
					Office_OfficeId = @OfficeId,
					UpdateDate = GETDATE()
				WHERE TargetId = @ID;

				SET @Status = 1;
			END
		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION;
	END CATCH;
END;
