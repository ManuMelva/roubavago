using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using QuartoService.Data;
using QuartoService.Dtos;
using QuartoService.Models;
using QuartoService.SyncDataServices.Http;

namespace QuartoService.Controllers
{
    [EnableCors("SiteCorsPolicy")]
    [Produces("application/json")]
    [ApiController]
    [Route("api/[controller]")]
    public class QuartosController: ControllerBase
    {
        private readonly IQuartoRepo _repository;
        private readonly IMapper _mapper;

        public QuartosController(IQuartoRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<QuartoReadDto>> GetAllQuartosForHotel(int hotelId)
        {
            if(!_repository.HotelExists(hotelId))
            {
                return NotFound();
            }

            var quartos = _repository.GetAllQuartosForHotel(hotelId);

            return Ok(_mapper.Map<IEnumerable<QuartoReadDto>>(quartos));
        }

        [HttpGet("{quartoId}", Name = "GetQuartoForHotel")]
        public ActionResult<QuartoReadDto> GetQuartoForHotel(int hotelId, int quartoId)
        {
            if(!_repository.HotelExists(hotelId))
            {
                return NotFound();
            }

            var quarto = _repository.GetQuarto(hotelId, quartoId);

            if(quarto == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<QuartoReadDto>(quarto));
        }

        [HttpPost]
        public ActionResult<QuartoReadDto> CreateQuartoForHotel(int hotelId, QuartoCreateDto quartoDto)
        {
            if(!_repository.HotelExists(hotelId))
            {
                return NotFound();
            }

            var quarto = _mapper.Map<Quarto>(quartoDto);

            _repository.CreateQuarto(hotelId, quarto);
            _repository.SaveChanges();

            var quartoReadDto = _mapper.Map<QuartoReadDto>(quarto);

            return CreatedAtAction(nameof(GetQuartoForHotel),
                new {hotelId = hotelId, quartoId = quartoReadDto.Id}, quartoReadDto);
        }

        [HttpPost("hotel")]
        public ActionResult<QuartoReadDto> ReceiveHotelFromApi(HotelCreateDto hotel)
        {
            var hotelModel = _mapper.Map<Hotel>(hotel);
            _repository.CreateHotel(hotelModel);
            _repository.SaveChanges();

            return Ok();
        }
    }
}