using ReactiveExt.Contracts;
using ReactiveExt.Models;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace ReactiveExt
{
    class Producer : IProducer
    {

        private readonly List<string> changingDataSource;
        private readonly Person person;

        public Producer()
        {
            changingDataSource = new List<string>();
            person = new Person();
        }

        public void InitDataSource()
        {
            changingDataSource.Clear();
            changingDataSource.AddRange(new List<string> { "hello", "word", "!", "this", "is", "a", "reactive", "app" });

            person.Name = "Jadid Amine";
            person.Age = 25;
            person.Address = "22 avenue de l'orangerie, Cergy";
        }

        public void ChangeDataSource()
        {
            changingDataSource.Clear();
            changingDataSource.AddRange(new List<string> { "Data", "was", "being", "changed" });
            //changingDataSource = new List<string> { "Data", "was", "being", "changed" };

            person.Age = 26;
        }

        public IObservable<string> Produce()
        {
            var test = Observable.FromAsync(() => Task.Run(() => Console.WriteLine()));
           return changingDataSource.ToObservable();
        }

        public IObservable<Person> ObservablePerson()
        {
            return Observable.Return(person);
        }
    }
}
