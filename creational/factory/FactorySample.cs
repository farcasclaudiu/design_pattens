using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace design_patterns.creational.factory
{
    /// <summary>
    /// Factory method
    /// provides an interface for creating objects in a superclass, 
    /// but allows subclasses to alter the type of objects 
    /// that will be created.
    /// </summary>
    public class FactorySample
    {
        public static async Task Run()
        {
            Console.WriteLine("Creational - Factory");

            // not allowed to create new Product
            // var pA = new ProductA();

            var productFactory = new ProductFactory();

            var productA = productFactory.CreateProduct<ProductA>();
            Console.WriteLine($"productA is of type { productA.GetType()}");
            productA.ShowDetails();

            var productB = productFactory.CreateProduct<ProductB>();
            Console.WriteLine($"productB is of type { productB.GetType()}");
            productB.ShowDetails();

            var productAasync = await productFactory.CreateProductAsync<ProductA>();
            Console.WriteLine($"productAasync is of type { productAasync.GetType()}");
            productAasync.ShowDetails();

            var productBasync = await productFactory.CreateProductAsync<ProductB>();
            Console.WriteLine($"productBasync is of type { productBasync.GetType()}");
            productBasync.ShowDetails();

            var productC = productFactory.CreateProduct<ProductC>();
            productC.ShowDetails();
            var newProductC = new ProductCFactory().CreateProductC("my extra C product");
            newProductC.ShowDetails();
        }
    }

    public interface IProduct
    {
        void Init();
        Task InitAsync();

        void ShowDetails();
        string Details {get;set;}
    }

    public abstract class ProductBase : IProduct
    {
        public string Details { get ; set; }

        public virtual void Init()
        {
            // default  implementation
            this.Details = $"Default product detail {Guid.NewGuid()}";
        }

        public virtual Task InitAsync()
        {
            // default  implementation
            return Task.FromResult(true);
        }

        public virtual void ShowDetails()
        {
            // default  implementation
            Console.WriteLine($"Default show details for {this.GetType()}: {this.Details}");
        }
    }
    public class ProductA : ProductBase {
        // protect constructor and prevent user to create it
        private ProductA() {}

        public override void Init()
        {
            base.Init();
            // fill details from some other source
            this.Details = $"Details {Guid.NewGuid()}";
        }

        public override async Task InitAsync()
        {
            this.Init();
            // do some other async initialization
            // var x = await Do Some stuff
            await Task.Delay(10);
        }

        public override void ShowDetails()
        {
            Console.WriteLine("ProductA detail: " + this.Details);
        }
    }

    public class ProductB : ProductBase {
        // protect constructor and prevent user to create it
        private ProductB() {}

        public override void Init()
        {
            base.Init();
            // fill details from some other source
            this.Details = $"Details {Guid.NewGuid()}";
        }

        public override async Task InitAsync()
        {
            this.Init();
            // do some other async initialization
            // var x = await Do Some stuff
            await Task.Delay(10);
        }
        public override void ShowDetails()
        {
            Console.WriteLine("ProductB detail: " + this.Details);
        }
    }

    public class ProductC : ProductBase
    {
        // protect constructor and prevent user to create it
        private ProductC() {}
    }
    
    /// <summary>
    /// Custom factory implemented just for ProductC
    /// </summary>
    public class ProductCFactory : ProductFactory {

        public IProduct CreateProductC(string extraDetail)
        {
            var concreteC =  base.CreateProduct<ProductC>();
            concreteC.Details = "Product from custom factory " + extraDetail;
            return concreteC;
        }
    }

    public class ProductFactory : FactoryBase
    {
        public virtual T CreateProduct<T>() where T : class, IProduct {
            var concrete = CreateInstance<T>();
            concrete?.Init();
            return concrete;
        }

        public virtual async Task<T> CreateProductAsync<T>() where T : class, IProduct {
            var concrete = CreateInstance<T>();
            await concrete?.InitAsync();
            return concrete;
        }
    }

    public class FactoryBase
    {
        protected T CreateInstance<T>() where T : class {
            // use of the following approach: activator of compiled expression
            // var concrete = CreateInstanceWithActivator<T>();
            var concrete = CreateInstanceWithLambda<T>();
            return concrete;
        }

        /// <summary>
        /// Create object instance with Activator
        /// </summary>
        /// <typeparam name="T">type of created instance</typeparam>
        private T CreateInstanceWithActivator<T>() where T : class
        {
            var concrete = Activator.CreateInstance(typeof(T), true) as T;
            return concrete;
        }

        // used for caching lambda compilation expressions to reduce reflection overhead
        private static Dictionary<Type, Func<object>> activators = new Dictionary<Type, Func<object>>();
        /// <summary>
        /// Create object instance with lambda compiled expression
        /// </summary>
        /// <typeparam name="T">type of created instance</typeparam>
        private T CreateInstanceWithLambda<T>() where T : class
        {
            var type = typeof(T);
            Func<object> activator;
            if(!activators.TryGetValue(type, out activator)){
                var ctor = type.GetConstructor(
                    BindingFlags.Instance | BindingFlags.CreateInstance |
                    BindingFlags.NonPublic,
                    null, new Type[] { }, null);

                var ctorExpression = Expression.New(ctor);
                activator =  Expression.Lambda<Func<T>>(ctorExpression).Compile();
                activators.Add(type, activator);
            }
            var concrete = activator() as T;
            return concrete;
        }
    }
}