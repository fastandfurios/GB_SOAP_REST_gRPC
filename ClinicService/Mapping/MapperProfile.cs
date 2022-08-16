using AutoMapper;
using ClientServiceProtos;
using ClinicService.Data.Infrastructure.Models;
using ConsultationServiceProtos;
using Google.Protobuf.WellKnownTypes;
using PetServiceProtos;

namespace ClinicService.Mapping
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Client, Client>();
            CreateMap<Client, ClientResponse>();
            CreateMap<CreateClientRequest, Client>();
            CreateMap<UpdateClientRequest, Client>();

            CreateMap<Pet, Pet>();
            CreateMap<Pet, PetResponse>()
                .ForMember(dest => dest.Birthday,
                    act => act.MapFrom(src => Timestamp.FromDateTime(src.Birthday.ToUniversalTime())));
            CreateMap<CreatePetRequest, Pet>()
                .ForMember(dest => dest.Birthday,
                    act => act.MapFrom(src => src.Birthday.ToDateTime()));
            CreateMap<UpdatePetRequest, Pet>()
                .ForMember(dest => dest.Birthday,
                    act => act.MapFrom(src => src.Birthday.ToDateTime()));

            CreateMap<Consultation, Consultation>();
            CreateMap<Consultation, ConsultationResponse>()
                .ForMember(dest => dest.ConsultationDate,
                    act => act.MapFrom(src => Timestamp.FromDateTime(src.ConsultationDate.ToUniversalTime())));
            CreateMap<CreateConsultationRequest, Consultation>()
                .ForMember(dest => dest.ConsultationDate,
                    act => act.MapFrom(src => src.ConsultationDate.ToDateTime()));
            CreateMap<UpdateConsultationRequest, Consultation>()
                .ForMember(dest => dest.ConsultationDate,
                    act => act.MapFrom(src => src.ConsultationDate.ToDateTime()));
        }
    }
}
