using NerdStore.Core.Messages;

namespace NerdStore.Core.Mediatr
{
    public interface IMediatrHandler
    {
        Task PublicarEvento<T>(T evento) where T : Event;
    }
}
