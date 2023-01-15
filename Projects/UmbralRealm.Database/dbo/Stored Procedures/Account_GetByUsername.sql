CREATE PROCEDURE [dbo].[Account_GetByUsername]
	@username VARCHAR(50)
AS
BEGIN
    SELECT
        [AccountId]
        , [Username]
        , [Password]
        , [Pin]
        , [Standing]
    FROM [dbo].[Account]
    WHERE
        [Username] = @username;
END
