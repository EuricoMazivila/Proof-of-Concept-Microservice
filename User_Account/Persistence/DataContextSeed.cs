using System.Text.Json;
using Domain;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Persistence;

public class DataContextSeed
{
    public static async Task SeedAsync(DataContext context, ILoggerFactory loggerFactory,
        UserManager<AppUser> userManager, IWebHostEnvironment env)
    {
        var path = env.IsDevelopment() ? "../Persistence/SeedData/" : "./SeedData";

        try
        {
            if (!userManager.Users.Any())
            {
                // Hack: Admin User
                var adminData = await File.ReadAllTextAsync($"{path}/adminSeed.json");
                var admin = JsonSerializer.Deserialize<List<AppUser>>(adminData);

                foreach (var item in admin!)
                {
                    await userManager.CreateAsync(item, "Pa$$w0rd");
                }
            }
        }
        catch (Exception e)
        {
            var logger = loggerFactory.CreateLogger<DataContext>();
            logger.LogError(e, e.Message);
        }
    }
}