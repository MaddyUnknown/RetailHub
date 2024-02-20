CREATE PROCEDURE usp_InsertInventoryOrder
    @Id BIGINT OUTPUT,
    @InventoryOrderNumber NVARCHAR(50),
    @ExternalOrderReferenceNumber NVARCHAR(MAX),
    @VendorId BIGINT,
    @SubTotal DECIMAL(18, 2),
    @Discount DECIMAL(18, 2),
    @ShippingFee DECIMAL(18, 2),
    @TotalCost DECIMAL(18, 2),
    @InventoryOrderStatus INT,
    @CreatedBy NVARCHAR(30),
    @CreationDate DATETIME
AS
BEGIN
    INSERT INTO tbl_InventoryOrder (InventoryOrderNumber, ExternalOrderReferenceNumber, VendorId, SubTotal, Discount, ShippingFee, TotalCost, InventoryOrderStatus, CreatedBy, CreationDate)
    VALUES (@InventoryOrderNumber, @ExternalOrderReferenceNumber, @VendorId, @SubTotal, @Discount, @ShippingFee, @TotalCost, @InventoryOrderStatus, @CreatedBy, @CreationDate);
    
    SELECT @Id = SCOPE_IDENTITY();
END