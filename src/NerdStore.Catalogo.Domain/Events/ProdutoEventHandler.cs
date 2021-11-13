using MediatR;
using NerdStore.Catalogo.Domain.Interfaces;

namespace NerdStore.Catalogo.Domain.Events
{
    public class ProdutoEventHandler : INotificationHandler<ProdutoBaixoEstoqueEvent>
    {
        private readonly IProdutoRepository _produtoRepository;

        public ProdutoEventHandler(IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }
        public async Task Handle(ProdutoBaixoEstoqueEvent notification, CancellationToken cancellationToken)
        {
            var produto = _produtoRepository.ObterPorIdAsync(notification.AggregateId);

            //TODO: implementar envio de email, no código do sistema do SENAI tem um exemplo.

            throw new NotImplementedException();
        }
    }
}
