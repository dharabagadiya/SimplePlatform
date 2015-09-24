SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

IF EXISTS (SELECT NULL FROM [SYS].[OBJECTS] WITH (NOLOCK) WHERE [OBJECT_ID] = OBJECT_ID(N'[dbo].[sproc_SimplePlatForm_GetEventByConventionID]') AND [type] IN (N'P', N'TF'))
DROP PROCEDURE [dbo].[sproc_SimplePlatForm_GetEventByConventionID];
GO

CREATE PROCEDURE [dbo].[sproc_SimplePlatForm_GetEventByConventionID]
(
	@conventionID	INT
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		[I].[EventId],
		[I].Name,
		[I].StartDate,
		[I].[EndDate],
		[I].[Description],
		[I].[City]
	FROM  dbo.Conventions AS [III]
	INNER JOIN [dbo].[Events] AS [I] ON I.IsDeleted = 0 AND [I].[convention_ConventionId] = [III].[ConventionId]
	WHERE [III].[ConventionId] = @conventionID

END;
