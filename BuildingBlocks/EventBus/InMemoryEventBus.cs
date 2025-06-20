// InMemoryEventBus.cs
using System;
using System.Collections.Concurrent;

namespace BuildingBlocks.EventBus;

public class InMemoryEventBus : IEventBus
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ConcurrentDictionary<Type, List<Type>> _handlers = new();

    public InMemoryEventBus(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task PublishAsync<T>(T @event) where T : class
    {
        var eventType = typeof(T);
        if (_handlers.TryGetValue(eventType, out var handlers))
        {
            foreach (var handlerType in handlers)
            {
                var handler = _serviceProvider.GetService(handlerType);
                if (handler is IEventHandler<T> eventHandler)
                {
                    eventHandler.Handle(@event);
                }
            }
        }

        return Task.CompletedTask;
    }

    public Task SubscribeAsync<T, TH>() where T : class where TH : IEventHandler<T>
    {
        var eventType = typeof(T);
        var handlerType = typeof(TH);

        _handlers.AddOrUpdate(eventType,
            addValueFactory: _ => new List<Type> { handlerType },
            updateValueFactory: (_, list) =>
            {
                list.Add(handlerType);
                return list;
            });
        return Task.CompletedTask;
    }
}