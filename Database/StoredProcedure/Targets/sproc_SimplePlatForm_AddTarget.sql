SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

IF EXISTS (SELECT NULL FROM [SYS].[OBJECTS] WITH (NOLOCK) WHERE [OBJECT_ID] = OBJECT_ID(N'[dbo].[sproc_SimplePlatForm_AddTarget]') AND [type] IN (N'P', N'TF'))
DROP PROCEDURE [dbo].[sproc_SimplePlatForm_AddTarget];
GO

CREATE PROCEDURE [dbo].[sproc_SimplePlatForm_AddTarget]
(
	@Status			INT OUTPUT,
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
			
			INSERT INTO dbo.Targets(Booking, FundRaising, GSB, Arrivals, DueDate, Office_OfficeId, IsDeleted, CreateDate, UpdateDate)
			VALUES  (@Booking, @FundRaising, @GSB, @Arrivals, @DueDate, @OfficeId, 0, GETDATE(), GETDATE());

			SET @Status = 1;

		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION;
	END CATCH;
END;
