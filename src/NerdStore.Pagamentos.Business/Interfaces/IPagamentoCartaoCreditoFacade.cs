using NerdStore.Pagamentos.Business.Entities;

namespace NerdStore.Pagamentos.Business.Interfaces
{
    public interface IPagamentoCartaoCreditoFacade
    {
        Transacao RealizarPagamento(Pedido pedido, Pagamento pagamento);
    }
}
