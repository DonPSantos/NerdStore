using MediatR;
using NerdStore.Core.Messages;
using NerdStore.Core.Messages.CommonMessages.Notifications;

namespace NerdStore.Core.Mediatr
{
    public class MediatorHandler : IMediatorHandler
    {
        private readonly IMediator _mediatr;

        public MediatorHandler(IMediator mediatr)
        {
            _mediatr = mediatr;
        }

        public async Task<bool> EnviarComando<T>(T comando) where T : Command
        {
            return await _mediatr.Send(comando);
        }

        public async Task PublicarEvento<T>(T evento) where T : Event
        {
        }

        public async Task PublicarNotificacao<T>(T notificacao) where T : DomainNotification
        {
            await _mediatr.Publish(notificacao);
        }
    }
}
