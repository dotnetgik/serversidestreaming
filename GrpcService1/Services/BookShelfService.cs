using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;
using GrpcService1.Protos;
using Microsoft.Extensions.Logging;

namespace GrpcService1.Services
{
    public class BookShelfService : Protos.BookService.BookServiceBase
    {
        private readonly ILogger<BookShelfService> _logger;
        public BookShelfService(ILogger<BookShelfService> logger)
        {
            _logger = logger;
        }
        public override async Task GetAllBooks(AllBooksRequest request,
            IServerStreamWriter<AllBooksReply> responseStream, ServerCallContext context)
        {
            _logger.Log(LogLevel.Information,"Function started");

            int counter = 0;

            while (!context.CancellationToken.IsCancellationRequested)
            {
                Thread.Sleep(100);
                if(counter==1000)
                    break;
                var books = new List<Book>()
                {
                    new Book(){Description = $"Book_{counter++}",Title = $"Title_{counter}"}
                };
                if (!books.Any())
                {
                    break;
                }

                _logger.Log(LogLevel.Information, $"Sending request for {counter}");

                var reply = new AllBooksReply();
                reply.Books.AddRange(books);
                await responseStream.WriteAsync(reply);
            }
        }
    }
}
