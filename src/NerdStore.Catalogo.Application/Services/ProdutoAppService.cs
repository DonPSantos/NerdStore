using AutoMapper;
using NerdStore.Catalogo.Application.DTOs;
using NerdStore.Catalogo.Application.Services.Interfaces;
using NerdStore.Catalogo.Domain.Entitys;
using NerdStore.Catalogo.Domain.Interfaces;
using NerdStore.Core.DomainObjects;

namespace NerdStore.Catalogo.Application.Services
{
    public class ProdutoAppService : IProdutoAppService
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IMapper _mapper;
        private readonly IEstoqueServices _estoqueServices;

        public ProdutoAppService(IProdutoRepository produtoRepository,
                                IMapper mapper,
                                IEstoqueServices estoqueServices)
        {
            _produtoRepository = produtoRepository;
            _mapper = mapper;
            _estoqueServices = estoqueServices;
        }

        #region Gets
        public async Task<IEnumerable<CategoriaDTO>> ObterCategorias()
        {
            return _mapper.Map<IEnumerable<CategoriaDTO>>(await _produtoRepository.ObterCategoriasAsync());
        }

        public async Task<IEnumerable<ProdutoDTO>> ObterPorCategoria(int codigo)
        {
            return _mapper.Map<IEnumerable<ProdutoDTO>>(await _produtoRepository.ObterPorCategoriaAsync(codigo));
        }

        public async Task<ProdutoDTO> ObterPorId(Guid produtoId)
        {
            return _mapper.Map<ProdutoDTO>(await _produtoRepository.ObterPorIdAsync(produtoId));
        }

        public async Task<IEnumerable<ProdutoDTO>> ObterTodos()
        {
            return _mapper.Map<IEnumerable<ProdutoDTO>>(await _produtoRepository.ObterTodosAsync());
        }
        #endregion

        #region Put and Post
        public async Task AdicionarProduto(ProdutoDTO produto)
        {
            var produtoAux = _mapper.Map<Produto>(produto);
            _produtoRepository.Adicionar(produtoAux);

            await _produtoRepository.UnitOfWork.Commit();
        }

        public async Task AlterarProduto(ProdutoDTO produto)
        {
            var produtoAux = _mapper.Map<Produto>(produto);
            _produtoRepository.Atualizar(produtoAux);

            await _produtoRepository.UnitOfWork.Commit();
        }

        public async Task<ProdutoDTO> DebitarEstoque(Guid produtoId, int quantidade)
        {
            if (!_estoqueServices.DebitarEstoque(produtoId, quantidade).Result)
            {
                throw new DomainException("Falha ao debitar estoque.");
            }

            return _mapper.Map<ProdutoDTO>(await _produtoRepository.ObterPorIdAsync(produtoId));
        }


        public async Task<ProdutoDTO> ReporEstoque(Guid produtoId, int quantidade)
        {
            if (!_estoqueServices.ReporEstoque(produtoId, quantidade).Result)
            {
                throw new DomainException("Falha ao repor estoque.");
            }

            return _mapper.Map<ProdutoDTO>(await _produtoRepository.ObterPorIdAsync(produtoId));
        }
        #endregion

        public void Dispose()
        {
            _produtoRepository.Dispose();
            _estoqueServices.Dispose();
        }

    }
}
