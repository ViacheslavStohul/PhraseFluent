IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
CREATE TABLE [__EFMigrationsHistory]
(
    [MigrationId]    nvarchar(150) NOT NULL,
    [ProductVersion] nvarchar(32)  NOT NULL,
    CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

IF OBJECT_ID(N'[ReleaseHistory]') IS NULL
BEGIN
CREATE TABLE [ReleaseHistory]
(
    [ReleaseId]      nvarchar(36) NOT NULL,
    [ProductVersion] nvarchar(32) NOT NULL,
    CONSTRAINT [PK___ReleaseHistory] PRIMARY KEY ([ReleaseId])
    );
END;

DECLARE @ReleaseIdentifier uniqueidentifier;
SET @ReleaseIdentifier = '7f16b1b5-f1f4-41ff-aacd-c8dd957eea2c'

    IF NOT EXISTS(SELECT 1
              FROM ReleaseHistory
              WHERE ReleaseId = @ReleaseIdentifier)
BEGIN

CREATE TABLE [Users]
(
    [Id]               bigint           NOT NULL IDENTITY,
    [Username]         nvarchar(255)    NOT NULL,
    [ClientSecret]     nvarchar(255)    NOT NULL,
    [RegistrationDate] datetime2        NOT NULL,
    [Uuid]             uniqueidentifier NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY ([Id])
    );

CREATE TABLE [UserSessions]
(
    [Id]                     bigint           NOT NULL IDENTITY,
    [UserId]                 bigint           NOT NULL,
    [RefreshToken]           nvarchar(250)    NOT NULL,
    [JwtId]                  nvarchar(36)     NOT NULL,
    [RefreshTokenExpiration] datetime2        NOT NULL,
    [Redeemed]               bit              NOT NULL,
    [Uuid]                   uniqueidentifier NOT NULL,
    CONSTRAINT [PK_UserSessions] PRIMARY KEY ([Id])
    );

CREATE UNIQUE INDEX [IX_Users_Username] ON [Users] ([Username]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240308193845_UserAndUserSession', N'8.0.2');

INSERT INTO [ReleaseHistory] ([ReleaseId], [ProductVersion])
VALUES (@ReleaseIdentifier, '0.0.0.1');
END