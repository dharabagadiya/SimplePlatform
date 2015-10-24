SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

IF EXISTS (SELECT NULL FROM [SYS].[OBJECTS] WITH (NOLOCK) WHERE [OBJECT_ID] = OBJECT_ID(N'[dbo].[sproc_SimplePlatForm_UpdateAudienceStatus]') AND [type] IN (N'P', N'TF'))
DROP PROCEDURE [dbo].[sproc_SimplePlatForm_UpdateAudienceStatus];
GO

CREATE PROCEDURE [dbo].[sproc_SimplePlatForm_UpdateAudienceStatus]
(
	@Status			INT OUTPUT,
	@AudienceID		INT,
	@ArrivalDate	DATETIME
)
AS
BEGIN
	SET NOCOUNT ON;

	SET @Status = 0;

	BEGIN TRY
		BEGIN TRANSACTION;

			IF EXISTS(SELECT 1 FROM dbo.Audiences WHERE AudienceID =  @AudienceID AND IsDeleted = 0)
			BEGIN
				DECLARE @CurrentStatus AS BIT, @BookingStatus AS INT;

				SELECT @CurrentStatus = [IsAttended] FROM dbo.Audiences WHERE AudienceID =  @AudienceID AND IsDeleted = 0;

				IF(@CurrentStatus = 0) SET @CurrentStatus = 1;
				ELSE SET @CurrentStatus = 0;

				IF(@CurrentStatus = 1) 
				BEGIN
					SET @BookingStatus = 2;
				END
				ELSE
				BEGIN
					SET @BookingStatus = 1;
					SET @ArrivalDate = NULL;
				END;

				UPDATE dbo.Audiences SET
						  UpdateDate = GETDATE(),
						  IsAttended = @CurrentStatus,
						  BookingStatus = @BookingStatus,
						  [ArrivalDate] = @ArrivalDate
				WHERE AudienceID =  @AudienceID AND IsDeleted = 0;

				SET @Status = 1;
			END

		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION;
	END CATCH;
END;
