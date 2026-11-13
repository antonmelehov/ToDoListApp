using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TodoListApp.WebApp.Data;

/// <summary>
/// Provides access to the application's Identity database.
/// </summary>
internal sealed class UsersDbContext : IdentityDbContext<IdentityUser>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UsersDbContext"/> class.
    /// </summary>
    /// <param name="options">The options for this database context.</param>
    public UsersDbContext(DbContextOptions<UsersDbContext> options)
        : base(options)
    {
    }
}
