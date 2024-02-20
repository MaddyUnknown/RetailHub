CREATE TABLE [dbo].[tbl_Product]
(
	[Id] BIGINT NOT NULL  IDENTITY(1,1),
    [ProductCode] NVARCHAR(50),
    [ProductName] NVARCHAR(60),
    [ProductDescription] NVARCHAR(500),
    [Price] DECIMAL(18, 2),
    [Quantity] INT,
	[CreatedBy] NVARCHAR(30),
    [UpdatedBy] NVARCHAR(30),
    [CreationDate] DATETIME,
    [UpdateDate] DATETIME, 
    CONSTRAINT [PK_tbl_Product] PRIMARY KEY ([Id])
)
