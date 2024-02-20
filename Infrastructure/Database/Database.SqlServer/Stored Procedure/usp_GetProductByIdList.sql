CREATE PROCEDURE usp_GetProductByIdList
    @IdList udt_LongList READONLY
AS
BEGIN
    SELECT * 
    FROM tbl_Product 
    WHERE Id IN (SELECT Value FROM @IdList);
END