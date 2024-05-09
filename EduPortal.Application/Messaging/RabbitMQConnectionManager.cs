using RabbitMQ.Client;
using System;

namespace EduPortal.Application.Messaging
{
    public class RabbitMQConnectionManager
    {
        //private readonly ConnectionFactory _factory;
        //private IConnection _connection;
        //private IModel _channel;
        //private int _connectionCount = 0;

        //private static readonly Lazy<RabbitMQConnectionManager> _instance = new Lazy<RabbitMQConnectionManager>(() => new RabbitMQConnectionManager());

        //public static RabbitMQConnectionManager Instance => _instance.Value;

        //private RabbitMQConnectionManager()
        //{
        //    _factory = new ConnectionFactory
        //    {
        //        Uri = new Uri("amqps://wmrjnkqq:mG_N9lRedSbwJDM8JRxA5QfuGi1-E0op@moose.rmq.cloudamqp.com/wmrjnkqq")
        //    };
        //}

        //public IConnection GetConnection()
        //{
        //    if (_connection == null || !_connection.IsOpen)
        //    {
        //        _connection = _factory.CreateConnection();
        //        _connectionCount++;
        //    }

        //    return _connection;
        //}

        //public IModel GetChannel()
        //{
        //    if (_channel == null || _channel.IsClosed)
        //        _channel = GetConnection().CreateModel();

        //    return _channel;
        //}

        //public int GetConnectionCount()
        //{
        //    return _connectionCount;
        //}
    }
}
