CREATE PROCEDURE [usp_DeleteVendor]
    @Id BIGINT
AS
BEGIN
    DELETE FROM [tbl_Vendor]
    WHERE Id = @Id
END