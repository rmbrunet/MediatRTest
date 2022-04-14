using AutoMapper;
using FluentAssertions;
using MediatRTest.Model;
using MediatRTestTests.Mocks;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace MediatRTestTests
{
    public class ListCustomersTests
    {
        private readonly IMapper mapper;

        public ListCustomersTests()
        {
            var config = new MapperConfiguration(c => c.AddProfile<MediatRTest.Features.ListCustomersFeature.MappingProfile>());
            mapper = config.CreateMapper();
        }

        [Fact]
        public async Task ListCustomersAsync()
        {
            var customerService = CustomerServiceMock.GetCustomerServiceMock(5).Object;

            var handler = new MediatRTest.Features.ListCustomersFeature.QueryHandler(customerService, mapper);

            var query = new MediatRTest.Features.ListCustomersFeature.Query() { Size = 5 };

            var result = await handler.Handle(query, CancellationToken.None);

            result.Should().NotBeNull();
            result.Customers.Count().Should().Be(5);
            result.Customers.Should().BeAssignableTo<IEnumerable<CustomerDto>>();
        }
    }
}