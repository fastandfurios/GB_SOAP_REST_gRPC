using AutoMapper;
using ClinicService.Data.Infrastructure.Contexts;
using ClinicService.Data.Infrastructure.Models;
using ClinicService.Interfaces.Repositories;

namespace ClinicService.Services
{
    public class PetRepository : IPetRepository
    {
        #region Serives

        private readonly ClinicServiceDbContext _dbContext;
        private readonly ILogger<PetRepository> _logger;
        private readonly IMapper _mapper;

        #endregion

        #region Constructors

        public PetRepository(ClinicServiceDbContext dbContext,
            ILogger<PetRepository> logger, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _dbContext = dbContext;
        }

        #endregion

        public int Add(Pet pet)
        {
            _dbContext.Pets.Add(pet);
            _dbContext.SaveChanges();
            return pet.PetId;
        }

        public void Delete(Pet pet)
        {
            if (pet == null)
                throw new NullReferenceException();
            Delete(pet.PetId);
        }

        public void Delete(int id)
        {
            var pet = GetById(id);
            if (pet == null)
                throw new KeyNotFoundException();
            _dbContext.Remove(pet);
            _dbContext.SaveChanges();
        }

        public IList<Pet> GetAll()
        {
            return _dbContext.Pets.ToList();
        }

        public Pet? GetById(int id)
        {
            return _dbContext.Pets.FirstOrDefault(pet => pet.PetId == id);
        }

        public void Update(Pet pet)
        {
            if (pet == null)
                throw new NullReferenceException();

            var searchingPet = GetById(pet.ClientId);
            if (searchingPet == null)
                throw new KeyNotFoundException();

            _mapper.Map<Pet>(searchingPet);

            _dbContext.Update(searchingPet);
            _dbContext.SaveChanges();
        }
    }
}
