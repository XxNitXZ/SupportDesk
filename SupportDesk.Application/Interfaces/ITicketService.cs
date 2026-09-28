using SupportDesk.Domain.Tickets;

namespace SupportDesk.Application.Interfaces
{
    public interface ITicketService
    {
        Task<List<SupportTicket>> GetAllAsync();

        Task<SupportTicket?> GetByIdAsync(Guid id);

        Task AddAsync(string title, string description, Guid customerId, TicketPriority priority);

        Task UpdateAsync(Guid id, string title, string description);

        Task AssignTechnicianAsync(Guid id, string technician);

        Task ChangePriorityAsync(Guid id, TicketPriority priority);

        Task ChangeStatusAsync(Guid id, TicketStatus status);

        Task AddCommentAsync(Guid id, string text);

        Task<List<SupportTicket>> SearchAsync(string searchTerm);

        Task<List<SupportTicket>> FilterByStatusAsync(TicketStatus status);

        Task<List<SupportTicket>> SearchAndFilterAsync(string searchTerm, TicketStatus status);

    }
}
