using AutoMapper;
using FluentValidation;
using MediatR;
using MediatRTest.Model;
using MediatRTest.Services;
using Microsoft.AspNetCore.Mvc;

namespace MediatRTest.Controllers;

[ApiController]
[Route("[controller]")]
public class CustomersController
{
    private readonly IMediator mediator;

    public CustomersController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet(Name = "GetCustomers")]
    public async Task<Result> Get([FromQuery] Query model)
    {
        return await this.mediator.Send(model);
    }

    public class Query : IRequest<Result>
    {
        public int Size { get; set; }
    }

    public class Result
    {
        public Result(IEnumerable<CustomerDto> customers)
        {
            Customers = customers;
        }

        public IEnumerable<CustomerDto> Customers { get; set; }
    }

    public class QueryHandler : IRequestHandler<Query, Result>
    {
        private readonly ICustomerService customerService;
        private readonly AutoMapper.IConfigurationProvider configuration;

        public QueryHandler(ICustomerService customerService, AutoMapper.IConfigurationProvider configuration)
        {
            this.customerService = customerService;
            this.configuration = configuration;
        }

        public async Task<Result> Handle(Query request, CancellationToken cancellationToken)
        {
            var customers = await this.customerService.GetCustomers(request.Size);

            var mapper = this.configuration.CreateMapper();

            var dtos = mapper.Map<IEnumerable<CustomerDto>>(customers);

            return new Result(dtos);
        }
    }

    public class ModelValidator : AbstractValidator<Query>
    {
        public ModelValidator()
        {
            RuleFor(x => x.Size).GreaterThan(0).WithMessage(m => $"Size must be greater than 0. It is {m.Size}");
        }
    }

    public class MappingProfile : Profile
    {
        public MappingProfile() => CreateMap<Customer, CustomerDto>()
                        .ForMember(d => d.BillingCity, opt => opt.MapFrom(c => c.BillingAddress != null ? c.BillingAddress.City : null))
                        .ForMember(d => d.BillingState, opt => opt.MapFrom(c => c.BillingAddress != null ? c.BillingAddress.State : null))
                        .ForMember(d => d.BillingZip, opt => opt.MapFrom(c => c.BillingAddress != null ? c.BillingAddress.Zip : null))
                        .ForMember(d => d.BillingCountry, opt => opt.MapFrom(c => c.BillingAddress != null ? c.BillingAddress.Country : null));
    }
}