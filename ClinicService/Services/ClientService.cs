using AutoMapper;
using ClientServiceProtos;
using ClinicService.Data.Infrastructure.Contexts;
using ClinicService.Data.Infrastructure.Models;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using static ClientServiceProtos.ClientService;

namespace ClinicService.Services
{
    [Authorize]
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

        public override Task<DeleteClientResponse> DeleteClient(DeleteClientRequest request, ServerCallContext context)
        {
            var client = _dbContext.Clients.FirstOrDefault(client => client.ClientId == request.ClientId);
            if (client is null)
                return Task.FromResult(new DeleteClientResponse
                {
                    ErrCode = 22,
                    ErrMessage = "the user could not be deleted because he was not found"
                });

            _dbContext.Remove(client);
            _dbContext.SaveChanges();

            return Task.FromResult(new DeleteClientResponse());
        }

        public override Task<UpdateClientResponse> UpdateClient(UpdateClientRequest request, ServerCallContext context)
        {
            if (request == null)
                return Task.FromResult(new UpdateClientResponse
                {
                    ErrCode = 1,
                    ErrMessage = "the request was null"
                });

            var searchingClient = _dbContext.Clients.FirstOrDefault(c => c.ClientId == request.ClientId);
            if (searchingClient == null)
                return Task.FromResult(new UpdateClientResponse
                {
                    ErrCode = 21,
                    ErrMessage = "such a user was not found"
                });

            searchingClient = _mapper.Map<Client>(request);
            _dbContext.Update(searchingClient);
            _dbContext.SaveChanges();

            return Task.FromResult(new UpdateClientResponse());
        }

        public override Task<ClientResponse> GetClientById(GetClientByIdRequest request, ServerCallContext context)
        {
            var client = _dbContext.Clients.FirstOrDefault(c => c.ClientId == request.ClientId);
            if (client is null)
                return Task.FromResult(new ClientResponse());

            client.Pets = _dbContext.Pets.Where(p => p.ClientId == request.ClientId).ToList();
            client.Consultations = _dbContext.Consultations.Where(c => c.ClientId == request.ClientId).ToList();
            var response = new ClientResponse
            {
                ClientId = client.ClientId,
                Document = client.Document,
                FirstName = client.FirstName,
                Patronymic = client.Patronymic,
                Surname = client.Surname
            };
            response.Pets.AddRange(client.Pets.Select(pet => _mapper.Map<ClientServiceProtos.Pet>(pet)).ToList());
            response.Consultations.AddRange(client.Consultations.Select(c => _mapper.Map<ClientServiceProtos.Consultation>(c)).ToList());

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