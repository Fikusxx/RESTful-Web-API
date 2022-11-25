using WebAPI;
using Microsoft.EntityFrameworkCore;
using WebAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Serialization;
using WebAPI.Services.MappingService;
using Microsoft.AspNetCore.Mvc.Formatters;


var builder = WebApplication.CreateBuilder();
var services = builder.Services;
var configuration = builder.Configuration;

#region EF Connection

var connection = configuration.GetConnectionString("DefaultConnection");
services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connection));

#endregion

#region AutoMapper

services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

#endregion


services.AddHttpCacheHeaders(
expirationOptions =>
{
    expirationOptions.MaxAge = 60;
    expirationOptions.CacheLocation = Marvin.Cache.Headers.CacheLocation.Public;
},
validationOptions =>
{
    validationOptions.MustRevalidate = true;
});
services.AddResponseCaching();

services.AddControllers(options =>
{
    options.CacheProfiles.Add("240SecondsCacheProfile", new CacheProfile() { Duration = 240 });
}).AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
    })
    .AddXmlDataContractSerializerFormatters()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var details = new ValidationProblemDetails(context.ModelState)
            {
                Type = "http://url",
                Title = "One or more model validation errors occured",
                Status = StatusCodes.Status422UnprocessableEntity,
                Detail = "See the errors property for details",
                Instance = context.HttpContext.Request.Path
            };

            details.Extensions.Add("traceId", context.HttpContext.TraceIdentifier);

            return new UnprocessableEntityObjectResult(details)
            {
                ContentTypes = { "application/problem+json" }
            };
        };
    });

services.Configure<MvcOptions>(options =>
{
    var newtonsoftJsonOutputFormatter = options.OutputFormatters.OfType<NewtonsoftJsonOutputFormatter>()?.FirstOrDefault();

    if (newtonsoftJsonOutputFormatter != null)
    {
        newtonsoftJsonOutputFormatter.SupportedMediaTypes.Add("application/vnd.marvin.hateoas+json");
    }
});

services.AddScoped<IRepository, Repository>();
services.AddTransient<IPropertyMappingService, PropertyMappingService>();
services.AddTransient<IPropertyCheckerService, PropertyCheckerService>();

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler(appBuilder =>
    {
        appBuilder.Run(async context =>
        {
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync("Error occured");
        });
    });
}

//app.UseResponseCaching();
app.UseHttpCacheHeaders();

app.UseRouting();
app.UseStaticFiles();
//app.UseHttpsRedirection();


app.MapControllers();

app.Run();