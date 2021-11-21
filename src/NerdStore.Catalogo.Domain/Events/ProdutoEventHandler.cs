using MediatR;
using NerdStore.Catalogo.Domain.Interfaces;
using NerdStore.Infra.EmailServices.Interfaces;

namespace NerdStore.Catalogo.Domain.Events
{
    public class ProdutoEventHandler : INotificationHandler<ProdutoBaixoEstoqueEvent>
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IEmailService _emailServices;

        public ProdutoEventHandler(IProdutoRepository produtoRepository
                                , IEmailService emailServices)
        {
            _produtoRepository = produtoRepository;
            _emailServices = emailServices;
        }
        public async Task Handle(ProdutoBaixoEstoqueEvent notification, CancellationToken cancellationToken)
        {
            var produto = await _produtoRepository.ObterPorIdAsync(notification.AggregateId);


            var emailDestino = "donrock333@gmail.com";
            var assunto = "Baixo Estoque";
            var corpo = $"O produto {produto.Nome} está com estoque baixo, por favor repor.";

            await _emailServices.EnviarEmail(emailDestino, assunto, corpo);
        }
    }
}
