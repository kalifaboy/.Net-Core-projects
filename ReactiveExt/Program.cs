using Autofac;
using ReactiveExt.Contracts;
using System;

namespace ReactiveExt
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = ContainerConfig.Configure();

            using var scope = container.BeginLifetimeScope();

            var application = scope.Resolve<IApplication>();

            application.Run();

            Console.WriteLine();
        }
    }
}
