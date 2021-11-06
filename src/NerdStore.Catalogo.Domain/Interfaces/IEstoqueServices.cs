namespace NerdStore.Catalogo.Domain.Interfaces
{
    public interface IEstoqueServices : IDisposable
    {
        Task<bool> DebitarEstoque(Guid produtoId, int quantidade);
        Task<bool> ReporEstoque(Guid produtoId, int quantidade);
    }
}
