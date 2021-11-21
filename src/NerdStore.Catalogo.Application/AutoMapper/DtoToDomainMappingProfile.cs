using AutoMapper;
using NerdStore.Catalogo.Application.DTOs;
using NerdStore.Catalogo.Domain.Entitys;

namespace NerdStore.Catalogo.Application.AutoMapper
{
    public class DtoToDomainMappingProfile : Profile
    {
        public DtoToDomainMappingProfile()
        {
            CreateMap<ProdutoDTO, Produto>()
                .ConstructUsing(p => new Produto(p.Nome, p.Descricao, p.Ativo, p.Valor,
                                    p.CategoriaId, p.DataCadastro, p.ImagemUpload, p.ImagemUrl,
                                    new Dimensoes(p.Altura, p.Largura, p.Profundidade))
                );

            CreateMap<CategoriaDTO, Categoria>()
                .ConstructUsing(c => new Categoria(c.Nome, c.Codigo));
        }

    }
}
