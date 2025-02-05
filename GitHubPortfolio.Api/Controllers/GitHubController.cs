using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using Service;
using Octokit;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GitHubPortfolio.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GitHubController : ControllerBase
    {
        private readonly GitHubService _gitHubService;

        public GitHubController(GitHubService gitHubService)
        {
            _gitHubService = gitHubService;
        }

        //[HttpGet("portfolio")]
        //public async Task<ActionResult<IEnumerable<Repository>>> GetPortfolio()
        //{
        //    var repositories = await _gitHubService.GetRepositoriesAsync();
        //    return Ok(repositories);
        //}
        [HttpGet("GetPortfolio")]
        public async Task<IActionResult> GetPortfolio()
        {
            var repositories = await _gitHubService.GetRepositoriesAsync();
            return Ok(repositories);
        }

        [HttpGet("SearchRepositories")]
        public async Task<IActionResult> SearchRepositories(
      [FromQuery] string repositoryName = null,
      [FromQuery] string language = null,
      [FromQuery] string user = null)
        {
            var results = await _gitHubService.SearchRepositoriesAsync(repositoryName, language, user);
            return Ok(results);
        }
    }
}
