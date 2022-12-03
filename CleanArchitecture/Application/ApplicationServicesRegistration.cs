using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application;


public static class ApplicationServicesRegistration
{
    public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        //services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        services.AddMediatR(Assembly.GetExecutingAssembly());
        //services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());

        return services;
    }
}
