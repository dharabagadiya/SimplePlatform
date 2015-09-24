SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

IF EXISTS (SELECT NULL FROM [SYS].[OBJECTS] WITH (NOLOCK) WHERE [OBJECT_ID] = OBJECT_ID(N'[dbo].[sproc_SimplePlatForm_DeleteAttachmentConvention]') AND [type] IN (N'P', N'TF'))
DROP PROCEDURE [dbo].[sproc_SimplePlatForm_DeleteAttachmentConvention];
GO

CREATE PROCEDURE [dbo].[sproc_SimplePlatForm_DeleteAttachmentConvention]
(
	@Status					INT OUTPUT,
	@conventionID			INT,
	@conventionAttachmentID	INT
)
AS
BEGIN
	SET NOCOUNT ON;

	SET @Status = 0;

	BEGIN TRY
		BEGIN TRANSACTION;

			DECLARE @fileResourceID AS INT = 0, @officeID AS INT = 0;

			SELECT @fileResourceID = FileResource_Id FROM dbo.ConventionAttachments WHERE ConventionAttachmentId = @conventionAttachmentID;
			
			DELETE FROM dbo.ConventionAttachments WHERE ConventionAttachmentId = @conventionAttachmentID;

			DELETE FROM dbo.FileResources WHERE Id = @fileResourceID;

			SET @Status = 1;

		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION;
	END CATCH;
END;
