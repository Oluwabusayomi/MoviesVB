using AutoMapper;
using MoviesVB.Core.Movies.Models;

namespace MoviesVB.DomainServices.Bootstrap
{
    public class DomainServicesMapperProfile : Profile
    {
        public DomainServicesMapperProfile()
        {
            CreateMap<MovieInfo, MovieDocument>().ReverseMap();
        }
    }
}
