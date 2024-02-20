CREATE PROCEDURE [usp_GetVendorById]
    @Id BIGINT
AS
BEGIN
    SELECT *
    FROM [tbl_Vendor]
    WHERE Id = @Id
END