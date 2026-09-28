using SupportDesk.Domain.Tickets;

namespace SupportDesk.Application.Interfaces
{
    public interface ITicketRepository
    {
        Task<List<SupportTicket>> GetAllAsync(); 

        Task<SupportTicket?> GetByIdAsync(Guid id);

        Task AddAsync(SupportTicket ticket);

        Task UpdateAsync(SupportTicket ticket);

    }
}
