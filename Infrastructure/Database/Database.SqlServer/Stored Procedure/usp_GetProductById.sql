CREATE PROCEDURE usp_GetProductById
    @Id BIGINT
AS
BEGIN
    SELECT * FROM tbl_Product WHERE Id = @Id;
END