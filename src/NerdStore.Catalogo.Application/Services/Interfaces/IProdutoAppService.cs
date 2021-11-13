using NerdStore.Catalogo.Application.DTOs;

namespace NerdStore.Catalogo.Application.Services.Interfaces
{
    public interface IProdutoAppService : IDisposable
    {
        Task<IEnumerable<ProdutoDTO>> ObterPorCategoria(int codigo);
        Task<ProdutoDTO> ObterPorId(Guid produtoId);
        Task<IEnumerable<ProdutoDTO>> ObterTodos();
        Task<IEnumerable<CategoriaDTO>> ObterCategorias();

        Task AdicionarProduto(ProdutoDTO produto);
        Task AlterarProduto(ProdutoDTO produto);

        Task<ProdutoDTO> DebitarEstoque(Guid produtoId, int quantidade);
        Task<ProdutoDTO> ReporEstoque(Guid produtoId, int quantidade);


    }
}
