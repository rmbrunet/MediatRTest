using AutoMapper;
using FluentAssertions;
using MediatRTestTests.Mocks;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Models.Customers;
using MediatRTest.Features.Customers;

namespace MediatRTestTests
{
    public class ListCustomersTests
    {
        private readonly IMapper mapper;

        public ListCustomersTests()
        {
            var config = new MapperConfiguration(c => c.AddProfile<GetCustomersFeature.MappingProfile>());
            mapper = config.CreateMapper();
        }


        [Fact]
        public async Task ListCustomers_Should_Return_The_Right_Type()
        {
            var customerService = CustomerServiceMock.GetCustomerServiceMock(5).Object;

            var handler = new GetCustomersFeature.Handler(customerService, mapper);

            var query = new GetCustomersFeature.Query(5);

            var result = await handler.Handle(query, CancellationToken.None);

            result.Should().NotBeNull();
            result.Customers.Should().BeAssignableTo<IEnumerable<Models.Customers.Customer>>();
        }


        [Fact]
        public async Task ListCustomers_Should_Return_The_Right_Numbers_Of_Items()
        {
            var customerService = CustomerServiceMock.GetCustomerServiceMock(5).Object;

            var handler = new GetCustomersFeature.Handler(customerService, mapper);

            var query = new GetCustomersFeature.Query(5);

            var result = await handler.Handle(query, CancellationToken.None);

            result.Should().NotBeNull();
            result.Customers.Count().Should().Be(5);
        }

        [Fact]
        public void ListCustomers_Should_Accept_Valid_Query()
        {
            var query = new GetCustomersFeature.Query(5);

            var validator = new GetCustomersFeature.ModelValidator();


            var result = validator.Validate(query);

            result.IsValid.Should().Be(true);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void ListCustomers_Should_Not_Accept_Invalid_Query(int size)
        {
            var query = new GetCustomersFeature.Query(size);

            var validator = new GetCustomersFeature.ModelValidator();


            var result = validator.Validate(query);

            result.IsValid.Should().Be(false);
        }


    }
}