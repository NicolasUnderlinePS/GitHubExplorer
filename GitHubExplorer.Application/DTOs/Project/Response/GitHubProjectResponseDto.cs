namespace GitHubExplorer.Application.DTOs.Repository.Respone
{
    public class GitHubProjectResponseDto
    {
        public long GitHubProjectId { get; set; }
        public string NameProject { get; set; }
        public string Description { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public bool IsPrivateProject { get; set; }
        public string OwnerName { get; set; }

        public int ForksCount { get; set; }
        public int StarsCount { get; set; }
        public int WatchersCount { get; set; }
        public bool IsFavoriteGitHubProject { get; set; }
    }
}
