using System;
using System.Threading;
using System.Threading.Tasks;

namespace design_patterns.creational.singleton
{
    /// <summary>
    /// Singleton is a creational design pattern that 
    /// lets you ensure that a class has only one instance, 
    /// while providing a global access point to this instance.
    /// 
    /// https://refactoring.guru/design-patterns/singleton
    /// </summary>
    /// <remarks>
    /// The Singleton pattern solves two problems at the same time, 
    /// violating the Single Responsibility Principle:
    /// 1. Ensure that a class has just a single instance. 
    ///     (ex: shared resource, a database or a file)
    /// 2. Provide a global access point to that instance. 
    /// 
    /// Solution:
    /// - Make the default constructor private, to prevent other objects 
    ///     from using the new operator with the Singleton class
    /// - Create a static creation method that acts as a constructor. 
    /// 
    /// Problem:
    /// - Testability
    /// - usage of singleton leads to missuse and 
    ///     wrong software design decisions
    /// 
    /// Solution:
    /// - AVOID using singleton class, instead use DI containers 
    ///     and configure the container to define lifetime of class as 
    ///     as singleton
    /// </remarks>
    public class SingletonSample
    {
        public static async Task Run()
        {
            Console.WriteLine("Creational - Singleton");

            var inst1 = SingletonDatabase.Instance;
            var data = inst1.GetData(Guid.NewGuid().ToString());
            Console.WriteLine($"data : {data}");

            var inst2 = SingletonDatabase.Instance;
            Console.WriteLine($"Are inst1 and inst2 the same? {inst1.Equals(inst2)}");

            var task1 =  Task.Factory.StartNew(() => {
                var instPerThread1 = PerThreadSingletonDatabase.Instance.GetData("pth");
                Console.WriteLine($"instPerThread1 {instPerThread1}");
            });
            
            var task2 = Task.Factory.StartNew(() => {
                var instPerThread2 = PerThreadSingletonDatabase.Instance.GetData("pth");
                Console.WriteLine($"instPerThread2 {instPerThread2}");
            });
            Task.WaitAll(task1, task2);

        }
    }

    public interface IDatabase {
        string GetData(string id);
    }

    /// <summary>
    /// singleton class
    /// </summary>
    public class SingletonDatabase : IDatabase
    {
        private SingletonDatabase() {
            //init resources
            Console.WriteLine("singleton resources init");
        }

        // lazy initialization of the class
        private static Lazy<SingletonDatabase> instance =  
            new Lazy<SingletonDatabase>(() => new SingletonDatabase());
        public static SingletonDatabase Instance { get { return instance.Value; } }
        public string GetData(string id)
        {
            return $"Data {id}";
        }
    }

    /// <summary>
    /// singleton per thread
    /// </summary>
    public class PerThreadSingletonDatabase : IDatabase
    {
        private PerThreadSingletonDatabase() {
            //init resources
            Console.WriteLine("per thread singleton resources init");
        }

        // declare and instantiate object into local thread scope
        private static ThreadLocal<PerThreadSingletonDatabase> threadInstance =  
            new ThreadLocal<PerThreadSingletonDatabase>(() => new PerThreadSingletonDatabase());
        public static PerThreadSingletonDatabase Instance { get { return threadInstance.Value; } }
        public string GetData(string id)
        {
            return $"Data {id} from ThreadId: {Thread.CurrentThread.ManagedThreadId}";
        }
    }
}