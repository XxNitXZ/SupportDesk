using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using SupportDesk.Presentation.Interfaces;
using SupportDesk.Presentation.Views;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SupportDesk.Presentation.Services;

public class NavigationService : INavigationService
{
    private readonly IServiceProvider _serviceProvider;

    private Frame? _frame;

    private readonly Stack<Page> _history = new();

    public NavigationService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public bool CanGoBack
    {
        get
        {
            return _history.Count > 0;
        }
    }

    public void SetFrame(Frame frame)
    {
        _frame = frame;
    }

    public void NavigateToDashboard()
    {
        var page =
            _serviceProvider.GetRequiredService<DashboardPage>();

        Navigate(page);
    }

    public void NavigateToTickets()
    {
        var page =
            _serviceProvider.GetRequiredService<TicketsPage>();

        Navigate(page);
    }

    public void NavigateToCustomers()
    {
        var page =
            _serviceProvider.GetRequiredService<CustomersPage>();

        Navigate(page);
    }

    public void NavigateToNewTicket()
    {
        var page =
            _serviceProvider.GetRequiredService<NewTicketPage>();

        Navigate(page);
    }

    public void NavigateToNewCustomer()
    {
        var page =
            _serviceProvider.GetRequiredService<NewCustomerPage>();

        Navigate(page);
    }

    public void NavigateToTicketDetail()
    {
        var page =
            _serviceProvider.GetRequiredService<TicketDetailPage>();

        Navigate(page);
    }

    public async Task NavigateToTicketDetailAsync(Guid ticketId)
    {
        if (_frame == null)
            return;

        var page = _serviceProvider.GetRequiredService<TicketDetailPage>();

        await page.LoadTicketAsync(ticketId);

        Navigate(page);
    }

    public void NavigateToCustomerDetail(Guid customerId)
    {
        var page =
            _serviceProvider.GetRequiredService<CustomerDetailPage>();

        page.SetCustomerId(customerId);

        Navigate(page);
    }

    public void GoBack()
    {
        if (_frame == null || !CanGoBack)
            return;

        _frame.Content = _history.Pop();
    }

    private void Navigate(Page page)
    {
        if (_frame == null)
            return;

        if (_frame.Content is Page currentPage)
        {
            _history.Push(currentPage);
        }

        _frame.Content = page;
    }
}