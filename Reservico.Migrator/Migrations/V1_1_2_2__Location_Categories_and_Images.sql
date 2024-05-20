BEGIN TRANSACTION;

CREATE TABLE [Categories] (
    [Id] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWSEQUENTIALID(),
    [Name] nvarchar(256) NOT NULL,
    [CreatedOn] datetime2 NOT NULL,
    [UpdatedOn] datetime2 NOT NULL,
    [IsDeleted] bit NOT NULL DEFAULT 0
    CONSTRAINT [PK_Categories] PRIMARY KEY ([Id])
);

CREATE TABLE [LocationCategories] (
    [LocationId] UNIQUEIDENTIFIER NOT NULL,
    [CategoryId] UNIQUEIDENTIFIER NOT NULL,      
    [CreatedOn] datetime2 NOT NULL,
    [UpdatedOn] datetime2 NOT NULL,
    [IsDeleted] bit NOT NULL DEFAULT 0
    CONSTRAINT [PK_LocationCategories] PRIMARY KEY ([LocationId], [CategoryId]),
    CONSTRAINT [FK_LocationCategories_Locations_LocationId] FOREIGN KEY ([LocationId]) REFERENCES [Locations] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_LocationCategories_Categories_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [Categories] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [LocationImages] (
    [Id] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWSEQUENTIALID(),
    [LocationId] UNIQUEIDENTIFIER NOT NULL,     
    [BlobPath] nvarchar(max) NOT NULL,
    [BlobName] nvarchar(max) NOT NULL,
    [CreatedOn] datetime2 NOT NULL,
    [UpdatedOn] datetime2 NOT NULL,
    [IsDeleted] bit NOT NULL DEFAULT 0
    CONSTRAINT [PK_LocationImages] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_LocationImages_Locations_LocationId] FOREIGN KEY ([LocationId]) REFERENCES [Locations] ([Id])
);

COMMIT;