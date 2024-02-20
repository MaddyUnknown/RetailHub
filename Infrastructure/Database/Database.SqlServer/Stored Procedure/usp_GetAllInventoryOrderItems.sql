CREATE PROCEDURE usp_GetAllInventoryOrderItems
AS
BEGIN
    SELECT * FROM tbl_InventoryOrderItem;
END