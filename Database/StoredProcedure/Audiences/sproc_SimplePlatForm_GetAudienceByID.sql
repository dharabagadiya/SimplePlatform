SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

IF EXISTS (SELECT NULL FROM [SYS].[OBJECTS] WITH (NOLOCK) WHERE [OBJECT_ID] = OBJECT_ID(N'[dbo].[sproc_SimplePlatForm_GetAudienceByID]') AND [type] IN (N'P', N'TF'))
DROP PROCEDURE [dbo].[sproc_SimplePlatForm_GetAudienceByID];
GO

CREATE PROCEDURE [dbo].[sproc_SimplePlatForm_GetAudienceByID]
(
	@ID	INT
)
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT
		[I].AudienceID,
		[I].Name ,
		[I].VisitDate ,
		[I].Contact ,
		[I].GSBAmount ,
		[I].IsAttended ,
		[I].FSMName ,
		[I].Amount ,
		[I].[BookingStatus],
		[II].[ConventionId],
		[II].[Name] AS [ConventionName],
		[III].[EventId],
		[III].[Name] AS [EventName],
		[IV].[OfficeId],
		[IV].[Name] AS [OfficeName],
		[V].[VisitTypeId],
		[V].[VisitTypeName],
		[VII].[ServiceId],
		[VII].[ServiceName]
	FROM dbo.Audiences AS [I]
	LEFT JOIN dbo.Conventions AS  [II] ON [II].[ConventionId] = [I].[Convention_ConventionId]
	LEFT JOIN dbo.Events AS  [III] ON [III].[EventId] = [I].[Event_EventId]
	LEFT JOIN dbo.Offices AS [IV] ON IV.OfficeId = I.Office_OfficeId
	LEFT JOIN dbo.VisitTypes AS [V] ON V.VisitTypeId = I.VisitType_VisitTypeId
	LEFT JOIN dbo.[Services] AS [VII] ON [VII].ServiceId = I.Sevice_ServiceId
	WHERE [I].[IsDeleted] = 0 AND [I].[AudienceID] = @ID;

END;
