CREATE PROCEDURE usp_GetInventoryOrderItemById
    @Id BIGINT
AS
BEGIN
    SELECT * FROM tbl_InventoryOrderItem WHERE Id = @Id;
END