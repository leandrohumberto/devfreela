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
        [Authorize(Roles = "client, freelancer")]
        [ProducesResponseType(typeof(IEnumerable<ProjectViewModel>), StatusCodes.Status200OK, applicationJsonMediaType)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Get(string query)
        {
            throw new Exception("Caralho");
            //var projects = _projectService.GetAll(query);
            var getAllProjectsQuery = new GetAllProjectsQuery(query);
            var projects = await _mediator.Send(getAllProjectsQuery);
            return Ok(projects);
        }

        // api/projects/1 GET
        [HttpGet("{id}")]
        [Authorize(Roles = "client, freelancer")]
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
        [Authorize(Roles = "client")]
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
        [HttpPut("{id}")]
        [Authorize(Roles = "client")]
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
        [HttpDelete("{id}")]
        [Authorize(Roles = "client")]
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
        [HttpPost("{id}/comments")]
        [Authorize(Roles = "client, freelancer")]
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
        [HttpPut("{id}/start")]
        [Authorize(Roles = "client")]
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
        [HttpPut("{id}/finish")]
        [Authorize(Roles = "client")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Finish(int id)
        {
            var projectExists = await _mediator.Send(new ProjectExistsQuery(id));
            if (!projectExists) return NotFound();

            // _projectService.Finish(id);
            _ = await _mediator.Send(new FinishProjectCommand(id));

            return NoContent();
        }
    }
}
