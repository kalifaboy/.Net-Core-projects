using Grpc.Core;
using Grpc.Net.Client;
using GrpcServer;
using GrpcServer.Protos;
using System;
using System.Threading.Tasks;

namespace GrpcClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var greeterClient = new Greeter.GreeterClient(channel);
            var request = new HelloRequest() { Name = "Amine" };
            var reply = await greeterClient.SayHelloAsync(request);

            Console.WriteLine(reply.Message);

            var customerClient = new Customer.CustomerClient(channel);
            var clientRequest = new CustomerLookupModel() { UserId = 1 };
            CustomerModel clientReply = await customerClient.GetCustomerInfoAsync(clientRequest);
            Console.WriteLine($"{clientReply.FirstName} {clientReply.LastName}");

            using (var call = customerClient.GetNewCustomers(new NewCustomerRequest()))
            {
                while (await call.ResponseStream.MoveNext())
                {
                    var currentCustomer = call.ResponseStream.Current;
                    Console.WriteLine($"{currentCustomer.FirstName} {currentCustomer.LastName} {currentCustomer.Email}");
                }
            }
            Console.ReadLine();
        }
    }
}
