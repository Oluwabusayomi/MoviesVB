using AutoMapper;
using Microsoft.AspNetCore.Http;
using MoviesVB.Core;
using MoviesVB.Core.Data;
using MoviesVB.Core.Helpers;
using MoviesVB.Core.Movies;
using MoviesVB.Core.Movies.Models;
using MoviesVB.Core.OpenMovieService;
using MoviesVB.Core.OpenMovieService.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;


namespace MoviesVB.DomainServices.Movies
{
    public class MovieService : IMovieService
    {
        private readonly IOpenMovieService _openMovieService;
        private readonly IMovieRepository _movieRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _accessor;
        private readonly IMemCacheService _memCacheService;

        private readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(60);

        public MovieService(IMovieRepository movieRepository,
            IOpenMovieService openMovieService,
            IHttpContextAccessor accessor,
            IMapper mapper,
            IMemCacheService memCacheService
            )
        {
            _openMovieService = openMovieService;
            _movieRepository = movieRepository;
            _mapper = mapper;
            _accessor = accessor;
            _memCacheService = memCacheService;
        }

        

        public async Task<MovieInfo> GetMovieAsync(Guid movieId)
        {
            ArgumentGuard.NotEmpty(movieId, nameof(movieId));

            var movieDocument = await _movieRepository.GetAsync(movieId);

            var movie = _mapper.Map<MovieInfo>(movieDocument);

            return movie;
        }

        public async Task<IList<MovieInfo>> GetMoviesAsync()
        {
            var movieDocuments = await _movieRepository.GetAllMoviesAsync();

            var movies = _mapper.Map<IList<MovieInfo>>(movieDocuments);

            return movies;
        }

        public async Task<Movie> SearchMovieAsync(MovieQuery query)
        {
            ArgumentGuard.NotNull(query, nameof(query));

            ArgumentGuard.NotNullOrWhiteSpace(query.Title, nameof(query.Title));

            var movie = default(Movie);

            var timestamp = DateTime.Now.ToUniversalTime();

            var watch = Stopwatch.StartNew();

             movie = _memCacheService.Get<Movie>(query.Title);

            if (movie.IsNull())
            {
                movie = await _openMovieService.SearchMovieAsync(query.Title);
            }

            var model = ToMovieInfoModel(movie, watch.Elapsed.TotalMilliseconds, timestamp);

            await AddMovieAsync(model);

            _memCacheService.Add(movie.Title, movie, CacheDuration, false);

            return movie;
        }

        public async Task<IList<MovieInfo>> SearchAsync(DateTimeRange dateTimeRange)
        {
            var movies = await _movieRepository.SearchAsync(dateTimeRange);

            return _mapper.Map<IList<MovieInfo>>(movies);
        }

        public async Task<IList<MovieInfo>>  SearchAsync(MovieRequestQuery query)
        {
            var queryResult = await _movieRepository.SearchAsync(query);

            return _mapper.Map<IList<MovieInfo>>(queryResult);
        }

        public async Task<IList<RequestReport>> GetUsageReportAsync()
        {
            return await _movieRepository.GetUsageReportAsync();
        }

        public async Task DeleteAsync(Guid movieId)
        {
            ArgumentGuard.NotEmpty(movieId, nameof(movieId));

            await _movieRepository.DeleteAsync(movieId);
        }

        private async Task<Guid> AddMovieAsync(MovieInfo movie)
        {
            ArgumentGuard.NotNull(movie, nameof(movie));

            var document = _mapper.Map<MovieDocument>(movie);

            document.Id = Guid.NewGuid();

            await _movieRepository.AddAsync(document);

            return document.Id;
        }

        private MovieInfo ToMovieInfoModel(Movie movie, double processingTime,
            DateTime timestamp)
        {
            return new MovieInfo
            {
                Timestamp = timestamp,
                Processing_Time_Ms = processingTime,
                ImdbID = movie.ImdbID,
                IP_Address = _accessor.HttpContext.Connection.RemoteIpAddress.ToString(),
                Search_Token = movie.Title

            };
        }
    }
}
