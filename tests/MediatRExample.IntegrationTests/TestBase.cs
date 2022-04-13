using MediatR;
using MediatrExample.ApplicationCore.Features.Auth;
using MediatrExample.ApplicationCore.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Linq.Expressions;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MediatRExample.IntegrationTests;

public class TestBase
{
    protected ApiWebApplicationFactory Application;

    /// <summary>
    /// Crea un usuario de prueba según los parámetros
    /// </summary>
    /// <returns></returns>
    public async Task<(HttpClient Client, string UserId)> CreateTestUser(string userName, string password, string[] roles)
    {
        using var scope = Application.Services.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

        var newUser = new IdentityUser(userName);

        await userManager.CreateAsync(newUser, password);

        foreach (var role in roles)
        {
            await userManager.AddToRoleAsync(newUser, role);
        }

        var accessToken = await GetAccessToken(userName, password);

        var client = Application.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        return (client, newUser.Id);
    }

    /// <summary>
    /// Al terminar cada prueba, se resetea la base de datos
    /// </summary>
    /// <returns></returns>
    [TearDown]
    public async Task Down()
    {
        await ResetState();
    }

    /// <summary>
    /// Crea un HttpClient incluyendo un JWT válido con usuario Admin
    /// </summary>
    public Task<(HttpClient Client, string UserId)> GetClientAsAdmin() =>
        CreateTestUser("user@admin.com", "Pass.W0rd", new string[] { "Admin" });

    /// <summary>
    /// Crea un HttpClient incluyendo un JWT válido con usuario default
    /// </summary>
    public Task<(HttpClient Client, string UserId)> GetClientAsDefaultUserAsync() =>
        CreateTestUser("user@normal.com", "Pass.W0rd", Array.Empty<string>());

    /// <summary>
    /// Libera recursos al terminar todas las pruebas
    /// </summary>
    [OneTimeTearDown]
    public void RunAfterAnyTests()
    {
        Application.Dispose();
    }

    /// <summary>
    /// Inicializa la API y la BD antes de iniciar las pruebas
    /// </summary>
    [OneTimeSetUp]
    public void RunBeforeAnyTests()
    {
        Application = new ApiWebApplicationFactory();

        EnsureDatabase();
    }

    /// <summary>
    /// Shortcut para ejecutar IRequests con el Mediador
    /// </summary>
    public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
    {
        using var scope = Application.Services.CreateScope();

        var mediator = scope.ServiceProvider.GetRequiredService<ISender>();

        return await mediator.Send(request);
    }

    /// <summary>
    /// Shortcut para agregar Entities a la BD
    /// </summary>
    protected async Task<TEntity> AddAsync<TEntity>(TEntity entity) where TEntity : class
    {
        using var scope = Application.Services.CreateScope();

        var context = scope.ServiceProvider.GetService<MyAppDbContext>();

        context.Add(entity);

        await context.SaveChangesAsync();

        return entity;
    }

    /// <summary>
    /// Shortcut para buscar entities por primary key
    /// </summary>
    protected async Task<TEntity> FindAsync<TEntity>(params object[] keyValues) where TEntity : class
    {
        using var scope = Application.Services.CreateScope();

        var context = scope.ServiceProvider.GetService<MyAppDbContext>();

        return await context.FindAsync<TEntity>(keyValues);
    }

    /// <summary>
    /// Shortcut para buscar entities según un criterio
    /// </summary>
    protected async Task<TEntity> FindAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
    {
        using var scope = Application.Services.CreateScope();

        var context = scope.ServiceProvider.GetService<MyAppDbContext>();

        return await context.Set<TEntity>().FirstOrDefaultAsync(predicate);
    }

    /// <summary>
    /// Se asegura de crear la BD
    /// </summary>
    private void EnsureDatabase()
    {
        using var scope = Application.Services.CreateScope();
        var context = scope.ServiceProvider.GetService<MyAppDbContext>();

        context.Database.EnsureCreated();
    }

    /// <summary>
    /// Shortcut para autenticar un usuario para pruebas
    /// </summary>
    private async Task<string> GetAccessToken(string userName, string password)
    {
        using var scope = Application.Services.CreateScope();

        var result = await SendAsync(new TokenCommand
        {
            UserName = userName,
            Password = password
        });

        return result.AccessToken;
    }
    /// <summary>
    /// Se asegura de limpiar la BD
    /// </summary>
    /// <returns></returns>
    private async Task ResetState()
    {
        using var scope = Application.Services.CreateScope();
        var context = scope.ServiceProvider.GetService<MyAppDbContext>();

        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        await MyAppDbContextSeed.SeedDataAsync(context);
    }
}