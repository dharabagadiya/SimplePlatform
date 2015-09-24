SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

IF EXISTS (SELECT NULL FROM [SYS].[OBJECTS] WITH (NOLOCK) WHERE [OBJECT_ID] = OBJECT_ID(N'[dbo].[sproc_SimplePlatForm_AddConvention]') AND [type] IN (N'P', N'TF'))
DROP PROCEDURE [dbo].[sproc_SimplePlatForm_AddConvention];
GO

CREATE PROCEDURE [dbo].[sproc_SimplePlatForm_AddConvention]
(
	@Status			INT OUTPUT,
	@name			VARCHAR(MAX), 
	@startDate		DATETIME,
	@endDate		DATETIME,
	@description	VARCHAR(MAX),
	@userID			INT,
	@city			VARCHAR(MAX),
	@path			VARCHAR(MAX),
	@fileName		VARCHAR(MAX)
)
AS
BEGIN
	SET NOCOUNT ON;

	SET @Status = 0;

	BEGIN TRY
		BEGIN TRANSACTION;

			DECLARE @fileResourceID AS INT = 0, @officeID AS INT = 0;

				IF (ISNULL(@fileName, '') <> '')
				BEGIN
					INSERT INTO dbo.FileResources
							( [path], name )
					VALUES  ( @path, -- path - nvarchar(max)
							  @fileName  -- name - nvarchar(max)
							  );

					SET @fileResourceID = SCOPE_IDENTITY();
				END;
			
			
			INSERT INTO dbo.Conventions
			        ( Name ,
			          StartDate ,
			          EndDate ,
			          Description ,
			          IsDeleted ,
			          CreateDate ,
			          UpdateDate ,
			          City ,
			          FileResource_Id
			        )
			VALUES  ( 
					  @name, -- Name - nvarchar(max)
			          @startDate , -- StartDate - datetime
			          @endDate , -- EndDate - datetime
			          @description , -- Description - nvarchar(max)
			          0, -- IsDeleted - bit
			          GETDATE() , -- CreateDate - datetime
			          GETDATE() , -- UpdateDate - datetime
			          @city, -- City - nvarchar(max)
			          @fileResourceID  -- FileResource_Id - int
			        );

			SET @Status = 1;

		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION;
	END CATCH;
END;
