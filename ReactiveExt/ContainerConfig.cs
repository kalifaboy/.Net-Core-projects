using Autofac;
using ReactiveExt.Contracts;

namespace ReactiveExt
{
    public static class ContainerConfig
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<Application>().As<IApplication>();
            builder.RegisterType<Producer>().As<IProducer>();
            return builder.Build();
        }
    }
}
