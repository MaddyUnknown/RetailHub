CREATE PROCEDURE [usp_UpdateVendor]
    @Id BIGINT,
    @VendorCode NVARCHAR(50),
    @VendorName NVARCHAR(100),
    @VendorAddress NVARCHAR(255),
    @UpdatedBy NVARCHAR(30),
    @UpdateDate DATETIME
AS
BEGIN
    UPDATE [tbl_Vendor]
    SET 
        VendorCode = @VendorCode,
        VendorName = @VendorName,
        VendorAddress = @VendorAddress,
        UpdatedBy = @UpdatedBy,
        UpdateDate = @UpdateDate
    WHERE Id = @Id
END