using CommunityToolkit.Mvvm.ComponentModel;
using SupportDesk.Application.Interfaces;
using System;
using System.Threading.Tasks;

namespace SupportDesk.Presentation.ViewModels;

public partial class CustomerDetailViewModel : ObservableObject
{
    private readonly ICustomerService _customerService;

    private Guid _customerId;

    [ObservableProperty]
    public partial string Name { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string Email { get; set; } = string.Empty;

    public CustomerDetailViewModel(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    public async Task LoadAsync(Guid customerId)
    {
        var customer = await _customerService.GetByIdAsync(customerId);

        if (customer == null)
        {
            throw new ArgumentException("Customer was not found.");
        }

        _customerId = customer.Id;
        Name = customer.Name;
        Email = customer.Email;
    }

    public async Task SaveAsync()
    {
        await _customerService.UpdateAsync(
            _customerId,
            Name,
            Email);
    }
}