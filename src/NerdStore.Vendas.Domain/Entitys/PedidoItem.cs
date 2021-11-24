using NerdStore.Core.DomainObjects;

namespace NerdStore.Vendas.Domain.Entitys
{
    public class PedidoItem : Entity
    {
        public Guid ProdutoId { get; private set; }
        public Guid PedidoId { get; private set; }
        public string ProdutoNome { get; private set; }
        public int Quantidade { get; private set; }
        public decimal ValorUnitario { get; private set; }

        //EF Relation 
        public virtual Pedido Pedido { get; private set; }
        protected PedidoItem() { }

        public PedidoItem(Guid produtoId, string produtoNome, int quantidade, decimal valorUnitario)
        {
            ProdutoId = produtoId;
            ProdutoNome = produtoNome;
            Quantidade = quantidade;
            ValorUnitario = valorUnitario;
        }
        internal void AssociarPedido(Guid pedidoId)
        {
            PedidoId = pedidoId;
        }

        internal decimal CalcularValor()
        {
            return Quantidade * ValorUnitario;
        }

        internal void AdicionarUnidades(int unidades)
        {
            Quantidade += unidades;
        }

        internal void AtualizarUnidades(int unidades)
        {
            Quantidade = unidades;
        }
    }
}
