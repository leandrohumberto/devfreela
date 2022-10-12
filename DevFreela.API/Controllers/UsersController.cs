using DevFreela.API.Models;
using DevFreela.Application.Commands.CreateUser;
using DevFreela.Application.Queries.GetUserById;
using DevFreela.Application.Queries.UserExists;
using DevFreela.Application.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DevFreela.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        //private readonly IUserService _userService;
        private readonly IMediator _mediator;

        public UsersController(/*IUserService userService, */IMediator mediator)
        {
            //_userService = userService;
            _mediator = mediator;
        }

        // api/users/1 GET
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserDetailViewModel), 200)]
        public async Task<IActionResult> GetById(int id)
        {
            //var user = _userService.GetById(id);
            var userExists = await _mediator.Send(new UserExistsQuery(id));
            if (!userExists) return NotFound();

            var user = await _mediator.Send(new GetUserByIdRequest(id));

            return Ok(user);
        }

        // api/users POST
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateUserCommand command)
        {
            //var id = _userService.Create(command);
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id }, command);
        }

        // api/users/1/login PUT
        [HttpPut("{id}/login")]
        public IActionResult Login(int id, [FromBody] LoginModel login)
        {
            return NoContent();
        }
    }
}
