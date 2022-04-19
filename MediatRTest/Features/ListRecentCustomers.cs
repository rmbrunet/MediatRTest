using AutoMapper;
using FluentValidation;
using MediatR;
using MediatRTest.Model;
using MediatRTest.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MediatRTest.Features
{
    public class ListRecentCustomers
    {
        public class Query : IRequest<Result>
        {
            public Query()
            {
                Ids = Array.Empty<int>();
            }

            public int[] Ids { get; set; }
        }

        public class Result
        {
            public IEnumerable<CustomerDto>? Customers { get; set; }
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
                var customers = await _customerService.GetCustomers(100); //Just testing

                customers = customers.Where(c => request.Ids.Contains(c.Id)).ToList();

                var dtos = _mapper.Map<IEnumerable<CustomerDto>>(customers);

                return new Result { Customers = dtos };
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


    }
}
