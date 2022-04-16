using Flurl;
using Models.Customers;
using System.Net.Http;
using System.Net.Http.Json;

namespace Services.Customers;


public class CustomerService : ICustomerService
{
    public static readonly string ApiIdentifier = nameof(CustomerService);

    private readonly IHttpClientFactory factory;

    public CustomerService(IHttpClientFactory factory)
    {
        this.factory = factory;
    }

    public async Task<IEnumerable<CoreCustomer>> GetCustomers(int size)
    {
        var client = this.factory.CreateClient(ApiIdentifier);

        var uri = "customers".SetQueryParam("size", size);

        var customers = await client.GetFromJsonAsync<IEnumerable<CoreCustomer>>(uri);

        return customers ?? Enumerable.Empty<CoreCustomer>();
    }
}