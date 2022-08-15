using AutoMapper;
using ClinicService.Data.Infrastructure.Contexts;
using ClinicService.Data.Infrastructure.Models;
using ClinicService.Interfaces.Repositories;

namespace ClinicService.Services
{
    public class ClientRepository : IClientRepository
    {
        #region Serives

        private readonly ClinicServiceDbContext _dbContext;
        private readonly ILogger<ClientRepository> _logger;
        private readonly IMapper _mapper;

        #endregion

        #region Constructors

        public ClientRepository(ClinicServiceDbContext dbContext,
            ILogger<ClientRepository> logger, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _dbContext = dbContext;
        }

        #endregion

        public int Add(Client client)
        {
            _dbContext.Clients.Add(client);
            _dbContext.SaveChanges();
            return client.ClientId;
        }

        public void Delete(Client client)
        {
            if (client == null)
                throw new NullReferenceException();
            Delete(client.ClientId);
        }

        public void Delete(int id)
        {
            var client = GetById(id);
            if (client == null)
                throw new KeyNotFoundException();
            _dbContext.Remove(client);
            _dbContext.SaveChanges();
        }

        public IList<Client> GetAll()
        {
            return _dbContext.Clients.ToList();
        }

        public Client? GetById(int id)
        {
            return _dbContext.Clients.FirstOrDefault(client => client.ClientId == id);
        }

        public void Update(Client client)
        {
            if (client == null)
                throw new NullReferenceException();

            var searchingClient = GetById(client.ClientId);
            if (searchingClient == null)
                throw new KeyNotFoundException();

            //searchingClient.ClientId = client.ClientId;
            //searchingClient.Document = client.Document;
            //searchingClient.Surname = client.Surname;
            //searchingClient.FirstName = client.FirstName;
            //searchingClient.Patronymic = client.Patronymic;
            _mapper.Map<Client>(searchingClient);

            _dbContext.Update(client);
            _dbContext.SaveChanges();
        }
    }
}
