using System;
using System.Threading.Tasks;

namespace design_patterns.structural.facade
{
    /// <summary>
    /// Facade is a structural design pattern that provides 
    /// a simplified interface to a library, a framework, 
    /// or any other complex set of classes.
    /// 
    /// - exposing several components through a single interface
    /// - hide inner complexity
    /// </summary>
    public class FacadeSample
    {
        public static async Task Run()
        {
            Console.WriteLine("Structural - Facade");

            var service = new ProcessingService();
            var result = service.ProcessData("start data");
            System.Console.WriteLine(result);
        }
    }

    /// <summary>
    /// expose simple interface 
    /// hiding inside complexity
    /// </summary>
    public interface IProcessingService
    {
        string ProcessData(string input);
    }

    public class ProcessingService : IProcessingService
    {
        private Component1 c1 = new();
        private Component2 c2 = new(); 
        private Component3 c3 = new();
        public string ProcessData(string input)
        {
            c1.LogInfo(input);
            var handler = c2.StartSubsystem(input);
            var res1 = c2.GetSubsystemData(handler);
            c1.LogInfo(res1);
            var res2 = c3.FilterInfo(res1);
            c1.LogInfo(res2);
            return res2;
        }

        public class Component1
        {
            internal void LogInfo(string input)
            {
                System.Console.WriteLine($"Logging: {input}");
            }
        }

        public class Component2
        {
            Guid handler;
            internal string GetSubsystemData(Guid handler)
            {
                return $"{handler}_{Guid.NewGuid()}";
            }

            internal Guid StartSubsystem(string input)
            {
                handler = Guid.NewGuid();
                return handler;
            }
        }

        public class Component3
        {
            internal string FilterInfo(string res1)
            {
                return $"{res1} with filters";
            }
        }
    }
}