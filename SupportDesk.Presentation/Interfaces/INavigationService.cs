using Microsoft.UI.Xaml.Controls;

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

    void GoBack();

    bool CanGoBack { get; }
}