using GitHubExplorer.Application.DTOs.Project.Request;
using GitHubExplorer.Application.DTOs.Repository.Request;
using GitHubExplorer.Application.DTOs.Repository.Respone;
using GitHubExplorer.Domain.Entities;

namespace GitHubExplorer.Application.Interfaces
{
    public interface IGitHubService
    {
        Task<List<GitHubProjectResponseDto>> GetAllGitHubProjectAsync();
        Task<List<GitHubProjectResponseDto>> GetOwnerGitHubProjectAsync(OwnerGitHubProjectRequestDto request);
        Task<bool> FavoriteGitHubProjectAsync(long gitHubProjectId);
        Task<List<GitHubProjectResponseDto>> GetAllFavoriteGitHubProjectAsync();
        Task<List<GitHubProjectResponseDto>> GetGitHubProjectMostRelevantAsync(GitHubProjectRelevantRequestDto request);
    }
}
