CREATE TABLE [dbo].[Account]
(
    [AccountId] INT NOT NULL PRIMARY KEY IDENTITY,
    [Username] VARCHAR(50) NOT NULL UNIQUE,
    [Password] VARCHAR(32) NOT NULL,
    [Pin] VARCHAR(32)
)
