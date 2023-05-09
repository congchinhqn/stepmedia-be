using Autofac.Extensions.DependencyInjection;
using Destructurama;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Metatrade.OrderService;
using Serilog;
using Serilog.Exceptions;
using Serilog.Exceptions.Core;
using Serilog.Exceptions.EntityFrameworkCore.Destructurers;
using Metatrade.Core.Exceptions;
using StepmediaBE.OrderService;

public class Program
{
    public static void Main(string[] args)
    {
        Host.CreateDefaultBuilder(args)
            .UseServiceProviderFactory(new AutofacServiceProviderFactory())
            .ConfigureWebHostDefaults(webHostBuilder =>
            {
                webHostBuilder
                    .UseStartup<Startup>();
            })
            .ConfigureLogging((hostingContext, logging) =>
            {
                var loggerConfiguration = new LoggerConfiguration()
                    .ReadFrom.Configuration(hostingContext.Configuration)
                    .Enrich.FromLogContext()
                    .Enrich.WithExceptionDetails(
                        new DestructuringOptionsBuilder()
                            .WithDefaultDestructurers()
                            .WithDestructurers(new[] {new DbUpdateExceptionDestructurer()})
                    )
                    .Destructure.UsingAttributes()
                    .Filter.ByExcluding(x =>
                    {
                        if (x.Exception == null)
                            return false;

                        var exType = x.Exception.GetType();
                        return exType == typeof(AppException) || exType == typeof(DomainException);
                    });

                Log.Logger = loggerConfiguration.CreateLogger();
                logging.AddSerilog(dispose: true);
            })
            .Build().Run();
    }
}