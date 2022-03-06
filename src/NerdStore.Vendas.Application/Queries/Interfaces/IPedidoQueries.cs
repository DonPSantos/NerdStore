using NerdStore.Vendas.Application.Queries.ViewModels;

namespace NerdStore.Vendas.Application.Queries.Interfaces
{
    public interface IPedidoQueries
    {
        Task<CarrinhoViewModel> ObterCarrinhoCliente(Guid clienteId);
        Task<IEnumerable<PedidoViewModel>> ObterPedidosCliente(Guid clienteId);
    }
}
