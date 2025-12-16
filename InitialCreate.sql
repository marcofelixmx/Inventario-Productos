
USE Redarbor; 
GO


CREATE TABLE [Category] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Category] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [InventoryMovement] (
    [Id] int NOT NULL IDENTITY,
    [ProductId] int NOT NULL,
    [Quantity] int NOT NULL,
    [MovementDate] datetime2 NOT NULL,
    CONSTRAINT [PK_InventoryMovement] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Product] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [Description] nvarchar(max) NULL,
    [Price] decimal(18,2) NOT NULL,
    [Stock] int NOT NULL,
    [CategoryId] int NOT NULL,
    [Status] bit NOT NULL,
    CONSTRAINT [PK_Product] PRIMARY KEY ([Id])
);
GO



