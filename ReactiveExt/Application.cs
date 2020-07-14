using ReactiveExt.Contracts;
using System;

namespace ReactiveExt
{
    class Application : IApplication
    {
        private readonly IProducer producer;
        public Application(IProducer producer)
        {
            this.producer = producer;
        }
        public void Run()
        {
            producer.InitDataSource();

            var observable = producer.Produce();
            var ObservablePerson = producer.ObservablePerson();

            observable.Subscribe(s => Console.WriteLine(s), () => Console.WriteLine("Completed initial data listings"));
            ObservablePerson.Subscribe(p => Console.WriteLine(p.ToString()));

            producer.ChangeDataSource();

            observable.Subscribe(s => Console.WriteLine(s), () => Console.WriteLine("Completed secondary data listings"));
            ObservablePerson.Subscribe(p => Console.WriteLine(p.ToString()));
        }
    }
}
