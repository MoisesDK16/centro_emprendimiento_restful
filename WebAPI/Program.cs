using Application;
using Application.Interfaces;
using Application.Services;
using Application.Services.PermissionS;
using Identity;
using Identity.Services;
using Persistence;
using Shared.Services;
using System.Text.Json.Serialization;
using WebAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddApiVersioningExtension();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplicationLayer();
builder.Services.AddPersistenceInfrastructure(builder.Configuration);
builder.Services.AddIdentityInfrastructure(builder.Configuration);
builder.Services.AddSharedInfrastructure(builder.Configuration);
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddScoped<IAzureStorageService, AzureStorageService>();
builder.Services.AddScoped<IUserService, UserServiceImplementation>();
builder.Services.AddScoped<IPermissionService, PermissionService>();

builder.Services.AddCors(options =>
{   
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin() // Permite cualquier origen
              .AllowAnyMethod()  // Permite cualquier método (GET, POST, PUT, DELETE, etc.)
              .AllowAnyHeader()); // Permite cualquier cabecera
});


var app = builder.Build();

await app.Services.SeedDatabaseAsync();
//GlobalServiceProvider.Instance = app.Services;

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseErrorHandlingMiddleware();

app.MapControllers();

app.Run();
