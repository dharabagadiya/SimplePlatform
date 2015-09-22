SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

IF EXISTS (SELECT NULL FROM [SYS].[OBJECTS] WITH (NOLOCK) WHERE [OBJECT_ID] = OBJECT_ID(N'[dbo].[sproc_SimplePlatForm_DeleteEvent]') AND [type] IN (N'P', N'TF'))
DROP PROCEDURE [dbo].[sproc_SimplePlatForm_DeleteEvent];
GO

CREATE PROCEDURE [dbo].[sproc_SimplePlatForm_DeleteEvent]
(
	@Status			INT OUTPUT,
	@eventID		INT
)
AS
BEGIN
	SET NOCOUNT ON;

	SET @Status = 0;

	BEGIN TRY
		BEGIN TRANSACTION;

		IF EXISTS (SELECT 1 FROM dbo.Events WHERE EventId = @eventID AND IsDeleted = 0)
		BEGIN

			UPDATE dbo.Events SET
				IsDeleted = 1
			WHERE EventId = @eventID AND IsDeleted = 0;

		END;

		SET @Status = 1;

		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION;
	END CATCH;
END;
