using Microsoft.UI.Xaml.Controls;

namespace SupportDesk.Presentation.Interfaces;

public interface INavigationService
{
    void SetFrame(Frame frame);

    void NavigateToDashboard();

    void NavigateToTickets();

    void NavigateToCustomers();

    void NavigateToTicketDetail();

    void GoBack();

    bool CanGoBack { get; }
}