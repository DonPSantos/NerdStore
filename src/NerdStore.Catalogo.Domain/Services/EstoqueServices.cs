using NerdStore.Catalogo.Domain.Interfaces;

namespace NerdStore.Catalogo.Domain.Services
{
    public class EstoqueServices : IEstoqueServices
    {
        private readonly IProdutoRepository _produtoRepository;

        public EstoqueServices(IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }

        public async Task<bool> DebitarEstoque(Guid produtoId, int quantidade)
        {
            var produto = await _produtoRepository.ObterPorIdAsync(produtoId);

            if (produto is null) return false;

            if (!produto.PossuiEstoque(quantidade)) return false;

            produto.DebitarEstoque(quantidade);

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
