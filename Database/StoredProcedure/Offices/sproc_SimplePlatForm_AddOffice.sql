SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

IF EXISTS (SELECT NULL FROM [SYS].[OBJECTS] WITH (NOLOCK) WHERE [OBJECT_ID] = OBJECT_ID(N'[dbo].[sproc_SimplePlatForm_AddOffice]') AND [type] IN (N'P', N'TF'))
DROP PROCEDURE [dbo].[sproc_SimplePlatForm_AddOffice];
GO

CREATE PROCEDURE [dbo].[sproc_SimplePlatForm_AddOffice]
(
	@Status			INT OUTPUT,
	@name			VARCHAR(MAX),
	@contactNo		VARCHAR(MAX),
	@city			VARCHAR(MAX),
	@userID			INT,
	@path			VARCHAR(MAX) = NULL,
	@fileName		VARCHAR(MAX)
)
AS
BEGIN
	SET NOCOUNT ON;

	SET @Status = 0;

	BEGIN TRY
		BEGIN TRANSACTION;

			IF NOT EXISTS(SELECT 1 FROM dbo.Offices WHERE Name = @name)
			BEGIN

				DECLARE @fileResourceID AS INT = 0, @officeID AS INT = 0;

				IF (ISNULL(@name, '') <> '')
				BEGIN
					INSERT INTO dbo.FileResources
							( path, name )
					VALUES  ( @path, -- path - nvarchar(max)
							  @name  -- name - nvarchar(max)
							  );

					SET @fileResourceID = SCOPE_IDENTITY();
				END;

				INSERT INTO dbo.Offices
				        ( Name ,
				          ContactNo ,
				          City ,
				          IsDeleted ,
				          FileResource_Id
				        )
				VALUES  ( @name, -- Name - nvarchar(max)
				          @contactNo, -- ContactNo - nvarchar(max)
				          @city, -- City - nvarchar(max)
				          0, -- IsDeleted - bit
				          @fileResourceID  -- FileResource_Id - int
				        )

				SET @officeID = SCOPE_IDENTITY();

				INSERT INTO dbo.UserOffices ( UserId, OfficeId ) VALUES  ( @userID, @officeID );
			
				SET @Status = 1;
			END;

		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION;
	END CATCH;
END;
