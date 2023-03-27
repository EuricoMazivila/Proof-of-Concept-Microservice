using MassTransit;
using Microsoft.Extensions.Logging;
using Models;
using Newtonsoft.Json;

namespace Infrastructure.MessageBroker;

public class UserCreatedEventConsumer : IConsumer<UserCreatedEvent>
{ 
    private readonly ILogger<UserCreatedEventConsumer> _logger;

    public UserCreatedEventConsumer(ILogger<UserCreatedEventConsumer> logger)
    {
        _logger = logger;
    } 
   
    public Task Consume(ConsumeContext<UserCreatedEvent> context)
    {
        var jsonMessage = JsonConvert.SerializeObject(context.Message);
        _logger.LogInformation("User Created: {@User}", jsonMessage);
       
        return Task.CompletedTask;
    }
}