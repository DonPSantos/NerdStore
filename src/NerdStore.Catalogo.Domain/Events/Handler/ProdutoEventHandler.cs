using MediatR;
using NerdStore.Catalogo.Domain.Interfaces;
using NerdStore.Core.Mediatr;
using NerdStore.Core.Messages.CommonMessages.IntegrationEvents;
using NerdStore.Infra.EmailServices.Interfaces;

namespace NerdStore.Catalogo.Domain.Events
{
    public class ProdutoEventHandler : INotificationHandler<ProdutoBaixoEstoqueEvent>
                                    , INotificationHandler<PedidoIniciadoEvent>
                                    , INotificationHandler<PedidoProcessamentoCanceladoEvent>
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IEstoqueServices _estoqueServices;
        private readonly IMediatorHandler _mediator;
        private readonly IEmailService _emailServices;

        public ProdutoEventHandler(IProdutoRepository produtoRepository
                                , IEstoqueServices estoqueServices
                                , IMediatorHandler mediator
                                , IEmailService emailServices)
        {
            _produtoRepository = produtoRepository;
            _estoqueServices = estoqueServices;
            _mediator = mediator;
            _emailServices = emailServices;
        }
        public async Task Handle(ProdutoBaixoEstoqueEvent message, CancellationToken cancellationToken)
        {
            var produto = await _produtoRepository.ObterPorIdAsync(message.AggregateId);


            var emailDestino = "donrock333@gmail.com";
            var assunto = "Baixo Estoque";
            var corpo = $"O produto {produto.Nome} está com estoque baixo, por favor repor.";

            await _emailServices.EnviarEmail(emailDestino, assunto, corpo);
        }

        public async Task Handle(PedidoIniciadoEvent message, CancellationToken cancellationToken)
        {
            var result = await _estoqueServices.DebitarListaProdutosPedidoEstoque(message.ProdutosPedido);

            if (result)
                await _mediator.PublicarEvento(new PedidoEstoqueConfirmadoEvent(message.PedidoId, message.ClienteId, message.ProdutosPedido, message.Total, message.NomeCartao, message.NumeroCartao, message.ExpiracaoCartao, message.CvvCartao));
            else
                await _mediator.PublicarEvento(new PedidoEstoqueRejeitadoEvent(message.PedidoId, message.ClienteId));
        }

        public async Task Handle(PedidoProcessamentoCanceladoEvent notification, CancellationToken cancellationToken)
        {
            await _estoqueServices.ReporListaProdutosPedidoEstoque(notification.ProdutosPedido);
        }
    }
}
