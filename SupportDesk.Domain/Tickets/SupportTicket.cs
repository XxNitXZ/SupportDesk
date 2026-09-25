namespace SupportDesk.Domain.Tickets
{
    public class SupportTicket
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public Customer Customer { get; private set; }
        public TicketPriority Priority { get; private set; }
        public TicketStatus Status { get; private set; }  
        public string? Technician { get; private set; }
        public List<Comment> Comments { get; private set; }


        public SupportTicket(string title, string description, Customer customer, TicketPriority priority)
        {
            Title = title;
            Description = description;
            Customer = customer;
            Priority = priority;
            Id = Guid.NewGuid();
            Status = TicketStatus.New;
            Comments = new List<Comment>();
        }
    }
}
