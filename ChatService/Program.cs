#region usings
using ChatService.Hubs;
using ChatService.Interfaces.Services;
using ChatService.Services;
#endregion

#region Add services to the container.
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();

builder.Services.AddSingleton<IMessageService, MessageService>();
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

app.MapControllers();

app.MapHub<MessageHub>("/hub/messages");

app.Run();
#endregion