using FluentAssertions;
using MediatrExample.ApplicationCore.Domain;
using MediatrExample.ApplicationCore.Features.Products.Commands;
using NUnit.Framework;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace MediatRExample.IntegrationTests.Features.Products;
public class CreateProductCommandTests : TestBase
{

    [Test]
    public async Task Product_IsCreated_WhenValidFieldsAreProvided_AndUserIsAdmin()
    {
        // Arrenge
        var (Client, UserId) = await GetClientAsAdmin();

        // Act
        var command = new CreateProductCommand
        {
            Description = "Test Product",
            Price = 999
        };

        var result = await Client.PostAsJsonAsync("api/products", command);

        // Assert

        FluentActions.Invoking(() => result.EnsureSuccessStatusCode())
            .Should().NotThrow();

        var product = await FindAsync<Product>(q => q.Description == command.Description);

        product.Should().NotBeNull();
        product.Description.Should().Be(command.Description);
        product.Price.Should().Be(command.Price);
        product.CreatedBy.Should().Be(UserId);
    }

    [Test]
    public async Task Product_IsNotCreated_WhenInvalidFieldsAreProvided_AndUserIsAdmin()
    {
        // Arrenge
        var (Client, UserId) = await GetClientAsAdmin();

        // Act
        var command = new CreateProductCommand
        {
            Description = "Test Product",
            Price = 0
        };

        var result = await Client.PostAsJsonAsync("api/products", command);

        // Assert
        FluentActions.Invoking(() => result.EnsureSuccessStatusCode())
            .Should().Throw<HttpRequestException>();

        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Test]
    public async Task Product_IsNotCreated_WhenValidFieldsAreProvided_AndDefaultUser()
    {
        // Arrenge
        var command = new CreateProductCommand
        {
            Description = "Test Product",
            Price = 999
        };
        var (Client, UserId) = await GetClientAsDefaultUserAsync();

        // Act        
        var result = await Client.PostAsJsonAsync("api/products", command);

        // Assert
        FluentActions.Invoking(() => result.EnsureSuccessStatusCode())
            .Should().Throw<HttpRequestException>();

        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}
