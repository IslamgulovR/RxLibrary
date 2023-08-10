using Microsoft.EntityFrameworkCore;
using RxGroup.Configurations;
using RxGroup.Features.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

var connectionString = configuration["DB_CONNECTION_STRING"];
if (connectionString == null)
    throw new Exception("DB connection string is empty");

builder.Host.UseSerilog((_, lc) => lc
    .WriteTo.Console()
    .WriteTo.Seq(Environment.GetEnvironmentVariable("SEQ") ?? string.Empty));

builder.Services.AddControllers();

builder.Services.AddDbContext<PgSqlContext>(opts => opts.UseNpgsql(connectionString));
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddTransient<BooksIssuanceService>();
builder.Services.AddTransient<BooksService>();
builder.Services.AddTransient<ReadersService>();

var app = builder.Build();

app.MapControllers();
app.UseRouting();
app.ConfigureLogging();

try
{
    app.Run();
}
catch (Exception e)
{
    Console.WriteLine(e);
    throw;
}