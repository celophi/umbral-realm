CREATE PROCEDURE [dbo].[Account_Insert]
    @name VARCHAR(50),
    @password VARCHAR(32)
AS
BEGIN
    MERGE [Account] WITH (SERIALIZABLE) AS T
    USING (SELECT @name AS username) AS S
    ON T.[Name] = S.[username]
    WHEN NOT MATCHED THEN
        INSERT ([Name], [Password])
        VALUES (@name, @password);

    SELECT SCOPE_IDENTITY();
END