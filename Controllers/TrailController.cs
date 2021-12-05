using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;
using WebApi.Models.Dtos;
using WebApi.Repository.IRepository;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrailController : ControllerBase
    {
        private ITrailRepository _npRepository;
        private IMapper _mapper;//System defined field

        public TrailController(ITrailRepository npRepo, IMapper mapper)
        {
            _npRepository = npRepo;
            _mapper = mapper;
        }

        [HttpGet(Name = "GetTrails")]
        //not mandatory these are for swagger cusomisations
        [ProducesResponseType(200, Type = typeof(List<TrailDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetTrails()
        {
            var objlist = _npRepository.GetTrails();

            var objDto = new List<TrailDto>();

            foreach (var obj in objlist)
            {
                objDto.Add(_mapper.Map<TrailDto>(obj));
            }
            return Ok(objDto);
        }



        [HttpGet("{id:int}", Name = "GetTrail")]
        [ProducesResponseType(200, Type = typeof(TrailDto))]
        [ProducesResponseType(400)]
        [ProducesDefaultResponseType]

        public IActionResult GetTrail(int id)
        {
            var obj = _npRepository.GetTrail(id);

            if (obj == null)
            {
                return NotFound();
            }

            var objDto = _mapper.Map<TrailDto>(obj);
            return Ok(objDto);

        }

        [HttpPost]

        [ProducesResponseType(201, Type = typeof(TrailDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult CreateTrail([FromBody] TrailDto nationalparkdto)
        {
            if (nationalparkdto == null && !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_npRepository.TrailExists(nationalparkdto.Name))
            {
                ModelState.AddModelError("", "Trail park already exists!");
                return StatusCode(404, ModelState);
            }



            bool status = _npRepository.CreateTrail(_mapper.Map<Trail>(nationalparkdto));
            if (!status)
            {
                ModelState.AddModelError("", $"Something went wrong in saving{nationalparkdto.Name}");

                return StatusCode(500, ModelState);
            }
            return CreatedAtRoute("GetTrail", new { nationalparkdto.Name }, nationalparkdto);
        }



        [HttpPatch("{id:int}", Name = "UpdateTrail")]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public IActionResult UpdateTrail([FromRoute] int id, [FromBody] TrailDto nationalparkdto)
        {
            if (nationalparkdto == null && nationalparkdto.Id != id)
            {
                return BadRequest(ModelState);
            }

            bool status = _npRepository.UpdateTrail(_mapper.Map<Trail>(nationalparkdto));
            if (!status)
            {
                ModelState.AddModelError("", $"Something went wrong in saving{nationalparkdto.Name}");

                return StatusCode(500, ModelState);
            }
            return NoContent();
        }


        [HttpDelete("{id:int}", Name = "DeleteTrail")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteTrail(int id)
        {
            if (!_npRepository.TrailExists(id))
            {
                return NotFound();
            }
            var nationalparkobj = _npRepository.GetTrail(id);

            if (!_npRepository.DeleteTrail(nationalparkobj))
            {
                ModelState.AddModelError("", $"Something went wrong in deleting {nationalparkobj.Name}");

                return StatusCode(500, ModelState);
            }

            return NoContent();

        }

    }
}

