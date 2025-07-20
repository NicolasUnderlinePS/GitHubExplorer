using GitHubExplorer.Domain.Enums;

namespace GitHubExplorer.Application.DTOs.Repository.Request
{
    /// <summary>
    /// Selecione os parâmetros de relevância para ordenar os projetos do GitHub.
    /// 3 > 2 > 1
    /// </summary>   
    public class GitHubProjectRelevantRequestDto
    {
        public RelevantOrderEnum StarsRelevantLevel { get; set; }
        public RelevantOrderEnum ForksRelevantLevel { get; set; }
        public RelevantOrderEnum WatchersRelevantLevel { get; set; }
    }
}
