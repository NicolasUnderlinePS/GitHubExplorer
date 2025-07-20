using GitHubExplorer.Domain.Entities;
using GitHubExplorer.Domain.Interfaces;
using GitHubExplorer.Domain.Request;
using System.Net.Http.Headers;
using System.Text.Json;

namespace GitHubExplorer.Infrastructure.Repositories
{
    public class GitHubRepository : IGitHubRepository
    {
        private readonly HttpClient _httpClient;
        public const string GITHUB_API_BASE_URL = "https://api.github.com/";


        public GitHubRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("GitHubExplorerApp", "1.0"));
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
        }

        public async Task<GitHubStorage> GetAllAsync()
        {
            try
            {
                string requestUrl = $"search/repositories?q=%7bnome";

                HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);
                response.EnsureSuccessStatusCode();

                string content = await response.Content.ReadAsStringAsync();

                GitHubStorage gitHubApiResponse = JsonSerializer.Deserialize<GitHubStorage>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return gitHubApiResponse;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<GitHubProject> GetOwnerProjectAsync(OwnerGitHubProjectRequest request)
        {
            try
            {
                string requestUrl = $"repos/{request.Owner}/{request.Repo}";

                HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);
                response.EnsureSuccessStatusCode();

                string content = await response.Content.ReadAsStringAsync();

                GitHubProject gitHubApiResponse = JsonSerializer.Deserialize<GitHubProject>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return gitHubApiResponse;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
