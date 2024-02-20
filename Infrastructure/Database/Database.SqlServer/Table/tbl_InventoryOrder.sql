CREATE TABLE [dbo].[tbl_InventoryOrder]
(
	[Id] BIGINT NOT NULL  IDENTITY(1,1),
    [InventoryOrderNumber] NVARCHAR(50),
    [ExternalOrderReferenceNumber] NVARCHAR(MAX),
    [VendorId] BIGINT,
    [SubTotal] DECIMAL(18, 2),
    [Discount] DECIMAL(18, 2),
    [ShippingFee] DECIMAL(18, 2),
    [TotalCost] DECIMAL(18, 2),
    [InventoryOrderStatus] INT,
	[CreatedBy] NVARCHAR(30),
    [UpdatedBy] NVARCHAR(30),
    [CreationDate] DATETIME,
    [UpdateDate] DATETIME, 
    CONSTRAINT [PK_tbl_InventoryOrder] PRIMARY KEY ([Id]), 
    CONSTRAINT [FK_tbl_InventoryOrder_tbl_Vendor_VendorId] FOREIGN KEY ([VendorId]) REFERENCES [tbl_Vendor]([Id]) 
)
