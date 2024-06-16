using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Project.Models;
using Project.Services;

namespace Project.ViewModels
{
    public class HomePageViewModel : INotifyPropertyChanged
    {
        private readonly TmdbService _tmdbService;
        private ObservableCollection<Movie> _movies;

        public event PropertyChangedEventHandler PropertyChanged;

        public HomePageViewModel()
        {
            _tmdbService = new TmdbService();
            Movies = new ObservableCollection<Movie>();
            FetchPopularMoviesAsync();
        }

        public ObservableCollection<Movie> Movies
        {
            get => _movies;
            set
            {
                _movies = value;
                OnPropertyChanged();
            }
        }

        public async Task FetchPopularMoviesAsync()
        {
            try
            {
                var movies = await _tmdbService.GetPopularMoviesAsync();
                foreach (var movie in movies)
                {
                    Movies.Add(movie);
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
