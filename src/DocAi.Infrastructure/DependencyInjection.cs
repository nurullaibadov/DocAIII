using DocAi.Core.Interfaces;
using DocAi.Infrastructure.Repositories;
using DocAi.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DocAi.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IDocService, DocService>();
        return services;
    }
}
