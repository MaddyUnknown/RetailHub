CREATE PROCEDURE usp_InsertProduct
    @Id BIGINT OUTPUT,
    @ProductCode NVARCHAR(50),
    @ProductName NVARCHAR(60),
    @ProductDescription NVARCHAR(500),
    @Price DECIMAL(18, 2),
    @Quantity INT,
    @CreatedBy NVARCHAR(30),
    @CreationDate DATETIME
AS
BEGIN
    INSERT INTO tbl_Product (ProductCode, ProductName, ProductDescription, Price, Quantity, CreatedBy, CreationDate)
    VALUES (@ProductCode, @ProductName, @ProductDescription, @Price, @Quantity, @CreatedBy, @CreationDate);
    
    SELECT @Id = SCOPE_IDENTITY();
END