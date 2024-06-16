using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Project.Models;
using Project.Services;
using Newtonsoft.Json.Linq;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace Project.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        private readonly TmdbService _tmdbService;
        private string _movieTitle;
        private string _movieDetails;
        private string _moviePosterUrl;

        public event PropertyChangedEventHandler PropertyChanged;

        public MainPageViewModel()
        {
            _tmdbService = new TmdbService();
            GetMovieDetailsCommand = new Command(async () => await GetMovieDetailsAsync());
        }

        public string MovieTitle
        {
            get => _movieTitle;
            set
            {
                _movieTitle = value;
                OnPropertyChanged();
            }
        }

        public string MovieDetails
        {
            get => _movieDetails;
            set
            {
                _movieDetails = value;
                OnPropertyChanged();
            }
        }

        public string MoviePosterUrl
        {
            get => _moviePosterUrl;
            set
            {
                _moviePosterUrl = value;
                OnPropertyChanged();
            }
        }

        public async Task GetMovieDetailsAsync()
        {
            if (string.IsNullOrEmpty(MovieTitle))
            {
                MovieDetails = "Please enter a movie title.";
                MoviePosterUrl = null;
                return;
            }

            try
            {
                JObject movieDetails = await _tmdbService.GetMovieDetailsAsync(MovieTitle);
                if (movieDetails != null)
                {
                    string title = movieDetails["title"]?.ToString() ?? "N/A";
                    string releaseDate = movieDetails["release_date"]?.ToString().Substring(0, 4) ?? "N/A";
                    string genres = movieDetails["genres"] != null ? string.Join(", ", movieDetails["genres"].Select(g => g["name"].ToString())) : "N/A";
                    string director = movieDetails["credits"]?["crew"]?.FirstOrDefault(c => c["job"]?.ToString() == "Director")?["name"]?.ToString() ?? "N/A";
                    string plot = movieDetails["overview"]?.ToString() ?? "N/A";
                    string posterPath = movieDetails["poster_path"]?.ToString();

                    MovieDetails = $"Title: {title}\n" +
                                   $"Year: {releaseDate}\n" +
                                   $"Genre: {genres}\n" +
                                   $"Director: {director}\n" +
                                   $"Plot: {plot}";
                    MoviePosterUrl = !string.IsNullOrEmpty(posterPath) ? $"https://image.tmdb.org/t/p/w500{posterPath}" : null;
                }
                else
                {
                    MovieDetails = "Movie not found.";
                    MoviePosterUrl = null;
                }
            }
            catch (Exception ex)
            {
                MovieDetails = $"Failed to get movie details: {ex.Message}";
                MoviePosterUrl = null;
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ICommand GetMovieDetailsCommand { get; }
    }
}
