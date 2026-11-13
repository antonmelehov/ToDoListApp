using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TodoListApp.WebApp.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var usersConnectionString = builder.Configuration.GetConnectionString("UsersDb")
                            ?? throw new InvalidOperationException(
                                "Connection string 'UsersDb' is missing.");

_ = builder.Services.AddDbContext<UsersDbContext>(options =>
    options.UseSqlServer(usersConnectionString));

_ = builder.Services.AddIdentityCore<IdentityUser>()
    .AddEntityFrameworkStores<UsersDbContext>();


var app = builder.Build();


if (app.Configuration.GetValue<bool>("DemoSeed:Enabled"))
{
    if (!app.Environment.IsDevelopment())
    {
        throw new InvalidOperationException("Demo seeding requires Development.");
    }

    var password = app.Configuration["DemoSeed:Password"]
                   ?? throw new InvalidOperationException("Set DemoSeed:Password using .NET User Secrets.");

    var outputPath = app.Configuration["DemoSeed:OutputPath"]
                     ?? throw new InvalidOperationException("Specify DemoSeed:OutputPath.");

    await using var scope = app.Services.CreateAsyncScope();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

    await DemoUserSeeder.SeedAsync(userManager, password, outputPath);
    Console.WriteLine($"Demo user IDs written to {outputPath}");
    return;
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    _ = app.UseExceptionHandler("/Home/Error");
    _ = app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
