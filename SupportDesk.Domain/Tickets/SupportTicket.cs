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
        public DateTime CreatedAt { get; private set; }


        public SupportTicket(string title, string description, Customer customer, TicketPriority priority)
        {
            if(string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("Title cannot be empty.");
            }
            if(string.IsNullOrWhiteSpace(description))
            {
                throw new ArgumentException("Description cannot be empty.");
            }
            if(customer == null)
            {
                throw new ArgumentException("Customer cannot be empty.");
            }

            Title = title;
            Description = description;
            Customer = customer;
            Priority = priority;
            Id = Guid.NewGuid();
            Status = TicketStatus.New;
            Comments = new List<Comment>();
            CreatedAt = DateTime.Now; 
            
        }

    }
}
