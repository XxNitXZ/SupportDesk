using Microsoft.UI.Xaml;
using System.Threading.Tasks;

namespace SupportDesk.Presentation.Interfaces;

public interface IDialogService
{
    void SetXamlRoot(XamlRoot xamlRoot);

    Task ShowMessageAsync(string title, string message);

    Task ShowErrorAsync(string title, string message);

    Task<bool> ShowConfirmAsync(string title, string message);
}