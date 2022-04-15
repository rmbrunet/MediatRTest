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
        public async Task ListCustomers_Should_Return_The_Right_Type()
        {
            var customerService = CustomerServiceMock.GetCustomerServiceMock(5).Object;

            var handler = new MediatRTest.Features.ListCustomersFeature.QueryHandler(customerService, mapper);

            var query = new MediatRTest.Features.ListCustomersFeature.Query() { Size = 5 };

            var result = await handler.Handle(query, CancellationToken.None);

            result.Should().NotBeNull();
            result.Customers.Should().BeAssignableTo<IEnumerable<CustomerDto>>();
        }


        [Fact]
        public async Task ListCustomers_Should_Return_The_Right_Numbers_Of_Items()
        {
            var customerService = CustomerServiceMock.GetCustomerServiceMock(5).Object;

            var handler = new MediatRTest.Features.ListCustomersFeature.QueryHandler(customerService, mapper);

            var query = new MediatRTest.Features.ListCustomersFeature.Query() { Size = 5 };

            var result = await handler.Handle(query, CancellationToken.None);

            result.Should().NotBeNull();
            result.Customers.Count().Should().Be(5);
        }

        [Fact]
        public void ListCustomers_Should_Accept_Valid_Query()
        {
            var query = new MediatRTest.Features.ListCustomersFeature.Query() { Size = 5 };

            var validator = new MediatRTest.Features.ListCustomersFeature.ModelValidator();


            var result = validator.Validate(query);

            result.IsValid.Should().Be(true);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void ListCustomers_Should_Not_Accept_Invalid_Query(int size)
        {
            var query = new MediatRTest.Features.ListCustomersFeature.Query() { Size = size };

            var validator = new MediatRTest.Features.ListCustomersFeature.ModelValidator();


            var result = validator.Validate(query);

            result.IsValid.Should().Be(false);
        }


    }
}