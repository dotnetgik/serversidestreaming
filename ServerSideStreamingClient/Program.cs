using System;
using System.Net.Http;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;
using GrpcService1.Protos;

namespace ServerSideStreamingClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2Support", true);

            var channel = GrpcChannel.ForAddress("https://localhost:32794");
            var client = new BookService.BookServiceClient(channel);
            using var call = client.GetAllBooks(new AllBooksRequest() {ItemsPerPage = 1});
            while (await call.ResponseStream.MoveNext())
            {
                
                Console.Write(Environment.NewLine);
                Console.Write(call.ResponseStream.Current.Books);
            }

            Console.ReadKey();
        }
    }
}
