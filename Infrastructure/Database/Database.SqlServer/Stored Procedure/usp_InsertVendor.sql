CREATE PROCEDURE [usp_InsertVendor]
    @Id BIGINT OUTPUT,
    @VendorCode NVARCHAR(50),
    @VendorName NVARCHAR(100),
    @VendorAddress NVARCHAR(255),
    @CreatedBy NVARCHAR(30),
    @CreationDate DATETIME
AS
BEGIN
    INSERT INTO [tbl_Vendor] (VendorCode, VendorName, VendorAddress, CreatedBy, CreationDate)
    VALUES (@VendorCode, @VendorName, @VendorAddress, @CreatedBy, @CreationDate)

    SELECT @Id = SCOPE_IDENTITY()
END