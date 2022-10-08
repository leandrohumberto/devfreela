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
    public class SkillsController : ControllerBase
    {
        private readonly ISkillService _skillService;

        public SkillsController(ISkillService skillService)
        {
            _skillService = skillService;
        }

        // api/skills GET
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SkillViewModel>), 200)]
        public IActionResult Get()
        {
            var skills = _skillService.GetAll();
            return Ok(skills);
        }

        // api/skills GET
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SkillViewModel), 200)]
        public IActionResult GetById(int id)
        {
            var skill = _skillService.GetById(id);

            if (skill == null) return NotFound();
            
            return Ok(skill);
        }

        // api/skills POST
        [HttpPost]
        public IActionResult Post([FromBody] CreateSkillInputModel inputModel)
        {
            if (string.IsNullOrWhiteSpace(inputModel.Description)) return BadRequest();

            var id = _skillService.Create(inputModel);

            if (!id.HasValue) return BadRequest();

            return CreatedAtAction(nameof(GetById), new { id }, inputModel);
        }

        // api/skills PUT
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UpdateSkillInputModel inputModel)
        {
            if (string.IsNullOrWhiteSpace(inputModel.Description)) return BadRequest();

            var skill = _skillService.GetById(id);

            if (skill == null) return NotFound();

            _skillService.Update(id, inputModel);
            return NoContent();
        }

        // api/skills DELETE
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var skill = _skillService.GetById(id);

            if (skill == null) return NotFound();

            _skillService.Delete(id);
            return NoContent();
        }
    }
}
