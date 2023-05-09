using System.Linq;
using FluentAssertions;
using MediatrExample.ApplicationCore.Common.Models;
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
    [TestCase(10)]
    [TestCase(20)]
    [TestCase(30)]
    public async Task Products_Obtained_WithAuthenticatedUser(int pageSize)
    {
        // Arrenge
        var (Client, UserId, _) = await GetClientAsDefaultUserAsync();


        // Act
        var products = await Client.GetFromJsonAsync<PagedResult<GetProductsQueryResponse>>($"/api/products?pageSize={pageSize}&currentPage=1");

        // Assert
        products.Should().NotBeNull();
        products?.Results.Count().Should().Be(pageSize);
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
