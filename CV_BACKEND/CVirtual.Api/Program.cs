

using Autofac.Extensions.DependencyInjection;
using Autofac;
using CVirtual.CrossCutting;
using CVirtual.Application.Utils;
using CVirtual.Api.Extensions;
using AutoMapper;
using CVirtual.Map;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//MAPPER

var mappingConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new CuentaUsuarioMap());
    
});

IMapper mapper = mappingConfig.CreateMapper();
builder.Services.AddSingleton(mapper);



//CORS
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
                            .AllowAnyMethod()
    );
    options.AddPolicy("_AllowAllOrigins",
        builder => builder.AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod()
    );
});

builder.Services.AddAppInsight(configuration)
                .AddCustomMVC(configuration)
                .AddCustomSwagger(configuration)
                .AddCustomHealthChecks(configuration)
                .AddCustomSecuritySwagger(configuration)
                .AddCustomIntegrations(configuration);


//INYECTION DEPENDENCY
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule(new ContextDbModule(configuration)));
builder.Host.ConfigureContainer<ContainerBuilder>(builder => builder.RegisterType<GlobalVariables>().AsSelf().SingleInstance());


var app = builder.Build();

app.UseCors("_AllowSpecificOrigins");

app.UseHttpsRedirection();

app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

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
