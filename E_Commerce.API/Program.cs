using E_Commerce.API;
using E_Commerce.Application;
using E_Commerce.Application.Profiles;
using E_Commerce.Infrastructure;
using E_Commerce.Infrastructure.Identity.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(); // Project APIs

#region Add Services

builder.Services.AddInfrastructureService(builder.Configuration); // Infrastructure Layer

builder.Services.AddApplicationService(); // Application Layer

builder.Services.Configure<UrlSettings>(builder.Configuration.GetSection("UrlSettings"));
builder.Services.Configure<JWTSettings>(builder.Configuration.GetSection("JWT"));

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
