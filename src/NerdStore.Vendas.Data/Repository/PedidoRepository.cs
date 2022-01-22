using Microsoft.EntityFrameworkCore;
using NerdStore.Core.Data;
using NerdStore.Vendas.Domain.Entitys;
using NerdStore.Vendas.Domain.Enums;
using NerdStore.Vendas.Domain.Interfaces;

namespace NerdStore.Vendas.Data.Repository
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly VendasContext _vendasContext;

        public PedidoRepository(VendasContext vendasContext)
        {
            _vendasContext = vendasContext;
        }
        public IUnitOfWork UnitOfWork => _vendasContext;

        public void Adicionar(Pedido pedido)
        {
            _vendasContext.Pedidos.Add(pedido);
        }

        public void AdicionarProduto(PedidoItem produto)
        {
            _vendasContext.PedidoItems.Add(produto);
        }

        public void Atualizar(Pedido pedido)
        {
            _vendasContext.Pedidos.Update(pedido);
        }

        public void AtualizarProduto(PedidoItem produto)
        {
            _vendasContext.PedidoItems.Update(produto);
        }

        public async Task<PedidoItem> ObterItemPorIdAsync(Guid produtoId)
        {
            return await _vendasContext.PedidoItems.FindAsync(produtoId);
        }

        public async Task<PedidoItem> ObterItemPorPedidoAsync(Guid pedidoId, Guid produtoId)
        {
            return await _vendasContext.PedidoItems.FirstOrDefaultAsync(p => p.ProdutoId == produtoId && p.PedidoId == pedidoId);
        }

        public async Task<IEnumerable<Pedido>> ObterListaPorClienteIdAsync(Guid clienteId)
        {
            return await _vendasContext.Pedidos.AsNoTracking().Where(p => p.ClienteId == clienteId).ToListAsync();
        }

        public async Task<Pedido> ObterPedidoRascunhoPorClienteIdAsync(Guid clienteId)
        {
            var pedido = await _vendasContext.Pedidos.FirstOrDefaultAsync(p => p.ClienteId == clienteId && p.PedidoStatus == PedidoStatus.Rascunho);
            if (pedido is null) return null;

            await _vendasContext.Entry(pedido)
                .Collection(i => i.PedidoItems).LoadAsync();

            if (pedido.VoucherId != null)
            {
                await _vendasContext.Entry(pedido)
                    .Reference(i => i.Voucher).LoadAsync();
            }

            return pedido;
        }

        public async Task<Pedido> ObterPorIdAsync(Guid pedidoId)
        {
            return await _vendasContext.Pedidos.FindAsync(pedidoId);
        }

        public async Task<Voucher> ObterVoucherPorCodigo(string codigo)
        {
            return await _vendasContext.Vouchers.FirstOrDefaultAsync(p => p.Codigo == codigo);
        }

        public void RemoverProduto(PedidoItem produto)
        {
            _vendasContext.PedidoItems.Remove(produto);
        }

        public void Dispose() => _vendasContext.Dispose();
    }
}
