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
        public Result(IEnumerable<Customer> customers)
        {
            Customers = customers;
        }
        public IEnumerable<Customer> Customers { get; set; }
    }

    public class QueryHandler : IRequestHandler<Query, Result>
    {
        private readonly ICustomerService customerService;

        public QueryHandler(ICustomerService customerService)
        {
            this.customerService = customerService;
        }
        public async Task<Result> Handle(Query request, CancellationToken cancellationToken)
        {
            var customers = await this.customerService.GetCustomers(request.Size);
            return new Result(customers);
        }
    }

    public class ModelValidator : AbstractValidator<Query>
    {
        public ModelValidator()
        {
            RuleFor(x => x.Size).GreaterThan(0).WithMessage(m => $"Size must be greater than 0. It is {m.Size}");
        }

    }


}
