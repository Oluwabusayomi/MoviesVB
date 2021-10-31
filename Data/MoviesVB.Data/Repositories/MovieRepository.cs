using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MoviesVB.Core.Data;
using MoviesVB.Core.Helpers;
using MoviesVB.Core.Movies;
using MoviesVB.Core.Movies.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesVB.Data.Repositories
{
    internal class MovieRepository : DataRepository<MovieDocument>, IMovieRepository
    {
        private readonly MovieDbContext _dbContext;

        public MovieRepository(MovieDbContext dbContext)
            : base(dbContext.Movies)
        {
            _dbContext = dbContext;
        }

        public async Task<IList<MovieDocument>> SearchAsync(DateTimeRange dateTimeRange)
        {
            ArgumentGuard.NotNull(dateTimeRange, nameof(dateTimeRange));

            var movieDocs = new List<MovieDocument>();

            var cursor = _dbContext.Movies.AsQueryable();

            foreach(var item in cursor)
            {
                if (item.Timestamp.Date >= dateTimeRange.BeginDateTime.Date
                       && item.Timestamp.Date <= dateTimeRange.EndDateTime.Date)
                {
                    movieDocs.Add(item);
                }
            }
            return await Task.FromResult(movieDocs);
        }

        public Task<List<RequestReport>> GetUsageReportAsync()
        {
            var reports = _dbContext.Movies.Aggregate()
                  .Group(
                          doc => new DateTime(doc.Timestamp.Year, doc.Timestamp.Month, doc.Timestamp.Day),
                          group => new { Date = group.Key, RequestCount = group.Sum(y => 1) }
                         ).ToList();

            var response = reports.Select(x => 
            new RequestReport { Date = $"{x.Date.Day}-{x.Date.Month}-{x.Date.Year}", RequestCount = x.RequestCount }).ToList();

            return Task.FromResult(response);

        }

        public Task<List<MovieDocument>> SearchAsync(MovieRequestQuery query)
        {
            ArgumentGuard.NotNull(query, nameof(query));

            var movies = _dbContext.Movies.AsQueryable();

            if (query.Search_Token.IsNotNull())
            {
               movies = movies.Where(x => x.Search_Token == query.Search_Token);
            }
            if (query.ImdbID.IsNotNull())
            {
                movies = movies.Where(x => x.ImdbID == query.ImdbID);

            }
            return Task.FromResult(movies.ToList());
        }

        public async Task<IList<MovieDocument>> GetAllMoviesAsync()
        {
            return await _dbContext.Movies.AsQueryable().ToListAsync();
        }
    }
}
