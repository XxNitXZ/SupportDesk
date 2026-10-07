using Microsoft.UI.Xaml;
using SupportDesk.Presentation.Views;

namespace SupportDesk.Presentation;

public sealed partial class MainWindow : Window
{
    public MainWindow(NewTicketPage newTicketPage)
    {
        InitializeComponent();

        MainFrame.Content = newTicketPage;
    }
}