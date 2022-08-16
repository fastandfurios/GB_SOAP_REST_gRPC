using AutoMapper;
using ClinicService.Data.Infrastructure.Contexts;
using ClinicService.Data.Infrastructure.Models;
using ClinicService.Interfaces.Repositories;

namespace ClinicService.Services
{
    public class ConsultationRepository : IConsultationRepository
    {
        #region Serives

        private readonly ClinicServiceDbContext _dbContext;
        private readonly ILogger<ConsultationRepository> _logger;
        private readonly IMapper _mapper;

        #endregion

        #region Constructors

        public ConsultationRepository(ClinicServiceDbContext dbContext,
            ILogger<ConsultationRepository> logger, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _dbContext = dbContext;
        }

        #endregion

        public int Add(Consultation consultation)
        {
            _dbContext.Consultations.Add(consultation);
            _dbContext.SaveChanges();
            return consultation.ConsultationId;
        }

        public void Delete(Consultation consultation)
        {
            if (consultation is null)
                throw new NullReferenceException();
            Delete(consultation.ConsultationId);
        }

        public void Delete(int id)
        {
            var consultation = GetById(id);
            if (consultation is null)
                throw new KeyNotFoundException();
            _dbContext.Consultations.Remove(consultation);
            _dbContext.SaveChanges();
        }

        public IList<Consultation> GetAll()
        {
            return _dbContext.Consultations.ToList();
        }

        public Consultation? GetById(int id)
        {
            return _dbContext.Consultations.FirstOrDefault(consultation => consultation.ConsultationId == id);
        }

        public void Update(Consultation consultation)
        {
            if (consultation is null)
                throw new NullReferenceException();

            var searchingConsultation = GetById(consultation.ConsultationId);
            if (searchingConsultation is null)
                throw new KeyNotFoundException();

            searchingConsultation = _mapper.Map<Consultation>(searchingConsultation);

            _dbContext.Update(searchingConsultation);
            _dbContext.SaveChanges();
        }
    }
}
