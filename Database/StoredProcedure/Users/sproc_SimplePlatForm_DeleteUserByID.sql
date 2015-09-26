SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

IF EXISTS (SELECT NULL FROM [SYS].[OBJECTS] WITH (NOLOCK) WHERE [OBJECT_ID] = OBJECT_ID(N'[dbo].[sproc_SimplePlatForm_DeleteUserByID]') AND [type] IN (N'P', N'TF'))
DROP PROCEDURE [dbo].[sproc_SimplePlatForm_DeleteUserByID];
GO

CREATE PROCEDURE [dbo].[sproc_SimplePlatForm_DeleteUserByID]
(
	@Status			INT OUTPUT,
	@UserID			INT
)
AS
BEGIN
	SET NOCOUNT ON;
	
	SET @Status = 0;

	BEGIN TRY
		BEGIN TRANSACTION;
			
			IF EXISTS (SELECT 1 FROM dbo.Users WHERE UserId = @UserID AND IsDeleted = 0)
			BEGIN		
				UPDATE dbo.Users
				SET
					[IsDeleted] = 1
				WHERE UserId = @UserID;

				SET @Status = 1;
			END

		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION;
	END CATCH;

END;
