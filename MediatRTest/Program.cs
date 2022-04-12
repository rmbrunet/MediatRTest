using FluentValidation.AspNetCore;
using MediatR;
using MediatRTest.Common;
using MediatRTest.Services;
using Polly;
using Polly.Extensions.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .AddControllers(/*options => options.Filters.Add(new ActionValidationFilter()*)*/)
    .AddFluentValidation(cfg => { cfg.RegisterValidatorsFromAssemblyContaining<Program>(); });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<ICustomerService, CustomerService>();

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddMediatR(typeof(Program));

builder.Services
    .AddHttpClient(Constants.CoreApiIdentifier, httpClient => //Better type client!
        {
            var url = builder.Configuration.GetValue<Uri>("CoreApiBaseUrl");
            var key = builder.Configuration.GetValue<string>("api-key");

            httpClient.BaseAddress = url;
            httpClient.DefaultRequestHeaders.Add("x-functions-key", key); // Bearer token...
        })
    .AddPolicyHandler(GetRetryPolicy());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
{
    return HttpPolicyExtensions
        .HandleTransientHttpError()
        .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
        .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
}