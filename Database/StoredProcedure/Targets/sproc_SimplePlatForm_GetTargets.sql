SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

IF EXISTS (SELECT NULL FROM [SYS].[OBJECTS] WITH (NOLOCK) WHERE [OBJECT_ID] = OBJECT_ID(N'[dbo].[sproc_SimplePlatForm_GetTargets]') AND [type] IN (N'P', N'TF'))
DROP PROCEDURE [dbo].[sproc_SimplePlatForm_GetTargets];
GO

CREATE PROCEDURE [dbo].[sproc_SimplePlatForm_GetTargets]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		[TargetId] AS [TargetId],
		[Booking] AS [Booking],
		[FundRaising] AS [FundRaising],
		[GSB] AS [GSB],
		[Arrivals] AS [Arrivals],
		[DueDate] AS [DueDate],
		[Office_OfficeId] AS [OfficeId],
		[CreateDate] AS [CreateDate],
		[UpdateDate] AS [UpdateDate],
		[II].[Name] AS [OfficeName]
	FROM [dbo].[Targets] AS [I]
	INNER JOIN [dbo].[Offices] AS [II] ON [II].[IsDeleted] = 0 AND [II].[OfficeId] = [I].[Office_OfficeId]
	WHERE [I].[IsDeleted] = 0;

END;
