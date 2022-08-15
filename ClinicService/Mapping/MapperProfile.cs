using AutoMapper;
using ClientServiceProtos;
using ClinicService.Data.Infrastructure.Models;

namespace ClinicService.Mapping
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Pet, Pet>();
            CreateMap<Client, Client>();
            CreateMap<CreateClientRequest, Client>();
            CreateMap<Client, ClientResponse>();
        }
    }
}
