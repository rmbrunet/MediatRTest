//using MediatRTest.Model;
//using MediatRTest.Services;
using Models.Customers;
using Services.Customers;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MediatRTestTests.Mocks
{
    public static class CustomerServiceMock
    {
        public static Mock<ICustomerService> GetCustomerServiceMock(int size)
        {
            var mock = new Mock<ICustomerService>();

            Expression<Func<ICustomerService, Task<IEnumerable<CoreCustomer>>>> call = x => x.GetCustomers(size);

            mock.Setup(call).ReturnsAsync(GetCustomers(size));

            return mock;
        }


        private static IEnumerable<CoreCustomer> GetCustomers(int size)
        {
            return size switch {
                > 0 => Enumerable.Range(1, size).Select(i => new CoreCustomer()).ToList(),
                _ => Array.Empty<CoreCustomer>()
            };
           
        }
    }
}