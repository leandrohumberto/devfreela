using DevFreela.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DevFreela.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly OpeningTimeOption _option;

        public ProjectsController(IOptions<OpeningTimeOption> option, ExampleClass exampleClass)
        {
            exampleClass.Name = $"Updated at {nameof(ProjectsController)}";
            _option = option.Value;
        }

        // api/projects?query=net core GET
        [HttpGet]
        public IActionResult Get(string query)
        {
            return Ok();
        }

        // api/projects/1 GET
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            // return NotFound();
            return Ok();
        }

        // api/projects POST
        [HttpPost]
        public IActionResult Post([FromBody] CreateProjectModel createProjectModel)
        {
            if (createProjectModel.Title?.Length > 50)
            {
                return BadRequest();
            }

            // Cadastro o projeto

            return CreatedAtAction(nameof(GetById), new { id = createProjectModel.Id }, createProjectModel);
        }

        // api/projects/1 PUT
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UpdateProjectModel updateProjectModel)
        {
            if (updateProjectModel.Description?.Length > 200)
            {
                return BadRequest();
            }

            // Atualizo o projeto

            return NoContent();
        }

        // api/projects/1 DELETE
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            // Buscar; se não existir, retorna NotFound

            // Remover

            return NoContent();
        }

        // api/projects/1/comments POST
        [HttpPost("{id}/comments")]
        public IActionResult PostComment(int id, [FromBody] CreateCommentModel createComment)
        {
            // Buscar; se não existir, retorna NotFound

            // Adicionar comentário

            return NoContent();
        }

        // api/projects/1/start PUT
        [HttpPut("{id}/start")]
        public IActionResult Start(int id)
        {
            // Buscar; se não existir, retorna NotFound

            // Iniciar projeto

            return NoContent();
        }

        // api/projects/1/finish PUT
        [HttpPut("{id}/finish")]
        public IActionResult Finish(int id)
        {
            // Buscar; se não existir, retorna NotFound

            // Finalizar projeto

            return NoContent();
        }
    }
}
