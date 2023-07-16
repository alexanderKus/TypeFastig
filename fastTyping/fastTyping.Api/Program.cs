using fastTyping.Api.Extensions;
using fastTyping.FastTypingHub;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
// Add services to the container.

builder.Services.AddCustomServices();
builder.Services.AddCustomCors(config);
builder.Services.AddCustomSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddSignalR();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");
app.UseAuthorization();

app.MapControllers();
app.MapHub<TypingHub>("/typinghHub");

app.Run();

