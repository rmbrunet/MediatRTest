using AutoMapper;
using FluentValidation;
using MediatR;
using MediatRTest.Model;
using MediatRTest.Services;

namespace MediatRTest.Features;

public class ListRecentCustomers
{
    public class Query : IRequest<Result2>
    {
        public Query()
        {
            Ids = Array.Empty<int>();
        }
        public int[] Ids { get; set; }
    }

    public class Result2
    {
        public IEnumerable<CustomerDto>? Customers { get; set; }

    }

    public class QueryHandler : IRequestHandler<Query, Result2>
    {
        private readonly ICustomerService customerService;
        private readonly AutoMapper.IConfigurationProvider configuration;

        public QueryHandler(ICustomerService customerService, AutoMapper.IConfigurationProvider configuration)
        {
            this.customerService = customerService;
            this.configuration = configuration;
        }

        public async Task<Result2> Handle(Query request, CancellationToken cancellationToken)
        {

            var customers = await this.customerService.GetCustomers(100); //Just testing

            customers = customers.Where(c => request.Ids.Contains(c.Id)).ToList();

            var mapper = this.configuration.CreateMapper();

            var dtos = mapper.Map<IEnumerable<CustomerDto>>(customers);

            return new Result2 { Customers = dtos };
        }
    }

    public class ModelValidator : AbstractValidator<Query>
    {
        public ModelValidator()
        {
            RuleFor(x => x.Ids).NotNull().WithMessage(m => $"You must pass at least one Id");
            RuleFor(x => x.Ids).NotEmpty().WithMessage(m => $"You must pass at least one Id");
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
