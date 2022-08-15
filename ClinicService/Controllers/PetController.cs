using AutoMapper;
using ClinicService.Data.Infrastructure.Models;
using ClinicService.Interfaces.Repositories;
using ClinicService.Models.Requests.Pets;
using Microsoft.AspNetCore.Mvc;

namespace ClinicService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PetController : ControllerBase
    {
        private readonly IPetRepository _petRepository;
        private readonly ILogger<PetController> _logger;
        private readonly IMapper _mapper;

        public PetController(IPetRepository petRepository,
            ILogger<PetController> logger,
            IMapper mapper)
        {
            _petRepository = petRepository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpPost("create")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        public IActionResult Create([FromBody] CreatePetRequest createRequest) =>
            Ok(_petRepository.Add(_mapper.Map<Pet>(createRequest)));

        [HttpPut("update")]
        public IActionResult Update([FromBody] UpdatePetRequest updatePetRequest)
        {
            _petRepository.Update(_mapper.Map<Pet>(updatePetRequest));
            return Ok();
        }

        [HttpDelete]
        public IActionResult Delete([FromQuery] int petId)
        {
            _petRepository.Delete(petId);
            return Ok();
        }

        [HttpGet("get-all")]
        [ProducesResponseType(typeof(IList<Pet>), StatusCodes.Status200OK)]
        public IActionResult GetAll() =>
            Ok(_petRepository.GetAll());

        [HttpGet("get/{id}")]
        [ProducesResponseType(typeof(Pet), StatusCodes.Status200OK)]
        public IActionResult GetById([FromRoute] int id) =>
            Ok(_petRepository.GetById(id));
    }
}
