SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

IF EXISTS (SELECT NULL FROM [SYS].[OBJECTS] WITH (NOLOCK) WHERE [OBJECT_ID] = OBJECT_ID(N'[dbo].[sproc_SimplePlatForm_AddFSMDetail]') AND [type] IN (N'P', N'TF'))
DROP PROCEDURE [dbo].[sproc_SimplePlatForm_AddFSMDetail];
GO

CREATE PROCEDURE [dbo].[sproc_SimplePlatForm_AddFSMDetail]
(
	@Status			INT OUTPUT,
	@name			VARCHAR(MAX),
	@emailAddress	VARCHAR(MAX),
	@phoneNumber	VARCHAR(MAX)
)
AS
BEGIN
	SET NOCOUNT ON;

	SET @Status = 0;

	BEGIN TRY
		BEGIN TRANSACTION;

		IF NOT EXISTS(SELECT 1 FROM dbo.FSMDetails WHERE EmailId = @emailAddress)
		BEGIN

			INSERT INTO dbo.FSMDetails
			        ( Name ,
					  EmailId,
					  PhoneNumber ,
			          IsDeleted ,
			          CreateDate ,
			          UpdateDate
			        )
			VALUES  ( @name , -- Name - varchar(50)
					  @emailAddress,
			          @phoneNumber, -- PhoneNumber - varchar(50)
			          0, -- IsDeleted - bit
			          GETDATE() , -- CreateDate - datetime
			          GETDATE()  -- UpdateDate - datetime
			        );

			SET @Status = SCOPE_IDENTITY();
		END;

		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION;
	END CATCH;
END;
