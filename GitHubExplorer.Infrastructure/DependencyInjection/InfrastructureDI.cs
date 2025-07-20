using GitHubExplorer.Domain.Interfaces;
using GitHubExplorer.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GitHubExplorer.Infrastructure.DependencyInjection
{
    public static class InfrastructureDI
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IMemoryFavoriteRepository, MemoryFavoriteRepository>();
            services.AddHttpClient<IGitHubRepository, GitHubRepository>(client =>
            {
                client.BaseAddress = new System.Uri(GitHubRepository.GITHUB_API_BASE_URL);
            });

            return services;
        }
    }
}