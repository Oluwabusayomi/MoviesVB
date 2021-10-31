using MoviesVB.Core.OpenMovieService.Models;
using System.Threading.Tasks;

namespace MoviesVB.Core.OpenMovieService
{
    public interface IOpenMovieService
    {
        Task<Movie> SearchMovieAsync(string title);
    }
}
