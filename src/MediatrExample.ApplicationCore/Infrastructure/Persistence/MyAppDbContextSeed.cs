using MediatrExample.ApplicationCore.Domain;
using Microsoft.AspNetCore.Identity;

namespace MediatrExample.ApplicationCore.Infrastructure.Persistence;
public class MyAppDbContextSeed
{
    public static async Task SeedDataAsync(MyAppDbContext context)
    {

        if (!context.Products.Any())
        {
            context.Products.AddRange(new List<Product>
        {
            new Product
            {
                Description = "Product 01",
                Price = 16000
            },
            new Product
            {
                Description = "Product 02",
                Price = 52200
            }
        });

            await context.SaveChangesAsync();
        }
    }
    public static async Task SeedUsersAsync(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        var testUser = await userManager.FindByNameAsync("test_user");
        if (testUser is null)
        {
            testUser = new IdentityUser
            {
                UserName = "test_user"
            };

            await userManager.CreateAsync(testUser, "Passw0rd.1234");
            await userManager.CreateAsync(new IdentityUser
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
