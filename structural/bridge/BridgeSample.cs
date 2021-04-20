using System;
using System.Threading.Tasks;

namespace design_patterns.structural.bridge
{
    /// <summary>
    /// Bridge is a structural design pattern that lets you split a large class 
    /// or a set of closely related classes into two separate 
    /// hierarchies— abstraction and implementation — which can be 
    /// developed independently of each other.
    /// 
    /// - Used when you want to divide and organize a monolithic class.
    /// - Used when you need to extend a class in several orthogonal 
    ///     (independent) dimensions.
    /// - Used if you need to be able to switch implementations at runtime.
    /// </summary>
    public class BridgeSample
    {
        public static async Task Run()
        {
            Console.WriteLine("Structural - Bridge");

            var notification = new Notification {
                Category = "Cat1",
                Priority = "High",
                Message = "Something happend"
            };
            var notSender = new NotificationSender(new EmailSenderService());
            notSender.SendNotification(notification);
            notSender = new NotificationSender(new SmsSenderService());
            notSender.SendNotification(notification);
        }
    }

    public class Notification
    {
        public string Category { get; set; }
        public string Message { get; set; }
        public string Priority { get; set; }
    }

    // Abstraction
    public class NotificationSender
    {
        private readonly ISenderService senderservice;
        public NotificationSender(ISenderService senderservice)
        {
            this.senderservice = senderservice;
        }

        public void SendNotification (Notification notification) {
            var message = $"\tCategory: {notification.Category}\n\r\tPriority: {notification.Priority}\n\r\tMessage: {notification.Message}";
            senderservice.SendMessage(message);
        }
    }

    // Implementation
    public interface ISenderService {
        void SendMessage(string message);
    }

    public class EmailSenderService : ISenderService
    {
        public void SendMessage(string message)
        {
            System.Console.WriteLine($"Emailing message:\n\r{message}");
        }
    }

    public class SmsSenderService : ISenderService
    {
        public void SendMessage(string message)
        {
            System.Console.WriteLine($"Sms message:\r\n{message}");
        }
    }
}