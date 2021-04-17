using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace design_patterns.creational.abstract_factory
{
    /// <summary>
    /// Abstract Factory is a creational design pattern, 
    /// which solves the problem of creating entire product families 
    /// without specifying their concrete classes.
    /// Complexity: 2/3
    /// Popularity: 3/3
    /// Usage examples: 
    ///     The Abstract Factory pattern is pretty common in C# code. 
    ///     Many frameworks and libraries use it to provide a way 
    ///     to extend and customize their standard components.
    /// Identification: 
    ///     The pattern is easy to recognize by methods, 
    ///     which return a factory object. Then, the factory is used 
    ///     for creating specific sub-components.
    /// </summary>
    public class AbstractFactorySample
    {
        public static async Task Run()
        {
            Console.WriteLine("Creational - Abstract Factory");
            
            var abstFactory = new WorkshopFactory();
            var p1 = abstFactory.GetProduct("a");
            Console.WriteLine($"product is of type {p1.GetType()}");
            p1.ShowDetails();
            var p2 = abstFactory.GetProduct("b");
            Console.WriteLine($"product is of type {p2.GetType()}");
            p2.ShowDetails();
            try
            {
                var p3 = abstFactory.GetProduct("c");
                p3.ShowDetails();
            }
            catch (System.Exception ex)
            {
                if(ex is ArgumentException exa && exa.Message.Contains("not available in existing factories")){
                    Console.WriteLine($"p3 cannot be create because there is no factory of this type.");
                }else{
                    throw;
                }
            }
            
        }
    }

    public interface IProduct
    {
        string Details {get;set;}
        void ShowDetails();
    }

    public class ProductA : IProduct
    {
        public string Details { get; set; }

        public void ShowDetails()
        {
            Console.WriteLine($"ProductA details: {this.Details}");
        }
    }

    public class ProductB : IProduct
    {
        public string Details { get; set; }

        public void ShowDetails()
        {
            Console.WriteLine($"ProductB details: {this.Details}");
        }
    }

    public interface IProductFactory {
        IProduct GetProduct();
    }

    public class ProductAFactory : IProductFactory
    {
        public IProduct GetProduct()
        {
            return new ProductA() {
                Details = "ProductA " + Guid.NewGuid()
            };
        }
    }

    public class ProductBFactory : IProductFactory
    {
        public IProduct GetProduct()
        {
            return new ProductB() {
                Details = "ProductB " + Guid.NewGuid()
            };
        }
    }

    public class WorkshopFactory
    {
        private Dictionary<string, IProductFactory> factories = new Dictionary<string, IProductFactory>() {
            {"a", new ProductAFactory()},
            {"b", new ProductBFactory()}
        };
        public IProduct GetProduct(string productType)
        {
            if(factories.TryGetValue(productType, out var factory)){
                var concrete = factory.GetProduct();
                // can do extra
                concrete.Details += " " + Guid.NewGuid();
                return concrete;
            }
            else {
                throw new ArgumentException($"product type {productType} not available in existing factories.");
            }
        }
    }
}