using DevFreela.API.Models;
using DevFreela.Application.InputModels;
using DevFreela.Application.Services.Interfaces;
using DevFreela.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DevFreela.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectsController(IProjectService projectService) => _projectService = projectService;

        // api/projects?query=net core GET
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ProjectViewModel>), 200)]
        public IActionResult Get(string query)
        {
            var projects = _projectService.GetAll(query);
            return Ok(projects);
        }

        // api/projects/1 GET
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProjectDetailsViewModel), 200)]
        public IActionResult GetById(int id)
        {
            var project = _projectService.GetById(id);

            if (project == null) return NotFound();
            
            return Ok(project);
        }

        // api/projects POST
        [HttpPost]
        public IActionResult Post([FromBody] NewProjectInputModel inputModel)
        {
            if (inputModel.Title?.Length > 50)
            {
                return BadRequest();
            }

            var id = _projectService.Create(inputModel);

            return CreatedAtAction(nameof(GetById), new { id }, inputModel);
        }

        // api/projects/1 PUT
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UpdateProjectInputModel inputModel)
        {
            if (inputModel.Description?.Length > 200)
            {
                return BadRequest();
            }

            if (_projectService.GetById(id) == null) return NotFound();

            _projectService.Update(id, inputModel);

            return NoContent();
        }

        // api/projects/1 DELETE
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (_projectService.GetById(id) == null) return NotFound();

            _projectService.Delete(id);

            return NoContent();
        }

        // api/projects/1/comments POST
        [HttpPost("{id}/comments")]
        public IActionResult PostComment(int id, [FromBody] CreateCommentInputModel inputModel)
        {
            if (_projectService.GetById(id) == null) return NotFound();

            _projectService.CreateComment(id, inputModel);

            return NoContent();
        }

        // api/projects/1/start PUT
        [HttpPut("{id}/start")]
        public IActionResult Start(int id)
        {
            if (_projectService.GetById(id) == null) return NotFound();

            _projectService.Start(id);

            return NoContent();
        }

        // api/projects/1/finish PUT
        [HttpPut("{id}/finish")]
        public IActionResult Finish(int id)
        {
            if (_projectService.GetById(id) == null) return NotFound();

            _projectService.Finish(id);

            return NoContent();
        }
    }
}
