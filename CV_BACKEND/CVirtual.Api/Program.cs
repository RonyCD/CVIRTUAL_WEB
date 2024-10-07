using Autofac.Extensions.DependencyInjection;
using Autofac;
using CVirtual.CrossCutting;
using CVirtual.Application.Utils;
using CVirtual.Api.Extensions;
using AutoMapper;
using CVirtual.Map;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using CVirtual.Application.Configurations;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Mapper
var mappingConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new UsuarioMap());    
});
IMapper mapper = mappingConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

// CORS
var valuesSection = configuration
    .GetSection("Cors:AllowSpecificOrigins")
    .GetChildren()
    .ToList()
    .Select(x => (x.GetValue<string>("origin"))).ToList();

builder.Services.AddCors(options =>
{
    options.AddPolicy("_AllowSpecificOrigins",
        builder => builder.WithOrigins(valuesSection.ToArray())
                            .AllowAnyHeader()
                            .AllowAnyMethod());
    options.AddPolicy("_AllowAllOrigins",
        builder => builder.AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod());
});

// JWT Authentication Configuration
var tokenConfigurations = configuration.GetSection("TokenConfigurations").Get<TokenConfigurations>();
builder.Services.AddSingleton(tokenConfigurations); // Opcional: para inyectar en servicios

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = tokenConfigurations.Issuer,
            ValidAudience = tokenConfigurations.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenConfigurations.SecretKey))
        };
    });

// Servicios adicionales
builder.Services.AddAppInsight(configuration)
                .AddCustomMVC(configuration)
                .AddCustomSwagger(configuration)
                .AddCustomHealthChecks(configuration)
                .AddCustomSecuritySwagger(configuration)
                .AddCustomIntegrations(configuration);

// Inyección de dependencias
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule(new ContextDbModule(configuration)));
builder.Host.ConfigureContainer<ContainerBuilder>(builder => builder.RegisterType<GlobalVariables>().AsSelf().SingleInstance());

var app = builder.Build();

// Configuración del pipeline
app.UseCors("_AllowSpecificOrigins");

app.UseHttpsRedirection();

// Activar la autenticación y autorización JWT en el pipeline
app.UseAuthentication();
app.UseAuthorization();

// Swagger en desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var pathBase = configuration["PATH_BASE"];
var swagger = configuration.GetSection("IIS:Site").Value.ToString();
if (swagger == "")
    swagger = "/swagger/v1/swagger.json";
else
    swagger = "/" + swagger + "/swagger/v1/swagger.json";

app.UseSwagger()
   .UseSwaggerUI(c =>
   {
       c.SwaggerEndpoint($"{(!string.IsNullOrEmpty(pathBase) ? pathBase : string.Empty)}" + swagger, "CVirtual.Api");
       c.OAuthClientId("CartaVirtualApi");
       c.OAuthAppName("CVirtual Api");
   });

app.MapControllers();

app.Run();
