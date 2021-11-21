using MediatR;
using NerdStore.Core.Messages;

namespace NerdStore.Core.Mediatr
{
    public class MediatrHandler : IMediatrHandler
    {
        private readonly IMediator _mediatr;

        public MediatrHandler(IMediator mediatr)
        {
            _mediatr = mediatr;
        }
        public async Task PublicarEvento<T>(T evento) where T : Event
        {
            await _mediatr.Publish(evento);
        }
    }
}
