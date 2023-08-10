using RxGroup.Extensions;
using Serilog;
using Serilog.Events;

namespace RxGroup.Configurations;

public static class SerilogRequestLogging
{
    public static void ConfigureLogging(this IApplicationBuilder app)
    {
        app.Use((context, next) =>
        {
            context.Request.EnableBuffering();

            return next();
        });
        
        app.UseSerilogRequestLogging(options =>
        {
            options.EnrichDiagnosticContext = OptionsEnrichDiagnosticContext;
        });
        
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("System", LogEventLevel.Warning)
            .Enrich.WithProperty("Environment", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") 
                                                ?? throw new Exception("Environment is empty"))
            .WriteTo.Seq(Environment.GetEnvironmentVariable("SEQ") ?? throw new InvalidOperationException())
            .CreateLogger();
    }
    
    private static async void OptionsEnrichDiagnosticContext(IDiagnosticContext diagnosticContext, HttpContext httpContext)
    {
        // Body
        try
        {
            if (!httpContext.Request.IsRequestBodyTextOrJson()) return;
            
            httpContext.Request.Body.Seek(0, SeekOrigin.Begin);
                
            using var reader = new StreamReader(httpContext.Request.Body);
            var body = await reader.ReadToEndAsync();
 
            diagnosticContext.Set("RequestBody", body);
        }
        catch (Exception e)
        {
            diagnosticContext.Set("RequestBodyFieldException", e);
        }
    }
}