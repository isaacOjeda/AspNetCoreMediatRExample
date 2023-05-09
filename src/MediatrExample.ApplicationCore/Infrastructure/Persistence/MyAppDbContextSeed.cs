using MediatrExample.ApplicationCore.Domain;
using Microsoft.AspNetCore.Identity;

namespace MediatrExample.ApplicationCore.Infrastructure.Persistence;
public class MyAppDbContextSeed
{
    public static async Task SeedDataAsync(MyAppDbContext context)
    {

        if (!context.Products.Any())
        {
            var random = new Random();

            var products = 
                Enumerable.Range(1, 100)
                .Select(s => new Product
                {
                    Description = $"Product {s}",
                    Price = random.NextInt64(250, 999)
                });

            context.AddRange(products);

            await context.SaveChangesAsync();
        }
    }
    public static async Task SeedUsersAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
        var testUser = await userManager.FindByNameAsync("test_user");
        if (testUser is null)
        {
            testUser = new User
            {
                UserName = "test_user"
            };

            await userManager.CreateAsync(testUser, "Passw0rd.1234");
            await userManager.CreateAsync(new User
            {
                UserName = "other_user"
            }, "Passw0rd.1234");
        }

        var adminRole = await roleManager.FindByNameAsync("Admin");
        if (adminRole is null)
        {
            await roleManager.CreateAsync(new IdentityRole
            {
                Name = "Admin"
            });

            await userManager.AddToRoleAsync(testUser, "Admin");
        }
    }
}
