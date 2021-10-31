using Autofac;
using System.Linq;


namespace MoviesVB.Data.Bootstrap
{
    public class DataDependencyModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MovieDbContext>()
               .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(GetType().Assembly)
                .Where(t => t.GetInterfaces().Any(i => i.Name.EndsWith("Repository")))
                .As(t => t.GetInterfaces().Where(i => i.Name.EndsWith("Repository")))
                .InstancePerLifetimeScope();

        }
    }
}
