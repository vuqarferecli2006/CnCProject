using Microsoft.Extensions.Hosting;

namespace CnC.Infrastructure.Services.EmailRabbitMQ;

public class EmailConsumerService /*: BackgroundService*/
{
    private readonly EmailConsumer _consumer;

    public EmailConsumerService(EmailConsumer consumer)
    {
        _consumer = consumer;
    }

    //protected override Task ExecuteAsync(CancellationToken stoppingToken)
    //{
    //    _consumer.Start();

    //    return Task.CompletedTask;
    //}
}
