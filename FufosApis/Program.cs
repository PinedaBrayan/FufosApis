using Appointment.SDK.Backend.Configuration;
using FufosApis.Database;
using FufosApis.Services;
using FufosEntities.DTOS;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddGlobalConfiguration<FufosContext>(builder.Configuration);
builder.Services.AddScoped<IJWTService, JWTService>();
builder.Services.Configure<TokenConfiguration>(builder.Configuration.GetSection("Authentication"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
