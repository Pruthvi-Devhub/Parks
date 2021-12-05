using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Repository.IRepository;
using AutoMapper;
using WebApi.Models.Dtos;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NationalParkController : ControllerBase
    {
        private INationalParkRepository _npRepository;
        private IMapper _mapper;//System defined field

        public NationalParkController(INationalParkRepository npRepo,IMapper mapper)
        {
            _npRepository = npRepo;
            _mapper = mapper;
        }

        [HttpGet(Name = "GetNationalParks")]
        //not mandatory these are for swagger cusomisations
        [ProducesResponseType(200,Type= typeof(List<NationalParkDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetNationalParks() 
        {
            var objlist = _npRepository.GetNationalParks();

            var objDto = new List<NationalParkDto>();

            foreach(var obj in objlist)
            {
                objDto.Add(_mapper.Map<NationalParkDto>(obj));
            }
            return Ok(objDto) ;
        }



        [HttpGet("{id:int}",Name = "GetNationalPark")]
        [ProducesResponseType(200, Type = typeof(NationalParkDto))]
        [ProducesResponseType(400)]
        [ProducesDefaultResponseType]

        public IActionResult GetNationalPark(int id)
        {
            var obj = _npRepository.GetNationalPark(id);

            if(obj == null)
            {
                return NotFound();
            }

            var objDto = _mapper.Map<NationalParkDto>(obj);
            return Ok(objDto);

        }

        [HttpPost]

        [ProducesResponseType(201, Type = typeof(NationalParkDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult CreateNationalPark([FromBody] NationalParkDto nationalparkdto)
        {
            if(nationalparkdto == null && !ModelState.IsValid )
            {
                return BadRequest(ModelState);
            }

            if (_npRepository.NationalParkExists(nationalparkdto.Name))
            {
                ModelState.AddModelError("", "NationalPark park already exists!");
                return StatusCode(404, ModelState);
            }

           

            bool status = _npRepository.CreateNationalPark(_mapper.Map<NationalPark>(nationalparkdto));
            if(!status)
            {
                ModelState.AddModelError("", $"Something went wrong in saving{nationalparkdto.Name}");

                return StatusCode(500, ModelState); 
            }
            return CreatedAtRoute("GetNationalPark", new { nationalparkdto.Name }, nationalparkdto);
        }



        [HttpPatch("{id:int}",Name = "UpdateNationalPark")]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
       
        public IActionResult UpdateNationalPark([FromRoute]int id,[FromBody] NationalParkDto nationalparkdto)
        {
            if (nationalparkdto == null && nationalparkdto.Id != id)
            {
                return BadRequest(ModelState);
            }

            bool status = _npRepository.UpdateNationalPark(_mapper.Map<NationalPark>(nationalparkdto));
            if (!status)
            {
                ModelState.AddModelError("", $"Something went wrong in saving{nationalparkdto.Name}");

                return StatusCode(500, ModelState);
            }
            return NoContent();
        }


        [HttpDelete("{id:int}", Name ="DeleteNationalPark")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteNationalPark(int id)
        {
            if(!_npRepository.NationalParkExists(id))
            {
                return NotFound();
            }
            var nationalparkobj = _npRepository.GetNationalPark(id);

            if(!_npRepository.DeleteNationalPark(nationalparkobj))
            {
                ModelState.AddModelError("", $"Something went wrong in deleting {nationalparkobj.Name}");

                return StatusCode(500, ModelState);
            }

            return NoContent();
           
        }

    }
}
