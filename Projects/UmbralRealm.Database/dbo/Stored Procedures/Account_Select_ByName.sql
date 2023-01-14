CREATE PROCEDURE [dbo].[Account_Select_ByName]
	@username VARCHAR(50)
AS
BEGIN
    SELECT
        [AccountId]
        , [Username]
        , [Password]
        , [Pin]
    FROM [dbo].[Account]
    WHERE
        [Username] = @username;
END
