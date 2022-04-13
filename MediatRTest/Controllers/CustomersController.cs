using MediatR;
using MediatRTest.Features;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

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
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ListCustomers.Result))]
    public async Task<ListCustomers.Result> GetCustomers([FromQuery] ListCustomers.Query model)
    {
        return await mediator.Send(model);
    }

    [HttpGet("recent", Name = "GetRecentCustomers")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ListRecentCustomers.Result))]
    public async Task<ListRecentCustomers.Result> GetRecentCustomers([Required, FromQuery] ListRecentCustomers.Query model)
    {
        return await mediator.Send(model);
    }
}