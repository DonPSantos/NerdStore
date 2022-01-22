using NerdStore.Core.Data;
using NerdStore.Vendas.Domain.Entitys;

namespace NerdStore.Vendas.Domain.Interfaces
{
    public interface IPedidoRepository : IRepository<Pedido>
    {
        Task<Pedido> ObterPorIdAsync(Guid PedidoId);
        Task<IEnumerable<Pedido>> ObterListaPorClienteIdAsync(Guid clienteId);
        Task<Pedido> ObterPedidoRascunhoPorClienteIdAsync(Guid clienteId);
        void Adicionar(Pedido pedido);
        void Atualizar(Pedido pedido);

        Task<PedidoItem> ObterItemPorIdAsync(Guid produtoId);
        Task<PedidoItem> ObterItemPorPedidoAsync(Guid pedidoId, Guid produtoId);
        void AdicionarProduto(PedidoItem produto);
        void AtualizarProduto(PedidoItem produto);
        void RemoverProduto(PedidoItem produto);

        Task<Voucher> ObterVoucherPorCodigo(string codigo);
    }
}
