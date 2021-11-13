using AutoMapper;
using NerdStore.Catalogo.Application.DTOs;
using NerdStore.Catalogo.Domain.Entitys;

namespace NerdStore.Catalogo.Application.AutoMapper
{
    public class DomainToDTOMappingProfile : Profile
    {
        public DomainToDTOMappingProfile()
        {
            CreateMap<Produto, ProdutoDTO>()
                .ForMember(d => d.Altura, o => o.MapFrom(e => e.Dimensoes.Altura))
                .ForMember(d => d.Largura, o => o.MapFrom(e => e.Dimensoes.Largura))
                .ForMember(d => d.Profundidade, o => o.MapFrom(e => e.Dimensoes.Profundidade));
            CreateMap<Categoria, CategoriaDTO>();
        }
    }
}
