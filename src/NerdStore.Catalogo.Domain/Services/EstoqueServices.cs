using NerdStore.Catalogo.Domain.Events;
using NerdStore.Catalogo.Domain.Interfaces;
using NerdStore.Core.Mediatr;

namespace NerdStore.Catalogo.Domain.Services
{
    public class EstoqueServices : IEstoqueServices
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IMediatrHandler _mediatr;

        public EstoqueServices(IProdutoRepository produtoRepository, IMediatrHandler mediatr)
        {
            _produtoRepository = produtoRepository;
            _mediatr = mediatr;
        }

        public async Task<bool> DebitarEstoque(Guid produtoId, int quantidade)
        {
            var produto = await _produtoRepository.ObterPorIdAsync(produtoId);

            if (produto is null) return false;

            if (!produto.PossuiEstoque(quantidade)) return false;

            produto.DebitarEstoque(quantidade);

            if (produto.QuantidadeEstoque < 10)
            {
                await _mediatr.PublicarEvento(new ProdutoBaixoEstoqueEvent(produto.Guid, produto.QuantidadeEstoque);
            }

            _produtoRepository.Atualizar(produto);

            return await _produtoRepository.UnitOfWork.Commit();
        }


        public async Task<bool> ReporEstoque(Guid produtoId, int quantidade)
        {
            var produto = await _produtoRepository.ObterPorIdAsync(produtoId);

            if (produto is null) return false;

            produto.ReporEstoque(quantidade);

            _produtoRepository.Atualizar(produto);

            return await _produtoRepository.UnitOfWork.Commit();
        }

        public void Dispose() => _produtoRepository.Dispose();
    }
}
