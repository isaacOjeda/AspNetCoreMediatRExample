using MediatrExample.ApplicationCore;
using MediatrExample.ApplicationCore.Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MediatRExample.CheckoutProcessor;
public class Program
{
    public static void Main()
    {
        var host = new HostBuilder()
            .ConfigureFunctionsWorkerDefaults()
            .ConfigureServices((hostBuilderContext, services) =>
            {
                var configuration = hostBuilderContext.Configuration;
                services.AddApplicationCore();
                services.AddPersistence(configuration);
                services.AddInfrastructure();

                services.AddTransient<ICurrentUserService, WorkerUserService>();
            })
            .Build();

        host.Run();
    }
}
