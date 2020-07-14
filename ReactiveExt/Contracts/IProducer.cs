using ReactiveExt.Models;
using System;

namespace ReactiveExt.Contracts
{
    public interface IProducer
    {
        IObservable<string> Produce();
        IObservable<Person> ObservablePerson();
        void InitDataSource();
        void ChangeDataSource();
    }
}
