using System;

namespace BootstrapperNetTester.Services
{
    public interface IHelloService
    {
        DateTime DateTime { get; set; }
        string SayHello();
    }

    public class HelloService : IHelloService
    {
        public HelloService()
        {
            DateTime = DateTime.Now;
        }

        public DateTime DateTime { set; get; }

        public string SayHello()
        {
            return $"Hello ! ({DateTime:dd.MM.yyyy HH:mm:ss:fff})";
        }
    }
}
