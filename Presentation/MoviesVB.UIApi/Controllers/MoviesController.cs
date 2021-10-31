using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoviesVB.Core.Movies;
using MoviesVB.Core.Movies.Models;
using MoviesVB.Core.OpenMovieService.Models;
using MoviesVB.UIApi.Attributes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoviesVB.UIApi.Controllers
{
    [ApiKey]
    [Route("movies")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        /// <summary>
        /// search for movies based on title
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        [HttpGet("_title")]
        [AllowAnonymous]
        public async Task<Movie> SearchMovieAsync([FromQuery]MovieQuery query)
        {
            return await _movieService.SearchMovieAsync(query);
        }

        /// <summary>
        /// get movie by id
        /// </summary>
        /// <param name="movieId"></param>
        /// <returns></returns>
        [HttpGet("{movieId:Guid}")]
        public async Task<MovieInfo> GetMovieAsync(Guid movieId)
        {
            return await _movieService.GetMovieAsync(movieId);
        }

        /// <summary>
        /// Get all movie requests
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IList<MovieInfo>> GetMoviesAsync()
        {
            return await _movieService.GetMoviesAsync();
        }

        /// <summary>
        /// search criteria to search for movie request  by date range
        /// </summary>
        /// <param name="dateTimeRange"></param>
        /// <returns></returns>
        [HttpGet("_range")]
        public async Task<IList<MovieInfo>> SearchAsync([FromQuery] DateTimeRange dateTimeRange)
        {
            return await _movieService.SearchAsync(dateTimeRange);
        }


        /// <summary>
        /// search criteria to search for movie request 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("_search")]
        public async Task<IList<MovieInfo>> SearchAsync([FromQuery] MovieRequestQuery query)
        {
            return await _movieService.SearchAsync(query);
        }

        /// <summary>
        /// This endpoint deletes a movie
        /// </summary>
        /// <param name="movieId"></param>
        /// <returns></returns>
        [HttpDelete("{movieId:Guid}")]
        public async Task DeleteAsync(Guid movieId)
        {
            await _movieService.DeleteAsync(movieId);
        }
        
        /// <summary>
        /// Endpoint to get daily usage
        /// </summary>
        /// <returns></returns>
        [HttpGet("_report")]
        public async Task<IList<RequestReport>> GetUsageReport()
        {
            return await _movieService.GetUsageReportAsync();
        }
    }
}
