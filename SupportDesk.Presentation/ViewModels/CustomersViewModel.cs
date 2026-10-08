using CommunityToolkit.Mvvm.ComponentModel;
using SupportDesk.Application.Interfaces;
using SupportDesk.Domain;
using SupportDesk.Presentation.Interfaces;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace SupportDesk.Presentation.ViewModels
{
    public partial class CustomersViewModel : ObservableObject
    {
        private readonly ICustomerService _customerService;
        public ObservableCollection<Customer> Customers { get; } = new();

        public CustomersViewModel(ICustomerService customerService)
        {
            _customerService = customerService;

        }

        public async Task LoadAsync()
        {
            var customers = await _customerService.GetAllAsync();

            Customers.Clear();

            foreach (var customer in customers)
            {
                Customers.Add(customer);
            }
        }
    }
} 