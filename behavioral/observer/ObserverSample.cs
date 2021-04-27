using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace design_patterns.behavioral.observer
{
    /// <summary>
    /// Observer is a behavioral design pattern that lets you 
    /// define a subscription mechanism to notify multiple objects 
    /// about any events that happen to the object they’re observing.
    /// 
    /// Use it when:
    /// - changes to the state of one object may require changing other objects, 
    ///     and the actual set of objects is unknown beforehand 
    ///     or changes dynamically.
    /// - some objects in your app must observe others, 
    ///     but only for a limited time or in specific cases.
    /// </summary>
    public class ObserverSample
    {
        public static async Task Run()
        {
            Console.WriteLine("Behavioral - Observer");

            StockMarket stockMarket = new();
            stockMarket.StockPrice += 1;
            Buyer buyer1 = new("John");
            stockMarket.Subscribe(buyer1);
            stockMarket.StockPrice += 1;
            Buyer buyer2 = new("Vanessa");
            stockMarket.Subscribe(buyer2);
            stockMarket.StockPrice += 1;
            stockMarket.StockPrice += 1;
            stockMarket.Unsubscribe(buyer1);
            stockMarket.StockPrice += 1;
        }

        // publisher interface 
        public interface IPublisher
        {
            void Subscribe(ISubscriber subscriber);
            void Unsubscribe(ISubscriber subscriber);
            void Notify();

            // extra
            int StockPrice { get; set; }
        }

        // subscriber (observer) interface 
        public interface ISubscriber
        {
            void Update(IPublisher publisher);
        }

        public class StockMarket : IPublisher
        {

            private int _stockPrice = 100;
            public int StockPrice
            {
                get => _stockPrice;
                set
                {
                    if (_stockPrice == value)
                        return;
                    _stockPrice = value;
                    Notify();
                }
            }

            private List<ISubscriber> subscribers = new List<ISubscriber>();
            public void Notify()
            {
                System.Console.WriteLine($"Publisher is pushing new price {StockPrice}.");
                foreach (var subscriber in subscribers)
                    subscriber.Update(this);
            }

            public void Subscribe(ISubscriber subscriber)
            {
                subscribers.Add(subscriber);
            }

            public void Unsubscribe(ISubscriber subscriber)
            {
                subscribers.Remove(subscriber);
            }
        }

        public class Buyer : ISubscriber
        {
            public Buyer(string name)
            {
                this.Name = name;

            }
            public string Name { get; set; }
            public void Update(IPublisher publisher)
            {
                System.Console.WriteLine($"Buyer {Name} got updated with price {publisher.StockPrice}");
            }
        }

    }
}