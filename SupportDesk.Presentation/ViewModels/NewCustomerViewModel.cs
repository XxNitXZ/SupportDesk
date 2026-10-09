using CommunityToolkit.Mvvm.ComponentModel;
using SupportDesk.Application.Interfaces;
using System.Threading.Tasks;

namespace SupportDesk.Presentation.ViewModels;

public partial class NewCustomerViewModel : ObservableObject
{
    private readonly ICustomerService _customerService;

    [ObservableProperty]
    public partial string Name { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string Email { get; set; } = string.Empty;

    public NewCustomerViewModel(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    public async Task CreateCustomerAsync()
    {
        await _customerService.AddAsync(Name, Email);
    }
}