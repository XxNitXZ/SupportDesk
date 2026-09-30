using System.Text.Json;
using System.Reflection;
using SupportDesk.Application.Interfaces;
using SupportDesk.Domain;
using SupportDesk.Domain.Tickets;

namespace SupportDesk.Infrastructure.Repository;

public class JsonTicketRepository : ITicketRepository
{
    private readonly string _filePath;
    private readonly JsonSerializerOptions _options;

    public JsonTicketRepository(string filePath = "Data/tickets.json")
    {
        _filePath = filePath;

        _options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        var directory = Path.GetDirectoryName(_filePath);

        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        if (!File.Exists(_filePath))
        {
            File.WriteAllText(_filePath, "[]");
        }
    }

    public async Task<List<SupportTicket>> GetAllAsync()
    {
        return await ReadTicketsAsync();
    }

    public async Task<SupportTicket?> GetByIdAsync(Guid id)
    {
        var tickets = await ReadTicketsAsync();

        return tickets.FirstOrDefault(ticket => ticket.Id == id);
    }

    public async Task AddAsync(SupportTicket ticket)
    {
        var tickets = await ReadTicketsAsync();

        tickets.Add(ticket);

        await WriteTicketsAsync(tickets);
    }

    public async Task UpdateAsync(SupportTicket ticket)
    {
        var tickets = await ReadTicketsAsync();

        var index = tickets.FindIndex(t => t.Id == ticket.Id);

        if (index == -1)
        {
            throw new KeyNotFoundException(
                $"Ticket with id {ticket.Id} was not found.");
        }

        tickets[index] = ticket;

        await WriteTicketsAsync(tickets);
    }

    private async Task<List<SupportTicket>> ReadTicketsAsync()
    {
        var json = await File.ReadAllTextAsync(_filePath);

        if (string.IsNullOrWhiteSpace(json))
        {
            return new List<SupportTicket>();
        }

        var data = JsonSerializer.Deserialize<List<TicketDto>>(
            json,
            _options);

        if (data == null)
        {
            return new List<SupportTicket>();
        }

        return data.Select(FromDto).ToList();
    }

    private async Task WriteTicketsAsync(List<SupportTicket> tickets)
    {
        var data = tickets
            .Select(ToDto)
            .ToList();

        var json = JsonSerializer.Serialize(data, _options);

        await File.WriteAllTextAsync(_filePath, json);
    }

    private static TicketDto ToDto(SupportTicket ticket)
    {
        return new TicketDto
        {
            Id = ticket.Id,
            Title = ticket.Title,
            Description = ticket.Description,

            Customer = new CustomerDto
            {
                Id = ticket.Customer.Id,
                Name = ticket.Customer.Name,
                Email = ticket.Customer.Email
            },

            Priority = ticket.Priority,
            Status = ticket.Status,
            Technician = ticket.Technician,
            CreatedAt = ticket.CreatedAt,

            Comments = ticket.GetComments()
                .Select(comment => new CommentDto
                {
                    Text = comment.Text,
                    CreatedAt = comment.CreatedAt
                })
                .ToList()
        };
    }

    private static SupportTicket FromDto(TicketDto dto)
    {
        var customer = new Customer(
            dto.Customer.Name,
            dto.Customer.Email);

        // Customer får normalt ett nytt Guid i konstruktorn.
        // Här återställer vi det ursprungliga Id:t.
        SetPrivateProperty(
            customer,
            nameof(Customer.Id),
            dto.Customer.Id);

        var ticket = new SupportTicket(
            dto.Title,
            dto.Description,
            customer,
            dto.Priority);

        // Återställ ticketens ursprungliga Id.
        SetPrivateProperty(
            ticket,
            nameof(SupportTicket.Id),
            dto.Id);

        // Återställ tekniker om det finns någon.
        if (!string.IsNullOrWhiteSpace(dto.Technician))
        {
            ticket.AssignTechnician(dto.Technician);
        }

        // Återställ status.
        ticket.ChangeStatus(dto.Status);

        // Återställ skapelsedatum.
        SetPrivateProperty(
            ticket,
            nameof(SupportTicket.CreatedAt),
            dto.CreatedAt);

        // Återställ kommentarer.
        foreach (var commentDto in dto.Comments)
        {
            var comment = new Comment(commentDto.Text);

            SetPrivateProperty(
                comment,
                nameof(Comment.CreatedAt),
                commentDto.CreatedAt);

            ticket.AddComment(comment);
        }

        return ticket;
    }

    private static void SetPrivateProperty(
        object target,
        string propertyName,
        object value)
    {
        var property = target.GetType().GetProperty(
            propertyName,
            BindingFlags.Instance |
            BindingFlags.Public |
            BindingFlags.NonPublic);

        var setter = property?.GetSetMethod(nonPublic: true);

        if (setter == null)
        {
            throw new InvalidOperationException(
                $"Could not set property '{propertyName}'.");
        }

        setter.Invoke(target, new[] { value });
    }

    private class TicketDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = "";

        public string Description { get; set; } = "";

        public CustomerDto Customer { get; set; } = new();

        public TicketPriority Priority { get; set; }

        public TicketStatus Status { get; set; }

        public string? Technician { get; set; }

        public DateTime CreatedAt { get; set; }

        public List<CommentDto> Comments { get; set; } = new();
    }

    private class CustomerDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = "";

        public string Email { get; set; } = "";
    }

    private class CommentDto
    {
        public string Text { get; set; } = "";

        public DateTime CreatedAt { get; set; }
    }
}
