using SupportDesk.Domain;
using SupportDesk.Domain.Tickets;

namespace SupportDesk.Tests.Domain;

public class SupportTicketTests
{
    private static Customer CreateCustomer()
    {
        return new Customer("Test Customer", "test@example.com");
    }

    private static SupportTicket CreateTicket()
    {
        return new SupportTicket(
            "Test ticket",
            "Test description",
            CreateCustomer(),
            TicketPriority.High);
    }

    [Fact]
    public void Constructor_Should_CreateTicketWhenValuesAreValid()
    {
        // Arrange
        var customer = CreateCustomer();

        // Act
        var ticket = new SupportTicket(
            "Test ticket",
            "Test description",
            customer,
            TicketPriority.High);

        // Assert
        Assert.NotEqual(Guid.Empty, ticket.Id);
        Assert.Equal("Test ticket", ticket.Title);
        Assert.Equal("Test description", ticket.Description);
        Assert.Equal(customer, ticket.Customer);
        Assert.Equal(TicketPriority.High, ticket.Priority);
        Assert.Equal(TicketStatus.New, ticket.Status);
        Assert.Null(ticket.Technician);
        Assert.Empty(ticket.Comments);
    }

    [Fact]
    public void Constructor_Should_TrimWhitespaceFromTitleAndDescription()
    {
        // Arrange & Act
        var ticket = new SupportTicket(
            "  Test ticket  ",
            "  Test description  ",
            CreateCustomer(),
            TicketPriority.High);

        // Assert
        Assert.Equal("Test ticket", ticket.Title);
        Assert.Equal("Test description", ticket.Description);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Constructor_Should_ThrowExceptionWhenTitleIsNullOrEmpty(string? title)
    {
        // Act
        var exception = Assert.Throws<ArgumentException>(() =>
            new SupportTicket(
                title!,
                "Test description",
                CreateCustomer(),
                TicketPriority.High));

        // Assert
        Assert.Equal("Title cannot be empty.", exception.Message);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Constructor_Should_ThrowExceptionWhenDescriptionIsNullOrEmpty(string? description)
    {
        // Act
        var exception = Assert.Throws<ArgumentException>(() =>
            new SupportTicket(
                "Test ticket",
                description!,
                CreateCustomer(),
                TicketPriority.High));

        // Assert
        Assert.Equal("Description cannot be empty.", exception.Message);
    }

    [Fact]
    public void Constructor_Should_ThrowExceptionWhenCustomerIsNull()
    {
        // Act
        var exception = Assert.Throws<ArgumentException>(() =>
            new SupportTicket(
                "Test ticket",
                "Test description",
                null!,
                TicketPriority.High));

        // Assert
        Assert.Equal("Customer cannot be empty.", exception.Message);
    }

    [Fact]
    public void AssignTechnician_Should_SetTechnicianWhenNameIsValid()
    {
        // Arrange
        var ticket = CreateTicket();

        // Act
        ticket.AssignTechnician("Anna Andersson");

        // Assert
        Assert.Equal("Anna Andersson", ticket.Technician);
    }

    [Fact]
    public void AssignTechnician_Should_TrimWhitespaceFromName()
    {
        // Arrange
        var ticket = CreateTicket();

        // Act
        ticket.AssignTechnician("  Anna Andersson  ");

        // Assert
        Assert.Equal("Anna Andersson", ticket.Technician);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void AssignTechnician_Should_ThrowExceptionWhenNameIsNullOrEmpty(string? technician)
    {
        // Arrange
        var ticket = CreateTicket();

        // Act
        var exception = Assert.Throws<ArgumentException>(() =>
            ticket.AssignTechnician(technician!));

        // Assert
        Assert.Equal("Technician cannot be empty.", exception.Message);
    }

    [Fact]
    public void ChangeStatus_Should_UpdateStatusWhenTechnicianIsAssigned()
    {
        // Arrange
        var ticket = CreateTicket();
        ticket.AssignTechnician("Anna Andersson");

        // Act
        ticket.ChangeStatus(TicketStatus.InProgress);

        // Assert
        Assert.Equal(TicketStatus.InProgress, ticket.Status);
    }

    [Fact]
    public void ChangeStatus_Should_ThrowExceptionWhenTechnicianIsMissing()
    {
        // Arrange
        var ticket = CreateTicket();

        // Act
        var exception = Assert.Throws<ArgumentException>(() =>
            ticket.ChangeStatus(TicketStatus.InProgress));

        // Assert
        Assert.Equal("A technician is required for this status.", exception.Message);
    }

    [Fact]
    public void ChangeStatus_Should_AllowNewStatusWithoutTechnician()
    {
        // Arrange
        var ticket = CreateTicket();

        // Act
        ticket.ChangeStatus(TicketStatus.New);

        // Assert
        Assert.Equal(TicketStatus.New, ticket.Status);
    }

    [Fact]
    public void ChangeStatus_Should_ThrowExceptionWhenStatusIsInvalid()
    {
        // Arrange
        var ticket = CreateTicket();
        var invalidStatus = (TicketStatus)999;

        // Act
        var exception = Assert.Throws<ArgumentException>(() =>
            ticket.ChangeStatus(invalidStatus));

        // Assert
        Assert.Equal("Invalid ticket status.", exception.Message);
    }

    [Fact]
    public void ChangePriority_Should_UpdatePriority()
    {
        // Arrange
        var ticket = CreateTicket();

        // Act
        ticket.ChangePriority(TicketPriority.Low);

        // Assert
        Assert.Equal(TicketPriority.Low, ticket.Priority);
    }

    [Fact]
    public void UpdateDetails_Should_UpdateTitleAndDescription()
    {
        // Arrange
        var ticket = CreateTicket();

        // Act
        ticket.UpdateDetails("Updated title", "Updated description");

        // Assert
        Assert.Equal("Updated title", ticket.Title);
        Assert.Equal("Updated description", ticket.Description);
    }

    [Fact]
    public void UpdateDetails_Should_TrimWhitespaceFromValues()
    {
        // Arrange
        var ticket = CreateTicket();

        // Act
        ticket.UpdateDetails("  Updated title  ", "  Updated description  ");

        // Assert
        Assert.Equal("Updated title", ticket.Title);
        Assert.Equal("Updated description", ticket.Description);
    }

    [Theory]
    [InlineData("", "Valid description")]
    [InlineData("Valid title", "")]
    [InlineData(" ", "Valid description")]
    [InlineData("Valid title", " ")]
    public void UpdateDetails_Should_ThrowExceptionWhenTitleOrDescriptionIsEmpty(
        string title,
        string description)
    {
        // Arrange
        var ticket = CreateTicket();

        // Act
        var exception = Assert.Throws<ArgumentException>(() =>
            ticket.UpdateDetails(title, description));

        // Assert
        Assert.Contains("cannot be empty.", exception.Message);
    }

    [Fact]
    public void AddComment_Should_AddCommentToTicket()
    {
        // Arrange
        var ticket = CreateTicket();
        var comment = new Comment("Test comment");

        // Act
        ticket.AddComment(comment);

        // Assert
        Assert.Single(ticket.Comments);
        Assert.Equal(comment, ticket.Comments[0]);
    }

    [Fact]
    public void AddComment_Should_ThrowExceptionWhenCommentIsNull()
    {
        // Arrange
        var ticket = CreateTicket();

        // Act
        var exception = Assert.Throws<ArgumentException>(() =>
            ticket.AddComment(null!));

        // Assert
        Assert.Equal("Comment cannot be empty.", exception.Message);
    }

    [Fact]
    public void GetComments_Should_ReturnAllAddedComments()
    {
        // Arrange
        var ticket = CreateTicket();
        var firstComment = new Comment("First comment");
        var secondComment = new Comment("Second comment");

        ticket.AddComment(firstComment);
        ticket.AddComment(secondComment);

        // Act
        var comments = ticket.GetComments();

        // Assert
        Assert.Equal(2, comments.Count);
        Assert.Contains(firstComment, comments);
        Assert.Contains(secondComment, comments);
    }
}