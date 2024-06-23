using RabbitMQ.Client;
using System;
using System.Text;

ConnectionFactory factory = new ConnectionFactory
{
    Uri = new Uri("amqp://guest:guest@localhost:5672"),
    ClientProvidedName = "Rabbit sender App"
};

using IConnection cnn = factory.CreateConnection();
using IModel channel = cnn.CreateModel();

string exchangeName = "DemoExchange";
string routingKey = "demo-routing-key";
string queueName = "DemoQueue";

channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
channel.QueueDeclare(queueName, false, false, false, null);
channel.QueueBind(queueName, exchangeName, routingKey, null);

for (int i = 0; i < 1000000; i++)
{
    string message = $"Hello {i}";
    byte[] messageBodyBytes = Encoding.UTF8.GetBytes(message);
    channel.BasicPublish(exchangeName, routingKey, null, messageBodyBytes);
    Console.WriteLine($"Sent message: {message}");
}

channel.Close();
cnn.Close();
