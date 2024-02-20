CREATE PROCEDURE usp_GetInventoryOrderById
    @Id BIGINT
AS
BEGIN
    SELECT * FROM tbl_InventoryOrder WHERE Id = @Id;
END