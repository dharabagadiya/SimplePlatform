SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

IF EXISTS (SELECT NULL FROM [SYS].[OBJECTS] WITH (NOLOCK) WHERE [OBJECT_ID] = OBJECT_ID(N'[dbo].[sproc_SimplePlatForm_GetSelectionSlipByAudienceID]') AND [type] IN (N'P', N'TF'))
DROP PROCEDURE [dbo].[sproc_SimplePlatForm_GetSelectionSlipByAudienceID];
GO

CREATE PROCEDURE [dbo].[sproc_SimplePlatForm_GetSelectionSlipByAudienceID]
(
	@ID	INT
)
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT
		[I].AudienceID,
		[I].Name,
		[I].[EmailAddress],
		ISNULL([II].[Name],ISNULL([VII].[ServiceName],''))  AS [ConventionServiceName],
		[VIII].[Name] AS [FSMDetailName]
	FROM dbo.Audiences AS [I]
	LEFT JOIN dbo.Conventions AS  [II] ON [II].[ConventionId] = [I].[Convention_ConventionId]
	LEFT JOIN dbo.[Services] AS [VII] ON [VII].ServiceId = I.Sevice_ServiceId
	LEFT JOIN dbo.FSMDetails AS [VIII] ON VIII.Id = I.FSMDetail_UserId
	WHERE [I].[IsDeleted] = 0 AND [I].[AudienceID] = @ID;

END;
