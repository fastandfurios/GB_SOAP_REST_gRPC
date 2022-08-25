#region usings
using Microsoft.AspNetCore.HttpLogging;
using NLog.Web;
using Polly;
using SampleService.Interfaces;
using SampleService.Services.Clients;
#endregion

#region Add services to the container.
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.All | HttpLoggingFields.RequestQuery;
    logging.RequestBodyLogLimit = 4096;
    logging.ResponseBodyLogLimit = 4096;
    logging.RequestHeaders.Add("Authorization");
    logging.RequestHeaders.Add("X-Real-IP");
    logging.RequestHeaders.Add("X-Forwarded-For");
});

builder.Host.ConfigureLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();

}).UseNLog(new NLogAspNetCoreOptions() { RemoveLoggerFactoryFilter = true });

builder.Services
    .AddHttpClient<IRootServiceClient, RootServiceClient>("RootServiceClient")
    .AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(retryCount: 3,
        sleepDurationProvider: (attemptCount) => TimeSpan.FromSeconds(attemptCount * 2),
        onRetry: (response, sleepDuration, attemptNumber, context) =>
        {
            var logger = builder.Services.BuildServiceProvider().GetService<ILogger<Program>>();

            logger.LogError(response.Exception != null ? response.Exception :
                new Exception($"\n{response.Result.StatusCode}: {response.Result.RequestMessage}"), $"(attempt: {attemptNumber} ) RootServiceClient request exception.");
        }));

builder.Services.AddHttpClientLogging();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
#endregion

#region Configure the HTTP request pipeline.
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.UseHttpLogging();
app.MapControllers();

app.Run();
#endregion