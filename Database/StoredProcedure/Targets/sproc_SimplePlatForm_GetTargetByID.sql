SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

IF EXISTS (SELECT NULL FROM [SYS].[OBJECTS] WITH (NOLOCK) WHERE [OBJECT_ID] = OBJECT_ID(N'[dbo].[sproc_SimplePlatForm_GetTargetByID]') AND [type] IN (N'P', N'TF'))
DROP PROCEDURE [dbo].[sproc_SimplePlatForm_GetTargetByID];
GO

CREATE PROCEDURE [dbo].[sproc_SimplePlatForm_GetTargetByID]
(
	@ID				INT
)
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
		[UpdateDate] AS [UpdateDate]
	FROM [dbo].[Targets]
	WHERE TargetId = @ID AND IsDeleted = 0;

END;
