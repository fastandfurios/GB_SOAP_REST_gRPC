using AutoMapper;
using ClientServiceProtos;
using ClinicService.Data.Infrastructure.Models;
using ConsultationServiceProtos;
using Google.Protobuf.WellKnownTypes;
using PetServiceProtos;
using Consultation = ClinicService.Data.Infrastructure.Models.Consultation;
using Pet = ClinicService.Data.Infrastructure.Models.Pet;

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
            CreateMap<Pet, ClientServiceProtos.Pet>()
                .ForMember(dest => dest.Birthday,
                    act => act.MapFrom(src => Timestamp.FromDateTime(src.Birthday.ToUniversalTime())));
            CreateMap<Pet, PetResponse>()
                .ForMember(dest => dest.Birthday,
                    act => act.MapFrom(src => Timestamp.FromDateTime(src.Birthday.ToUniversalTime())));
            CreateMap<CreatePetRequest, Pet>()
                .ForMember(dest => dest.Birthday,
                    act => act.MapFrom(src => src.Birthday.ToDateTime()));
            CreateMap<UpdatePetRequest, Pet>()
                .ForMember(dest => dest.Birthday,
                    act => act.MapFrom(src => src.Birthday.ToDateTime()));

            CreateMap<Consultation, ClientServiceProtos.Consultation>()
                .ForMember(dest => dest.ConsultationDate,
                    act => act.MapFrom(src => Timestamp.FromDateTime(src.ConsultationDate.ToUniversalTime())));
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
