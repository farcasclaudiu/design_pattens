using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace design_patterns.structural.flyweight
{
    /// <summary>
    /// Flyweight is a structural design pattern that lets you 
    /// fit more objects into the available amount of RAM 
    /// by sharing common parts of state between multiple objects 
    /// instead of keeping all of the data in each object.
    /// 
    /// - to avoid redundancy of stored data
    /// </summary>
    public class FlyweightSample
    {
        private static readonly string[] availableNames = {"alfa", "beta", "gama"};
        
        public static async Task Run()
        {
            Console.WriteLine("Structural - Flyweight");

            Random rnd = new Random();
            List<Person> lst = new List<Person>();
            for (int i = 0; i < 100000; i++)
            {
                lst.Add(new Person(availableNames[rnd.Next(0,2)]));
            }
            System.Console.WriteLine($"lst has {lst.Count} persons.");
        }

        public class Person {

            // data storage
            private static List<string> names = new List<string>();

            private int nameIndex = -1;
            public Person(string name) {
                Name = name;
            }

            public string Name {
                get {
                    return names[nameIndex];
                }
                set {
                    // optimize names storage by place it into 
                    // a centralized storage and reference data
                    // by an index
                    nameIndex = GetIndex(value);
                }
            }

            private static int GetIndex(string name) {
                var idx = names.IndexOf(name);
                if(idx==-1) {
                    names.Add(name);
                    idx = names.Count-1;
                }
                return idx;
            }
        }
    }
}