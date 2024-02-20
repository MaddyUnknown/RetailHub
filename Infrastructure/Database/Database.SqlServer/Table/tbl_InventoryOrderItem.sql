CREATE TABLE [dbo].[tbl_InventoryOrderItem]
(
	[Id] BIGINT NOT NULL IDENTITY(1,1),
    [CreationDate] DATETIME,
    [UpdateDate] DATETIME,
    [InventoryOrderId] BIGINT,
    [ProductId] BIGINT,
    [Quantity] INT,
    [UnitPrice] DECIMAL(18, 2),
	[CreatedBy] NVARCHAR(30),
    [UpdatedBy] NVARCHAR(30),
    CONSTRAINT [PK_tbl_InventoryOrderItem] PRIMARY KEY ([Id]), 
    CONSTRAINT [FK_tbl_InventoryOrderItem_tbl_Product_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [tbl_Product]([Id]), 
    CONSTRAINT [FK_tbl_InventoryOrderItem_tbl_InventoryOrder_InventoryOrderId] FOREIGN KEY ([InventoryOrderId]) REFERENCES [tbl_InventoryOrder]([Id])
)
