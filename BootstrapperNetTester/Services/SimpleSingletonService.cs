using System;

namespace BootstrapperNetTester.Services
{
    public interface ISimpleSingletonService
    {
        DateTime DateTime { get; set; }
        string SayHello();
    }

    public class SimpleSingletonService : ISimpleSingletonService
    {
        public SimpleSingletonService()
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
