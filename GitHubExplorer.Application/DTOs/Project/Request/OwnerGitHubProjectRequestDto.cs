using GitHubExplorer.Domain.Enums;

namespace GitHubExplorer.Application.DTOs.Project.Request
{
    public class OwnerGitHubProjectRequestDto
    {
        public string? OwnerName { get; set; }
        public string? GitHubProjectName { get; set; }

        public RelevantOrderEnum StarsRelevantLevel { get; set; }
        public RelevantOrderEnum ForksRelevantLevel { get; set; }
        public RelevantOrderEnum WatchersRelevantLevel { get; set; }
    }
}
