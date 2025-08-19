using CnC.Application.Abstracts.Services;
using CnC.Application.DTOs;
using CnC.Application.Shared.Responses;
using CnC.Application.Shared.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Net;
using System.Text;

namespace CnC.Infrastructure.Services.EmailRabbitMQ;

public class RabbitMqEmailQueueService : IEmailQueueService
{

    private readonly RabbitMqSettings _settings;

    public RabbitMqEmailQueueService(IOptions<RabbitMqSettings> options)
    {
        _settings = options.Value;
    }

    public async Task<EmailQueueResponse> PublishAsync(EmailMessageDto email)
    {
        var response = new EmailQueueResponse();

        var factory = new ConnectionFactory
        {
            HostName = _settings.Host,
            UserName = _settings.Username,
            Password = _settings.Password,
            VirtualHost = _settings.VirtualHost,
            Port = _settings.Port,

            Ssl = new SslOption
            {
                Enabled = false,
                ServerName = _settings.Host,
                AcceptablePolicyErrors =
                    System.Net.Security.SslPolicyErrors.RemoteCertificateChainErrors |
                    System.Net.Security.SslPolicyErrors.RemoteCertificateNameMismatch |
                    System.Net.Security.SslPolicyErrors.RemoteCertificateNotAvailable
            }
        };

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.ExchangeDeclare(
            _settings.Exchange,
            ExchangeType.Direct,
            durable: true
        );

        channel.QueueDeclare(
            _settings.Queue,
            durable: true,
            exclusive: false,
            autoDelete: false
        );
        channel.QueueBind(
            _settings.Queue,
            _settings.Exchange,
            _settings.RoutingKey
        );

        var json = JsonConvert.SerializeObject(email);
        var body = Encoding.UTF8.GetBytes(json);

        channel.ConfirmSelect();

        channel.BasicPublish(
            exchange: _settings.Exchange,
            routingKey: _settings.RoutingKey,
            basicProperties: null,
            body: body
        );
        bool confirmed = channel.WaitForConfirms(TimeSpan.FromSeconds(5));

        response.Success = confirmed;
        response.Message = confirmed
            ? "Email RabbitMQ-ya göndərildi."
            : "Email RabbitMQ-ya göndərilə bilmədi.";

        return await Task.FromResult(response);
    }
}

