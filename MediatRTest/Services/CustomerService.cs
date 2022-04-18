using Flurl;
using MediatRTest.Common;
using MediatRTest.Model;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace MediatRTest.Services
{
    public interface ICustomerService
    {
        Task<IEnumerable<Customer>> GetCustomers(int size);
    }

    public class CustomerService : ICustomerService
    {
        private readonly IHttpClientFactory _factory;

        public CustomerService(IHttpClientFactory factory)
        {
            _factory = factory;
        }

        public async Task<IEnumerable<Customer>> GetCustomers(int size)
        {
            var client = _factory.CreateClient(Constants.CoreApiIdentifier);

            var uri = "customers".SetQueryParam("size", size);

            var customers = await client.GetFromJsonAsync<IEnumerable<Customer>>(uri);

            return customers ?? Enumerable.Empty<Customer>();
        }
    }
}
