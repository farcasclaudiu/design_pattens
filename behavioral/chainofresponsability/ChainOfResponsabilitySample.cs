using System;
using System.Threading.Tasks;
using System.Collections.Generic

namespace design_patterns.behavioral.chainofresponsability
{
    /// <summary>
    /// Chain of Responsibility (CoR) is a behavioral design pattern that 
    /// lets you pass requests along a chain of handlers. 
    /// Upon receiving a request, each handler decides either 
    /// to process the request or to pass it to the next handler in the chain.
    /// 
    /// Use it when:
    /// - your program is expected to process different kinds of requests 
    ///     in various ways, but the exact types of requests and their sequences
    ///     are unknown beforehand.
    /// - it’s essential to execute several handlers in a particular order.
    /// - the set of handlers and their order are supposed to change at runtime.
    /// </summary>
    public class ChainOfResponsabilitySample
    {
        public static async Task Run()
        {
            Console.WriteLine("Behavioral - ChainOfResponsability");

            var request = new RequestInfo {
                ID = Guid.NewGuid().ToString(),
                Data = "request data",
                Date = DateTime.Now
            };
            
            var handler = new BaseReqHandler { Request = request };
            handler.Add(new SecurityCheckReqHandler(false));
            handler.Add(new DataValidationReqHandler());

            handler.Handle();

            handler = new BaseReqHandler { Request = request };
            handler.Add(new SecurityCheckReqHandler(true));
            handler.Add(new DataValidationReqHandler());

            handler.Handle();
        }
    }

    public interface IReqHandler {
        RequestInfo Request {get; set;}
        void Add(IReqHandler reqHandler);
        void Handle();
    }

    public class BaseReqHandler : IReqHandler {
        public virtual RequestInfo Request { get; set; }
        private IReqHandler next;

        public void Add(IReqHandler reqHandler){
            if(next!=null) {
                next.Add(reqHandler);
            }
            else
            {
                reqHandler.Request = this.Request;
                next = reqHandler;
            }
        }
        public virtual void Handle() {
            next?.Handle();
        }
    }

    public class DataValidationReqHandler : BaseReqHandler {
        public override void Handle()
        {
            Console.WriteLine($"DataValidating the request {Request.ID}");
            base.Handle();
        }
    }
    public class SecurityCheckReqHandler : BaseReqHandler {
        private bool isSecure;
        public SecurityCheckReqHandler(bool isSecure) => this.isSecure = isSecure;

        public override void Handle()
        {
            Console.WriteLine($"SecurityCheck the request {Request.ID}");
            if(isSecure) {
                base.Handle();
            }
            else {
                Console.WriteLine("Unauthorized!");
            }
        }
    }

    public class RequestInfo {
        public string ID { get; set; }
        public string Data { get; set; }
        public DateTime Date { get; set; }
    }
}