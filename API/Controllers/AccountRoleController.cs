﻿using API.Contracts;
using API.DTOs.AccountRoles;
using API.Models;
using API.Services;
using API.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Net;

namespace API.Controllers
{
    [ApiController]
    [Route("api/account-roles")]
    //[Authorize(Roles = "Employee")]
    public class AccountRoleController : ControllerBase
    {
        private readonly AccountRoleService _service;

        public AccountRoleController(AccountRoleService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var entities = _service.GetAccountRole();

            if (entities == null)
            {
                return NotFound(new HandlerForResponseEntity<GetAccountRoleDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data not found"
                });
            }

            return Ok(new HandlerForResponseEntity<IEnumerable<GetAccountRoleDto>>
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
            var accountRole = _service.GetAccountRole(guid);
            if (accountRole is null)
            {
                return NotFound(new HandlerForResponseEntity<GetAccountRoleDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data not found"
                });
            }

            return Ok(new HandlerForResponseEntity<GetAccountRoleDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Data found",
                Data = accountRole
            });
        }

        [HttpPost]
        public IActionResult Create(NewAccountRoleDto newAccountRoleDto)
        {
            var createAccountRole = _service.CreateAccountRole(newAccountRoleDto);
            if (createAccountRole is null)
            {
                return BadRequest(new HandlerForResponseEntity<GetAccountRoleDto>
                {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "Data not created"
                });
            }

            return Ok(new HandlerForResponseEntity<GetAccountRoleDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Successfully created",
                Data = createAccountRole
            });
        }

        [HttpPut]
        public IActionResult Update(UpdateAccountRoleDto updateAccountRoleDto)
        {
            var update = _service.UpdateAccountRole(updateAccountRoleDto);
            if (update is -1)
            {
                return NotFound(new HandlerForResponseEntity<UpdateAccountRoleDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Id not found"
                });
            }
            if (update is 0)
            {
                return BadRequest(new HandlerForResponseEntity<UpdateAccountRoleDto>
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = HttpStatusCode.InternalServerError.ToString(),
                    Message = "Check your data"
                });
            }
            return Ok(new HandlerForResponseEntity<UpdateAccountRoleDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Successfully updated"
            });
        }

        [HttpDelete]
        public IActionResult Delete(Guid guid)
        {
            var delete = _service.DeleteAccountRole(guid);

            if (delete is -1)
            {
                return NotFound(new HandlerForResponseEntity<GetAccountRoleDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Id not found"
                });
            }
            if (delete is 0)
            {
                return BadRequest(new HandlerForResponseEntity<GetAccountRoleDto>
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = HttpStatusCode.InternalServerError.ToString(),
                    Message = "Check connection to database"
                });
            }

            return Ok(new HandlerForResponseEntity<GetAccountRoleDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Successfully deleted"
            });
        }
    }
}
