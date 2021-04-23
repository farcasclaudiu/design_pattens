using System.Collections.Concurrent;
using System;
using System.Threading.Tasks;

namespace design_patterns.structural.proxy
{
    /// <summary>
    /// Proxy is a structural design pattern that lets you 
    /// provide a substitute or placeholder for another object. 
    /// A proxy controls access to the original object, 
    /// allowing you to perform something either before or after 
    /// the request gets through to the original object.
    /// 
    /// - Create a new proxy class with the same interface 
    ///     as an original service object. 
    /// - Then you update your app so that it passes the proxy object 
    ///     to all of the original object’s clients. 
    /// - Upon receiving a request from a client, the proxy creates 
    ///     a real service object and delegates all the work to it.
    /// 
    /// Usages:
    /// - Lazy initialization (virtual proxy).
    /// - Access control (protection proxy).
    /// - Local execution of a remote service (remote proxy). 
    /// - Logging requests (logging proxy).
    /// - Caching request results (caching proxy).
    /// - Smart reference. To dismiss a object/rsource once there are no clients using it.
    /// </summary>
    public class ProxySample
    {
        public static async Task Run()
        {
            Console.WriteLine("Structural - Proxy");

            var proxy = new ProxyService();
            var data = Guid.NewGuid().ToString();
            proxy.ProcessData(data);
            proxy.ProcessData(data);
        }

        public interface IService {
            public bool ProcessData(string input);
        }

        public class RealService : IService
        {
            public bool ProcessData(string input)
            {
                System.Console.WriteLine($"Real service with input: {input}");
                return input.Length % 2 == 0;
            }
        }

        public class ProxyService : IService
        {
            // can do lazy loading
            private Lazy<RealService> realService = 
                new Lazy<RealService>(() => new RealService());

            private static ConcurrentDictionary<string, bool> cache = new ConcurrentDictionary<string, bool>();
            public bool ProcessData(string input)
            {
                // can log info
                System.Console.WriteLine($"Proxy service started with input {input}");
                var service = realService.Value;
                
                // could cache the result for later user
                if(!cache.TryGetValue(input, out bool result)){
                    result = service.ProcessData(input);
                    cache[input] = result;
                }
                // can log info
                System.Console.WriteLine($"Proxy service ended with result {result}");
                return result;
            }
        }
    }
}