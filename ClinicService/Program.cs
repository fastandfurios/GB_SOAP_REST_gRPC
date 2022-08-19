#region references
using ClinicService.Data.Infrastructure.Contexts;
using ClinicService.Interfaces.Repositories;
using ClinicService.Services;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using NLog.Web;
using System.Net;
using System.Text;
using ClinicService;
using ClinicService.Extensions;
using ClinicService.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
#endregion

#region Add services to the container.
var builder = WebApplication.CreateBuilder(args);

var password = builder.Configuration.GetSection("PasswordCertificate");
var path = builder.Configuration.GetSection("PathCertificate");
AuthenticateService.SecretKey = builder.Configuration.GetSection("SecretKey").Value;
PasswordUtils.SecretKey = builder.Configuration.GetSection("Secret_Key").Value;

builder.WebHost.ConfigureKestrel(options =>
{
    options.Listen(IPAddress.Any, 5001, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http2;
        listenOptions.UseHttps(path.Value, password.Value);
    });
});

builder.Services.AddDbContext<ClinicServiceDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration["Settings:DatabaseOptions:ConnectionString"]);
});

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

builder.Services.AddGrpc();

builder.Services.AddSingleton<IAuthenticateService, AuthenticateService>();

builder.Services.AddScoped<IPetRepository, PetRepository>();
builder.Services.AddScoped<IConsultationRepository, ConsultationRepository>();
builder.Services.AddScoped<IClientRepository, ClientRepository>();

builder.Services.AddControllers();

builder.Services.AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme =
            JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme =
            JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(x =>
    {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new
            TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(AuthenticateService.SecretKey)),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            };
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMapper();
#endregion

#region Configure the HTTP request pipeline.
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseWhen(
    ctx => ctx.Request.ContentType != "application/grpc",
    config =>
    {
        config.UseHttpLogging();
    }
);

app.UseRouting();
app.MapControllers();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    // Communication with gRPC endpoints must be made through a gRPC client.
    // To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909
    endpoints.MapGrpcService<ClientService>();
    endpoints.MapGrpcService<PetService>();
    endpoints.MapGrpcService<ConsultationService>();
    endpoints.MapGrpcService<AuthService>();
});

app.Run();
#endregion