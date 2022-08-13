using System;
using System.ServiceModel;
using WorkingClient.AppClient.Handlers;
using WorkingClient.WorkingServiceReference;

namespace WorkingClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var instanceContext = new InstanceContext(new CallbackHandler());
            var client = new WorkingServiceClient(instanceContext);

            client.CompileScript();
            client.RunScript();

            Console.ReadKey();
            client.Close();
        }
    }
}
