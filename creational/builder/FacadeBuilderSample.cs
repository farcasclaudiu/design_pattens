using System;
using System.Threading.Tasks;

namespace design_patterns.creational.facadebuilder
{
    /// <summary>
    /// this sample covers the case when we have multiple builders 
    /// for the same class, that we want to chain
    /// </summary>
    public class FacadeBuilderSample
    {
        public static async Task Run() {
            Console.WriteLine("Creational - Facade Builder");

            var product = new ProductBuilder()
                .Info
                    .WithName("New Product")
                    .WithDetails("product details")
                .Price
                    .WithPrice(67.12m)
                    .WithDiscountLevel1(4)
                    .WithDiscountLevel2(12)
                .Build();
            
            Console.WriteLine("product: " +  product.ToString());

            // sample with implicit operator casting
            Product product2 = new ProductBuilder()
                .Info
                    .WithName("New Product 2")
                    .WithDetails("product details 2")
                .Price
                    .WithPrice(23.11m)
                    .WithDiscountLevel1(5)
                    .WithDiscountLevel2(10);
            
            Console.WriteLine("product: " +  product2.ToString());
        }
    }

    public class Product {
        // first section handled by a builder
        public string Name { get; set; }
        public string Details { get; set; }

        // second section handled by another builder
        public decimal Price { get; set; }
        public int DiscountPercentLevel1 { get; set; }
        public int DiscountPercentLevel2 { get; set; }
        

        public override string ToString()
        {
            return $"{Name} : {Details} : {Price} : {DiscountPercentLevel1} : {DiscountPercentLevel2}";
        }
    }

    // this acts as a facade
    public class ProductBuilder
    {
        protected Product instance = new Product();

        public ProductInfoBuilder Info => new ProductInfoBuilder(instance);
        public ProductPriceBuilder Price => new ProductPriceBuilder(instance);

        public Product Build() {
            return instance;
        }

        public static implicit operator Product(ProductBuilder pb) => pb.instance;
    }

    public class ProductInfoBuilder : ProductBuilder {
        public ProductInfoBuilder(Product instance) {
            this.instance = instance;
        }

        public ProductInfoBuilder WithName(string name) {
            this.instance.Name = name;
            return this;
        }

        public ProductInfoBuilder WithDetails(string details) {
            this.instance.Details = details;
            return this;
        }
    }

    public class ProductPriceBuilder : ProductBuilder {
        public ProductPriceBuilder(Product instance) {
            this.instance = instance;
        }

        public ProductPriceBuilder WithPrice(decimal price) {
            this.instance.Price = price;
            return this;
        }

        public ProductPriceBuilder WithDiscountLevel1(int discount) {
            this.instance.DiscountPercentLevel1 = discount;
            return this;
        }

        public ProductPriceBuilder WithDiscountLevel2(int discount) {
            this.instance.DiscountPercentLevel2 = discount;
            return this;
        }
    }
}