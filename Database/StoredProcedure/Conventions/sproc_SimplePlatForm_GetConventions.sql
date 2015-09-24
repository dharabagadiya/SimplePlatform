SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

IF EXISTS (SELECT NULL FROM [SYS].[OBJECTS] WITH (NOLOCK) WHERE [OBJECT_ID] = OBJECT_ID(N'[dbo].[sproc_SimplePlatForm_GetConventions]') AND [type] IN (N'P', N'TF'))
DROP PROCEDURE [dbo].[sproc_SimplePlatForm_GetConventions];
GO

CREATE PROCEDURE [dbo].[sproc_SimplePlatForm_GetConventions]
AS
BEGIN
	SET NOCOUNT ON;

	WITH [ConventonsList] AS (
		SELECT
			[I].ConventionId, 
			[I].Name, 
			[I].StartDate, 
			[I].EndDate, 
			[I].[Description], 
			[I].City, 
			[II].Id AS [FileResourcesID],
			[II].[path] AS FileResourcesPath,
			[II].[name] AS FileResourcesName
		FROM dbo.Conventions AS [I]
		LEFT JOIN dbo.FileResources AS [II] ON II.Id = I.FileResource_Id
		WHERE IsDeleted =  0
	),
	[ConventionAttachmentCount] AS (
		SELECT 
			[I].ConventionId, 
			COUNT([II].[ConventionAttachmentId]) AS [IsAttachment]
		FROM [ConventonsList] AS [I]
		LEFT JOIN dbo.ConventionAttachments AS [II] ON II.Convention_ConventionId = I.ConventionId
		GROUP BY [I].ConventionId
	)
	SELECT
		[I].ConventionId, 
		[I].Name, 
		[I].StartDate, 
		[I].EndDate, 
		[I].[Description], 
		[I].City, 
		[I].[FileResourcesID],
		[I].[FileResourcesPath],
		[I].[FileResourcesName],
		[II].[IsAttachment]
	FROM  [ConventonsList] AS [I]
	INNER JOIN [ConventionAttachmentCount] AS [II] ON II.ConventionId = I.ConventionId;
END;
