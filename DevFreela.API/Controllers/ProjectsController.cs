using DevFreela.Application.Commands.CreateComment;
using DevFreela.Application.Commands.CreateProject;
using DevFreela.Application.Commands.DeleteProject;
using DevFreela.Application.Commands.FinishProject;
using DevFreela.Application.Commands.StartProject;
using DevFreela.Application.Commands.UpdateProject;
using DevFreela.Application.Queries.GetAllProjects;
using DevFreela.Application.Queries.GetProjectById;
using DevFreela.Application.Queries.ProjectExists;
using DevFreela.Application.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevFreela.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProjectsController : ControllerBase
    {
        private const string applicationJsonMediaType = "application/json";

        //private readonly IProjectService _projectService;
        private readonly IMediator _mediator;

        public ProjectsController(/*IProjectService projectService, */IMediator mediator)
        {
            //_projectService = projectService;
            _mediator = mediator;
        }

        // api/projects?query=net core GET
        [HttpGet]
        [Authorize(Roles = "Client, Freelancer")]
        [ProducesResponseType(typeof(IEnumerable<ProjectViewModel>), StatusCodes.Status200OK, applicationJsonMediaType)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Get(string query)
        {
            //var projects = _projectService.GetAll(query);
            var getAllProjectsQuery = new GetAllProjectsQuery(query);
            var projects = await _mediator.Send(getAllProjectsQuery);
            return Ok(projects);
        }

        // api/projects/1 GET
        [HttpGet("{id:int}")]
        [Authorize(Roles = "Client, Freelancer")]
        [ProducesResponseType(typeof(ProjectDetailsViewModel), StatusCodes.Status200OK, applicationJsonMediaType)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            if (!await _mediator.Send(new ProjectExistsQuery(id))) return NotFound();

            //var project = _projectService.GetById(id);
            var getProjectByIdQuery = new GetProjectByIdQuery(id);
            var project = await _mediator.Send(getProjectByIdQuery);
            
            return Ok(project);
        }

        // api/projects POST
        [HttpPost]
        [Authorize(Roles = "Client")]
        [Consumes(applicationJsonMediaType)]
        [ProducesResponseType(typeof(CreateProjectCommand), StatusCodes.Status201Created, applicationJsonMediaType)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest, applicationJsonMediaType)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Post([FromBody] CreateProjectCommand command)
        {
            // var id = _projectService.Create(inputModel);
            var id = await _mediator.Send(command);

            return CreatedAtAction(nameof(GetById), new { id }, command);
        }

        // api/projects/1 PUT
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Client")]
        [Consumes(applicationJsonMediaType)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateProjectCommand command)
        {
            var projectExists = await _mediator.Send(new ProjectExistsQuery(id));
            if (!projectExists) return NotFound();

            //_projectService.Update(id, inputModel);
            command.SetId(id);
            _ = await _mediator.Send(command);

            return NoContent();
        }

        // api/projects/1 DELETE
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Client")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var projectExists = await _mediator.Send(new ProjectExistsQuery(id));
            if (!projectExists) return NotFound();

            //_projectService.Delete(id);
            _ = await _mediator.Send(new DeleteProjectCommand(id));

            return NoContent();
        }

        // api/projects/1/comments POST
        [HttpPost("{id:int}/comments")]
        [Authorize(Roles = "Client, Freelancer")]
        [Consumes(applicationJsonMediaType)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PostComment(int id, [FromBody] CreateCommentCommand command)
        {
            var projectExists = await _mediator.Send(new ProjectExistsQuery(id));
            if (!projectExists) return NotFound();

            command.SetIdProject(id);
            //_projectService.CreateComment(id, inputModel);
            _ = await _mediator.Send(command);

            return NoContent();
        }

        // api/projects/1/start PUT
        [HttpPut("{id:int}/start")]
        [Authorize(Roles = "Client")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Start(int id)
        {
            var projectExists = await _mediator.Send(new ProjectExistsQuery(id));
            if (!projectExists) return NotFound();

            //_projectService.Start(id);
            _ = await _mediator.Send(new StartProjectCommand(id));

            return NoContent();
        }

        // api/projects/1/finish PUT
        [HttpPut("{id:int}/finish")]
        [Authorize(Roles = "Client")]
        [Consumes(applicationJsonMediaType)]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Finish(int id, [FromBody] FinishProjectCommand command)
        {
            command.Id = id;

            var result = await _mediator.Send(command);

            if (!result)
            {
                return BadRequest("O pagamento não pôde ser processado.");
            }

            return Accepted();
        }
    }
}
