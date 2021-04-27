using System;
using System.Threading.Tasks;

namespace design_patterns.behavioral.mediator
{
    /// <summary>
    /// Mediator is a behavioral design pattern that lets you reduce 
    /// chaotic dependencies between objects. 
    /// The pattern restricts direct communications between the objects 
    /// and forces them to collaborate only via a mediator object.
    /// The Mediator makes it easy to modify, extend and reuse 
    /// individual components because they’re no longer dependent 
    /// on the dozens of other classes.
    /// 
    /// Use when:
    /// - it’s hard to change some of the classes because they are 
    ///     tightly coupled to a bunch of other classes.
    /// - you can’t reuse a component in a different program because 
    ///     it’s too dependent on other components.
    /// - you find yourself creating tons of component subclasses 
    ///     just to reuse some basic behavior in various contexts.
    /// </summary>
    public class MediatorSample
    {
        public static async Task Run()
        {
            Console.WriteLine("Behavioral - Mediator");

            var comp1 = new Component1();
            var comp2 = new Component2();
            var mediator = new Mediator(comp1, comp2);

            System.Console.WriteLine("First test");
            comp1.Exec11();
            System.Console.WriteLine("Second test");
            comp2.Exec22();
        }

        public interface IMediator {
            void Notify(object sender, string eventName);
        }

        /// <summary>
        /// mediator acts as a singleton
        /// </summary>
        public class Mediator : IMediator
        {
            private readonly Component1 _c1;
            private readonly Component2 _c2;

            public Mediator(Component1 c1, Component2 c2)
            {
                this._c1 = c1;
                this._c1.SetMediator(this);
                this._c2 = c2;
                this._c2.SetMediator(this);
            }

            public void Notify(object sender, string eventName)
            {
                System.Console.WriteLine($"Mediator received event {eventName}");
                switch (eventName)
                {
                    case Component1.EX11:
                        _c2.Exec21();
                        break;
                    case Component2.EX22:
                        _c1.Exec12();
                        break;
                    default:
                        break;
                }
            }
        }

        public class BaseComponent {
            protected IMediator _mediator;

            public BaseComponent() : this(null) { }
            public BaseComponent(IMediator mediator)
            {
                this._mediator = mediator;
            }

            public void SetMediator (IMediator mediator) {
                this._mediator = mediator;
            }
        }
        public class Component1 : BaseComponent {

            public const string EX11 = "EX11";
            public const string EX12 = "EX12";

            public void Exec11() {
                System.Console.WriteLine("Comp1 is doing exec11");
                _mediator.Notify(this, EX11);
            }
            public void Exec12() {
                System.Console.WriteLine("Comp1 is doing exec12");
                _mediator.Notify(this, EX12);
            }
        }

        public class Component2 : BaseComponent {
            public const string EX21 = "EX21";
            public const string EX22 = "EX22";

            public void Exec21() {
                System.Console.WriteLine("Comp2 is doing exec21");
                _mediator.Notify(this, EX21);
            }
            public void Exec22() {
                System.Console.WriteLine("Comp2 is doing exec22");
                _mediator.Notify(this, EX22);
            }
        }
    }
}