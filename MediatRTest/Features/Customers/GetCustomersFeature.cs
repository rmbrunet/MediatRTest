using AutoMapper;
using FluentValidation;
using MediatR;
using Models.Customers;
using Services.Customers;

namespace MediatRTest.Features.Customers;

public class GetCustomersFeature
{
    public record Query (int Size) : IRequest<Result>;
    //{
    //    public int Size { get; set; }
    //}

    public class Result
    {
        public Result(IEnumerable<Customer> customers)
        {
            Customers = customers;
        }

        public IEnumerable<Customer> Customers { get; set; }
    }

    public class Handler : IRequestHandler<Query, Result>
    {
        private readonly ICustomerService customerService;
        private readonly IMapper mapper;

        public Handler(ICustomerService customerService, IMapper mapper)
        {
            this.customerService = customerService;
            this.mapper = mapper;
        }

        public async Task<Result> Handle(Query request, CancellationToken cancellationToken)
        {
            IEnumerable<CoreCustomer> customers = await customerService.GetCustomers(request.Size);

            var dtos = mapper.Map<IEnumerable<Customer>>(customers);

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
        public MappingProfile() => CreateMap<CoreCustomer, Customer>()
                        .ForMember(d => d.BillingCity, opt => opt.MapFrom(c => c.BillingAddress != null ? c.BillingAddress.City : null))
                        .ForMember(d => d.BillingState, opt => opt.MapFrom(c => c.BillingAddress != null ? c.BillingAddress.State : null))
                        .ForMember(d => d.BillingZip, opt => opt.MapFrom(c => c.BillingAddress != null ? c.BillingAddress.Zip : null))
                        .ForMember(d => d.BillingCountry, opt => opt.MapFrom(c => c.BillingAddress != null ? c.BillingAddress.Country : null));
    }
}