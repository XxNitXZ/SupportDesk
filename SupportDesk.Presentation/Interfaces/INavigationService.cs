using Microsoft.UI.Xaml.Controls;
using System;
using System.Threading.Tasks;

namespace SupportDesk.Presentation.Interfaces;

public interface INavigationService
{
    void SetFrame(Frame frame);

    void NavigateToDashboard();

    void NavigateToTickets();

    void NavigateToCustomers();

    void NavigateToNewTicket();

    void NavigateToNewCustomer();

    void NavigateToTicketDetail();
    Task NavigateToTicketDetailAsync(Guid ticketId);

    void NavigateToCustomerDetail(Guid customerId);

    void GoBack();

    bool CanGoBack { get; }
}