using FluentAssertions;
using MediatrExample.ApplicationCore.Features.Auth;
using NUnit.Framework;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace MediatRExample.IntegrationTests.Features.Auth;
public class RequestTokenCommandTests : TestBase
{

    [Test]
    public async Task User_CanLogin()
    {
        var client = Application.CreateClient();

        var result = await client.PostAsJsonAsync("api/auth", new TokenCommand
        {
            UserName = "test_user",
            Password = "Passw0rd.1234"
        });


        FluentActions.Invoking(() => result.EnsureSuccessStatusCode())
            .Should().NotThrow();

        var response = JsonSerializer.Deserialize<TokenCommandResponse>(result.Content.ReadAsStream(), new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        response.Should().NotBeNull();
        response?.AccessToken.Should().NotBeNullOrEmpty();
    }

    [Test]
    public async Task User_CannotLogin()
    {
        var client = Application.CreateClient();

        var result = await client.PostAsJsonAsync("api/auth", new TokenCommand
        {
            UserName = "test_user",
            Password = "123456"
        });

        FluentActions.Invoking(() => result.EnsureSuccessStatusCode())
            .Should().Throw<HttpRequestException>();
        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}
