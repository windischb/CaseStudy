namespace CaseStudy.Application;

public record ContactPersonDto(Guid Id, string FirstName, string LastName, string? Phone, string Mail);

public record CreateContactPersonDto(string FirstName, string LastName, string? Phone, string Mail);

public record UpdateContactPersonDto(string FirstName, string LastName, string? Phone, string Mail);