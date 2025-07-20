using GitHubExplorer.Domain.Entities;

namespace GitHubExplorer.Domain.Interfaces
{
    public interface IMemoryFavoriteRepository
    {
        Task<bool> AddFavoriteAsync(GitHubProject gitHubProject);
        Task<bool> RemoveFavoriteAsync(long gitHubProjectId);
        Task<List<GitHubProject>> GetAllFavoriteAsync();
    }
}
