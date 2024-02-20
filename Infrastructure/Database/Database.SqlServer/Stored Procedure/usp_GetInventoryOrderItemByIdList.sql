CREATE PROCEDURE usp_GetInventoryOrderItemByIdList
    @IdList udt_LongList READONLY
AS
BEGIN
    SELECT * 
    FROM tbl_InventoryOrderItem 
    WHERE Id IN (SELECT Value FROM @IdList);
END