using GitHubExplorer.Application.DTOs.Project.Request;
using GitHubExplorer.Application.DTOs.Repository.Request;
using GitHubExplorer.Application.DTOs.Repository.Respone;
using GitHubExplorer.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GitHubExplorer.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GitHubProjectController : ControllerBase
    {
        private readonly IGitHubService _gitHubService;

        public GitHubProjectController(IGitHubService gitHubService)
        {
            _gitHubService = gitHubService;
        }

        [HttpGet("GetFilterGitHubProjectAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<GitHubProjectResponseDto>>> GetOwnerGitHubProjectAsync([FromQuery] OwnerGitHubProjectRequestDto request)
        {
            return Ok(await _gitHubService.GetOwnerGitHubProjectAsync(request));
        }

        [HttpGet("GetAllProject")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<GitHubProjectResponseDto>>> GetAllGitHubProjectAsync()
        {
            return Ok(await _gitHubService.GetAllGitHubProjectAsync());
        }

        [HttpGet("GetMostRelevantProject")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<GitHubProjectResponseDto>>> GetGitHubProjectMostRelevantAsync([FromQuery] GitHubProjectRelevantRequestDto request)
        {
            return Ok(await _gitHubService.GetGitHubProjectMostRelevantAsync(request));
        }

        [HttpPost("FavoriteGitHubProject")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<GitHubProjectResponseDto>>> FavoriteGitHubProjectAsync(long gitHubProjectId)
        {
            try
            {
                return Ok(await _gitHubService.FavoriteGitHubProjectAsync(gitHubProjectId));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Exceção ao favoritar repositório.");
            }
        }

        [HttpGet("GetMyFavoriteProject")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<GitHubProjectResponseDto>>> GetAllFavoriteGitHubProjectAsync()
        {
            return Ok(await _gitHubService.GetAllFavoriteGitHubProjectAsync());
        }
    }
}
