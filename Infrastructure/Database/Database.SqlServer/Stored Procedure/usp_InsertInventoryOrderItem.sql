CREATE PROCEDURE usp_InsertInventoryOrderItem
    @Id BIGINT OUTPUT,
    @InventoryOrderId BIGINT,
    @ProductId BIGINT,
    @Quantity INT,
    @UnitPrice DECIMAL(18, 2),
    @CreatedBy NVARCHAR(30),
    @CreationDate DATETIME
AS
BEGIN
    INSERT INTO tbl_InventoryOrderItem (InventoryOrderId, ProductId, Quantity, UnitPrice, CreatedBy, CreationDate)
    VALUES (@InventoryOrderId, @ProductId, @Quantity, @UnitPrice, @CreatedBy, @CreationDate);
    
    SELECT @Id = SCOPE_IDENTITY();
END