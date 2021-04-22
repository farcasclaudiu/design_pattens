using System;
using System.Text;
using System.Threading.Tasks;

namespace design_patterns.structural.decorator
{
    /// <summary>
    /// Decorator is a structural design pattern that lets you attach 
    /// new behaviors to objects by placing these objects inside 
    /// special wrapper objects that contain the behaviors.
    /// </summary>
    public class DecoratorSample
    {
        public static async Task Run()
        {
            Console.WriteLine("Structural - Decorator");

            // example 1
            Console.WriteLine("example1 - extending with new features");
            Console.WriteLine("Service1");
            var s1 = new ServiceV1();
            s1.DoSomethingOnV1();

            Console.WriteLine("Service2 - multiple inheritance");
            var s2 = new ServiceV2();
            s2.DoSomethingOnV1();
            s2.DoSomethingOnV2();

            // example 2
            Console.WriteLine("example2");
            var sall = new AllExternalServices();
            sall.ServiceAddress = "ALL_Address";
            _ = sall.ProcessData1();
            _ = sall.ProcessData2();

            //IExternalService2.DoExtra()
        }


        public class ServiceV1
        {
            public void DoSomethingOnV1() {
                System.Console.WriteLine($"Doing somethin on V1 from {this.GetType().Name}");
            }
        }

        /// <summary>
        /// this decorates ServiceV1 and adds new functionality
        /// </summary>
        public class ServiceV2 {
            private ServiceV1 serviceV1 = new ServiceV1();
            public void DoSomethingOnV1() {
                serviceV1.DoSomethingOnV1();
            }

            /// <summary>
            /// extra functionality
            /// </summary>
            public void DoSomethingOnV2() {
                System.Console.WriteLine($"Doing somethin on V2 from {this.GetType().Name}");
            }
        }




        // EXAMPLE 2
        public interface IExternalService1 {
            public bool ProcessData1();
            public string ServiceAddress { get; set; }
        }
        public class ExternalService1 : IExternalService1 {
            public string ServiceAddress { get; set; }

            public bool ProcessData1() {
                Console.WriteLine($"ProcessData1 with address {ServiceAddress}");
                return true;
            }
        }

        public interface IExternalService2 {

            public static void DoExtra() {
                //
            }

            // default interface method
            public bool ProcessData2() {
                Console.WriteLine($"ProcessData2 with address {ServiceAddress}");
                return false;
            }
            public string ServiceAddress { get; set; }
        }
        public class ExternalService2 : IExternalService2 {

            public string ServiceAddress { get; set; }
            
            /// NOTE: this one also works
            // public bool ProcessData2() {
            //     Console.WriteLine($"ProcessData2 changed with address {ServiceAddress}");
            //     return true;
            // }
        }

        public class AllExternalServices : IExternalService1, IExternalService2
        {
            private IExternalService1 s1 = new ExternalService1();
            private IExternalService2 s2 = new ExternalService2();
            private string serviceAddress;

            public string ServiceAddress { 
                get => serviceAddress; 
                set {
                    serviceAddress = value;
                    s1.ServiceAddress = serviceAddress;
                    s2.ServiceAddress = serviceAddress;
                } 
             }

            public bool ProcessData1()
            {
                //forward call to s1
                return s1.ProcessData1();
            }

            public bool ProcessData2()
            {
                //forward call to s2
                return s2.ProcessData2();
            }
        }
    }
}