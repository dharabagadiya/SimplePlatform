SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

IF EXISTS (SELECT NULL FROM [SYS].[OBJECTS] WITH (NOLOCK) WHERE [OBJECT_ID] = OBJECT_ID(N'[dbo].[sproc_SimplePlatForm_UpdateUserByID]') AND [type] IN (N'P', N'TF'))
DROP PROCEDURE [dbo].[sproc_SimplePlatForm_UpdateUserByID];
GO

CREATE PROCEDURE [dbo].[sproc_SimplePlatForm_UpdateUserByID]
(
	@Status			INT OUTPUT,
	@UserID			INT,
	@firstName		VARCHAR(MAX),
	@lastName		VARCHAR(MAX),
	@emildID		VARCHAR(MAX),
	@userRoleID		INT,
	@officeID		INT, 
	@fileName		VARCHAR(MAX) = '',
	@path			VARCHAR(MAX) = ''
)
AS
BEGIN
	SET NOCOUNT ON;
	
	SET @Status = 0;

	BEGIN TRY
		BEGIN TRANSACTION;
			
			IF NOT EXISTS(SELECT 1 FROM dbo.Users WHERE UserId <> @UserID AND Email = @emildID)
			BEGIN

				IF EXISTS (SELECT 1 FROM dbo.Users WHERE UserId = @UserID AND IsDeleted = 0)
				BEGIN		
				
					DECLARE @fileResourceID AS INT = 0;

					IF (ISNULL(@fileName, '') <> '')
					BEGIN
						SELECT	@fileResourceID = FileResource_Id FROM dbo.UserDetails WHERE UserId = @UserID;

						UPDATE dbo.FileResources SET
								[path] = @path, 
								name = @fileName
						WHERE Id = @fileResourceID;
					END;

					UPDATE dbo.UserRoles
					SET
						RoleId = @userRoleID
					WHERE UserId =  @UserID;
					
					DECLARE @UserDetailID AS INT = 0;

					IF(@officeID <= 0) SET @officeID = 0;

					SELECT @UserDetailID = UserId FROM dbo.UserDetails WHERE UserId = @UserID;

					IF(@officeID <> 0)
					BEGIN
						UPDATE dbo.UserOffices
						SET
							OfficeId = @officeID
						WHERE UserId = @UserDetailID;
					END;

					UPDATE dbo.Users SET
						  FirstName = @firstName,
						  LastName = @lastName,
						  Email = @emildID
					WHERE IsDeleted = 0 AND UserId = @UserID;

					SET @Status = 1;
				END
			END

		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION;
	END CATCH;

END;
