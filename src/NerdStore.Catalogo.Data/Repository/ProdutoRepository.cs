using Microsoft.EntityFrameworkCore;
using NerdStore.Catalogo.Domain.Entitys;
using NerdStore.Catalogo.Domain.Interfaces;
using NerdStore.Core.Data;

namespace NerdStore.Catalogo.Data.Repository
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly CatalogoContext _catalogoContext;

        public ProdutoRepository(CatalogoContext catalogoContext)
        {
            _catalogoContext = catalogoContext;
        }
        public IUnitOfWork UnitOfWork => _catalogoContext;

        public void Adicionar(Produto produto)
        {
            _catalogoContext.Produtos.Add(produto);
        }

        public void Adicionar(Categoria categoria)
        {
            _catalogoContext.Categorias.Add(categoria);
        }

        public void Atualizar(Produto produto)
        {
            _catalogoContext.Produtos.Update(produto);
        }

        public void Atualizar(Categoria categoria)
        {
            _catalogoContext.Categorias.Update(categoria);
        }

        public async Task<IEnumerable<Categoria>> ObterCategoriasAsync()
        {
            return await _catalogoContext.Categorias.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<Produto>> ObterPorCategoriaAsync(int codigoCategoria)
        {
            return await _catalogoContext
                .Produtos
                .AsNoTracking()
                .Include(p => p.Categoria)
                .Where(p => p.Categoria.Codigo == codigoCategoria)
                .ToListAsync();
        }

#pragma warning disable CS8603 // Possível retorno de referência nula.
        public async Task<Produto> ObterPorIdAsync(Guid id) => await _catalogoContext.Produtos.AsNoTracking().FirstOrDefaultAsync(p => p.Guid == id);
#pragma warning restore CS8603 // Possível retorno de referência nula.

        public async Task<IEnumerable<Produto>> ObterTodosAsync()
        {
            return await _catalogoContext.Produtos.AsNoTracking().ToListAsync();
        }

        public void Dispose()
        {
        }
    }
}
