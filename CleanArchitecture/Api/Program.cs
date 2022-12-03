global using Application.DTOs;
global using Application.Responses;
using Application;
using Identity;
using Infrastructure;
using Microsoft.OpenApi.Models;
using Persistence;


var builder = WebApplication.CreateBuilder();
var services = builder.Services;
var configuration = builder.Configuration;


services.AddHttpContextAccessor();

#region Swagger Services

builder.Services.AddSwaggerGen(x =>
{
    x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Description = "JWT Authorization",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    x.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme()
            {
                Reference = new OpenApiReference()
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
         new List<string>()
        }
    });

    x.SwaggerDoc("v1", new OpenApiInfo { Title = "CleanAPI", Version = "v1" });
});

#endregion


#region Other projects services

services.ConfigureApplicationServices();
services.ConfigureInfrastructureServices(configuration);
services.ConfigurePersistenceServices(configuration);
services.ConfigureIdentityServices(configuration);

#endregion

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();


services.AddCors(x =>
{
    x.AddPolicy("CorsPolicy", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(x =>
    {
        x.SwaggerEndpoint("/swagger/v1/swagger.json", "CleanAPI v1");
    });
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors("CorsPolicy");

app.MapControllers();

app.Run();
