CREATE PROCEDURE [dbo].[Account_Select_ByName]
	@name VARCHAR(50)
AS
BEGIN
    SELECT
        [AccountId]
        , [Name]
        , [Password]
        , [Pin]
    FROM [dbo].[Account]
    WHERE
        [Name] = @name;
END
