using CommunityToolkit.Mvvm.ComponentModel;
using SupportDesk.Application.Interfaces;
using SupportDesk.Domain.Tickets;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SupportDesk.Presentation.ViewModels;

public class TicketDetailViewModel : ObservableObject
{
    private readonly ITicketService _ticketService;
    private Guid _ticketId;

    private string _title = string.Empty;
    public string Title
    {
        get => _title;
        set => SetProperty(ref _title, value);
    }

    private string _description = string.Empty;
    public string Description
    {
        get => _description;
        set => SetProperty(ref _description, value);
    }

    private string _customerName = string.Empty;
    public string CustomerName
    {
        get => _customerName;
        set => SetProperty(ref _customerName, value);
    }

    private TicketPriority _priority;
    public TicketPriority Priority
    {
        get => _priority;
        set => SetProperty(ref _priority, value);
    }

    private TicketStatus _status;
    public TicketStatus Status
    {
        get => _status;
        set => SetProperty(ref _status, value);
    }

    private string _technician = string.Empty;
    public string Technician
    {
        get => _technician;
        set => SetProperty(ref _technician, value);
    }

    private DateTime _createdAt;
    public DateTime CreatedAt
    {
        get => _createdAt;
        set => SetProperty(ref _createdAt, value);
    }

    private List<Comment> _comments = new();
    public List<Comment> Comments
    {
        get => _comments;
        set => SetProperty(ref _comments, value);
    }

    private string _newComment = string.Empty;
    public string NewComment
    {
        get => _newComment;
        set => SetProperty(ref _newComment, value);
    }

    public TicketPriority[] Priorities => Enum.GetValues<TicketPriority>();

    public TicketStatus[] Statuses => Enum.GetValues<TicketStatus>();

    public TicketDetailViewModel(ITicketService ticketService)
    {
        _ticketService = ticketService;
    }

    public async Task LoadAsync(Guid ticketId)
    {
        var ticket = await _ticketService.GetByIdAsync(ticketId);

        if (ticket == null)
            throw new ArgumentException("Ticket was not found.");

        _ticketId = ticket.Id;

        Title = ticket.Title;
        Description = ticket.Description;
        CustomerName = ticket.Customer.Name;
        Priority = ticket.Priority;
        Status = ticket.Status;
        Technician = ticket.Technician ?? string.Empty;
        CreatedAt = ticket.CreatedAt;
        Comments = ticket.Comments;
    }

    public async Task SaveAsync()
    {
        await _ticketService.UpdateTicketAsync(
            _ticketId,
            Title,
            Description,
            Priority,
            Technician,
            Status);
    }

    public async Task AddCommentAsync()
    {
        if (string.IsNullOrWhiteSpace(NewComment))
            throw new ArgumentException("Comment cannot be empty.");

        await _ticketService.AddCommentAsync(
            _ticketId,
            NewComment);

        NewComment = string.Empty;

        await LoadAsync(_ticketId);
    }
}