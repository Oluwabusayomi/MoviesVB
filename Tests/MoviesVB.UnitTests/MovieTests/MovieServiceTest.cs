using AutoMapper;
using Microsoft.AspNetCore.Http;
using Moq;
using MoviesVB.Core;
using MoviesVB.Core.Data;
using MoviesVB.Core.Movies.Models;
using MoviesVB.Core.OpenMovieService;
using MoviesVB.Core.OpenMovieService.Models;
using MoviesVB.DomainServices.Movies;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace MoviesVB.UnitTests.MovieTests
{
    public class MovieServiceTest
    {
        [Fact]
        public async Task SearchMovieAsync_Should_Throw_Exception_If_Search_Query_Is_Null()
        {
            var query = default(MovieQuery);

            var openMovieService = new Mock<IOpenMovieService>();

            var memCacheService = new Mock<IMemCacheService>();

            var movieRepo = new Mock<IMovieRepository>();

            var accessor = new Mock<IHttpContextAccessor>();

            var service = new MovieService(movieRepo.Object, openMovieService.Object, accessor.Object, GetMapper(), memCacheService.Object);

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await service.SearchMovieAsync(query));
        }


        [Fact]
        public async Task SearchMovieAsync_Should_Throw_Exception_If_Search_token_Is_Null_Or_Empty()
        {
            var query = new MovieQuery { Title = default };

            var openMovieService = new Mock<IOpenMovieService>();

            var memCacheService = new Mock<IMemCacheService>();

            var movieRepo = new Mock<IMovieRepository>();

            var accessor = new Mock<IHttpContextAccessor>();

            var service = new MovieService(movieRepo.Object, openMovieService.Object, accessor.Object, GetMapper(), memCacheService.Object);

            var ex = await Assert.ThrowsAsync<ArgumentException>(async () => await service.SearchMovieAsync(query));

            Assert.Equal($"'{nameof(query.Title)}' cannot be null or empty", ex.Message);
        }

        [Fact]
        public async Task SearchMovieAsync_Should_Search_Movie_And_Save_Successfully()
        {
            var query = new MovieQuery { Title = "Flash" };

            var searchedResult = new Movie { Actors = "Jack", Title ="24 hours", ImdbID ="62728hd" };
            
            var openMovieService = new Mock<IOpenMovieService>();

             openMovieService.Setup(x => x.SearchMovieAsync(It.IsAny<string>())).Returns(Task.FromResult(searchedResult));

            var movieRepo = new Mock<IMovieRepository>();

            var settings = new Mock<IAppSettings>();

            var memCacheService = new Mock<IMemCacheService>();

            var accessor = new Mock<IHttpContextAccessor>();

            var context = new DefaultHttpContext(); 

            context.Connection.RemoteIpAddress = Dns.GetHostEntry((Dns.GetHostName())).AddressList[0];

            accessor.Setup(_ => _.HttpContext).Returns(context);

            var service = new MovieService(movieRepo.Object, openMovieService.Object, accessor.Object, GetMapper(), memCacheService.Object);

            var movie =  await service.SearchMovieAsync(query);


            openMovieService.Verify(x => x.SearchMovieAsync(It.IsAny<string>()), Times.Once);

            movieRepo.Verify(x => x.AddAsync(It.IsAny<MovieDocument>()), Times.Once);

            memCacheService.Verify(x => x.Add(It.IsAny<string>(), It.IsAny<Movie>(), It.IsAny<TimeSpan>(),false), Times.Once);

            memCacheService.Verify(x => x.Get<Movie>(It.IsAny<string>()), Times.Once);

        }

        [Fact]
        public async Task GetMovieAsync_Should_Throw_Exception_If_Search_Id_Is_Empty()
        {
            var movieId = Guid.Empty;

            var openMovieService = new Mock<IOpenMovieService>();

            var movieRepo = new Mock<IMovieRepository>();

            var accessor = new Mock<IHttpContextAccessor>();

            var memCacheService = new Mock<IMemCacheService>();

            var service = new MovieService(movieRepo.Object, openMovieService.Object, accessor.Object, GetMapper(), memCacheService.Object);

            var ex = await Assert.ThrowsAsync<ArgumentException>(async () => await service.GetMovieAsync(movieId));

            Assert.Equal($"'{nameof(movieId)}' cannot be an empty Guid", ex.Message);
        }

        [Fact]
        public async Task GetMovieAsync_Should_Get_Movie_Successfully()
        {
            var movieId = Guid.NewGuid();
            var movieDoc = new MovieDocument {Id = movieId, };

            var openMovieService = new Mock<IOpenMovieService>();

            var movieRepo = new Mock<IMovieRepository>();
            movieRepo.Setup(x => x.GetAsync(It.IsAny<Guid>(), false)).Returns(Task.FromResult(movieDoc));

            var accessor = new Mock<IHttpContextAccessor>();

            var memCacheService = new Mock<IMemCacheService>();

            var service = new MovieService(movieRepo.Object, openMovieService.Object, accessor.Object, GetMapper(), memCacheService.Object);

            var actualMovie = await service.GetMovieAsync(movieId);

            movieRepo.Verify(x => x.GetAsync(It.IsAny<Guid>(), false), Times.Once);

            Assert.Equal(movieDoc.Id, actualMovie.Id);

        }

        [Fact]
        public async Task GetMoviesAsync_Should_Get_Movies_Successfully()

        {
            IList<MovieDocument> movieDocs = new List<MovieDocument>() { 
                new MovieDocument { Id = Guid.NewGuid() }, 
                new MovieDocument { Id = Guid.NewGuid() },
                new MovieDocument { Id = Guid.NewGuid()}
            };

            var openMovieService = new Mock<IOpenMovieService>();

            var movieRepo = new Mock<IMovieRepository>();

            movieRepo.Setup(x => x.GetAllMoviesAsync()).Returns(Task.FromResult(movieDocs));

            var accessor = new Mock<IHttpContextAccessor>();
            var memCacheService = new Mock<IMemCacheService>();

            var service = new MovieService(movieRepo.Object, openMovieService.Object, accessor.Object, GetMapper(), memCacheService.Object);

            var actualMovies = await service.GetMoviesAsync();

            movieRepo.Verify(x => x.GetAllMoviesAsync(), Times.Once);

            Assert.Equal(movieDocs.Count, actualMovies.Count);

        }

        [Fact]
        public async Task DeleteAsync_Should_Throw_Exception_If_Search_Id_Is_Empty()
        {
            var movieId = Guid.Empty;

            var openMovieService = new Mock<IOpenMovieService>();

            var movieRepo = new Mock<IMovieRepository>();

            var accessor = new Mock<IHttpContextAccessor>();

            var memCacheService = new Mock<IMemCacheService>();

            var service = new MovieService(movieRepo.Object, openMovieService.Object, accessor.Object, GetMapper(), memCacheService.Object);

            var ex = await Assert.ThrowsAsync<ArgumentException>(async () => await service.DeleteAsync(movieId));

            Assert.Equal($"'{nameof(movieId)}' cannot be an empty Guid", ex.Message);
        }

        [Fact]
        public async Task DeleteAsync_Should_Delete_Movie_Successfully()
        {
            var movieId = Guid.NewGuid();
            var movieDoc = new MovieDocument { Id = movieId, };

            var openMovieService = new Mock<IOpenMovieService>();

            var movieRepo = new Mock<IMovieRepository>();

            var accessor = new Mock<IHttpContextAccessor>();

            var memCacheService = new Mock<IMemCacheService>();

            var service = new MovieService(movieRepo.Object, openMovieService.Object, accessor.Object, GetMapper(), memCacheService.Object);

             await service.DeleteAsync(movieId);

            movieRepo.Verify(x => x.DeleteAsync(It.IsAny<Guid>(), true), Times.Once);


        }

        [Fact]
        public async Task GetUsageReportAsync_should_Get_Report_Successfully()
        {
            var reports = new List<RequestReport>
            {
                new RequestReport {Date = DateTime.Now.AddDays(-2).ToShortDateString(), RequestCount = 22 },
                new RequestReport {Date = DateTime.Now.AddDays(-1).ToShortDateString(), RequestCount = 10},
                new RequestReport {Date = DateTime.Now.ToShortDateString(), RequestCount = 12 }
            };

            var openMovieService = new Mock<IOpenMovieService>();

            var movieRepo = new Mock<IMovieRepository>();
            movieRepo.Setup(x => x.GetUsageReportAsync()).Returns(Task.FromResult(reports));

            var accessor = new Mock<IHttpContextAccessor>();

            var memCacheService = new Mock<IMemCacheService>();

            var service = new MovieService(movieRepo.Object, openMovieService.Object, accessor.Object, GetMapper(), memCacheService.Object);

            var response = await service.GetUsageReportAsync();

            movieRepo.Verify(x => x.GetUsageReportAsync(), Times.Once);

            Assert.Equal(reports, response);

            Assert.Equal(reports.Count, response.Count);
        }

        private IMapper GetMapper()
        {
            var config = new MapperConfiguration(cfg => 
            {
                cfg.CreateMap<MovieInfo, MovieDocument>().ReverseMap();
            });

            return config.CreateMapper();
        }
    }
}
