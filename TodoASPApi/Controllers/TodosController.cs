using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;
using TodoASPApi.Models;
using TodoASPApi.Services;

namespace TodoASPApi.Controllers {
    [Route("api/todos")]
    [ApiController]
    public class TodosController : ControllerBase {
        private readonly ITodosService _todosService;
        private readonly ILogger<TodosController> _logger;

        public TodosController(ITodosService todosService, ILogger<TodosController> logger) {
            _todosService = todosService;
            _logger = logger;
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Get TODOs list",
            Description = "Returns TODO list (limit 20), sorted by date."
        )]
        [Produces(MediaTypeNames.Application.Json)]
        [SwaggerResponse(200, null, typeof(IEnumerable<Todo>))]
        [SwaggerResponse(400, "The offset query is invalid")]
        public async Task<ActionResult<IEnumerable<Todo>>> Get([SwaggerParameter("Pagination offset")] int offset = 0) {
            return Ok(await _todosService.GetMany(offset));
        }

        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Get TODO by Id",
            Description = "Returns TODO if exists"
        )]
        [Produces(MediaTypeNames.Application.Json)]
        [SwaggerResponse(200, null, typeof(Todo))]
        [SwaggerResponse(404, "The TODO with provided Id doesn't exists")]
        public async Task<ActionResult<IEnumerable<Todo>>> GetById(int id) {
            Todo? todo = await _todosService.GetById(id);
            if (todo == null) return NotFound();

            return Ok(todo);
        }

        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = "Update TODO by Id",
            Description = "Update TODO and returnsif exists"
        )]
        [Produces(MediaTypeNames.Application.Json)]
        [SwaggerResponse(200, null, typeof(Todo))]
        [SwaggerResponse(404, "The TODO with provided Id doesn't exists")]
        public async Task<ActionResult<Todo>> Update(int id, UpdateTodoDto dto) {
            Todo? todo = await _todosService.Update(id, dto);
            if (todo == null) return NotFound();

            return Ok(todo);
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = "Delete TODO by Id"
        )]
        [Produces(MediaTypeNames.Application.Json)]
        [SwaggerResponse(204)]
        [SwaggerResponse(404, "The TODO with provided Id doesn't exists")]
        [SwaggerResponse(400, "The Body is invalid")]
        public async Task<IActionResult> Delete(int id) {
            bool isDeleted = await _todosService.Delete(id);
            if (isDeleted) return NotFound();

            return NoContent();
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Create TODO"
        )]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        [SwaggerResponse(201, "Returns created TODO", typeof(Todo))]
        [SwaggerResponse(400, "The Body is invalid")]
        public async Task<ActionResult<Todo>> Create(CreateTodoDto dto) {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            Todo todo = await _todosService.Create(dto);
            return CreatedAtAction(nameof(Get), new { id = todo.Id }, todo);
        }
    }
}
