CREATE DATABASE capstone;
go
USE capstone;
go
CREATE TABLE [dbo].[Users] (
    [Id] BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [Name] NVARCHAR (200) NULL,
    [Number] NVARCHAR (200) NULL,
    [Email] NVARCHAR (200) NULL, 
    [Password] NVARCHAR (200) NULL,
    [isWorker] BIT NULL,
    [isAdmin] BIT NULL
);
go