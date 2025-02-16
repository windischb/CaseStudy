namespace CaseStudy.Application;

public record BankAccountDto(Guid Id, string IBAN, string BIC, string Name);

public record CreateBankAccountDto(string IBAN, string BIC, string Name);

public record UpdateBankAccountDto(string IBAN, string BIC, string Name);