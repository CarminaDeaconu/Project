using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Project.Models;

namespace Project.Services
{
    public class TmdbService
    {
        private readonly HttpClient _httpClient;
        private const string ApiKey = "ef4c15de60f6b858a044ee1e79c04d0f"; // Replace with your TMDb API key
        private const string BaseUrl = "https://api.themoviedb.org/3";

        public TmdbService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<Movie>> GetPopularMoviesAsync()
        {
            var url = $"{BaseUrl}/movie/popular?api_key={ApiKey}";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var json = JObject.Parse(content);
            var results = json["results"];

            var movies = new List<Movie>();
            foreach (var result in results)
            {
                var movie = new Movie
                {
                    Title = result["title"]?.ToString(),
                    PosterUrl = $"https://image.tmdb.org/t/p/w500{result["poster_path"]}"
                };
                movies.Add(movie);
            }

            return movies;
        }

        public async Task<JObject> GetMovieDetailsAsync(string title)
        {
            var searchUrl = $"{BaseUrl}/search/movie?query={Uri.EscapeDataString(title)}&api_key={ApiKey}";
            var searchResponse = await _httpClient.GetAsync(searchUrl);
            searchResponse.EnsureSuccessStatusCode();

            var searchContent = await searchResponse.Content.ReadAsStringAsync();
            var searchResult = JObject.Parse(searchContent);
            var movieId = searchResult["results"]?.FirstOrDefault()?["id"]?.ToString();

            if (movieId == null)
                return null;

            var detailsUrl = $"{BaseUrl}/movie/{movieId}?api_key={ApiKey}";
            var detailsResponse = await _httpClient.GetAsync(detailsUrl);
            detailsResponse.EnsureSuccessStatusCode();

            var detailsContent = await detailsResponse.Content.ReadAsStringAsync();
            return JObject.Parse(detailsContent);
        }
    }
}
