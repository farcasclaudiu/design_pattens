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
                await AdapterSample.Run();
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"EXCEPTION: {ex}");
            }
        }
    }
}
