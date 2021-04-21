using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace design_patterns.structural.composite
{
    /// <summary>
    /// Composite is a structural design pattern that lets you 
    /// compose objects into tree structures and then 
    /// work with these structures as if they were individual objects.
    /// 
    /// - makes sense only when the core model of your app 
    ///     can be represented as a tree.
    /// </summary>
    public class CompositeSample
    {
        public static async Task Run()
        {
            Console.WriteLine("Structural - Composite");

            var computer = new Computer("IMB PC") {
                Components = {
                    new ComponentGroup {
                        Name = "disk drives",
                        Components = {
                            new Component("HDD1", "Primary hard drive", 50.44m),
                            new Component("HDD2", "Secondary hard drive", 33.10m)
                        }
                    },
                    new Component ("CPU", "Pentium i9", 120m),
                    new GraphicCardNvidiaComponent(160m)
                }
            };

            System.Console.WriteLine($"computer price is : {computer.GetPrice()}");

        }

        public interface IPriceComputable
        {
            public decimal GetPrice();
        }
        public class Computer : ComponentGroup
        {
            public Computer(string computerName)
            {
                this.Name = computerName;
            }
        }

        public class Component : IPriceComputable
        {
            public Component(decimal price)
            {
                this.Price = price;
            }
            public Component(string code, string name, decimal price)
            {
                this.Code = code;
                this.Name = name;
                this.Price = price;
            }
            public virtual string Code { get; set; }
            public virtual string Name { get; set; }
            public virtual decimal Price { get; set; }

            public decimal GetPrice()
            {
                return this.Price;
            }
        }

        public class ComponentGroup : IPriceComputable
        {
            public virtual string Name { get; set; }

            private Lazy<List<IPriceComputable>> _components { get; set; } = new Lazy<List<IPriceComputable>>();
            public List<IPriceComputable> Components { get { return _components.Value; } }

            public decimal GetPrice()
            {
                return Components.Select(p=>p.GetPrice()).Sum();
            }
        }

        public class GraphicCardNvidiaComponent : Component
        {
            public GraphicCardNvidiaComponent(decimal price) : base(price)
            {
            }
            public override string Code { get; set; } = "GPU_Nvidia";
            public override string Name { get; set; } = "RTX 3080";
            
        }
    }
}