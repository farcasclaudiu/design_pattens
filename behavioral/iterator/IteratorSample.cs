using System.Runtime.InteropServices;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;

namespace design_patterns.behavioral.iterator
{
    /// <summary>
    /// Iterator is a behavioral design pattern that lets you traverse elements 
    /// of a collection without exposing its underlying representation 
    /// (list, stack, tree, etc.).
    /// 
    /// Use it:
    /// - when your collection has a complex data structure under the hood, 
    ///     but you want to hide its complexity from clients.
    /// - to reduce duplication of the traversal code across your app.
    /// - when you want your code to be able to traverse different data structures 
    ///     or when types of these structures are unknown beforehand.
    /// </summary>
    public class IteratorSample
    {
        public static async Task Run()
        {
            Console.WriteLine("Behavioral - Iterator");

            var boss = new Employee() { Name = "Boss" };
            var emp1 = new Employee { Name = "Employee 1" };
            boss.AddUnder(emp1);
            emp1.AddUnder(new Employee { Name = "Emp 1.1"});
            emp1.AddUnder(new Employee { Name = "Emp 1.2"});
            var emp13 = new Employee { Name = "Emp 1.3"};
            emp1.AddUnder(emp13);
            emp13.AddUnder(new Employee { Name = "Emp 1.3.1"});
            emp13.AddUnder(new Employee { Name = "Emp 1.3.2"});
            emp13.AddUnder(new Employee { Name = "Emp 1.3.3"});
            emp1.AddUnder(new Employee { Name = "Emp 1.4"});
            var emp2 = new Employee { Name = "Employee 2" };
            boss.AddUnder(emp2);
            emp2.AddUnder(new Employee { Name = "Emp 2.1"});

            var custEnum = new Organisation(boss);
            foreach(var emp in custEnum)
                Console.WriteLine($"employee {emp.Name}");
        }

        public class Employee {
            public string Name { get; set; }
            public Employee Boss { get; set; }
            public List<Employee> Underlings = new List<Employee>();

            public void AddUnder(Employee employee)
            {
                Underlings.Add(employee);
                employee.Boss = this;
            }
        }

        public class Organisation {
            private Employee root;

            public Organisation(Employee root)
            {
                this.root = root;
            }

            // enumerator method to iterate in the hierarchy
            public OrganisationIterator GetEnumerator() {
                return new OrganisationIterator(root);
            }
        }

        public class OrganisationIterator {

            private Employee root;

            public OrganisationIterator(Employee root) {
                this.root = root;
                this.Current = root;
            }

            // Current property is mandatory
            public Employee Current { get; set; }

            private bool yieldStarted;
            private Dictionary<int, int> idx = new Dictionary<int, int>();

            // MoveNext method is mandatory
            public bool MoveNext() {
                if(!yieldStarted){
                    Current = root;
                    yieldStarted = true;
                    return true;
                }
            position1:
                var currentKey = Current.GetHashCode();
                if(!idx.ContainsKey(currentKey)){
                    idx[currentKey] = -1;
                }
                if(Current.Underlings!=null) {
                    if(Current.Underlings.Count>(idx[currentKey]+1)) {
                        idx[currentKey] += 1;
                        Current = Current.Underlings[idx[currentKey]];
                        return true;
                    }
                }
                if(Current.Boss!=null)
                {
                    Current = Current.Boss;
                    goto position1;
                }
                return false;
            }

            public void Reset() {
                Current = root;
                idx.Clear();
                yieldStarted = false;
            }
        }
    }
}