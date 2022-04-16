using MediatR;
using MediatRTest.Features.Customers;
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
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetCustomersFeature.Result))]
    public async Task<GetCustomersFeature.Result> Get([FromQuery] GetCustomersFeature.Query model)
    {
        return await mediator.Send(model);
    }

    [HttpGet("recent", Name = "GetRecentCustomers")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetRecentCustomersFeature.Result))]
    public async Task<GetRecentCustomersFeature.Result> Get([FromQuery] GetRecentCustomersFeature.Query model)
    {
        return await mediator.Send(model);
    }

    [HttpPost (Name = "CreateCustomer")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CreateCustomerFeature.Result))]
    public async Task<CreateCustomerFeature.Result> Post([FromBody] CreateCustomerFeature.Command command )
    {
        return await mediator.Send(command);
    }

}