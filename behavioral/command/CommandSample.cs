using System.Windows.Input;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace design_patterns.behavioral.command
{
    /// <summary>
    /// Command is a behavioral design pattern that turns a request 
    /// into a stand-alone object that contains all information about the request.
    /// This transformation lets you pass requests 
    /// as a method arguments, delay or queue a request’s execution, 
    /// and support undoable operations.
    /// 
    /// Use when:
    /// - you want to parametrize objects with operations.
    /// - you want to queue operations, schedule their execution, 
    ///     or execute them remotely. (serialize commands)
    /// - you want to implement reversible operations.
    /// </summary>
    public class CommandSample
    {
        public static async Task Run()
        {
            Console.WriteLine("Behavioral - Command");

            var order = new Order {
                ID = Guid.NewGuid().ToString()
            };
            List<ICommand> operations = new List<ICommand>();
            operations.Add(new AddOrderCommand(order));
            operations.Add(new RemoveOrderCommand(order.ID));
            
            // batch execution
            foreach(var cmd in operations){
                cmd.Execute();
            }
            // undo
            operations.Reverse();
            foreach(var cmd in operations){
                cmd.Undo();
            }
        }
    }

    public interface ICommand
    {
        bool HasSucceded {get;set;}

        void Execute();
        void Undo();
    }

    public class AddOrderCommand : ICommand {
        public AddOrderCommand(Order order) => this.Order = order;

        public Order Order { get; private set; }
        public bool HasSucceded { get; set; }

        public void Execute()
        {
            Console.WriteLine($"Execute add order command for order {Order.ID}");
            // this should be set by a command processor externally
            HasSucceded = true;
        }

        public void Undo()
        {
            if(HasSucceded){
                Console.WriteLine($"UNDO - add order for order {Order.ID}");
            }
        }
    }

    public class RemoveOrderCommand : ICommand {
        public RemoveOrderCommand(string orderId) => this.OrderID = orderId;

        public string OrderID { get; private set; }
        public bool HasSucceded { get; set; }

        public void Execute()
        {
            Console.WriteLine($"Execute remove order command for order {OrderID}");
            // this should be set by a command processor externally
            // FORCE failure
            HasSucceded = false;
        }

        public void Undo()
        {
            if(HasSucceded){
                Console.WriteLine($"UNDO - remove order for order {OrderID}");
            }
        }
    }

    public class Order {
        public string ID { get; set; }
    }
}