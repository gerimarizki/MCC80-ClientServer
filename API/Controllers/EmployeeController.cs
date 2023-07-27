using API.Contracts;
using API.DTOs.Employees;
using API.Models;
using API.Services;
using API.Utilities;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers
{
    [ApiController]
    [Route("api/employees")]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeService _service;

        public EmployeeController(EmployeeService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var entities = _service.GetEmployee();

            if (entities == null)
            {
                return NotFound(new HandlerForResponseEntity<GetEmployeeDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data not found"
                });
            }

            return Ok(new HandlerForResponseEntity<IEnumerable<GetEmployeeDto>>
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
            var employee = _service.GetEmployee(guid);
            if (employee is null)
            {
                return NotFound(new HandlerForResponseEntity<GetEmployeeDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data not found"
                });
            }

            return Ok(new HandlerForResponseEntity<GetEmployeeDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Data found",
                Data = employee
            });
        }

        [HttpPost]
        public IActionResult Create(NewEmployeeDto newEmployeeDto)
        {
            var createEmployee = _service.CreateEmployee(newEmployeeDto);
            if (createEmployee is null)
            {
                return BadRequest(new HandlerForResponseEntity<GetEmployeeDto>
                {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "Data not created"
                });
            }

            return Ok(new HandlerForResponseEntity<GetEmployeeDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Successfully created",
                Data = createEmployee
            });
        }

        [HttpPut]
        public IActionResult Update(UpdateEmployeeDto updateEmployeeDto)
        {
            var update = _service.UpdateEmployee(updateEmployeeDto);
            if (update is -1)
            {
                return NotFound(new HandlerForResponseEntity<UpdateEmployeeDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Id not found"
                });
            }
            if (update is 0)
            {
                return BadRequest(new HandlerForResponseEntity<UpdateEmployeeDto>
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = HttpStatusCode.InternalServerError.ToString(),
                    Message = "Check your data"
                });
            }
            return Ok(new HandlerForResponseEntity<UpdateEmployeeDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Successfully updated"
            });
        }

        [HttpDelete]
        public IActionResult Delete(Guid guid)
        {
            var delete = _service.DeleteEmployee(guid);

            if (delete is -1)
            {
                return NotFound(new HandlerForResponseEntity<GetEmployeeDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Id not found"
                });
            }
            if (delete is 0)
            {
                return BadRequest(new HandlerForResponseEntity<GetEmployeeDto>
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = HttpStatusCode.InternalServerError.ToString(),
                    Message = "Check connection to database"
                });
            }

            return Ok(new HandlerForResponseEntity<GetEmployeeDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Successfully deleted"
            });

        }

        [HttpGet("employees-detail")]
        public IActionResult GetAllEmployeeDetail()
        {
            var result = _service.GetAllEmployeeDetail();
            if (!result.Any())
            {
                return NotFound(new HandlerForResponseEntity<EmployeeDetailDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data not found"
                });
            }

            return Ok(new HandlerForResponseEntity<IEnumerable<EmployeeDetailDto>>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Success Retrieve Data",
                Data = result
            });
        
        }

        [HttpGet("employees-detail/guid")]
        public IActionResult GetAllEmployeeDetailByGuid(Guid guid)
        {
            var result = _service.GetAllEmployeeDetailByGuid(guid);
            if (result is null)
            {
                return NotFound(new HandlerForResponseEntity<EmployeeDetailDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data not found"
                });
            }

            return Ok(new HandlerForResponseEntity<EmployeeDetailDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Success Retrieve Data",
                Data = result
            });

        }
    }
}
