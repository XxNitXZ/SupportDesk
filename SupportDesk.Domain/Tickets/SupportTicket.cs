using System.Security.Cryptography.X509Certificates;

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

        public void AssignTechnician(string technician)
        {
            if(string.IsNullOrWhiteSpace(technician))
            {
                throw new ArgumentException("Technician cannot be empty.");
            }
            Technician = technician;
        }
        
        public void ChangeStatus(TicketStatus status)
        {
            if(status == TicketStatus.InProgress || status == TicketStatus.Resolved)
            {
                if(string.IsNullOrWhiteSpace(Technician))
                {
                    throw new ArgumentException("A technician is required for this status.");
                }
                
            }
            Status = status;
        }

        public void ChangePriority(TicketPriority priority)
        {
            Priority = priority;
        }

        public void UpdateDetails(string title, string description)
        {
            if(string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("Title cannot be empty.");
            }
            if(string.IsNullOrWhiteSpace(description))
            {
                throw new ArgumentException("Description cannot be empty.");
            }
            Title = title;
            Description = description;
        }
        
        public void AddComment(Comment comment)
        {
            if(comment == null)
            {
                throw new ArgumentException("Comment cannot be empty.");
            }
            Comments.Add(comment);
        }
    }
}
