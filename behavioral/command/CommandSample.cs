using System;
using System.Threading.Tasks;

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
            ICommand addCommand = new AddOrderCommand(order);
            addCommand.Execute();
            ICommand removeCommand = new RemoveOrderCommand(order.ID);
            removeCommand.Execute();
        }
    }

    public interface ICommand
    {
        void Execute();
    }

    public class AddOrderCommand : ICommand {
        public AddOrderCommand(Order order) => this.Order = order;

        public Order Order { get; private set; }

        public void Execute()
        {
            Console.WriteLine($"Execute add order command for order {Order.ID}");
        }
    }

    public class RemoveOrderCommand : ICommand {
        public RemoveOrderCommand(string orderId) => this.OrderID = orderId;

        public string OrderID { get; private set; }

        public void Execute()
        {
            Console.WriteLine($"Execute remove order command for order {OrderID}");
        }
    }

    public class Order {
        public string ID { get; set; }
    }
}