CREATE TABLE [dbo].[tbl_Vendor]
(
	[Id] BIGINT NOT NULL IDENTITY(1,1),
    [VendorCode] NVARCHAR(50),
    [VendorName] NVARCHAR(100),
    [VendorAddress] NVARCHAR(255), 
	[CreatedBy] NVARCHAR(30),
    [UpdatedBy] NVARCHAR(30),
    [CreationDate] DATETIME,
    [UpdateDate] DATETIME,
    CONSTRAINT [PK_tbl_Vendor] PRIMARY KEY ([Id])
)
