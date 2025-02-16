using CaseStudy.Domain.Common;

namespace CaseStudy.Domain;

public class BankAccount: IEntity
{
    public Guid Id { get; set; }
    public string IBAN { get; set; } = null!;
    public string BIC { get; set; } = null!;
    public string Name { get; set; } = null!;
}