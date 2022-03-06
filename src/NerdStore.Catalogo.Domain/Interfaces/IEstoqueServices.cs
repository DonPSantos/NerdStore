using NerdStore.Core.DomainObjects.DTO;

namespace NerdStore.Catalogo.Domain.Interfaces
{
    public interface IEstoqueServices : IDisposable
    {
        Task<bool> ReporEstoque(Guid produtoId, int quantidade);
        Task<bool> ReporListaProdutosPedidoEstoque(ListaProdutosPedido listaProdutosPedido);

        Task<bool> DebitarEstoque(Guid produtoId, int quantidade);
        Task<bool> DebitarListaProdutosPedidoEstoque(ListaProdutosPedido listaProdutosPedido);
    }
}
