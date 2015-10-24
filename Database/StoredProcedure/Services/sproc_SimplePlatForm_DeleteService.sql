﻿SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

IF EXISTS (SELECT NULL FROM [SYS].[OBJECTS] WITH (NOLOCK) WHERE [OBJECT_ID] = OBJECT_ID(N'[dbo].[sproc_SimplePlatForm_DeleteService]') AND [type] IN (N'P', N'TF'))
DROP PROCEDURE [dbo].[sproc_SimplePlatForm_DeleteService];
GO

CREATE PROCEDURE [dbo].[sproc_SimplePlatForm_DeleteService]
(
	@Status			INT OUTPUT,
	@ID				INT
)
AS
BEGIN
	SET NOCOUNT ON;

	SET @Status = 0;

	BEGIN TRY
		BEGIN TRANSACTION;

		IF EXISTS(SELECT 1 FROM [dbo].[Services] WHERE [ServiceId] = @ID)
		BEGIN

			UPDATE [dbo].[Services] SET
				[IsDeleted] = 1,
				[UpdateDate] = GETDATE()
			WHERE [ServiceId] = @ID AND [IsDeleted] = 0;

			SET @Status = 1;
		END;

		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION;
	END CATCH;
END;
