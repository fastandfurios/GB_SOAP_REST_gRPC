using AutoMapper;
using ClinicService.Data.Infrastructure.Contexts;
using ClinicService.Data.Infrastructure.Models;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using PetServiceProtos;

namespace ClinicService.Services
{
    [Authorize]
    public class PetService : PetServiceProtos.PetService.PetServiceBase
    {
        #region Serives

        private readonly ClinicServiceDbContext _dbContext;
        private readonly ILogger<PetService> _logger;
        private readonly IMapper _mapper;

        #endregion

        #region Constructors

        public PetService(ClinicServiceDbContext dbContext,
            ILogger<PetService> logger,
            IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _dbContext = dbContext;
        }

        #endregion

        public override Task<CreatePetResponse> CreatePet(CreatePetRequest request, ServerCallContext context)
        {
            var pet = _mapper.Map<Pet>(request);
            _dbContext.Pets.Add(pet);
            _dbContext.SaveChanges();

            var response = new CreatePetResponse
            {
                PetId = pet.PetId
            };

            return Task.FromResult(response);
        }

        public override Task<DeletePetResponse> DeletePet(DeletePetRequest request, ServerCallContext context)
        {
            var pet = _dbContext.Pets.FirstOrDefault(pet => pet.PetId == request.PetId);
            if (pet is null)
                return Task.FromResult(new DeletePetResponse
                {
                    ErrCode = 32,
                    ErrMessage = "failed to delete the animal"
                });

            _dbContext.Remove(pet);
            _dbContext.SaveChanges();

            return Task.FromResult(new DeletePetResponse());
        }

        public override Task<UpdatePetResponse> UpdatePet(UpdatePetRequest request, ServerCallContext context)
        {
            if (request is null)
                return Task.FromResult(new UpdatePetResponse
                {
                    ErrCode = 1,
                    ErrMessage = "the request was null"
                });

            var searchingPet = _dbContext.Pets.FirstOrDefault(p => p.PetId == request.PetId);
            if (searchingPet is null)
                return Task.FromResult(new UpdatePetResponse
                {
                    ErrCode = 31,
                    ErrMessage = "failed to update entity"
                });

            searchingPet = _mapper.Map<Pet>(request);
            _dbContext.Update(searchingPet);
            _dbContext.SaveChanges();

            return Task.FromResult(new UpdatePetResponse());
        }

        public override Task<PetResponse> GetPetById(GetPetByIdRequest request, ServerCallContext context)
        {
            var pet = _dbContext.Pets.FirstOrDefault(p => p.PetId == request.PetId);
            var response = _mapper.Map<PetResponse>(pet);
            return Task.FromResult(response);
        }

        public override Task<GetPetsResponse> GetPets(GetPetsRequest request, ServerCallContext context)
        {
            var response = new GetPetsResponse();
            response.Pets.AddRange(_dbContext.Pets.Select(pet => _mapper.Map<PetResponse>(pet))
                .ToList());

            return Task.FromResult(response);
        }
    }
}