using AutoMapper;
using ClinicService.Data.Infrastructure.Models;
using ClinicService.Interfaces.Repositories;
using ClinicService.Models.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ClientController : ControllerBase
    {
        #region Serives

        private readonly IClientRepository _clientRepository;
        private readonly ILogger<ClientController> _logger;
        private readonly IMapper _mapper;

        #endregion

        #region Constructors

        public ClientController(IClientRepository clientRepository,
            ILogger<ClientController> logger,
            IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _clientRepository = clientRepository;
        }

        #endregion

        #region Public Methods

        [HttpPost("create")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        public IActionResult Create([FromBody] CreateClientRequest createRequest) =>
            Ok(_clientRepository.Add(_mapper.Map<Client>(createRequest)));

        [HttpPut("update")]
        public IActionResult Update([FromBody] UpdateClientRequest updateRequest)
        {
            _clientRepository.Update(_mapper.Map<Client>(updateRequest));
            return Ok();
        }

        [HttpDelete("delete")]
        public IActionResult Delete([FromQuery] int clientId)
        {
            _clientRepository.Delete(clientId);
            return Ok();
        }

        [HttpGet("get-all")]
        [ProducesResponseType(typeof(IList<Client>), StatusCodes.Status200OK)]
        public IActionResult GetAll() =>
            Ok(_clientRepository.GetAll());

        [HttpGet("get/{id}")]
        [ProducesResponseType(typeof(Client), StatusCodes.Status200OK)]
        public IActionResult GetById([FromRoute] int id) =>
            Ok(_clientRepository.GetById(id));

        #endregion
    }
}