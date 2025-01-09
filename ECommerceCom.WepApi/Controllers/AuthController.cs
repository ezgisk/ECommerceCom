﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ECommerceCom.WepApi.Models;
using ECommerceCom.Business.Operations.User.Dtos;
using ECommerceCom.Business.Operations.User;

namespace ECommerceCom.WepApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService) 
        {
            _userService = userService;
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var addUserDto = new AddUserDto
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Password = request.Password,
            };
            var result = await _userService.AddUser(addUserDto);
            if (result.IsSucceed)
                return Ok();
            else
                return BadRequest(result.Message);
        }
        
    }
}
