SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

IF EXISTS (SELECT NULL FROM [SYS].[OBJECTS] WITH (NOLOCK) WHERE [OBJECT_ID] = OBJECT_ID(N'[dbo].[sproc_SimplePlatForm_UpdateFSMDetail]') AND [type] IN (N'P', N'TF'))
DROP PROCEDURE [dbo].[sproc_SimplePlatForm_UpdateFSMDetail];
GO

CREATE PROCEDURE [dbo].[sproc_SimplePlatForm_UpdateFSMDetail]
(
	@Status			INT OUTPUT,
	@ID				INT,
	@name			VARCHAR(MAX),
	@emailAddress	VARCHAR(MAX),
	@phoneNumber	VARCHAR(MAX)
)
AS
BEGIN
	SET NOCOUNT ON;

	SET @Status = 0;

	BEGIN TRY
		BEGIN TRANSACTION;

		IF NOT EXISTS(SELECT 1 FROM dbo.FSMDetails WHERE EmailId = @emailAddress AND Id <> @ID)
		BEGIN
			
			UPDATE FSMDetails SET
				[Name] = @name,
				[EmailId] = @emailAddress,
				[PhoneNumber] = @phoneNumber,
				[UpdateDate] = GETDATE()
			WHERE Id = @ID;

			SET @Status = 1;
		END;

		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION;
	END CATCH;
END;
