using CaseStudy.Application.Common;
using ErrorOr;

namespace CaseStudy.Application;

public class BankAccountService(IBankAccountRepository repository): IBankAccountService
{
    public async Task<IEnumerable<BankAccountDto>> GetAllAsync()
    {
        var bankAccounts = await repository.GetAllAsync();
        return bankAccounts.Select(BankAccountMapper.ToDto);
    }

    public async Task<BankAccountDto?> GetByIdAsync(Guid id)
    {
        var bankAccount = await repository.GetByIdAsync(id);
        return bankAccount is null ? null : BankAccountMapper.ToDto(bankAccount);
    }

    public async Task<Guid> AddAsync(CreateBankAccountDto dto)
    {
        var newBankAccount = BankAccountMapper.ToEntity(dto);
        await repository.AddAsync(newBankAccount);
        return newBankAccount.Id;
    }

    public async Task<ErrorOr<Guid>> UpdateAsync(Guid id, UpdateBankAccountDto dto)
    {
        var bankAccount = await repository.GetByIdAsync(id);
        if (bankAccount is null)
        {
            return Error.NotFound("BankAccount.NotFound");
        }

        BankAccountMapper.Map(dto, bankAccount);
        await repository.UpdateAsync(bankAccount);
        return bankAccount.Id;
    }

    public Task DeleteAsync(Guid id)
    {
        return repository.DeleteAsync(id);
    }
}