namespace Shared.Contracts.Events;

public interface IEventHandler<in TEvent> 
{
    Task HandleAsync(TEvent @event);
}