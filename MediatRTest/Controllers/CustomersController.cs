using MediatR;
using MediatRTest.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace MediatRTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomersController
    {
        private readonly IMediator _mediator;

        public CustomersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet(Name = "GetCustomers")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ListCustomersFeature.Result))]
        public async Task<ListCustomersFeature.Result> GetCustomers([FromQuery] ListCustomersFeature.Query model)
        {
            return await _mediator.Send(model);
        }

        [HttpGet("recent", Name = "GetRecentCustomers")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ListRecentCustomers.Result))]
        public async Task<ListRecentCustomers.Result> GetRecentCustomers([Required, FromQuery] ListRecentCustomers.Query model)
        {
            return await _mediator.Send(model);
        }
    }
}
