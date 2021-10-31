using MoviesVB.Core.Movies.Models;
using MoviesVB.Core.OpenMovieService.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoviesVB.Core.Movies
{
    public interface IMovieService
    {
        Task<Movie> SearchMovieAsync(MovieQuery  query);
        Task<IList<MovieInfo>> GetMoviesAsync();
        Task<MovieInfo> GetMovieAsync(Guid movieId);
        Task<IList<MovieInfo>> SearchAsync(DateTimeRange dateTimeRange);
        Task<IList<MovieInfo>> SearchAsync(MovieRequestQuery query);
        Task DeleteAsync(Guid movieId);
        Task<IList<RequestReport>> GetUsageReportAsync();
    }

}
