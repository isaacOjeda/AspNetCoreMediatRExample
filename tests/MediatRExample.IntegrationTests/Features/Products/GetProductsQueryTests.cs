using FluentAssertions;
using MediatrExample.ApplicationCore.Features.Products.Queries;
using NUnit.Framework;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace MediatRExample.IntegrationTests.Features.Products;
public class GetProductsQueryTests : TestBase
{

    [Test]
    public async Task Products_Obtained_WithAuthenticatedUser()
    {
        // Arrenge
        var (Client, UserId) = await GetClientAsDefaultUserAsync();


        // Act
        var products = await Client.GetFromJsonAsync<List<GetProductsQueryResponse>>("/api/products");

        // Assert
        products.Should().NotBeNullOrEmpty();
        products?.Count.Should().Be(2);
    }

    [Test]
    public async Task Products_ProducesException_WithAnonymUser()
    {
        // Arrenge
        var client = Application.CreateClient();

        // Act and Assert
        await FluentActions.Invoking(() =>
                client.GetFromJsonAsync<List<GetProductsQueryResponse>>("/api/products"))
                    .Should().ThrowAsync<HttpRequestException>();
    }
}
