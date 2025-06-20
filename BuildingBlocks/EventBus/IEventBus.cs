// IEventBus.cs
namespace BuildingBlocks.EventBus;

public interface IEventBus
{
    Task PublishAsync<T>(T @event) where T : class;
    Task SubscribeAsync<T, TH>() where T : class where TH : IEventHandler<T>;
}