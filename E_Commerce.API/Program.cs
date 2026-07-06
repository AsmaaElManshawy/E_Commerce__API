using E_Commerce.API;
using E_Commerce.Application;
using E_Commerce.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(); // Project APIs

#region Add Services

builder.Services.AddInfrastructureService(builder.Configuration); // Infrastructure Layer

builder.Services.AddApplicationService(); // Application Layer

#endregion


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

#region seed and migrate data

await app.SeedAndMigrateDataAsync();

#endregion

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
