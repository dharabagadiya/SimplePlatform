SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

IF EXISTS (SELECT NULL FROM [SYS].[OBJECTS] WITH (NOLOCK) WHERE [OBJECT_ID] = OBJECT_ID(N'[dbo].[sproc_SimplePlatForm_DeleteOffice]') AND [type] IN (N'P', N'TF'))
DROP PROCEDURE [dbo].[sproc_SimplePlatForm_DeleteOffice];
GO

CREATE PROCEDURE [dbo].[sproc_SimplePlatForm_DeleteOffice]
(
	@Status			INT OUTPUT,
	@officeID		INT
)
AS
BEGIN
	SET NOCOUNT ON;

	SET @Status = 0;

	BEGIN TRY
		BEGIN TRANSACTION;

			IF NOT EXISTS(SELECT 1 FROM dbo.Offices WHERE OfficeId = @officeID)
			BEGIN

				UPDATE dbo.Offices SET
					IsDeleted = 1
				WHERE OfficeId = @officeID;

				SET @Status = 1;
			END;

		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION;
	END CATCH;
END;
