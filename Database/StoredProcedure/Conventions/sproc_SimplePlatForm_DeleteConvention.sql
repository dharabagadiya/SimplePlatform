SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

IF EXISTS (SELECT NULL FROM [SYS].[OBJECTS] WITH (NOLOCK) WHERE [OBJECT_ID] = OBJECT_ID(N'[dbo].[sproc_SimplePlatForm_DeleteConvention]') AND [type] IN (N'P', N'TF'))
DROP PROCEDURE [dbo].[sproc_SimplePlatForm_DeleteConvention];
GO

CREATE PROCEDURE [dbo].[sproc_SimplePlatForm_DeleteConvention]
(
	@Status			INT OUTPUT,
	@conventionID	INT
)
AS
BEGIN
	SET NOCOUNT ON;

	SET @Status = 0;

	BEGIN TRY
		BEGIN TRANSACTION;

			DECLARE @fileResourceID AS INT = 0;

			SELECT	@fileResourceID = FileResource_Id FROM dbo.Conventions WHERE ConventionId = @conventionID;

			DELETE FROM dbo.FileResources WHERE Id = @fileResourceID;

			UPDATE dbo.Conventions SET
					  IsDeleted = 0,
			          UpdateDate = GETDATE()
			WHERE IsDeleted = 0 AND ConventionId = @conventionID;

			SET @Status = 1;

		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION;
	END CATCH;
END;
