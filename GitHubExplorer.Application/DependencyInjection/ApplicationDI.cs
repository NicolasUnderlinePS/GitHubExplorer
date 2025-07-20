using GitHubExplorer.Application.Features;
using GitHubExplorer.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace GitHubExplorer.Application.DependencyInjection
{
    public static class ApplicationDI
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IGitHubService, GitHubService>();

            return services;
        }
    }
}
