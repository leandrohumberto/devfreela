using DevFreela.Application.Commands.CreateProject;
using DevFreela.Application.Validators;
using DevFreela.Core.Repositories;
using DevFreela.Infrastructure.Persistence.Repositories;
using FluentValidation.AspNetCore;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NLog;
using DevFreela.API.Filters;
using DevFreela.Core.Services;
using DevFreela.Infrastructure.Auth;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using DevFreela.Infrastructure.Logging;
using DevFreela.API.Extensions;
using DevFreela.Infrastructure.Persistence;
using System.Text.Json.Serialization;
using DevFreela.Infrastructure.Payments;
using DevFreela.Infrastructure.MessageBus;
using DevFreela.Application.Consumers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(options => options.Filters.Add(typeof(ValidationFilter)))
    .AddJsonOptions(x =>
    {
        // Serialize enums as strings in API responses
        x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<CreateUserCommandValidator>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Configure Swagger
builder.Services.AddSwaggerGen( c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "DevFreela.API", Version = "v1" });

    // Add JWT security definition
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header usando o schema Bearer."
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                  Type = ReferenceType.SecurityScheme,
                  Id = "Bearer",
                }
            },
            Array.Empty<string>()
        }
    });
});

// Configure JWT authentication scheme
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        };
    });

var connectionString = builder.Configuration.GetConnectionString("DevFreelaCs");
//builder.Services.AddDbContext<DevFreelaDbContext>(p => p.UseSqlServer(connectionString));
builder.Services.AddDbContext<DevFreelaDbContext>(p => p.UseInMemoryDatabase("DevFreela"));
//builder.Services.AddScoped<IProjectService, ProjectService>();
//builder.Services.AddScoped<IUserService, UserService>();
//builder.Services.AddScoped<ISkillService, SkillService>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<ISkillRepository, SkillRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddSingleton<ILoggerService, LoggerService>();
builder.Services.AddSingleton<IMessageBusService, MessageBusService>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateProjectCommand).Assembly));
builder.Services.AddHttpClient();
builder.Services.AddHostedService<PaymentApprovedConsumer>();

// Configure Logger Service
LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

var app = builder.Build();

// Configure exception hadling
var logger = app.Services.GetRequiredService<ILoggerService>();
app.ConfigureExceptionHandler(logger);
app.ConfigureCustomExceptionMiddleware();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DevFreela.API v1"));

    app.ConfigureExceptionHandler(logger);
    app.ConfigureCustomExceptionMiddleware();
}

app.UseHttpsRedirection();

// Use authorization and authentication
// UseAuthentication method MUST be called BEFORE UseAuthorization, otherwise token validation will FAIL
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
