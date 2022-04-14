using MediatRTest.Model;
using MediatRTest.Services;
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

            Expression<Func<ICustomerService, Task<IEnumerable<Customer>>>> call = x => x.GetCustomers(size);

            mock.Setup(call).Returns(Task.FromResult(GetCustomers(size)));

            return mock;
        }

        private static IEnumerable<Customer> GetCustomers(int size)
        {
            if (size > 0)
            {
                return Enumerable.Range(1, size).Select(i => new Customer()).ToList();
            }
            else
            {
                return Array.Empty<Customer>();
            }
        }
    }
}