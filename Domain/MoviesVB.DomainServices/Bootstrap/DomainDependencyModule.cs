using Autofac;
using System.Linq;


namespace MoviesVB.DomainServices.Bootstrap
{
    public class DomainDependencyModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var assembly = GetType().Assembly;

            builder.RegisterAssemblyTypes(assembly)
                    .Where(t => t.GetInterfaces().Any(i => i.Name.EndsWith("Service")))
                    .As(t => t.GetInterfaces().Where(i => i.Name.EndsWith("Service")))
                    .InstancePerLifetimeScope();
        }
    }
}
