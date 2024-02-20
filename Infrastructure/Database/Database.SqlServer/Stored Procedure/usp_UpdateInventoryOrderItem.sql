CREATE PROCEDURE usp_UpdateInventoryOrderItem
    @Id BIGINT,
    @InventoryOrderId BIGINT,
    @ProductId BIGINT,
    @Quantity INT,
    @UnitPrice DECIMAL(18, 2),
    @UpdatedBy NVARCHAR(30),
    @UpdateDate DATETIME
AS
BEGIN
    UPDATE tbl_InventoryOrderItem
    SET InventoryOrderId = @InventoryOrderId,
        ProductId = @ProductId,
        Quantity = @Quantity,
        UnitPrice = @UnitPrice,
        UpdatedBy = @UpdatedBy,
        UpdateDate = @UpdateDate
    WHERE Id = @Id;
END