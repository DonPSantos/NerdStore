using MediatR;
using Microsoft.AspNetCore.Mvc;
using NerdStore.Core.Mediatr;
using NerdStore.Core.Messages.CommonMessages.Notifications;

namespace NerdStore.WebApps.MVC.Controllers
{
    public abstract class ControllerBase : Controller
    {
        protected Guid ClienteId = Guid.NewGuid();
        private readonly DomainNotificationHandler _notifications;
        private readonly IMediatorHandler _mediatorHandler;

        public ControllerBase(INotificationHandler<DomainNotification> notifications, IMediatorHandler mediatorHandler)
        {
            _notifications = (DomainNotificationHandler)notifications;
            _mediatorHandler = mediatorHandler;
        }

        protected bool OperacaoValida() => !_notifications.TemNotificacao();
        protected void NotificarErro(string codigo, string message) => _mediatorHandler.PublicarNotificacao(new DomainNotification(codigo, message));
        protected IEnumerable<string> ObterMenssagensErro() => _notifications.ObterNotificacoes().Select(x => x.Value).ToList();
    }
}
