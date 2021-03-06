﻿SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

IF EXISTS (SELECT NULL FROM [SYS].[OBJECTS] WITH (NOLOCK) WHERE [OBJECT_ID] = OBJECT_ID(N'[dbo].[func_SimplePlatForm_StartOfWeek]') AND [type] IN (N'IF', N'TF', N'FN'))
DROP FUNCTION [dbo].[func_SimplePlatForm_StartOfWeek];
GO

CREATE FUNCTION [dbo].[func_SimplePlatForm_StartOfWeek]
(
    @date DATETIME
)
RETURNS DATE
AS
BEGIN
    RETURN (SELECT DATEADD(DAY, 1-DATEPART(WEEKDAY, @date), @date));
END;
