namespace Application.Interfaces;

public interface IEventBus
{
    Task SendAsync<T>(T message, string endpointDescription, CancellationToken cancellationToken = default)
        where T : class;
}