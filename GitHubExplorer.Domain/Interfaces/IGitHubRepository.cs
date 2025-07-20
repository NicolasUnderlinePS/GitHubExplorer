using GitHubExplorer.Domain.Entities;
using GitHubExplorer.Domain.Request;

namespace GitHubExplorer.Domain.Interfaces
{
    public interface IGitHubRepository
    {
        Task<GitHubStorage> GetAllAsync();
        Task<GitHubProject> GetOwnerProjectAsync(OwnerGitHubProjectRequest request);
        
    }
}
