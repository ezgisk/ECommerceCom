using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ECommerceCom.WepApi.Models;
using ECommerceCom.Business.Operations.User.Dtos;
using ECommerceCom.Business.Operations.User;
using ECommerceCom.WepApi.Jwt;
using Microsoft.AspNetCore.Authorization;
using ECommerceCom.Business.Operations.Order.Dtos;
using ECommerceCom.WepApi.Filters;
using ECommerceCom.Business.Operations.Order;

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
        [HttpPost("register")]
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
        [HttpPost("Login")]
        public IActionResult Login(LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = _userService.LoginUser(new LoginUserDto { Email = request.Email, Password = request.Password });
            if (!result.IsSucceed)
                return BadRequest(result.Message);
            var user = result.Data;

            var configuration = HttpContext.RequestServices.GetService<IConfiguration>();
            var token = JwtHelper.GenerateJwtToken(new JwtDto
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.FirstName,
                LastName = user.LastName,
                Role = user.Role,
                SecretKey = configuration["Jwt:SecretKey"],
                Audience = configuration["Jwt:Audience"],
                Issuer = configuration["Jwt:Issuer"],
                ExpireMinutes = int.Parse(configuration["Jwt:ExpireMinutes"]!)

            });
            return Ok(new LoginResponse
            {
                Message =" Giris basariyla tamamlandi",
                Token = token,
            });

        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var order = await _userService.GetUser(id);
            if (order is null)
                return NotFound();
            else
                return Ok(order);
        }
        [HttpGet("me")]
        [Authorize]
        public IActionResult GetMyUser()
        {
            return Ok();
        }
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetUsers();
            return Ok(users);

        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteUser(id);
            if (!result.IsSucceed)
                return NotFound(result.Message);
            else
                return Ok();

        }
        [HttpPatch("{id}/email")]
        [Authorize(Roles = "Admin")]
        [TimeControllerFilter]
        public async Task<IActionResult> AdjustUserEmail(int id, string changeTo)
        {
            var result = await _userService.AdjustUserEmail(id, changeTo);
            if (!result.IsSucceed)
                return NotFound();
            else
                return Ok();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        [TimeControllerFilter]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDto updateUserRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); 
            }

            var updateUserDto = new UpdateUserDto
            {
                Id = id,
                Email = updateUserRequest.Email,
                FirstName = updateUserRequest.FirstName,
                LastName = updateUserRequest.LastName,
                Role= updateUserRequest.Role 
            };

            var result = await _userService.UpdateUser(updateUserDto);

            if (!result.IsSucceed)
            {
                return NotFound(result.Message);
            }

            return await GetUser(id);
        }

    }
}
