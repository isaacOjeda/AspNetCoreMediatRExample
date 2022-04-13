using MediatrExample.WebApi;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;

namespace MediatRExample.IntegrationTests;
public class ApiWebApplicationFactory : WebApplicationFactory<Api>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        base.ConfigureWebHost(builder);
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Configurar cualquier Mock o similares
        });

        return base.CreateHost(builder);
    }
}
