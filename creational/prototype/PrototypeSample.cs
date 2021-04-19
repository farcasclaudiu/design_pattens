using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;

namespace design_patterns.creational.prototype
{
    /// <summary>
    /// Prototype is a creational design pattern that 
    /// lets you copy (clone, deep copy) existing objects without making 
    /// your code dependent on their classes.
    /// </summary>
    public class PrototypeSample
    {
        public static async Task Run()
        {
            Console.WriteLine("Creational - Prototype");
            Product p1 = new Product {
                Name = "Product 1",
                Details = "details 1",
                Price = 44.22m,
                DiscountPercentLevel1 = 5,
                DiscountPercentLevel2 = 10
            };

            Console.WriteLine($"p1 -> {p1}");

            // var p2 = p1.DeepCopyWithBinaryFormatter();
            var p2 = p1.DeepCopyWithXmlFormatter();
            p2.Name = "Product 2";
            p2.Price = 11.22m;
            Console.WriteLine($"p2 -> {p2}");
        }
    }

    public static class PrototypeExtensions {

        // deep copy with binaryformatter
        // ATTENTION ! 
        // - https://docs.microsoft.com/en-gb/dotnet/standard/serialization/binaryformatter-security-guide
        // - every type has to be serializable
        public static T DeepCopyWithBinaryFormatter<T>(this T from) {
            var formatter = new BinaryFormatter();
            using(var stream  = new MemoryStream()) {
                formatter.Serialize(stream, from);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }


        /// <summary>
        /// Better alternative for security concern
        /// </summary>
        /// <param name="from">original object</param>
        /// <typeparam name="T">type of original object</typeparam>
        /// <returns></returns>
        public static T DeepCopyWithXmlFormatter<T>(this T from) {
            var serializer = new XmlSerializer(typeof(T));
            using(var stream  = new MemoryStream()) {
                serializer.Serialize(stream, from);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)serializer.Deserialize(stream);
            }
        }
    }

    // [Serializable]
    public class Product {
        public string Name { get; set; }
        public string Details { get; set; }

        public decimal Price { get; set; }
        public int DiscountPercentLevel1 { get; set; }
        public int DiscountPercentLevel2 { get; set; }
        

        public override string ToString()
        {
            return $"{Name} : {Details} : {Price} : {DiscountPercentLevel1} : {DiscountPercentLevel2}";
        }
    }
}