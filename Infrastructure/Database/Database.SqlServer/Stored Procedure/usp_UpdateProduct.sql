CREATE PROCEDURE usp_UpdateProduct
    @Id BIGINT,
    @ProductCode NVARCHAR(50),
    @ProductName NVARCHAR(60),
    @ProductDescription NVARCHAR(500),
    @Price DECIMAL(18, 2),
    @Quantity INT,
    @UpdatedBy NVARCHAR(30),
    @UpdateDate DATETIME
AS
BEGIN
    UPDATE tbl_Product
    SET ProductCode = @ProductCode,
        ProductName = @ProductName,
        ProductDescription = @ProductDescription,
        Price = @Price,
        Quantity = @Quantity,
        UpdatedBy = @UpdatedBy,
        UpdateDate = @UpdateDate
    WHERE Id = @Id;
END