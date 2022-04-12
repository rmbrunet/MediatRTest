using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MediatRTest.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};
    private readonly IMediator mediator;

    public WeatherForecastController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<IEnumerable<WeatherForecast>> Get([FromQuery]Query model)
    {
        return await this.mediator.Send(model); 
    
    }


    public class ModelValidator: AbstractValidator<Query>
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
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                })
            .ToList();

            return Task.FromResult<IEnumerable<WeatherForecast>>(result);
        }
    }

}
