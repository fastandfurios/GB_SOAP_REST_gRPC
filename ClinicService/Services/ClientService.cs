using AutoMapper;
using ClientServiceProtos;
using ClinicService.Data.Infrastructure.Contexts;
using ClinicService.Data.Infrastructure.Models;
using Grpc.Core;
using static ClientServiceProtos.ClientService;

namespace ClinicService.Services
{
    public class ClientService : ClientServiceBase
    {
        #region Serives

        private readonly ClinicServiceDbContext _dbContext;
        private readonly ILogger<ClientService> _logger;
        private readonly IMapper _mapper;

        #endregion

        #region Constructors

        public ClientService(ClinicServiceDbContext dbContext,
            ILogger<ClientService> logger, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _dbContext = dbContext;
        }

        #endregion

        public override Task<CreateClientResponse> CreateClient(CreateClientRequest request, ServerCallContext context)
        {
            var client = _mapper.Map<Client>(request);
            _dbContext.Clients.Add(client);
            _dbContext.SaveChanges();

            var response = new CreateClientResponse
            {
                ClientId = client.ClientId
            };

            return Task.FromResult(response);
        }

        public override Task<GetClientsResponse> GetClients(GetClientsRequest request, ServerCallContext context)
        {
            var response = new GetClientsResponse();
            response.Clients.AddRange(_dbContext.Clients.Select(client => _mapper.Map<ClientResponse>(client))
                .ToList());

            return Task.FromResult(response);
        }
    }
}
