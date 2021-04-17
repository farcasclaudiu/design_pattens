using System;
using System.Threading.Tasks;

namespace design_patterns.creational.fluentbuilder
{
    /// <summary>
    /// Builder is a creational design pattern that 
    /// lets you construct complex objects step by step. 
    /// The pattern allows you to produce different types 
    /// and representations of an object using the same construction code.
    /// 
    /// Fluent builder - limited as it cannot be extended via inheritance
    /// </summary>
    public class FluentBuilderSample
    {
        public static async Task Run() {
            Console.WriteLine("Creational - Fluent Builder");

            var product = ProductBuilder
                .CreateProduct()
                .WithName("New Product")
                .WithCategory("Best category")
                .WithAge(30)
                .WithPrice(45.22m)
                .Build();
            
            Console.WriteLine("product: " +  product.ToString());
        }
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

    public interface IProductBuilder : 
        IProductName,
        IProductAge,
        IProductPrice,
        IProductCategory {
            Product Build();
    }
    public interface IProductName {
        IProductBuilder WithName(string productName);
    }

    public interface IProductAge {
        IProductBuilder WithAge(int productAge);
    }

    public interface IProductPrice {
        IProductBuilder WithPrice(decimal productPrice);
    }

    public interface IProductCategory {
        IProductBuilder WithCategory(string productCategory);
    }

    public class ProductBuilderWithInfo : ProductBuilder {

        IProductBuilder WithInfo(string info){
            this.instance.Info = info;
            return this;
        }

    }


    public abstract class ProductBuilderBase {

    }
    
    public class ProductBuilder :
        IProductBuilder
    {
        // set as protected to favor inheritance for extensibility of builder class
        protected Product instance;

        protected ProductBuilder() {}
        public static ProductBuilder CreateProduct()
        {
            var pb = new ProductBuilder() {
                instance = new Product()
            };
            return pb;
        }

        public Product Build()
        {
            return instance;
        }

        public IProductBuilder WithAge(int productAge)
        {
            this.instance.Age = productAge;
            return this;
        }

        public IProductBuilder WithCategory(string productCategory)
        {
            this.instance.Category = productCategory;
            return this;
        }

        public IProductBuilder WithName(string productName)
        {
            this.instance.ProductName = productName;
            return this;
        }

        public IProductBuilder WithPrice(decimal productPrice)
        {
            this.instance.Price = productPrice;
            return this;
        }
    }
}