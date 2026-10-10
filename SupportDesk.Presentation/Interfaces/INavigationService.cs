using Microsoft.UI.Xaml.Controls;
using SupportDesk.Domain.Tickets;
using System;
using System.Threading.Tasks;

namespace SupportDesk.Presentation.Interfaces;

public interface INavigationService
{
    void SetFrame(Frame frame);

    void NavigateToDashboard();

    void NavigateToTickets();

    Task NavigateToTicketsByStatusAsync(TicketStatus status);

    void NavigateToCustomers();

    void NavigateToNewTicket();

    void NavigateToNewCustomer();

    Task NavigateToTicketDetailAsync(Guid ticketId);

    void NavigateToCustomerDetail(Guid customerId);

    void GoBack();

    bool CanGoBack { get; }
}