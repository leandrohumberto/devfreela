using DevFreela.Application.Commands.CreateUser;
using DevFreela.Application.Commands.LoginUser;
using DevFreela.Application.Queries.GetUserById;
using DevFreela.Application.Queries.UserExists;
using DevFreela.Application.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevFreela.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private const string applicationJsonMediaType = "application/json";

        //private readonly IUserService _userService;
        private readonly IMediator _mediator;

        public UsersController(/*IUserService userService, */IMediator mediator)
        {
            //_userService = userService;
            _mediator = mediator;
        }

        // api/users/1 GET
        [HttpGet("{id:int}")]
        [Authorize(Roles = "Client, Freelancer")]
        [ProducesResponseType(typeof(IEnumerable<UserDetailViewModel>), StatusCodes.Status200OK, applicationJsonMediaType)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetById(int id)
        {
            //var user = _userService.GetById(id);
            var userExists = await _mediator.Send(new UserExistsQuery(id));
            if (!userExists) return NotFound();

            var user = await _mediator.Send(new GetUserByIdQuery(id));

            return Ok(user);
        }

        // api/users POST
        [HttpPost]
        [AllowAnonymous]
        [Consumes(applicationJsonMediaType)]
        [ProducesResponseType(typeof(CreateUserCommand), StatusCodes.Status201Created, applicationJsonMediaType)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest, applicationJsonMediaType)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Post([FromBody] CreateUserCommand command)
        {
            //var id = _userService.Create(command);
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id }, command);
        }

        // api/users/login PUT
        [HttpPut("login")]
        [AllowAnonymous]
        [Consumes(applicationJsonMediaType)]
        [ProducesResponseType(typeof(LoginUserViewModel), StatusCodes.Status200OK, applicationJsonMediaType)]
        [ProducesResponseType(typeof(LoginUserViewModel), StatusCodes.Status400BadRequest, applicationJsonMediaType)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest, applicationJsonMediaType)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
        {
            var viewModel = await _mediator.Send(command);

            if (viewModel == null)
            {
                return BadRequest();
            }

            return Ok(viewModel);
        }
    }
}
