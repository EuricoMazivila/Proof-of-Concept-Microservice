using Application.Interfaces;
using MassTransit;

namespace Infrastructure.MessageBroker;

public sealed class EventBus : IEventBus
{
    private readonly IBus _bus;
    
    public EventBus(IBus bus)
    {
        _bus = bus;
    }

    public async Task SendAsync<T>(T message, string endpointDescription, CancellationToken cancellationToken = default)
        where T : class
    {
        var uri = new Uri($"rabbitmq://localhost/{endpointDescription}");
        var endpoint = await _bus.GetSendEndpoint(uri);
        await endpoint.Send(message, cancellationToken);
    } 
}