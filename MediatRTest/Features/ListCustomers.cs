using AutoMapper;
using FluentValidation;
using MediatR;
using MediatRTest.Model;
using MediatRTest.Services;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MediatRTest.Features
{
    public class ListCustomersFeature
    {
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
            private readonly ICustomerService _customerService;
            private readonly IMapper _mapper;

            public QueryHandler(ICustomerService customerService, IMapper mapper)
            {
                _customerService = customerService;
                _mapper = mapper;
            }

            public async Task<Result> Handle(Query request, CancellationToken cancellationToken)
            {
                IEnumerable<Customer> customers = await _customerService.GetCustomers(request.Size);

                var dtos = _mapper.Map<IEnumerable<CustomerDto>>(customers);

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
}
