using System;
using System.Threading.Tasks;
using design_patterns.creational.abstract_factory;
using design_patterns.creational.fluentbuilder;
using design_patterns.creational.functionalbuilder;
using design_patterns.creational.factory;
using design_patterns.creational.facadebuilder;
using design_patterns.creational.singleton;
using design_patterns.creational.prototype;
using design_patterns.structural.adapter;
using design_patterns.structural.bridge;
using design_patterns.structural.composite;
using design_patterns.structural.decorator;
using design_patterns.structural.facade;
using design_patterns.structural.flyweight;
using design_patterns.structural.proxy;
using design_patterns.behavioral.chainofresponsability;
using design_patterns.behavioral.command;
using design_patterns.behavioral.iterator;
using design_patterns.behavioral.mediator;
using design_patterns.behavioral.memento;
using design_patterns.behavioral.observer;
using design_patterns.behavioral.state;

namespace design_patterns
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                // creational
                // await FactorySample.Run();
                // await AbstractFactorySample.Run();
                // await FluentBuilderSample.Run();
                // await FunctionalBuilderSample.Run();
                // await FacadeBuilderSample.Run();
                // await SingletonSample.Run();
                // await PrototypeSample.Run();

                // structural
                // await AdapterSample.Run();
                // await BridgeSample.Run();
                // await CompositeSample.Run();
                // await DecoratorSample.Run();
                // await FacadeSample.Run();
                // await FlyweightSample.Run();
                // await ProxySample.Run();

                // behavioral
                // await ChainOfResponsabilitySample.Run();
                // await CommandSample.Run();
                // await IteratorSample.Run();
                // await MediatorSample.Run();
                // await MementoSample.Run();
                // await ObserverSample.Run();
                await StateSample.Run();
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"EXCEPTION: {ex}");
            }
        }
    }
}
