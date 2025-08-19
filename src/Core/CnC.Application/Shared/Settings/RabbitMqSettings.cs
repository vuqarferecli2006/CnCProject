namespace CnC.Application.Shared.Settings;

public class RabbitMqSettings
{
    public string Host { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string VirtualHost { get; set; } = null!;
    public int Port { get; set; }
    public string Exchange { get; set; } = null!;
    public string Queue { get; set; } = null!;
    public string RoutingKey { get; set; } = null!;
}
