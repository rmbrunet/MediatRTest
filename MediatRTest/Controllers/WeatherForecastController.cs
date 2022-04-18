using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MediatRTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

        private readonly IMediator _mediator;

        public WeatherForecastController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IEnumerable<WeatherForecast>> Get([FromQuery] Query model)
        {
            return await _mediator.Send(model);
        }

        public class ModelValidator : AbstractValidator<Query>
        {
            public ModelValidator()
            {
                RuleFor(x => x.Size).GreaterThan(0).WithMessage(m => $"Id must be greater than 0. It is {m.Size}");
            }
        }

        public class Query : IRequest<IEnumerable<WeatherForecast>>
        {
            public int Size { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, IEnumerable<WeatherForecast>>
        {
            public Task<IEnumerable<WeatherForecast>> Handle(Query request, CancellationToken cancellationToken)
            {
                var result = Enumerable.Range(1, request.Size).Select(index => new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = new Random().Next(-20, 55),
                    Summary = Summaries[new Random().Next(Summaries.Length)]
                })
                .ToList();

                return Task.FromResult<IEnumerable<WeatherForecast>>(result);
            }
        }
    }
}
