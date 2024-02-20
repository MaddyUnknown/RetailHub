CREATE PROCEDURE usp_DeleteInventoryOrder
    @Id BIGINT
AS
BEGIN
    DELETE FROM tbl_InventoryOrder WHERE Id = @Id;
END