using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.DTOs;
using PlatformService.Models;

namespace PlatformService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepo _repository;
        private readonly IMapper _mapper;

        public PlatformsController(IPlatformRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDTO>> GetPlatforms()
        {
            var platformsItems = _repository.GetAllPlatforms();

            return Ok(_mapper.Map<IEnumerable<PlatformReadDTO>>(platformsItems));
        }

        [HttpGet]
        [Route("{id:int}", Name = "GetPlatformById")]
        public ActionResult<PlatformReadDTO> GetPlatformById(int id)
        {
            var platform = _repository.GetPlatformById(id);

            if (platform != null)
            {
                return Ok(_mapper.Map<PlatformReadDTO>(platform));
            }

            return NotFound($"Platform with {id} not found.");
        }

        [HttpPost]
        public ActionResult<PlatformReadDTO> CreatePlatform([FromBody] PlatformCreateDTO platformCreateDTO)
        {
            if (platformCreateDTO == null) return BadRequest();

            var platform = _mapper.Map<Platform>(platformCreateDTO);

            _repository.CreatePlatform(platform);
            _repository.SaveChanges();

            var platformReadDTO = _mapper.Map<PlatformReadDTO>(platform);

            return CreatedAtRoute(nameof(GetPlatformById), new { id = platform.Id }, platformReadDTO);
        }
    }
}