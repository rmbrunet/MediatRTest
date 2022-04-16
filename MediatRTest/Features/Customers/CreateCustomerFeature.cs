using FluentValidation;
using MediatR;
using Models.Customers;

namespace MediatRTest.Features.Customers;

public class CreateCustomerFeature
{
    public record Command (string CustomerNumber, string CustomerName) : IRequest<Result>;
    //{
    //    public string? CustomerNumber { get; set; }
    //    public string? CustomerName { get; set; }  
    //}

    public class Result
    {
        public Result(Customer customer)
        {
            Customer = customer;
        }
        public Customer Customer { get; set; }
    }

    public class Handler : IRequestHandler<Command, Result>
    {
        public Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            //Persist Customer, etc.. 
            var customer = new Customer {  CustomerNumber = request.CustomerNumber, CustomerName = request.CustomerName };
            return Task.FromResult(new Result(customer));
        }
    }

    public class ModelValidator : AbstractValidator<Command>
    {
        public ModelValidator()
        {
            RuleFor(x => x.CustomerNumber).NotNull().WithMessage("Customer Number is required");
            RuleFor(x => x.CustomerNumber).NotEmpty().WithMessage("Customer Number is required");
            RuleFor(x => x.CustomerName).NotNull().WithMessage("Customer Name is required");
            RuleFor(x => x.CustomerName).NotEmpty().WithMessage("Customer Name is required");
        }
    }

}
