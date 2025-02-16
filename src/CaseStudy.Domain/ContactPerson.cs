using CaseStudy.Domain.Common;

namespace CaseStudy.Domain;

public class ContactPerson: IEntity
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? Phone { get; set; }
    public string Mail { get; set; } = null!;
}