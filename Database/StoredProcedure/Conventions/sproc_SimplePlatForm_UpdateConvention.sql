SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

IF EXISTS (SELECT NULL FROM [SYS].[OBJECTS] WITH (NOLOCK) WHERE [OBJECT_ID] = OBJECT_ID(N'[dbo].[sproc_SimplePlatForm_UpdateConvention]') AND [type] IN (N'P', N'TF'))
DROP PROCEDURE [dbo].[sproc_SimplePlatForm_UpdateConvention];
GO

CREATE PROCEDURE [dbo].[sproc_SimplePlatForm_UpdateConvention]
(
	@Status			INT OUTPUT,
	@conventionID	INT,
	@name			VARCHAR(MAX), 
	@startDate		DATETIME,
	@endDate		DATETIME,
	@description	VARCHAR(MAX),
	@userID			INT = 0,
	@city			VARCHAR(MAX),
	@path			VARCHAR(MAX) = '',
	@fileName		VARCHAR(MAX) = ''
)
AS
BEGIN
	SET NOCOUNT ON;

	SET @Status = 0;

	BEGIN TRY
		BEGIN TRANSACTION;

			DECLARE @fileResourceID AS INT = 0;

			IF (ISNULL(@fileName, '') <> '')
			BEGIN

				SELECT	@fileResourceID = FileResource_Id FROM dbo.Conventions WHERE ConventionId = @conventionID;

				UPDATE dbo.FileResources SET
						[path] = @path, 
						name = @fileName
				WHERE Id = @fileResourceID;
			END;
						
			UPDATE dbo.Conventions SET
					  Name = @name,
			          StartDate = @startDate,
			          EndDate = @endDate,
			          [Description] = @description,
			          UpdateDate = GETDATE(),
			          City = @city
			WHERE IsDeleted = 0 AND ConventionId = @conventionID

			SET @Status = 1;

		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION;
	END CATCH;
END;
