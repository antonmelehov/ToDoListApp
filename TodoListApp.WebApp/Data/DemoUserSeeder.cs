using System.Text.Json;
using Microsoft.AspNetCore.Identity;

namespace TodoListApp.WebApp.Data;

/// <summary>
/// Creates development accounts and exports their Identity identifiers.
/// </summary>
internal static class DemoUserSeeder
{
    public static async Task SeedAsync(
        UserManager<IdentityUser> userManager,
        string password,
        string outputPath)
    {
        var alice = await GetOrCreateUserAsync(userManager, "alice@example.test", password);
        var bob = await GetOrCreateUserAsync(userManager, "bob@example.test", password);
        var carol = await GetOrCreateUserAsync(userManager, "carol@example.test", password);

        var userIds = new
        {
            AliceId = alice.Id,
            BobId = bob.Id,
            CarolId = carol.Id,
        };

        await File.WriteAllTextAsync(outputPath, JsonSerializer.Serialize(userIds));
    }

    private static async Task<IdentityUser> GetOrCreateUserAsync(
        UserManager<IdentityUser> userManager,
        string email,
        string password)
    {
        var existingUser = await userManager.FindByEmailAsync(email);
        if (existingUser is not null)
        {
            return existingUser;
        }

        var user = new IdentityUser
        {
            UserName = email,
            Email = email,
        };

        var result = await userManager.CreateAsync(user, password);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(error => error.Description));
            throw new InvalidOperationException(
                $"Could not create demo account '{email}': {errors}");
        }

        return user;
    }
}
