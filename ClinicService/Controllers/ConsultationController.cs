using AutoMapper;
using ClinicService.Data.Infrastructure.Models;
using ClinicService.Interfaces.Repositories;
using ClinicService.Models.Requests.Consultations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ConsultationController : ControllerBase
    {
        private readonly IConsultationRepository _consultationRepository;
        private readonly ILogger<ConsultationController> _logger;
        private readonly IMapper _mapper;

        public ConsultationController(IConsultationRepository consultationRepository,
            ILogger<ConsultationController> logger,
            IMapper mapper)
        {
            _consultationRepository = consultationRepository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpPost("create")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        public IActionResult Create([FromBody] CreateConsultationRequest createRequest) =>
            Ok(_consultationRepository.Add(_mapper.Map<Consultation>(createRequest)));

        [HttpPut("update")]
        public IActionResult Update([FromBody] UpdateConsultationRequest updateRequest)
        {
            _consultationRepository.Update(_mapper.Map<Consultation>(updateRequest));
            return Ok();
        }

        [HttpDelete("delete")]
        public IActionResult Delete([FromQuery] int consultationId)
        {
            _consultationRepository.Delete(consultationId);
            return Ok();
        }

        [HttpGet("get-all")]
        [ProducesResponseType(typeof(IList<Consultation>), StatusCodes.Status200OK)]
        public IActionResult GetAll() =>
            Ok(_consultationRepository.GetAll());

        [HttpGet("get/{id}")]
        [ProducesResponseType(typeof(Consultation), StatusCodes.Status200OK)]
        public IActionResult GetById([FromRoute] int id) =>
            Ok(_consultationRepository.GetById(id));
    }
}
