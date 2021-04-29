using System;
using System.Threading.Tasks;

namespace design_patterns.behavioral.template
{
    /// <summary>
    /// Template Method is a behavioral design pattern that defines 
    /// the skeleton of an algorithm in the superclass but lets 
    /// subclasses override specific steps of the algorithm 
    /// without changing its structure.
    /// 
    /// Use it when:
    /// - you want to let clients extend only particular steps 
    ///     of an algorithm, but not the whole algorithm or its structure.
    /// - you have several classes that contain almost identical algorithms
    ///     with some minor differences. As a result, you might need to modify 
    ///     all classes when the algorithm changes.
    /// </summary>
    public class TemplateSample
    {
        public static async Task Run()
        {
            Console.WriteLine("Behavioral - Template");
            System.Console.WriteLine("Execute Service1");
            var service1 = new Service1();
            service1.RunService();
            System.Console.WriteLine("Execute Service2");
            var service2 = new Service2();
            service2.RunService();
        }

        // template
        public abstract class AbstractService {
            public void RunService(){
                this.BaseOperation1();
                this.Operation1();
                this.Operation2();
                this.Operation3();
                this.Operation4();
                this.BaseOperation2();
            }

            protected void BaseOperation1() {
                // some base implementation
                System.Console.WriteLine("Base implementation for BaseOperation1");
            }
            protected void BaseOperation2() {
                // some base implementation
                System.Console.WriteLine("Base implementation for BaseOperation2");
            }

            // abstract methods that needs to be implemented by subclasses
            public abstract void Operation1();
            public abstract void Operation2();

            // virtual methods that can be overriden by subclasses
            public virtual void Operation3() {
                System.Console.WriteLine("Base implementation for Operation3");
            }
            public virtual void Operation4() {
                System.Console.WriteLine("Base implementation for Operation4");
            }
        }

        public class Service1 : AbstractService
        {
            public override void Operation1()
            {
                System.Console.WriteLine("Service1 override for Operation1");
            }

            public override void Operation2()
            {
                System.Console.WriteLine("Service1 override for Operation2");
            }

            public override void Operation4()
            {
                System.Console.WriteLine("Service1 override for Operation4");
                // can call eventually the base implementation
                base.Operation4();
            }
        }

        public class Service2 : AbstractService
        {
            public override void Operation1()
            {
                System.Console.WriteLine("Service2 override for Operation1");
            }

            public override void Operation2()
            {
                System.Console.WriteLine("Service2 override for Operation2");
            }
        }
    }
}