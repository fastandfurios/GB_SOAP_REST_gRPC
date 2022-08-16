using AutoMapper;
using ClinicService.Data.Infrastructure.Contexts;
using ClinicService.Data.Infrastructure.Models;
using ConsultationServiceProtos;
using Grpc.Core;

namespace ClinicService.Services
{
    public class ConsultationService : ConsultationServiceProtos.ConsultationService.ConsultationServiceBase
    {
        private readonly ClinicServiceDbContext _dbContext;
        private readonly ILogger<ConsultationService> _logger;
        private readonly IMapper _mapper;

        public ConsultationService(ClinicServiceDbContext dbContext, ILogger<ConsultationService> logger, IMapper mapper)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;
        }

        public override Task<CreateConsultationResponse> CreateConsultation(CreateConsultationRequest request, ServerCallContext context)
        {
            var consultation = _mapper.Map<Consultation>(request);
            _dbContext.Consultations.Add(consultation);
            _dbContext.SaveChanges();

            var response = new CreateConsultationResponse
            {
                ConsultationId = consultation.ConsultationId
            };

            return Task.FromResult(response);
        }

        public override Task<DeleteConsultationResponse> DeleteConsultation(DeleteConsultationRequest request, ServerCallContext context)
        {
            var consultation = _dbContext.Consultations.FirstOrDefault(c => c.ConsultationId == request.ConsultationId);
            if (consultation is null)
                return Task.FromResult(new DeleteConsultationResponse
                {
                    ErrCode = 42,
                    ErrMessage = "the consultation could not be deleted because it was not found"
                });

            _dbContext.Remove(consultation);
            _dbContext.SaveChanges();

            return Task.FromResult(new DeleteConsultationResponse());
        }

        public override Task<UpdateConsultationResponse> UpdateConsultation(UpdateConsultationRequest request, ServerCallContext context)
        {
            if (request is null)
                return Task.FromResult(new UpdateConsultationResponse
                {
                    ErrCode = 1,
                    ErrMessage = "the request was null"
                });

            var searchingConsultation =
                _dbContext.Consultations.FirstOrDefault(c => c.ConsultationId == request.ConsultationId);
            if (searchingConsultation is null)
                return Task.FromResult(new UpdateConsultationResponse
                {
                    ErrCode = 41,
                    ErrMessage = "the consultation data could not be changed"
                });

            var consultation = _mapper.Map<Consultation>(request);
            _dbContext.Consultations.Update(consultation);
            _dbContext.SaveChanges();

            return Task.FromResult(new UpdateConsultationResponse());
        }

        public override Task<ConsultationResponse> GetConsultationById(GetConsultationByIdRequest request, ServerCallContext context)
        {
            var consultation = _dbContext.Consultations.FirstOrDefault(c => c.ConsultationId == request.ConsultationId);
            var response = _mapper.Map<ConsultationResponse>(consultation);
            return Task.FromResult(response);
        }

        public override Task<GetConsultationsResponse> GetConsultations(GetConsultationsRequest request, ServerCallContext context)
        {
            var response = new GetConsultationsResponse();
            response.Consultations.AddRange(_dbContext.Consultations.Select(c => _mapper.Map<ConsultationResponse>(c))
                .ToList());

            return Task.FromResult(response);
        }
    }
}
