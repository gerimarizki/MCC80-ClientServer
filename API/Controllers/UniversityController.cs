using API.Contracts;
using API.DTOs.Universities;
using API.Models;
using API.Repositories;
using API.Services;
using API.Utilities;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers
{
    [ApiController]
    [Route("api/univerities")]
    public class UniversityController : ControllerBase
    {
        private readonly UniversityService _service;

        public UniversityController(UniversityService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var entities = _service.GetUniversity();

            if (!entities.Any())
            {
                return NotFound(new HandlerForResponseEntity<GetUniversityDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data not found"
                });
            }

            return Ok(new HandlerForResponseEntity<IEnumerable<GetUniversityDto>>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Data found",
                Data = entities
            });
        }

        [HttpGet("{guid}")]
        public IActionResult GetByGuid(Guid guid)
        {
            var university = _service.GetUniversity(guid);
            if (university is null)
            {
                return NotFound(new HandlerForResponseEntity<GetUniversityDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data not found"
                });
            }

            return Ok(new HandlerForResponseEntity<GetUniversityDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Data found",
                Data = university
            });
        }

        [HttpPost]
        public IActionResult Create(NewUniversityDto newUniversityDto)
        {
            var createdUniversity = _service.CreateUniversity(newUniversityDto);
            if (createdUniversity is null)
            {
                return BadRequest(new HandlerForResponseEntity<GetUniversityDto>
                {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "Data not created"
                });
            }

            return Ok(new HandlerForResponseEntity<GetUniversityDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Successfully created",
                Data = createdUniversity
            });
        }

        [HttpPut]
        public IActionResult Update(UpdateUniversityDto updateUniversityDto)
        {
            var update = _service.UpdateUniversity(updateUniversityDto);
            if (update is -1)
            {
                return NotFound(new HandlerForResponseEntity<UpdateUniversityDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Id not found"
                });
            }
            if (update is 0)
            {
                return BadRequest(new HandlerForResponseEntity<UpdateUniversityDto>
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = HttpStatusCode.InternalServerError.ToString(),
                    Message = "Check your data"
                });
            }
            return Ok(new HandlerForResponseEntity<UpdateUniversityDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Successfully updated"
            });
        }

        [HttpDelete]
        public IActionResult Delete(Guid guid)
        {
            var delete = _service.DeleteUniversity(guid);

            if (delete is -1)
            {
                return NotFound(new HandlerForResponseEntity<GetUniversityDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Id not found"
                });
            }
            if (delete is 0)
            {
                return BadRequest(new HandlerForResponseEntity<GetUniversityDto>
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = HttpStatusCode.InternalServerError.ToString(),
                    Message = "Check connection to database"
                });
            }

            return Ok(new HandlerForResponseEntity<GetUniversityDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Successfully deleted"
            });
        }
    }
}
