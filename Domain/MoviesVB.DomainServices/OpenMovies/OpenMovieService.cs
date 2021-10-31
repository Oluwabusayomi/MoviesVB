using MoviesVB.Core;
using MoviesVB.Core.Helpers;
using MoviesVB.Core.OpenMovieService;
using MoviesVB.Core.OpenMovieService.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MoviesVB.DomainServices.OpenMovies
{
    public class OpenMovieService : IOpenMovieService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IAppSettings _settings;


        public OpenMovieService(IHttpClientFactory httpClientFactory, IAppSettings settings)
        {
            _httpClientFactory = httpClientFactory;
            _settings = settings;
        }

        public async Task<Movie> SearchMovieAsync(string title)
        {
            ArgumentGuard.NotNullOrWhiteSpace(title, nameof(title));

            var route = $"t={title}&apikey={_settings.OpenMovieApiKey}";

            var httpResponse = await GetAsync(route);

            var result = await httpResponse.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Movie>(result);

        }

        private async Task<HttpResponseMessage> GetAsync(string route)
        {
            var client = _httpClientFactory.CreateClient("open_movie");

            client.BaseAddress = new Uri(_settings.OpenMovieBaseUrl);

            client.DefaultRequestHeaders.Add("Accept", "application/json");

            return await client.GetAsync($"/?{route}");

        }
    }
}
