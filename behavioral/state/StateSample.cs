using System;
using System.Threading.Tasks;
using Stateless;

namespace design_patterns.behavioral.state
{
    /// <summary>
    /// State is a behavioral design pattern that lets an object 
    /// alter its behavior when its internal state changes. 
    /// It appears as if the object changed its class.
    /// 
    /// Use it when:
    /// - you have an object that behaves differently depending 
    ///     on its current state, the number of states is enormous, 
    ///     and the state-specific code changes frequently.
    /// - you have a class polluted with massive conditionals 
    ///     that alter how the class behaves according to the current values 
    ///     of the class’s fields.
    /// - you have a lot of duplicate code across similar states 
    ///     and transitions of a condition-based state machine.
    /// 
    /// easy to use STATELESS nuget package
    /// https://www.nuget.org/packages/stateless
    /// https://github.com/dotnet-state-machine/stateless
    /// </summary>
    public class StateSample
    {
        public static async Task Run()
        {
            Console.WriteLine("Behavioral - State");

            var machineOrder = BuildMachine();

            var trigger =OrderTrigger.CreateOrder;
            machineOrder.Fire(trigger);
            System.Console.WriteLine($"state: {machineOrder.State}");
            if(machineOrder.State != OrderState.New)
                System.Console.WriteLine($"Error with order state trigger {trigger} - state {machineOrder.State}");

            trigger =OrderTrigger.RegisterOrder;
            machineOrder.Fire(trigger);
            System.Console.WriteLine($"state: {machineOrder.State}");
            if(machineOrder.State != OrderState.Registered)
                System.Console.WriteLine($"Error with order state trigger {trigger} - state {machineOrder.State}");


            // // once cancelled it cannot migrate to other sates
            // trigger = OrderTrigger.CancellByCustomer;
            // machineOrder.Fire(trigger);
            // System.Console.WriteLine($"state: {machineOrder.State}");

            trigger = OrderTrigger.BeginProcessing;
            machineOrder.Fire(trigger);
            System.Console.WriteLine($"state: {machineOrder.State}");

            trigger = OrderTrigger.Packaging;
            machineOrder.Fire(trigger);
            System.Console.WriteLine($"state: {machineOrder.State}");

            trigger = OrderTrigger.Shipping;
            machineOrder.Fire(trigger);
            System.Console.WriteLine($"state: {machineOrder.State}");

            trigger = OrderTrigger.Delivering; // or .ReturnByCustomer or .ReturnByShipment
            machineOrder.Fire(trigger);
            System.Console.WriteLine($"state: {machineOrder.State}");
        }

        public static StateMachine<OrderState, OrderTrigger> BuildMachine() {
            var machineOrder = new StateMachine<OrderState, OrderTrigger>(OrderState.New);
            // from new to draft
            machineOrder.Configure(OrderState.New)
                .PermitReentry(OrderTrigger.CreateOrder)
                .Permit(OrderTrigger.SaveAsDraft, OrderState.Draft)
                .Permit(OrderTrigger.RegisterOrder, OrderState.Registered);

            // from draft to registered
            machineOrder.Configure(OrderState.Draft)
                .Permit(OrderTrigger.RegisterOrder, OrderState.Registered);
            // from registered to cancelled or begin processing
            machineOrder.Configure(OrderState.Registered)
                .Permit(OrderTrigger.CancellByCustomer, OrderState.Cancelled)
                .Permit(OrderTrigger.BeginProcessing, OrderState.Processing);
            // from processing to cancelled of packaging
            machineOrder.Configure(OrderState.Processing)
                .Permit(OrderTrigger.CancellBySupplier, OrderState.Cancelled)
                .Permit(OrderTrigger.Packaging, OrderState.Packaged);
            // from packaged to shipped or return
            machineOrder.Configure(OrderState.Packaged)
                .Permit(OrderTrigger.Shipping, OrderState.Shipped)
                .Permit(OrderTrigger.ReturnByShipment, OrderState.ReturnedByShipment);
            // from shipped to 
            machineOrder.Configure(OrderState.Shipped)
                .Permit(OrderTrigger.Delivering, OrderState.Completed)
                .Permit(OrderTrigger.ReturnByShipment, OrderState.ReturnedByShipment)
                .Permit(OrderTrigger.ReturnByCustomer, OrderState.ReturnedByCustomer);
            return machineOrder;
        }

        public enum OrderState {
            New,
            Draft,
            Registered,
            Processing,
            Packaged,
            Shipped,
            Completed,
            // other states
            Cancelled,
            ReturnedByShipment,
            ReturnedByCustomer
        }

        public enum OrderTrigger {
            CreateOrder,
            SaveAsDraft,
            RegisterOrder,
            CancellByCustomer,
            CancellBySupplier,
            BeginProcessing,
            Packaging,
            Shipping,
            Delivering,
            ReturnByShipment,
            ReturnByCustomer
        }
    }
}