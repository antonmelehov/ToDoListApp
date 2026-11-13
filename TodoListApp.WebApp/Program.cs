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
