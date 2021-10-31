using Autofac;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using MoviesVB.Core;
using MoviesVB.Data.Bootstrap;
using MoviesVB.DomainServices.Bootstrap;

namespace MoviesVB.UIApi.Bootstrap
{
    public static class Config
    {
        public static void AddDependencies(this ContainerBuilder builder, IConfiguration configuration)
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {

                cfg.AddProfile<DomainServicesMapperProfile>();
            });

            var appSettings = new AppSettings();

            configuration.Bind(appSettings);

            builder.RegisterInstance(mapperConfig.CreateMapper()).SingleInstance();

            builder.RegisterInstance<IAppSettings>(appSettings).SingleInstance();

            builder.RegisterModule<DomainDependencyModule>();

            builder.RegisterModule<DataDependencyModule>();
            
        }
    }
}
