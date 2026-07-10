using Hangfire;
using Hangfire.SqlServer;
using Serilog;
using Vistony.Worker.Api;
using Vistony.Worker.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Puerto del proyecto Vistony.Worker
//builder.WebHost.UseUrls("http://0.0.0.0:7879");

// Permite ejecutar la app como Servicio Windows
builder.Host.UseWindowsService();

// Serilog + Seq
builder.Host.UseSerilog((context, config) =>
{
    config
        .ReadFrom.Configuration(context.Configuration)
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .WriteTo.Seq(context.Configuration["Seq:Url"] ?? "http://SERVER06:5341");
});

builder.Services.AddControllers();
builder.Services.AddHttpClient();


// Dependencias de Infrastructure
builder.Services.AddInfrastructure(builder.Configuration);

// Hangfire + SQL Server
builder.Services.AddHangfire(config =>
{
    config.UseSqlServerStorage(
        builder.Configuration.GetConnectionString("HangfireConnection"),
        new SqlServerStorageOptions
        {
            PrepareSchemaIfNecessary = true
        });
});

// Servidor Hangfire solo para la cola vistony
builder.Services.AddHangfireServer(options =>
{
    options.ServerName = "Vistony.Worker";
    options.WorkerCount = 5;
    options.Queues = new[] { "vistony", "default" };
});

var app = builder.Build();

app.UseSerilogRequestLogging(options =>
{
    options.GetLevel = (httpContext, elapsed, ex) =>
    {
        var path = httpContext.Request.Path.Value ?? "";

        if (path.StartsWith("/swagger") || path.StartsWith("/hangfire"))
            return Serilog.Events.LogEventLevel.Verbose;

        return ex != null
            ? Serilog.Events.LogEventLevel.Error
            : Serilog.Events.LogEventLevel.Information;
    };
});

// Dashboard Hangfire
app.UseHangfireDashboard("/hangfire");

// Registrar jobs recurrentes
JobsConfiguration.RegisterJobs(builder.Configuration);

// Redirigir raíz hacia Hangfire
app.MapGet("/", context =>
{
    context.Response.Redirect("/hangfire");
    return Task.CompletedTask;
});

app.MapControllers();

app.Run();