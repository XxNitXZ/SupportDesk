using System.Linq;
using SupportDesk.Application.Interfaces;
using SupportDesk.Domain;
using SupportDesk.Domain.Tickets;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SupportDesk.Presentation.ViewModels;

public class NewTicketViewModel
{
    private readonly ITicketService _ticketService;
    private readonly ICustomerService _customerService;

    public NewTicketViewModel(
        ITicketService ticketService,
        ICustomerService customerService)
    {
        _ticketService = ticketService;
        _customerService = customerService;
    }

    public List<Customer> Customers { get; private set; } = new();

    public List<TicketPriority> Priorities { get; } =
        Enum.GetValues<TicketPriority>().ToList();

    public async Task LoadCustomersAsync()
    {
        Customers = await _customerService.GetAllAsync();
    }

    public async Task CreateTicketAsync()
    {
        if (SelectedCustomer == null)
        {
            throw new ArgumentException("Please select a customer.");
        }

        await _ticketService.AddAsync(
            Title,
            Description,
            SelectedCustomer.Id,
            SelectedPriority);
    }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public Customer? SelectedCustomer { get; set; }

    public TicketPriority SelectedPriority { get; set; } = TicketPriority.Normal;
}