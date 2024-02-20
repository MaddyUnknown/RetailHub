CREATE PROCEDURE usp_GetInventoryOrderItemByInventoryOrderId
    @InventoryOrderId BIGINT
AS
BEGIN
    SELECT * 
    FROM tbl_InventoryOrderItem 
    WHERE InventoryOrderId = @InventoryOrderId;
END