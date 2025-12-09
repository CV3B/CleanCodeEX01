using Inlämningsuppgift_1.Api.Attributes;
using Inlämningsuppgift_1.Api.DTOs.Requests;
using Inlämningsuppgift_1.Api.DTOs.Responses;
using Inlämningsuppgift_1.Api.Extensions;
using Inlämningsuppgift_1.Core.Commands.Users;
using Inlämningsuppgift_1.Data.Constants;
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace Inlämningsuppgift_1.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest req)
        {
            var command = new RegisterUserCommand
            {
                Username = req.Username,
                Password = req.Password,
                Email = req.Email
            };

            var result = await _mediator.Send(command);
            return Ok(result.Value);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest req)
        {
            var command = new LoginUserCommand
            {
                Username = req.Username,
                Password = req.Password
            };

            var result = await _mediator.Send(command);
            return Ok(new { Token = result.Value });
        }

        [HttpGet("profile")]
        [TokenAuth]
        public async Task<IActionResult> Profile([FromHeader(Name = TokenSettings.TokenName)] string token)
        {
            var profile = HttpContext.GetAuthUser();

            return Ok(new UserResponse
            {
                Id = profile.Id,
                Username = profile.Username,
                Email = profile.Email
            });
        }
    }
}
