using NerdStore.Catalogo.Domain.Events;
using NerdStore.Catalogo.Domain.Interfaces;
using NerdStore.Core.DomainObjects.DTO;
using NerdStore.Core.Mediatr;

namespace NerdStore.Catalogo.Domain.Services
{
    public class EstoqueServices : IEstoqueServices
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IMediatorHandler _mediatr;

        public EstoqueServices(IProdutoRepository produtoRepository, IMediatorHandler mediatr)
        {
            _produtoRepository = produtoRepository;
            _mediatr = mediatr;
        }

        public async Task<bool> ReporListaProdutosPedidoEstoque(ListaProdutosPedido lista)
        {
            foreach (var item in lista.Itens)
            {
                if (!await ReporItemEstoque(item.Id, item.Quantidade)) return false;
            }

            return await _produtoRepository.UnitOfWork.Commit();
        }

        public async Task<bool> ReporEstoque(Guid produtoId, int quantidade)
        {
            if (!await ReporItemEstoque(produtoId, quantidade)) return false;

            return await _produtoRepository.UnitOfWork.Commit();
        }

        public async Task<bool> DebitarEstoque(Guid produtoId, int quantidade)
        {
            if (!await DebitarItemEstoque(produtoId, quantidade)) return false;

            return await _produtoRepository.UnitOfWork.Commit();
        }

        public async Task<bool> DebitarListaProdutosPedidoEstoque(ListaProdutosPedido lista)
        {
            foreach (var item in lista.Itens)
            {
                if (!await DebitarItemEstoque(item.Id, item.Quantidade)) return false;
            }

            return await _produtoRepository.UnitOfWork.Commit();
        }

        #region Privates
        private async Task<bool> ReporItemEstoque(Guid produtoId, int quantidade)
        {
            var produto = await _produtoRepository.ObterPorIdAsync(produtoId);

            if (produto is null) return false;

            produto.ReporEstoque(quantidade);

            _produtoRepository.Atualizar(produto);

            return await _produtoRepository.UnitOfWork.Commit();
        }

        private async Task<bool> DebitarItemEstoque(Guid produtoId, int quantidade)
        {
            var produto = await _produtoRepository.ObterPorIdAsync(produtoId);

            if (produto is null) return false;

            if (!produto.PossuiEstoque(quantidade)) return false;

            produto.DebitarEstoque(quantidade);

            if (produto.QuantidadeEstoque < 10)
            {
                await _mediatr.PublicarEvento(new ProdutoBaixoEstoqueEvent(produto.Id, produto.QuantidadeEstoque));
            }

            _produtoRepository.Atualizar(produto);

            return await _produtoRepository.UnitOfWork.Commit();
        }
        #endregion

        public void Dispose() => _produtoRepository.Dispose();


    }
}
