using GitHubExplorer.Application.DTOs.Repository.Request;
using GitHubExplorer.Application.DTOs.Repository.Respone;
using GitHubExplorer.Application.Features;
using GitHubExplorer.Domain.Entities;
using GitHubExplorer.Domain.Enums;
using GitHubExplorer.Domain.Interfaces;
using Moq;

namespace GitHubExplorer.Test.Features
{
    public class GitHubServiceTests
    {
        private readonly Mock<IGitHubRepository> _mockGitHubRepository;
        private readonly Mock<IMemoryFavoriteRepository> _mockMemoryFavoriteRepository;
        private readonly GitHubService _gitHubService;

        public GitHubServiceTests()
        {
            _mockGitHubRepository = new Mock<IGitHubRepository>();
            _mockMemoryFavoriteRepository = new Mock<IMemoryFavoriteRepository>();
            _gitHubService = new GitHubService(_mockGitHubRepository.Object, _mockMemoryFavoriteRepository.Object);
        }

        [Fact]
        public async Task GetAllGitHubProjectAsync_ShouldReturnList()
        {
            GitHubStorage fakeGitHubStorage = new GitHubStorage
            {
                Items = new List<GitHubProject>
            {
                new GitHubProject { Id = 1, Name = "Projeto A" },
                new GitHubProject { Id = 2, Name = "Projeto B" }
            }
            };

            _mockGitHubRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(fakeGitHubStorage);

            List<GitHubProjectResponseDto> result = await _gitHubService.GetAllGitHubProjectAsync();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetAllGitHubProjectAsync_ShouldReturnEmpty_WhenNoProjects()
        {
            _mockGitHubRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(new GitHubStorage { Items = new List<GitHubProject>() });

            _mockMemoryFavoriteRepository.Setup(r => r.GetAllFavoriteAsync()).ReturnsAsync(new List<GitHubProject>());

            List<GitHubProjectResponseDto> result = await _gitHubService.GetAllGitHubProjectAsync();

            Assert.Empty(result);
        }

        [Fact]
        public async Task GetGitHubProjectMostRelevantAsync_ShouldReturnProjectsOrderedByStars_WhenStarsRelevantLevelIsHigh()
        {
            List<GitHubProject> fakeProjects = new List<GitHubProject>
            {
                new GitHubProject { Id = 1, Name = "Projeto A", StargazersCount = 10, WatchersCount = 6, ForksCount = 2 },
                new GitHubProject { Id = 2, Name = "Projeto B", StargazersCount = 20, WatchersCount = 6, ForksCount = 1 },
                new GitHubProject { Id = 3, Name = "Projeto C", StargazersCount = 15, WatchersCount = 6, ForksCount = 0 },
                new GitHubProject { Id = 4, Name = "Projeto D", StargazersCount = 20, WatchersCount = 3, ForksCount = 1 },
                new GitHubProject { Id = 5, Name = "Projeto E", StargazersCount = 10, WatchersCount = 9, ForksCount = 2 },
            };

            GitHubStorage gitHubStorage = new GitHubStorage { Items = fakeProjects };

            _mockGitHubRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(gitHubStorage);

            GitHubProjectRelevantRequestDto request = new GitHubProjectRelevantRequestDto
            {
                StarsRelevantLevel = RelevantOrderEnum.High,                
                WatchersRelevantLevel = RelevantOrderEnum.Medium
            };

            bool isOrdered = true;
            List<GitHubProjectResponseDto> result = await _gitHubService.GetGitHubProjectMostRelevantAsync(request);

            for (int i = (result.Count -1); i >= 0; i--)
            {
                if (i == 0) continue;

                if (result[i].StarsCount > result[i - 1].StarsCount)
                {
                    isOrdered = false;
                    break;
                }
            }

            Assert.Equal(true, isOrdered);
        }

        [Fact]
        public async Task GetGitHubProjectMostRelevantAsync_ShouldReturnProjectsOrderedByWatchersAndStars_WhenWathersRelevantLevelIsHighAndStarsRelevantLevelIsLow()
        {
            List<GitHubProject> fakeProjects = new List<GitHubProject>
            {
                new GitHubProject { Id = 1, Name = "Projeto A", StargazersCount = 10, WatchersCount = 6, ForksCount = 2 },
                new GitHubProject { Id = 2, Name = "Projeto B", StargazersCount = 20, WatchersCount = 6, ForksCount = 1 },
                new GitHubProject { Id = 3, Name = "Projeto C", StargazersCount = 15, WatchersCount = 6, ForksCount = 0 },
                new GitHubProject { Id = 4, Name = "Projeto D", StargazersCount = 20, WatchersCount = 3, ForksCount = 1 },
                new GitHubProject { Id = 5, Name = "Projeto E", StargazersCount = 10, WatchersCount = 9, ForksCount = 2 },
            };

            GitHubStorage gitHubStorage = new GitHubStorage { Items = fakeProjects };

            _mockGitHubRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(gitHubStorage);

            GitHubProjectRelevantRequestDto request = new GitHubProjectRelevantRequestDto
            {
                StarsRelevantLevel = RelevantOrderEnum.Low,
                WatchersRelevantLevel = RelevantOrderEnum.High
            };

            bool isOrdered = true;
            List<GitHubProjectResponseDto> result = await _gitHubService.GetGitHubProjectMostRelevantAsync(request);


            Assert.Equal(true, isOrdered);

            Assert.Equal(5, result.Count);
            Assert.Equal(10, result[0].StarsCount);
            Assert.Equal(20, result[1].StarsCount);
            Assert.Equal(15, result[2].StarsCount);
            Assert.Equal(10, result[3].StarsCount);
            Assert.Equal(20, result[4].StarsCount);

            Assert.Equal(9, result[0].WatchersCount);
            Assert.Equal(6, result[1].WatchersCount);
            Assert.Equal(6, result[2].WatchersCount);
            Assert.Equal(6, result[3].WatchersCount);
            Assert.Equal(3, result[4].WatchersCount);
        }

    }
}
