// IEventHandler.cs
namespace BuildingBlocks.EventBus;

public interface IEventHandler<in T> where T : class
{
    Task Handle(T @event);
}