using FluentValidation;
using FluentValidation.AspNetCore;
using HireLink.Api.Middleware;
using HireLink.Data.Contexts;
using HireLink.Data.IRepositories;
using HireLink.Data.Repositories;
using HireLink.Service.Interfaces;
using HireLink.Service.Mappers;
using HireLink.Service.Services;
using HireLink.Service.Validators;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Logging
builder.Logging.ClearProviders();
builder.Host.UseSerilog((context, loggerConfig) =>
    loggerConfig.ReadFrom.Configuration(context.Configuration));

// Contexts
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Fluent validation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<CandidateUpsertDtoValidator>();

// Mappers
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Repositories
builder.Services.AddScoped<ICandidateRepository, CandidateRepository>();

// Caching
builder.Services.AddMemoryCache();
builder.Services.Decorate<ICandidateRepository, CachedCandidateRepository>();

// Services
builder.Services.AddScoped<ICandidateService, CandidateService>();

// Controllers
builder.Services.AddControllers();

// Swagger Configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<ExceptionMiddleware>();
app.UseAuthorization();

app.MapControllers();

#region Applying migration to the database

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
var context = services.GetRequiredService<AppDbContext>();
var logger = services.GetRequiredService<ILogger<Program>>();
try
{
    await context.Database.MigrateAsync();
}
catch (Exception ex)
{
    logger.LogError(ex, "An error occured during migration");
}

#endregion

app.Run();
