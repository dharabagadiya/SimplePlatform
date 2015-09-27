SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

IF EXISTS (SELECT NULL FROM [SYS].[OBJECTS] WITH (NOLOCK) WHERE [OBJECT_ID] = OBJECT_ID(N'[dbo].[sproc_SimplePlatForm_CreateUser]') AND [type] IN (N'P', N'TF'))
DROP PROCEDURE [dbo].[sproc_SimplePlatForm_CreateUser];
GO

CREATE PROCEDURE [dbo].[sproc_SimplePlatForm_CreateUser]
(
	@Status			INT OUTPUT,
	@firstName		VARCHAR(MAX),
	@lastName		VARCHAR(MAX),
	@password		VARCHAR(MAX) = '12345',
	@emildID		VARCHAR(MAX),
	@userRoleID		INT,
	@officeID		INT
)
AS
BEGIN
	SET NOCOUNT ON;
	
	SET @Status = 0;

	BEGIN TRY
		BEGIN TRANSACTION;
			
			IF NOT EXISTS(SELECT 1 FROM dbo.Users WHERE Email = @emildID)
			BEGIN		

				INSERT dbo.Users
				        ( UserName ,
				          Email ,
				          Password ,
				          FirstName ,
				          LastName ,
				          IsActive ,
				          CreateDate ,
				          IsDeleted
				        )
				VALUES  ( @emildID, -- UserName - nvarchar(max)
				          @emildID, -- Email - nvarchar(max)
				          @password, -- Password - nvarchar(max)
				          @firstName, -- FirstName - nvarchar(max)
				          @lastName, -- LastName - nvarchar(max)
				          1, -- IsActive - bit
				          GETDATE() , -- CreateDate - datetime
				          0 -- IsDeleted - bit
				        );

				DECLARE @UserID	AS INT = 0;

				SET @UserID = SCOPE_IDENTITY();

				INSERT INTO dbo.UserRoles
				        ( UserId, RoleId )
				VALUES  ( @UserID, -- UserId - int
				          @userRoleID  -- RoleId - int
				          );

				INSERT INTO dbo.UserDetails
				        ( UserId ,
				          FileResource_Id ,
				          StartDate ,
				          EndDate
				        )
				VALUES  ( @UserID, -- UserId - int
				          NULL, -- FileResource_Id - int
				          NULL, -- StartDate - datetime
				          NULL  -- EndDate - datetime
				        );

				IF (@officeID <= 0) SET @officeID = 0;

				IF (@officeID <> 0)
				BEGIN
					INSERT INTO dbo.UserOffices
							( UserId, OfficeId )
					VALUES  ( @UserID, -- UserId - int
							  @officeID  -- OfficeId - int
							  )
				END;

				SET @Status = 1;
			END;

		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
		SELECT ERROR_MESSAGE();
		ROLLBACK TRANSACTION;
	END CATCH;

END;
