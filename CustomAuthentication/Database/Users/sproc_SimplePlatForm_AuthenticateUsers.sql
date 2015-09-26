SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

IF EXISTS (SELECT NULL FROM [SYS].[OBJECTS] WITH (NOLOCK) WHERE [OBJECT_ID] = OBJECT_ID(N'[dbo].[sproc_SimplePlatForm_AuthenticateUsers]') AND [type] IN (N'P', N'TF'))
DROP PROCEDURE [dbo].[sproc_SimplePlatForm_AuthenticateUsers];
GO

CREATE PROCEDURE [dbo].[sproc_SimplePlatForm_AuthenticateUsers]
(
	@Status			INT OUTPUT,
	@username		VARCHAR(MAX),
	@password		VARCHAR(MAX)
)
AS
BEGIN
	SET NOCOUNT ON;

	SET @Status = 0;

	IF EXISTS(SELECT 1 FROM dbo.Users WHERE Email = @username AND [Password] = @password AND IsDeleted = 0)
	BEGIN
		SET @Status = 1;
	END;

END;
