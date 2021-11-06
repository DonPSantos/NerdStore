using NerdStore.Catalogo.Domain.Entitys;
using NerdStore.Core.Data;

namespace NerdStore.Catalogo.Domain.Interfaces
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        Task<IEnumerable<Produto>> ObterTodosAsync();
        Task<Produto> ObterPorIdAsync(Guid id);
        Task<IEnumerable<Produto>> ObterPorCategoriaAsync(int codigoCategoria);
        Task<IEnumerable<Categoria>> ObterCategoriasAsync();


        void Adicionar(Produto produto);
        void Atualizar(Produto produto);
        void Adicionar(Categoria categoria);
        void Atualizar(Categoria categoria);

    }
}
