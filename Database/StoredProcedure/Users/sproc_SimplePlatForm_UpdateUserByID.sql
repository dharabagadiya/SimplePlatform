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
	@officeID		VARCHAR(MAX), 
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

						IF(@fileResourceID <> 0)
						BEGIN
							UPDATE dbo.FileResources SET
									[path] = @path, 
									name = @fileName
							WHERE Id = @fileResourceID;
						END
						ELSE
						BEGIN

							INSERT INTO dbo.FileResources
							        ( path, name )
							VALUES  ( @path, -- path - nvarchar(max)
							          @fileName  -- name - nvarchar(max)
							          );

						 	SET @fileResourceID = SCOPE_IDENTITY();

							UPDATE dbo.UserDetails SET
									[FileResource_Id] = @fileResourceID
							WHERE UserId = @UserID;
							
						END
					END;

					UPDATE dbo.UserRoles
					SET
						RoleId = @userRoleID
					WHERE UserId =  @UserID;
					
					DECLARE @UserDetailID AS INT = 0;

					SELECT @UserDetailID = UserId FROM dbo.UserDetails WHERE UserId = @UserID;

					IF(@officeID <> '')
					BEGIN
						DELETE dbo.UserOffices WHERE UserId = @UserDetailID;

						WITH [OfficeID] AS 
						(
							SELECT
								[Value] AS [OfficeID]
							FROM dbo.func_SimplePlatForm_GetParamsToList(@officeID) AS [I]
							INNER JOIN dbo.Offices AS [II] ON [II].OfficeId = I.Value
						)
						INSERT INTO dbo.UserOffices (UserId, OfficeId)
						SELECT
							@UserID AS [UserId],
							[OfficeID] AS [OfficeId]
						FROM OfficeID;

					END
					ELSE IF @userRoleID = 1 -- User Role is Set To Admin And Admin Dont Have Any Mapping
					BEGIN
						DELETE dbo.UserOffices
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
