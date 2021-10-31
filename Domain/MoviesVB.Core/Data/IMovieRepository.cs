using MoviesVB.Core.Movies;
using MoviesVB.Core.Movies.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoviesVB.Core.Data
{
    public interface IMovieRepository : IDataRepository<MovieDocument>, IDeleteDataRepository<MovieDocument>
    {
        Task<IList<MovieDocument>> SearchAsync(DateTimeRange dateTimeRange);
        Task<List<MovieDocument>> SearchAsync(MovieRequestQuery query);
        Task<List<RequestReport>> GetUsageReportAsync();
        Task<IList<MovieDocument>> GetAllMoviesAsync();
    }
}
