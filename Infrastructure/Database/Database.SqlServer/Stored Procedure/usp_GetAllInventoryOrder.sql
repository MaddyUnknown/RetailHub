CREATE PROCEDURE usp_GetAllInventoryOrder
AS
BEGIN
    SELECT * FROM tbl_InventoryOrder;
END