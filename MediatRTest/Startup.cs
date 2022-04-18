using FluentValidation.AspNetCore;
using MediatR;
using MediatRTest.Common;
using MediatRTest.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;
using Polly.Extensions.Http;
using System;
using System.Net.Http;

namespace MediatRTest
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add services to the container.
            services
                .AddControllers(/*options => options.Filters.Add(new ActionValidationFilter()*)*/)
                .AddFluentValidation(cfg => { cfg.RegisterValidatorsFromAssemblyContaining<Program>(); });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            //services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.CustomSchemaIds(x => x?.FullName?.Replace("+",":"));
            });

            services.AddTransient<ICustomerService, CustomerService>();

            services.AddAutoMapper(typeof(Program));
            services.AddMediatR(typeof(Program));

            services
                .AddHttpClient(Constants.CoreApiIdentifier, httpClient => //Better type client!
                {
                    var url = Configuration.GetValue<Uri>("CoreApiBaseUrl");
                    var key = Configuration.GetValue<string>("api-key");

                    httpClient.BaseAddress = url;
                    httpClient.DefaultRequestHeaders.Add("x-functions-key", key); // Bearer token...
                })
                .AddPolicyHandler(GetRetryPolicy());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Configure the HTTP request pipeline.
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }
    }
}
