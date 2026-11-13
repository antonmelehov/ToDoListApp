using Microsoft.EntityFrameworkCore;
using TodoListApp.WebApi.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var connectionString = builder.Configuration.GetConnectionString("TodoListDb")
                       ?? throw new InvalidOperationException(
                           "Connection string 'TodoListDb' is missing.");

_ = builder.Services.AddDbContext<TodoListDbContext>(options => options.UseSqlServer(connectionString));

var app = builder.Build();

if (app.Configuration.GetValue<bool>("DemoSeed:Enabled"))
{
    if (!app.Environment.IsDevelopment())
    {
        throw new InvalidOperationException("Demo seeding requires Development.");
    }

    var inputPath = app.Configuration["DemoSeed:InputPath"]
                    ?? throw new InvalidOperationException("Specify DemoSeed:InputPath.");

    await using var scope = app.Services.CreateAsyncScope();
    var db = scope.ServiceProvider.GetRequiredService<TodoListDbContext>();

    await DemoDataSeeder.SeedAsync(db, inputPath);
    Console.WriteLine("Demo lists are ready.");
    return;
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    _ = app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
