CREATE PROCEDURE usp_UpdateInventoryOrder
    @Id BIGINT,
    @InventoryOrderNumber NVARCHAR(50),
    @ExternalOrderReferenceNumber NVARCHAR(MAX),
    @VendorId BIGINT,
    @SubTotal DECIMAL(18, 2),
    @Discount DECIMAL(18, 2),
    @ShippingFee DECIMAL(18, 2),
    @TotalCost DECIMAL(18, 2),
    @InventoryOrderStatus INT,
    @UpdatedBy NVARCHAR(30),
    @UpdateDate DATETIME
AS
BEGIN
    UPDATE tbl_InventoryOrder
    SET InventoryOrderNumber = @InventoryOrderNumber,
        ExternalOrderReferenceNumber = @ExternalOrderReferenceNumber,
        VendorId = @VendorId,
        SubTotal = @SubTotal,
        Discount = @Discount,
        ShippingFee = @ShippingFee,
        TotalCost = @TotalCost,
        InventoryOrderStatus = @InventoryOrderStatus,
        UpdatedBy = @UpdatedBy,
        UpdateDate = @UpdateDate
    WHERE Id = @Id;
END