using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Octokit;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Caching.Memory;

namespace Service
{
 
    public class GitHubService
    {
        private readonly GitHubClient _client;
        private readonly string _username;
        private readonly IMemoryCache _cache;

        public class SearchRepositoryDto
        {
            public string Name { get; set; }
            public string Owner { get; set; }
            public int Stars { get; set; }
            public string Url { get; set; }
        }


        public GitHubService(IOptions<GitHubOptions> options, IMemoryCache cache)
        {
            _client = new GitHubClient(new Octokit.ProductHeaderValue("GitHubPortfolio"));
            _client.Credentials = new Credentials(options.Value.Token);
            _username = options.Value.Username;
            _cache = cache;
        }
        public async Task<IReadOnlyList<Repository>> GetRepositoriesAsync()
        {
            if (!_cache.TryGetValue("GitHubPortfolio", out IReadOnlyList<Repository> repositories))
            {
                repositories = await _client.Repository.GetAllForUser(_username);
                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(10));
                _cache.Set("GitHubPortfolio", repositories, cacheOptions);
            }
            return repositories;
        }

        public async Task<List<Repository>> SearchRepositoriesAsync(string repoName = null, string language = null, string username = null)
        {
            // אם repoName ריק או null, יש להשאיר את הערך כ-null
            if (string.IsNullOrWhiteSpace(repoName))
            {
                repoName = null;
            }

            var request = new SearchRepositoriesRequest()
            {
                // אם language לא null, ממירים אותו לסוג Language
                Language = language != null ? (Language)Enum.Parse(typeof(Language), language, true) : null,
                User = username
            };

            // שולחים את הבקשה
            var result = await _client.Search.SearchRepo(request);
            return result.Items.ToList();
        }
    }
}


public class GitHubOptions
{
    public string Token { get; set; }
    public string Username { get; set; }
}

