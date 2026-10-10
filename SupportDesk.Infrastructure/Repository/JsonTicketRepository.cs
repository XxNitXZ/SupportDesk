using SupportDesk.Application.Interfaces;
using SupportDesk.Domain.Tickets;
using SupportDesk.Infrastructure.Data;

namespace SupportDesk.Infrastructure.Repository;

public class JsonTicketRepository : ITicketRepository
{
    private readonly JsonFileHandler _jsonFileHandler;

    public JsonTicketRepository(JsonFileHandler jsonFileHandler)
    {
        _jsonFileHandler = jsonFileHandler;
    }

    public async Task<List<SupportTicket>> GetAllAsync()
    {
        return await _jsonFileHandler.ReadAsync<SupportTicket>(
            DataPaths.TicketsFilePath);
    }

    public async Task<SupportTicket?> GetByIdAsync(Guid id)
    {
        var tickets = await GetAllAsync();

        System.Diagnostics.Debug.WriteLine($"SEARCH ID: {id}");

        foreach (var ticket in tickets)
        {
            System.Diagnostics.Debug.WriteLine($"LOADED ID: {ticket.Id}");
        }

        return tickets.FirstOrDefault(ticket => ticket.Id == id);
    }

    public async Task AddAsync(SupportTicket ticket)
    {
        var tickets = await GetAllAsync();

        tickets.Add(ticket);

        await _jsonFileHandler.WriteAsync(
            DataPaths.TicketsFilePath,
            tickets);
    }

    public async Task UpdateAsync(SupportTicket ticket)
    {
        var tickets = await GetAllAsync();

        var index = tickets.FindIndex(t => t.Id == ticket.Id);

        if (index == -1)
        {
            throw new KeyNotFoundException(
                "Ticket was not found.");
        }

        tickets[index] = ticket;

        await _jsonFileHandler.WriteAsync(
            DataPaths.TicketsFilePath,
            tickets);
    }
}