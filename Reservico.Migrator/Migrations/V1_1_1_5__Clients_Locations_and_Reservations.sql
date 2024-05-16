BEGIN TRANSACTION;

CREATE TABLE [Locations] (
    [Id] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWSEQUENTIALID(),
    [Name] nvarchar(256) NOT NULL,
    [Address] nvarchar(256) NOT NULL,
    [City] nvarchar(256) NOT NULL,
    [Postcode] nvarchar(256) NULL,
    [Country] nvarchar(256) NULL,
    [CreatedOn] datetime2 NOT NULL,
    [UpdatedOn] datetime2 NOT NULL,
    [IsDeleted] bit NOT NULL DEFAULT 0,
    [ClientId] UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_Locations] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Location_ClientId] FOREIGN KEY ([ClientId]) REFERENCES [Clients] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [Tables] (
    [Id] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWSEQUENTIALID(),
    [Name] nvarchar(256) NOT NULL,  
    [Capacity] int NOT NULL,
    [Description] nvarchar(256) NULL,
    [CreatedOn] datetime2 NOT NULL,
    [UpdatedOn] datetime2 NOT NULL,
    [IsDeleted] bit NOT NULL DEFAULT 0,
    [LocationId] UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_Tables] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Table_LocationId] FOREIGN KEY ([LocationId]) REFERENCES [Locations] ([Id])
);

CREATE TABLE [Reservations] (
    [Id] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWSEQUENTIALID(),
    [FirstName] nvarchar(256) NOT NULL,  
    [LastName] nvarchar(256) NOT NULL,
    [Note] nvarchar(max) NULL,
    [GuestsArrivingAt] datetime2 NOT NULL, 
    [NumberOfGuests] int NOT NULL,
    [CreatedOn] datetime2 NOT NULL,
    [UpdatedOn] datetime2 NOT NULL,
    [IsDeleted] bit NOT NULL DEFAULT 0,
    [LocationId] UNIQUEIDENTIFIER NOT NULL,
    [TableId] UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_Reservations] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Reservation_LocationId] FOREIGN KEY ([LocationId]) REFERENCES [Locations] ([Id]),
    CONSTRAINT [FK_Reservation_TableId] FOREIGN KEY ([TableId]) REFERENCES [Tables] ([Id])
);
COMMIT;