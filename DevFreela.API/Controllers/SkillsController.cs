using DevFreela.Application.Commands.CreateSkill;
using DevFreela.Application.Commands.DeleteSkill;
using DevFreela.Application.Commands.UpdateSkill;
using DevFreela.Application.Queries.GetAllSkills;
using DevFreela.Application.Queries.GetSkillById;
using DevFreela.Application.Queries.SkillExists;
using DevFreela.Application.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevFreela.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SkillsController : ControllerBase
    {
        private const string applicationJsonMediaType = "application/json";

        //private readonly ISkillService _skillService;
        private readonly IMediator _mediator;

        public SkillsController(/*ISkillService skillService,*/ IMediator mediator)
        {
            //_skillService = skillService;
            _mediator = mediator;
        }

        // api/skills GET
        [HttpGet]
        [Authorize(Roles = "Client, Freelancer")]
        [ProducesResponseType(typeof(IEnumerable<SkillViewModel>), StatusCodes.Status200OK, applicationJsonMediaType)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Get()
        {
            //var skills = _skillService.GetAll();
            var skills = await _mediator.Send(new GetAllSkillsQuery());
            return Ok(skills);
        }

        // api/skills GET
        [HttpGet("{id:int}")]
        [Authorize(Roles = "Client, Freelancer")]
        [ProducesResponseType(typeof(SkillViewModel), StatusCodes.Status200OK, applicationJsonMediaType)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            //var skill = _skillService.GetById(id);
            var skill = await _mediator.Send(new GetSkillByIdQuery(id));

            if (skill == null) return NotFound();
            
            return Ok(skill);
        }

        // api/skills POST
        [HttpPost]
        [Authorize(Roles = "Client, Freelancer")]
        [Consumes(applicationJsonMediaType)]
        [ProducesResponseType(typeof(CreateSkillCommand), StatusCodes.Status201Created, applicationJsonMediaType)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest, applicationJsonMediaType)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Post([FromBody] CreateSkillCommand command)
        {
            //var id = _skillService.Create(command);
            var id = await _mediator.Send(command);

            if (!id.HasValue) return BadRequest();

            return CreatedAtAction(nameof(GetById), new { id }, command);
        }

        // api/skills PUT
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Client, Freelancer")]
        [Consumes(applicationJsonMediaType)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest, applicationJsonMediaType)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateSkillCommand command)
        {
            //var skill = _skillService.GetById(id);
            var skillExists = await _mediator.Send(new SkillExistsQuery(id));
            if (!skillExists) return NotFound();

            //_skillService.Update(id, command);
            command.SetId(id);
            await _mediator.Send(command);
            return NoContent();
        }

        // api/skills DELETE
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Client, Freelancer")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            //var skill = _skillService.GetById(id);
            var skillExists = await _mediator.Send(new SkillExistsQuery(id));
            if (!skillExists) return NotFound();

            //_skillService.Delete(id);
            await _mediator.Send(new DeleteSkillCommand(id));
            return NoContent();
        }
    }
}
