using System;
using System.Reflection;
using System.Text;
using FluentValidation.AspNetCore;
using MassTransit;
using MediatR;
using Metatrade.Infrastructure.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Metatrade.OrderService.Dtos;
using Scrutor;
using StepmediaBE.Infrastructure;
using StepmediaBE.Infrastructure.Repositories;

namespace StepmediaBE.OrderService;

public class Startup
{
    #region Fields

    private readonly IConfiguration _configuration;
    private IServiceCollection _services;

    #endregion

    #region Constructors

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    #endregion
    
    #region Methods

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostEnvironment env)
    {
        if (env.IsDevelopment() || env.IsEnvironment("Testing"))
        {
            app.UseDeveloperExceptionPage();
            app.Map("/testservices", builder => builder.Run(async context =>
            {
                var sb = new StringBuilder();
                sb.Append("<h1>All Services</h1>");
                sb.Append("<table><thead>");
                sb.Append("<tr><th>Type</th><th>Lifetime</th><th>Instance</th></tr>");
                sb.Append("</thead><tbody>");
                foreach (var svc in _services)
                {
                    sb.Append("<tr>");
                    sb.Append($"<td>{svc.ServiceType.FullName}</td>");
                    sb.Append($"<td>{svc.Lifetime}</td>");
                    sb.Append($"<td>{svc.ImplementationType?.FullName}</td>");
                    sb.Append("</tr>");
                }

                sb.Append("</tbody></table>");
                await context.Response.WriteAsync(sb.ToString());
            }));
        }
        app.UseHttpsRedirection();

        app.UseRouting();

#if DEBUG
        // Enable middleware to serve generated Swagger as a JSON endpoint.
        app.UseSwagger();

        // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
        // specifying the Swagger JSON endpoint.
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Stepmedia BE API V1");
            c.RoutePrefix = "doc";
        });
#endif
        app.UseCors(cors => cors.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapHealthChecks("/ping");
            endpoints.MapDefaultControllerRoute();
        });
    }

    public void ConfigureServices(IServiceCollection services)
    {
#if DEBUG
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Stepmedia BE API", Version = "v1" });
        });
#endif
        services
            .AddHttpClient()
            .AddHttpContextAccessor()
            .AddHealthChecks();

        services.AddCors(c => { c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin()); });

        services.AddMvc()
            .AddFluentValidation(fv =>
            {
                Assembly[] types = { typeof(BaseValidator<>).Assembly, typeof(Startup).Assembly };
                fv.RegisterValidatorsFromAssemblies(types);
                fv.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
                fv.ImplicitlyValidateChildProperties = true;
            });

        services.AddDataAccessLayer(_configuration["ConnectionStrings:Default"]);
        
        services.Scan(scan => scan
            // Repositories
            .FromAssemblyOf<CustomerRepository>().AddClasses()
            .UsingRegistrationStrategy(RegistrationStrategy.Skip).AsMatchingInterface().WithScopedLifetime()
        );
        
        services.AddMediatR(Assembly.GetExecutingAssembly()); 
        
        _services = services;
    }
    
    #endregion
}