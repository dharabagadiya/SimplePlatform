SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

IF EXISTS (SELECT NULL FROM [SYS].[OBJECTS] WITH (NOLOCK) WHERE [OBJECT_ID] = OBJECT_ID(N'[dbo].[func_SimplePlatForm_GetParamsToList]') AND [type] IN (N'IF', N'TF', N'FN'))
DROP FUNCTION [dbo].[func_SimplePlatForm_GetParamsToList];
GO

CREATE FUNCTION [dbo].[func_SimplePlatForm_GetParamsToList]
(
	@Parameters VARCHAR(MAX)
)
RETURNS @result TABLE ([Value] INT)
AS
BEGIN
	SET @Parameters = LTRIM(RTRIM(@Parameters));
	DECLARE @DELIMITER CHAR(1) = '|';
	IF RIGHT(@Parameters, 1) <> @DELIMITER SET @Parameters = @Parameters + @DELIMITER;

	DECLARE @TempList TABLE ( [Value] VARCHAR(16) );
	DECLARE @Pos INT = CHARINDEX(@DELIMITER, @Parameters, 1);

	WHILE @Pos > 0
	BEGIN
		INSERT INTO @TempList ([Value]) VALUES (LEFT(@Parameters, @Pos - 1));
		SET @Parameters = SUBSTRING(@Parameters, @Pos + 1, 20000);
		SET @Pos = CHARINDEX(@DELIMITER, @Parameters, 1);
	END;

	WITH [Grouped] AS (SELECT LTRIM(RTRIM([Value])) AS [Value] FROM @TempList GROUP BY [Value])
	INSERT @result SELECT [Value] FROM [Grouped] WITH (NOLOCK) WHERE [Value] <> '';
	RETURN;
END;
