SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

IF EXISTS (SELECT NULL FROM [SYS].[OBJECTS] WITH (NOLOCK) WHERE [OBJECT_ID] = OBJECT_ID(N'[dbo].[sproc_SimplePlatForm_DeleteAudience]') AND [type] IN (N'P', N'TF'))
DROP PROCEDURE [dbo].[sproc_SimplePlatForm_DeleteAudience];
GO

CREATE PROCEDURE [dbo].[sproc_SimplePlatForm_DeleteAudience]
(
	@Status			INT OUTPUT,
	@AudienceID		INT
)
AS
BEGIN
	SET NOCOUNT ON;

	SET @Status = 0;

	BEGIN TRY
		BEGIN TRANSACTION;

			IF EXISTS(SELECT 1 FROM dbo.Audiences WHERE AudienceID =  @AudienceID AND IsDeleted = 0)
			BEGIN
				UPDATE dbo.Audiences SET
						  UpdateDate = GETDATE(),
						  IsDeleted = 1
				WHERE AudienceID =  @AudienceID AND IsDeleted = 0;

				SET @Status = 1;
			END

		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION;
	END CATCH;
END;
