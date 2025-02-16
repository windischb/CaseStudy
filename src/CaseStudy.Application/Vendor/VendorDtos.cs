namespace CaseStudy.Application;

public record VendorDto(
    Guid Id,
    string Name,
    string? Name2,
    string Address1, 
    string? Address2, 
    string Zip, 
    string Country, 
    string City, 
    string Mail, 
    string? Phone, 
    string? Notes);

public record CreateVendorDto(
    string Name,
    string? Name2,
    string Address1,
    string? Address2,
    string Zip,
    string Country,
    string City,
    string Mail,
    string? Phone,
    string? Notes);

public record UpdateVendorDto(
    string Name,
    string? Name2,
    string Address1,
    string? Address2,
    string Zip,
    string Country,
    string City,
    string Mail,
    string? Phone,
    string? Notes);