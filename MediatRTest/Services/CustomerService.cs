//using Flurl;
//using MediatRTest.Common;
//using MediatRTest.Model;

//namespace MediatRTest.Services;

//public interface ICustomerService
//{
//    Task<IEnumerable<Customer>> GetCustomers(int size);
//}

//public class CustomerService : ICustomerService
//{
//    private readonly IHttpClientFactory factory;

//    public CustomerService(IHttpClientFactory factory)
//    {
//        this.factory = factory;
//    }

//    public async Task<IEnumerable<Customer>> GetCustomers(int size)
//    {
//        var client = this.factory.CreateClient(Constants.CoreApiIdentifier);

//        var uri = "customers".SetQueryParam("size", size);

//        var customers = await client.GetFromJsonAsync<IEnumerable<Customer>>(uri);

//        return customers ?? Enumerable.Empty<Customer>();
//    }
//}