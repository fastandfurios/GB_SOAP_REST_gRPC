using AutoMapper;
using ClientServiceProtos;
using ClinicService.Data.Infrastructure.Models;
using ConsultationServiceProtos;
using PetServiceProtos;
using CreateConsultationRequest = ClinicService.Models.Requests.Consultations.CreateConsultationRequest;
using CreatePetRequest = ClinicService.Models.Requests.Pets.CreatePetRequest;
using UpdateConsultationRequest = ClinicService.Models.Requests.Consultations.UpdateConsultationRequest;
using UpdatePetRequest = ClinicService.Models.Requests.Pets.UpdatePetRequest;

namespace ClinicService.Mapping
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Pet, Pet>();
            CreateMap<Client, Client>();
            CreateMap<Consultation, Consultation>();

            CreateMap<Client, ClientResponse>();
            CreateMap<ClientResponse, Client>();
            CreateMap<Pet, PetResponse>();
            CreateMap<Consultation, ConsultationResponse>();

            CreateMap<CreateClientRequest, Client>();
            CreateMap<UpdateClientRequest, Client>();
            CreateMap<CreatePetRequest, Pet>();
            CreateMap<UpdatePetRequest, Pet>();
            CreateMap<CreateConsultationRequest, Consultation>();
            CreateMap<UpdateConsultationRequest, Consultation>();
        }
    }
}
