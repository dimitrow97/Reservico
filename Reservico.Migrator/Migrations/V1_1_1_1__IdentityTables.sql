BEGIN TRANSACTION;

CREATE TABLE [Roles] (
    [Id] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWSEQUENTIALID(),
    [Name] nvarchar(256) NULL,
    [NormalizedName] nvarchar(256) NULL,
    [CreatedOn] datetime2 NOT NULL,
    [UpdatedOn] datetime2 NOT NULL,
    [IsDeleted] bit NOT NULL DEFAULT 0
    CONSTRAINT [PK_Roles] PRIMARY KEY ([Id])
);

CREATE TABLE [Users] (
    [Id] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWSEQUENTIALID(),
    [Email] nvarchar(256) NULL,
    [EmailConfirmed] bit NOT NULL,
    [PasswordHash] nvarchar(max) NULL,
    [SecurityStamp] nvarchar(max) NULL,
    [PhoneNumber] nvarchar(max) NULL,
    [PhoneNumberConfirmed] bit NOT NULL,
    [FirstName] nvarchar(256) NOT NULL,
    [LastName] nvarchar(256) NOT NULL,
    [IsActive] bit NOT NULL DEFAULT 1,
    [IsUsingDefaultPassword] bit NULL,
    [CreatedOn] datetime2 NOT NULL,
    [UpdatedOn] datetime2 NOT NULL,
    [IsDeleted] bit NOT NULL DEFAULT 0
    CONSTRAINT [PK_Users] PRIMARY KEY ([Id])
);

CREATE TABLE [UserRoles] (
    [UserId] UNIQUEIDENTIFIER NOT NULL,
    [RoleId] UNIQUEIDENTIFIER NOT NULL,    
    [CreatedOn] datetime2 NOT NULL,
    [UpdatedOn] datetime2 NOT NULL,
    [IsDeleted] bit NOT NULL DEFAULT 0
    CONSTRAINT [PK_UserRoles] PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK_UserRoles_Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Roles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_UserRoles_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [Clients] (
    [Id] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWSEQUENTIALID(),
    [Name] nvarchar(256) NULL,
    [Address] nvarchar(256) NULL,
    [City] nvarchar(256) NULL,
    [Postcode] nvarchar(256) NULL,
    [Country] nvarchar(256) NULL,
    [CreatedOn] datetime2 NOT NULL,
    [UpdatedOn] datetime2 NOT NULL,
    [IsDeleted] bit NOT NULL DEFAULT 0
    CONSTRAINT [PK_Clients] PRIMARY KEY ([Id])
);

CREATE TABLE [UserClients] (
    [UserId] UNIQUEIDENTIFIER NOT NULL,
    [ClientId] UNIQUEIDENTIFIER NOT NULL,  
    [IsSelected] bit NOT NULL DEFAULT 0,
    [CreatedOn] datetime2 NOT NULL,
    [UpdatedOn] datetime2 NOT NULL,
    [IsDeleted] bit NOT NULL DEFAULT 0
    CONSTRAINT [PK_UserClients] PRIMARY KEY ([UserId], [ClientId]),
    CONSTRAINT [FK_UserClients_Clients_ClientId] FOREIGN KEY ([ClientId]) REFERENCES [Clients] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_UserClients_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [IdentityTokens] (
    [Id] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWSEQUENTIALID(),
    [AccessToken] nvarchar(450) NOT NULL,
    [RefreshToken] nvarchar(450) NOT NULL,
    [AccessTokenExpirationDate] datetime2 NOT NULL,
    [RefreshTokenExpirationDate] datetime2 NOT NULL,
    [CreatedOn] datetime2 NOT NULL,
    [UpdatedOn] datetime2 NOT NULL,
    [IsDeleted] bit NOT NULL DEFAULT 0
);

CREATE TABLE [IdentityAuthorizationCodes] (
    [Id] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWSEQUENTIALID(),
    [Code] nvarchar(456) NOT NULL,
    [IsUsed] bit NOT NULL,
    [ExpirationDate] datetime2 NOT NULL,
    [CreatedOn] datetime2 NOT NULL,
    [UpdatedOn] datetime2 NOT NULL,
    [IsDeleted] bit NOT NULL DEFAULT 0
);

CREATE UNIQUE INDEX [RoleNameIndex] ON [Roles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL;

CREATE INDEX [EmailIndex] ON [Users] ([Email]);
CREATE INDEX [NameIndex] ON [Clients] ([Name]);

COMMIT;