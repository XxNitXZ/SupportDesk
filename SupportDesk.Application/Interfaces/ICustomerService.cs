using SupportDesk.Domain;

namespace SupportDesk.Application.Interfaces;

public interface ICustomerService
{
    Task<List<Customer>> GetAllAsync();

    Task<Customer?> GetByIdAsync(Guid id);

    Task AddAsync(string name, string email);

    Task UpdateAsync(Guid id, string name, string email);
}
