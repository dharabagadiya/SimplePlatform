﻿SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

IF EXISTS (SELECT NULL FROM [SYS].[OBJECTS] WITH (NOLOCK) WHERE [OBJECT_ID] = OBJECT_ID(N'[dbo].[sproc_SimplePlatForm_UpdateOffice]') AND [type] IN (N'P', N'TF'))
DROP PROCEDURE [dbo].[sproc_SimplePlatForm_UpdateOffice];
GO

CREATE PROCEDURE [dbo].[sproc_SimplePlatForm_UpdateOffice]
(
	@Status			INT OUTPUT,
	@officeID		INT,
	@name			VARCHAR(MAX),
	@contactNo		VARCHAR(MAX),
	@city			VARCHAR(MAX),
	@userID			VARCHAR(MAX) = '',
	@path			VARCHAR(MAX) = '',
	@fileName		VARCHAR(MAX) =  ''
)
AS
BEGIN
	SET NOCOUNT ON;

	SET @Status = 0;

	BEGIN TRY
		BEGIN TRANSACTION;

			IF NOT EXISTS(SELECT 1 FROM dbo.Offices WHERE Name = @name AND OfficeId <> @officeID AND IsDeleted = 0)
			BEGIN

				DECLARE @fileResourceID AS INT = 0;

				IF (ISNULL(@fileName, '') <> '')
				BEGIN

					SELECT	@fileResourceID = FileResource_Id FROM dbo.Offices WHERE OfficeId = @officeID

					UPDATE dbo.FileResources SET
							[path] = @path, 
							name = @fileName
					WHERE Id = @fileResourceID;
				END;

				UPDATE dbo.Offices SET
						  Name = @name,
				          ContactNo = @contactNo,
				          City = @city
				WHERE OfficeId = @officeID;

				DELETE [I] FROM dbo.UserOffices AS [I] INNER JOIN dbo.UserRoles AS [II] ON II.RoleId = 2 AND II.UserId = I.UserId WHERE OfficeId = @officeID;

				IF(@userID <> '')
				BEGIN
					WITH [UserIDs] AS 
					(
						SELECT
							[Value] AS [UserID]
						FROM dbo.func_SimplePlatForm_GetParamsToList(@userID) AS [I]
						INNER JOIN dbo.Users AS [II] ON [II].UserId = I.Value
					)
					INSERT INTO dbo.UserOffices ( UserId, OfficeId )
					SELECT
						[UserID] AS [UserId],
						@officeID AS [OfficeId]
					FROM UserIDs;
				END

				SET @Status = 1;
			END;

		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION;
	END CATCH;
END;
