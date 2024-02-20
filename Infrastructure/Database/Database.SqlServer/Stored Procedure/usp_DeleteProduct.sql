CREATE PROCEDURE usp_DeleteProduct
    @Id BIGINT
AS
BEGIN
    DELETE FROM tbl_Product WHERE Id = @Id;
END