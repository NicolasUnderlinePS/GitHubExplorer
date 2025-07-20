using GitHubExplorer.Application.DTOs.Project.Request;
using GitHubExplorer.Application.DTOs.Repository.Request;
using GitHubExplorer.Application.DTOs.Repository.Respone;
using GitHubExplorer.Application.Interfaces;
using GitHubExplorer.Domain.Entities;
using GitHubExplorer.Domain.Enums;
using GitHubExplorer.Domain.Interfaces;
using GitHubExplorer.Domain.Request;
using System.Collections.Generic;

namespace GitHubExplorer.Application.Features
{
    public class GitHubService : IGitHubService
    {
        private readonly IGitHubRepository _gitHubRepository;
        private readonly IMemoryFavoriteRepository _memoryFavoriteRepository;

        public GitHubService(IGitHubRepository gitHubRepository, IMemoryFavoriteRepository memoryFavoriteRepository)
        {
            _gitHubRepository = gitHubRepository;
            _memoryFavoriteRepository = memoryFavoriteRepository;
        }

        public async Task<bool> FavoriteGitHubProjectAsync(long gitHubProjectId)
        {
            bool respone = false;
            
            try
            {
                List<GitHubProject> memoryGitHubProjects = await _memoryFavoriteRepository.GetAllFavoriteAsync();

                if (memoryGitHubProjects.Any(e => e.Id == gitHubProjectId))
                {
                    respone = await _memoryFavoriteRepository.RemoveFavoriteAsync(gitHubProjectId);
                }
                else
                {
                    GitHubProject gitHubProject = await GetGitHubProjectByIdAsync(gitHubProjectId);

                    if (gitHubProject != null)
                        respone = await _memoryFavoriteRepository.AddFavoriteAsync(gitHubProject);
                }

            }
            catch (Exception)
            {
                respone = false;
            }

            return respone;
        }

        public async Task<List<GitHubProjectResponseDto>> GetAllFavoriteGitHubProjectAsync()
        {
            try
            {

                List<GitHubProject> gitHubStorageRaw = await _memoryFavoriteRepository.GetAllFavoriteAsync();

                return gitHubStorageRaw.Select(s => MapperGitHubRepoResponseDto(s)).ToList();
            }
            catch (Exception)
            {

                return new List<GitHubProjectResponseDto>();
            }
        }

        public async Task<List<GitHubProjectResponseDto>> GetAllGitHubProjectAsync()
        {
            try
            {
                GitHubStorage gitHubStorageRaw = await _gitHubRepository.GetAllAsync();

                return gitHubStorageRaw.Items.Select(s => MapperGitHubRepoResponseDto(s)).ToList();
            }
            catch (Exception)
            {
                return new List<GitHubProjectResponseDto>();
            }
        }

        public async Task<List<GitHubProjectResponseDto>> GetGitHubProjectMostRelevantAsync(GitHubProjectRelevantRequestDto request)
        {
            try
            {
                GitHubStorage gitHubStorageRaw = await _gitHubRepository.GetAllAsync();

                List<GitHubProjectResponseDto> gitHubProjects = gitHubStorageRaw.Items.Select(s => MapperGitHubRepoResponseDto(s)).ToList();

                return ApplyRelevantOrder(gitHubProjects, request);
            }
            catch (Exception)
            {
                return new List<GitHubProjectResponseDto>();
            }
        }

        public async Task<List<GitHubProjectResponseDto>> GetOwnerGitHubProjectAsync(OwnerGitHubProjectRequestDto request)
        {
            List<GitHubProjectResponseDto> gitHubProjects = new List<GitHubProjectResponseDto>();

            try
            {
                if (String.IsNullOrWhiteSpace(request.OwnerName) || string.IsNullOrWhiteSpace(request.GitHubProjectName))
                {
                    gitHubProjects = await GetAllGitHubProjectAsync();

                    if (String.IsNullOrWhiteSpace(request.OwnerName) == false)
                        gitHubProjects = gitHubProjects.Where(e => e.OwnerName == request.OwnerName).ToList();

                    if (String.IsNullOrWhiteSpace(request.GitHubProjectName) == false)
                        gitHubProjects = gitHubProjects.Where(e => e.NameProject.Contains(request.GitHubProjectName)).ToList();
                }
                else
                {
                    OwnerGitHubProjectRequest requestGitHubRepo = MapperOwnerGitHubProjectRequest(request);

                    GitHubProject gitHubProjectRaw = await _gitHubRepository.GetOwnerProjectAsync(requestGitHubRepo);

                    gitHubProjects = new List<GitHubProjectResponseDto>();

                    if (gitHubProjectRaw == null)
                        return gitHubProjects;

                    gitHubProjects.Add(MapperGitHubRepoResponseDto(gitHubProjectRaw));
                }

                if (gitHubProjects.Any() == false)
                    return gitHubProjects;

                List<GitHubProject> favoriteGitHubProjects = await _memoryFavoriteRepository.GetAllFavoriteAsync();


                gitHubProjects.ForEach(gitHubProject =>
                {
                    gitHubProject.IsFavoriteGitHubProject = favoriteGitHubProjects.Any(fav => fav.Id == gitHubProject.GitHubProjectId);
                });

                return ApplyRelevantOrder(gitHubProjects, new GitHubProjectRelevantRequestDto() {
                    ForksRelevantLevel = request.ForksRelevantLevel, 
                    StarsRelevantLevel = request.StarsRelevantLevel, 
                    WatchersRelevantLevel = request.WatchersRelevantLevel 
                });
            }
            catch (Exception)
            {
                return new List<GitHubProjectResponseDto>();
            }
        }

        private async Task<GitHubProject> GetGitHubProjectByIdAsync(long gitHubProjectId)
        {
            try
            {
                GitHubStorage gitHubStorageRaw = await _gitHubRepository.GetAllAsync();

                return gitHubStorageRaw.Items.FirstOrDefault(e => e.Id == gitHubProjectId);
            }
            catch (Exception)
            {

                throw;
            }
        }

        private List<GitHubProjectResponseDto> ApplyRelevantOrder(List<GitHubProjectResponseDto> gitHubProjects, GitHubProjectRelevantRequestDto request)
        {
            if (gitHubProjects == null || gitHubProjects.Count == 0)
                return new List<GitHubProjectResponseDto>();

            #region Quanto maior for o nível de relevância do parâmetro, mais ao topo ele será exibido.
            if (request.StarsRelevantLevel == RelevantOrderEnum.Low)
                gitHubProjects = gitHubProjects.OrderByDescending(e => e.StarsCount).ToList();

            if (request.ForksRelevantLevel == RelevantOrderEnum.Low)
                gitHubProjects = gitHubProjects.OrderByDescending(e => e.ForksCount).ToList();

            if (request.WatchersRelevantLevel == RelevantOrderEnum.Low)
                gitHubProjects = gitHubProjects.OrderByDescending(e => e.WatchersCount).ToList();

            if (request.StarsRelevantLevel == RelevantOrderEnum.Medium)
                gitHubProjects = gitHubProjects.OrderByDescending(e => e.StarsCount).ToList();

            if (request.ForksRelevantLevel == RelevantOrderEnum.Medium)
                gitHubProjects = gitHubProjects.OrderByDescending(e => e.ForksCount).ToList();

            if (request.WatchersRelevantLevel == RelevantOrderEnum.Medium)
                gitHubProjects = gitHubProjects.OrderByDescending(e => e.WatchersCount).ToList();

            if (request.StarsRelevantLevel == RelevantOrderEnum.High)
                gitHubProjects = gitHubProjects.OrderByDescending(e => e.StarsCount).ToList();

            if (request.ForksRelevantLevel == RelevantOrderEnum.High)
                gitHubProjects = gitHubProjects.OrderByDescending(e => e.ForksCount).ToList();

            if (request.WatchersRelevantLevel == RelevantOrderEnum.High)
                gitHubProjects = gitHubProjects.OrderByDescending(e => e.WatchersCount).ToList();
            #endregion

            return gitHubProjects;
        }

        private GitHubProjectResponseDto MapperGitHubRepoResponseDto(GitHubProject repo)
        {
            return new GitHubProjectResponseDto
            {
                GitHubProjectId = repo.Id,
                NameProject = repo.Name,
                Description = repo.Description,
                CreatedAt = repo.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"), 
                UpdatedAt = repo.UpdatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
                IsPrivateProject = repo.Private,  
                OwnerName =  String.IsNullOrEmpty(repo.Owner?.Login) == false ? repo.Owner.Login : "Desconhecido",
                ForksCount = repo.ForksCount,
                StarsCount = repo.StargazersCount,
                WatchersCount = repo.WatchersCount
            };
        }

        private OwnerGitHubProjectRequest MapperOwnerGitHubProjectRequest(OwnerGitHubProjectRequestDto dto)
        {
            return new OwnerGitHubProjectRequest
            {
                Owner = dto.OwnerName,
                Repo = dto.GitHubProjectName
            };
        }
    }
}
