using Main.ApiRequest;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOpenTelemetry().WithMetrics(metrics => {
    metrics.AddMeter("Microsoft.AspNetCore.Hosting");
    metrics.AddMeter("Microsoft.AspNetCore.Server.Kestrel");
    metrics.AddMeter("System.Net.Http");
    metrics.AddPrometheusExporter();

    // This is for Aspire .NET 
    // metrics.AddOtlpExporter();
});

builder.Logging.AddOpenTelemetry(options => {
    options.AddOtlpExporter();
});

builder.Services.AddApiRequestModule();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();
app.MapPrometheusScrapingEndpoint();
app.Run();
