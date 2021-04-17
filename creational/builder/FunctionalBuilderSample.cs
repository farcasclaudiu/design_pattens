using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace design_patterns.creational.functionalbuilder
{
    /// <summary>
    /// Builder is a creational design pattern that 
    /// lets you construct complex objects step by step. 
    /// The pattern allows you to produce different types 
    /// and representations of an object using the same construction code.
    /// 
    /// Functional builder - allow builder extension via extended methods
    /// </summary>
    public class FunctionalBuilderSample
    {
        public static async Task Run() {
            Console.WriteLine("Creational - Functional Builder");

            var product = new ProductBuilder()
                .WithName("New Product")
                .WithCategory("Best category")
                .WithAge(30)
                .WithPrice(45.22m)
                .Build();
            
                Console.WriteLine("product: " +  product.ToString());
        }
    }

    public abstract class FunctionalBuilderBase<T, SELF> 
        where SELF : FunctionalBuilderBase<T, SELF>
        where T : new()
    {
        private readonly List<Func<T, T>> actions = new List<Func<T, T>>();

        public SELF Apply(Action<T> action) {
            actions.Add(t => {
                action(t);
                return t;
            });
            return this as SELF;
        }

        public T Build() {
            return actions.Aggregate(new T(), (t, f) => f(t));
        }

    }
    public sealed class ProductBuilder : FunctionalBuilderBase<Product, ProductBuilder> {
        public ProductBuilder WithName(string name) =>
            Apply(p => p.ProductName = name);
    }

    /// <summary>
    /// allow extending the builder
    /// </summary>
    public static class ProductBuilderExtensions {
        public static ProductBuilder WithAge(this ProductBuilder builder, int age) =>
            builder.Apply(p => p.Age = age);
        public static ProductBuilder WithCategory(this ProductBuilder builder, string category) =>
            builder.Apply(p => p.Category = category);
        public static ProductBuilder WithPrice(this ProductBuilder builder, decimal price) =>
            builder.Apply(p => p.Price = price);
    }

    public class Product {
        public int Age { get; set; }
        public decimal Price { get; set; }
        public string ProductName { get; set; }
        public string Category { get; set; }
        public string Info { get; internal set; }

        public override string ToString()
        {
            return $"{ProductName} : {Category} : {Age} : {Price}";
        }
    }
}