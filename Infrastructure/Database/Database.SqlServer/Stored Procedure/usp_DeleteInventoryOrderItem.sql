CREATE PROCEDURE usp_DeleteInventoryOrderItem
    @Id BIGINT
AS
BEGIN
    DELETE FROM tbl_InventoryOrderItem WHERE Id = @Id;
END