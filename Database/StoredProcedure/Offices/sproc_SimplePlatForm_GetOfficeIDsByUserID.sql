SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

IF EXISTS (SELECT NULL FROM [SYS].[OBJECTS] WITH (NOLOCK) WHERE [OBJECT_ID] = OBJECT_ID(N'[dbo].[sproc_SimplePlatForm_GetOfficeIDsByUserID]') AND [type] IN (N'P', N'TF'))
DROP PROCEDURE [dbo].[sproc_SimplePlatForm_GetOfficeIDsByUserID];
GO

CREATE PROCEDURE [dbo].[sproc_SimplePlatForm_GetOfficeIDsByUserID]
(
	@userID			INT
)
AS
BEGIN
	SET NOCOUNT ON;

	WITH [officeIDs] AS(
		SELECT 
			[OfficeId] AS [OfficeId]
		FROM dbo.Offices
		WHERE @userID = 0
		UNION
		SELECT 
			[OfficeId] AS [OfficeId]
		FROM dbo.UserOffices
		WHERE UserId = @userID
		GROUP BY [OfficeId]
	)
	SELECT
		[I].[OfficeId]
 	FROM [dbo].[Offices] AS [I]
	INNER JOIN officeIDs AS [II] ON	[II].OfficeId = I.OfficeId
	WHERE I.IsDeleted = 0;

END;
