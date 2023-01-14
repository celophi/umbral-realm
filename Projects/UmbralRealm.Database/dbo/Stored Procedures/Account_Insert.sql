CREATE PROCEDURE [dbo].[Account_Insert]
    @username VARCHAR(50),
    @password VARCHAR(32)
AS
BEGIN
    MERGE [Account] WITH (SERIALIZABLE) AS T
    USING (SELECT @username AS username) AS S
    ON T.[Username] = S.[username]
    WHEN NOT MATCHED THEN
        INSERT ([Username], [Password])
        VALUES (@username, @password);

    SELECT SCOPE_IDENTITY();
END
