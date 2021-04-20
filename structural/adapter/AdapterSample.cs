using System;
using System.Threading.Tasks;

namespace design_patterns.structural.adapter
{
    /// <summary>
    /// Adapter is a structural design pattern that 
    /// allows objects with incompatible interfaces to collaborate.
    /// 
    /// This is a special object that converts the interface
    /// of one object so that another object can understand it.
    /// 
    /// An adapter wraps one of the objects to hide the complexity 
    /// of conversion happening behind the scenes. 
    /// The wrapped object isn’t even aware of the adapter.
    /// 
    /// It’s very often used in systems based on some legacy code.
    /// </summary>
    public class AdapterSample
    {
        public static async Task Run()
        {
            Console.WriteLine("Structural - Adapter");

            var person = new Person {
                ID = Guid.NewGuid().ToString(),
                Name = "Test person"
            };
            ExternalService service = new ExternalService();

            ExternalServiceAdapter adapter = new ExternalServiceAdapter();
            var status = adapter.GetStatus(person, service);

            Console.WriteLine($"Status of {status.ID} - {status.PersonStatus}");
        }
    }

    // client DDD models - we don't want to modify their structure
    // to match external system and inherit unwanted data and behavior
    public class Person {
        public string Name { get; set; }
        public string ID { get; set; }
    }
    public class PersonInfo {
        public string ID { get; set; }
        public string PersonStatus { get; set; }
    }

    // defined inside client DDD core
    // interface for adapting external service
    public interface IExternalService {
        PersonInfo GetStatus(Person person, ExternalService service);
    }

    // (legacy) external Service
    public class ExternalService {
        public string GetPersonStatus(string id) {
            return $"Status OK";
        }
    }

    // adapter implementation, we can have 
    // multiple adapter implementations if needed
    public class ExternalServiceAdapter : IExternalService
    {
        public PersonInfo GetStatus(Person person, ExternalService service)
        {
            return new PersonInfo {
                ID = person.ID,
                PersonStatus = service.GetPersonStatus(person.ID)
            };
        }
    }


}