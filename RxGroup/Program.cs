using Microsoft.EntityFrameworkCore;
using RxGroup.Configurations;
using RxGroup.Features.Books.Services;
using RxGroup.Features.BooksIssuance.Services;
using RxGroup.Features.Readers.Services;
using Serilog;
using Serilog.Enrichers.Span;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Default");
if (connectionString == null)
    throw new Exception("DB connection string is empty");

builder.Host.UseSerilog((context, services, lc) => lc
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext()
    .Enrich.WithSpan()
    .WriteTo.Console());

builder.Services.AddControllers();

builder.Services.AddDbContext<PgSqlContext>(opts => opts.UseNpgsql(connectionString));
builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddTransient<BooksIssuanceService>();
builder.Services.AddTransient<BooksService>();
builder.Services.AddTransient<ReadersService>();

var app = builder.Build();

app.MapControllers();
app.UseRouting();

try
{
    app.Run();
}
catch (Exception e)
{
    Console.WriteLine(e);
    throw;
}