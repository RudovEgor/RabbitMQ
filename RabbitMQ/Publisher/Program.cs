using RabbitMQ.Client;
using System;
using System.Diagnostics.Tracing;
using System.Text;
using System.Threading;

namespace Publisher
{
    internal class Program
    {
        
        static void Main(string[] args)
        {
            var counter = 1;
            do
            {
                int timeToSleep = new Random().Next(1000, 3000);//от 1 до 3 секунд
                Thread.Sleep(timeToSleep);
                var factory = new ConnectionFactory() { HostName = "localhost" };
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "dev-queue",
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);
                    string message = $"Message from publisher N {counter}";
                    var body = Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish(exchange: "",
                        routingKey: "dev-queue",
                        basicProperties: null,
                        body: body);
                    Console.WriteLine($"Сообщение было отправлено [N:{counter++}]");
                }
            }
            while (true);
        }
    }
}
