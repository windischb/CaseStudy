using CaseStudy.Domain.Interfaces;

namespace CaseStudy.Domain;

public class Vendor: IEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Name2 { get; set; }
    public string Address1 { get; set; } = null!;
    public string? Address2 { get; set; }
    public string Zip { get; set; } = null!;
    public string Country { get; set; } = null!;
    public string City { get; set; } = null!;
    public string Mail { get; set; } = null!;
    public string? Phone { get; set; }
    public string? Notes { get; set; }
}