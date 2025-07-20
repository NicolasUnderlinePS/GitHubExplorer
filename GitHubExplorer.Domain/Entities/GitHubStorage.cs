namespace GitHubExplorer.Domain.Entities
{
    public class GitHubStorage
    {
        public int Total_Count { get; set; }
        public bool Incomplete_Results { get; set; }
        public List<GitHubProject> Items { get; set; } = new();
    }
}
