using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace design_patterns.behavioral.visitor
{
    /// <summary>
    /// Visitor is a behavioral design pattern that lets you 
    /// separate algorithms from the objects on which they operate.
    /// 
    /// Use it when:
    /// - you need to perform an operation on all elements of a 
    ///     complex object structure (for example, an object tree).
    /// - to clean up the business logic of auxiliary behaviors.
    /// - a behavior makes sense only in some classes of a class hierarchy, 
    ///     but not in others.
    /// </summary>
    public class VisitorSample
    {
        public static async Task Run()
        {
            Console.WriteLine("Behavioral - Visitor");
            List<IComponent> components = new List<IComponent>();
            components.Add(new ComponentA());
            components.Add(new ComponentB());

            System.Console.WriteLine("With Visitor1 ....");
            Visitor1 visitor1 = new Visitor1();
            foreach (var component in components)
            {
                component.Visit(visitor1);
            }

            System.Console.WriteLine("With Visitor2 ....");
            Visitor2 visitor2 = new Visitor2();
            foreach (var component in components)
            {
                component.Visit(visitor2);
            }
        }

        // implemented by components/hierarchy that needs to be visited
        public interface IComponent {
            void Visit(IVisitor visitor);
        }

        public class ComponentA : IComponent {
            public void IamComponentA() {
                System.Console.WriteLine("IamComponentA");
            }

            public void Visit(IVisitor visitor)
            {
                visitor.VisitComponentA(this);
            }
        }

        public class ComponentB : IComponent {
            public void SomeStuffFromComponentB() {
                System.Console.WriteLine("SomeStuffFromComponentB");
            }

            public void Visit(IVisitor visitor)
            {
                visitor.VisitComponentB(this);
            }
        }

        // visitor interface declares a set of visiting methods that correspond
        // to component classes. The signature of a visiting method allows the
        // visitor to identify the exact class of the component that it's dealing
        // with.
        public interface IVisitor {
            void VisitComponentA(ComponentA component);
            void VisitComponentB(ComponentB component);
        }

        public class Visitor1 : IVisitor
        {
            public void VisitComponentA(ComponentA component)
            {
                System.Console.Write($"From {this.GetType().Name} ");
                component.IamComponentA();
            }

            public void VisitComponentB(ComponentB component)
            {
                System.Console.Write($"From {this.GetType().Name} ");
                component.SomeStuffFromComponentB();
            }
        }

        public class Visitor2 : IVisitor
        {
            public void VisitComponentA(ComponentA component)
            {
                System.Console.Write($"From {this.GetType().Name} ");
                component.IamComponentA();
            }

            public void VisitComponentB(ComponentB component)
            {
                System.Console.Write($"From {this.GetType().Name} ");
                component.SomeStuffFromComponentB();
            }
        }
    }
}