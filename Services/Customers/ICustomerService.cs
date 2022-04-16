using Models.Customers;

namespace Services.Customers;

public interface ICustomerService
{
    Task<IEnumerable<CoreCustomer>> GetCustomers(int size);
}
