using Application.DTOs;
using AutoMapper;

namespace Application.Services;

// TODO (Grupo DTO/Mapping): Implementar conversão de Produto -> ProdutoReadDto.
// Discutir se mapping manual é suficiente ou se vão demonstrar o uso de AutoMapper (opcional).
// Possível extensão: adicionar campo calculado no DTO.
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Produto, ProdutoCreateDto>().ReverseMap();
        CreateMap<Produto, ProdutoReadDto>().ReverseMap();
    }
    
}