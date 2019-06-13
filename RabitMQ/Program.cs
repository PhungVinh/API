using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace RabitMQ
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "msgKey",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                Console.WriteLine("Enter message to send");
                var msg = Console.ReadLine();
                var body = Encoding.UTF8.GetBytes(msg);
                channel.BasicPublish(exchange: "",
                                     routingKey: "msgKey",
                                     basicProperties: null,
                                     body: body);
                Console.WriteLine(" [x] Sent {0}", msg);
            }

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
