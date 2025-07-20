using GitHubExplorer.Domain.Entities;
using GitHubExplorer.Domain.Interfaces;

namespace GitHubExplorer.Infrastructure.Repositories
{
    public class MemoryFavoriteRepository : IMemoryFavoriteRepository
    {
        private readonly List<GitHubProject> _favoriteProjects = new List<GitHubProject>(); 

        public async Task<bool> AddFavoriteAsync(GitHubProject gitHubProject)
        {
            try
            {
                _favoriteProjects.Add(gitHubProject);

                return _favoriteProjects.Any(p => p.Id == gitHubProject.Id);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> RemoveFavoriteAsync(long gitHubProjectId)
        {
            try
            {
                return _favoriteProjects.RemoveAll(p => p.Id == gitHubProjectId) == 0 ? false : true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task<List<GitHubProject>> GetAllFavoriteAsync()
        {
            try
            {
                if (_favoriteProjects == null)
                    return new List<GitHubProject>();
                else
                    return _favoriteProjects;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
