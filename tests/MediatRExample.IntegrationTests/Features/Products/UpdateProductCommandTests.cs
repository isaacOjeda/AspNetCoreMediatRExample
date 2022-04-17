using FluentAssertions;
using MediatrExample.ApplicationCore.Common.Helpers;
using MediatrExample.ApplicationCore.Domain;
using MediatrExample.ApplicationCore.Features.Products.Commands;
using NUnit.Framework;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace MediatRExample.IntegrationTests.Features.Products;
public class UpdateProductCommandTests : TestBase
{

    [Test]
    public async Task Product_IsUpdated_WithValidFields_AndAuthUser()
    {
        // Arrenge
        var productDemo = await FindAsync<Product>(q => 1 == 1);
        var (Client, UserId) = await GetClientAsAdmin();
        var command = new UpdateProductCommand
        {
            ProductId = productDemo.ProductId.ToHashId(),
            Description = "Updated Product",
            Price = 123456
        };

        // Act
        var result = await Client.PutAsJsonAsync("api/products", command);

        // Assert
        FluentActions.Invoking(() => result.EnsureSuccessStatusCode())
            .Should().NotThrow();

        var product = await FindAsync<Product>(command.ProductId.FromHashId());

        product.Should().NotBeNull();
        product.Description.Should().Be(command.Description);
        product.Price.Should().Be(command.Price);
        product.LastModifiedBy.Should().Be(UserId);
    }

    [Test]
    public async Task Product_IsUpdated_WithInvalidFields_AndAuthUser()
    {
        // Arrenge
        var productDemo = await FindAsync<Product>(q => 1 == 1);
        var (Client, UserId) = await GetClientAsAdmin();
        var command = new UpdateProductCommand
        {
            ProductId = productDemo.ProductId.ToHashId(),
            Description = string.Empty,
            Price = 0
        };

        // Act
        var result = await Client.PutAsJsonAsync("api/products", command);

        // Assert
        FluentActions.Invoking(() => result.EnsureSuccessStatusCode())
            .Should().Throw<HttpRequestException>();

        var updatedProduct = await FindAsync<Product>(command.ProductId.FromHashId());

        updatedProduct.Should().NotBeNull();
        updatedProduct.Description.Should().NotBe(command.Description);
        updatedProduct.Price.Should().NotBe(command.Price);
    }

    [Test]
    public async Task Product_IsNotUpdated_WithValidFields_AndAnonymUser()
    {
        // Arrenge
        var productDemo = await FindAsync<Product>(q => 1 == 1);
        var (Client, UserId) = await GetClientAsDefaultUserAsync();
        var command = new UpdateProductCommand
        {
            ProductId = productDemo.ProductId.ToHashId(),
            Description = "Updated Product",
            Price = 123456
        };

        // Act
        var result = await Client.PutAsJsonAsync("api/products", command);

        // Assert
        FluentActions.Invoking(() => result.EnsureSuccessStatusCode())
            .Should().Throw<HttpRequestException>();

        var updatedProduct = await FindAsync<Product>(command.ProductId.FromHashId());

        updatedProduct.Should().NotBeNull();
        updatedProduct.Description.Should().NotBe(command.Description);
        updatedProduct.Price.Should().NotBe(command.Price);
    }
}
