using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Todos.DTOs;
using Todos.Models;
using Todos.Repositories;

namespace Todos.Controllers;

[ApiController]
[Route("api/todos")]
public class TodoController : ControllerBase
{
    private readonly ILogger<TodoController> _logger;
    private readonly ITodoRepository _todo;

    public TodoController(ILogger<TodoController> logger, ITodoRepository todo)
    {
        _logger = logger;
        _todo = todo;
    }

    [HttpPost]
    [Authorize]

    public async Task<ActionResult<TodoDTO>> CreateTodo([FromBody] TodoCreateDTO Data)
    {

        var id = GetCurrentUserId();
        var toCreateTodo = new Todo
        {
            UserId = Int32.Parse(id),
            Description = Data.Description.Trim(),
            IsCompleted = false,


        };

        var createdTodo = await _todo.Create(toCreateTodo);

        return StatusCode(StatusCodes.Status201Created, createdTodo);
    }

    [HttpGet("mytodos")]
    [Authorize]
    public async Task<ActionResult<List<Todo>>> GetMyTodos()
    {
        var id = GetCurrentUserId();
        var todos = await _todo.GetMyTodos(Int32.Parse(id));

        return Ok(todos);
    }


    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<TodoDTO>> GetUserById([FromRoute] long id)
    {
        var todo = await _todo.GetById(id);

        if (todo is null)
            return NotFound("No user found with given id");

        // var dto = todo;
        // dto.User = await _user.GetAlltodos(user.Id);


        return Ok(todo);
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<ActionResult> Updatetodo([FromRoute] long id, [FromBody] TodoUpdateDTO Data)
    {
        var existing = await _todo.GetById(id);
        var currentUserId = GetCurrentUserId();
        if(existing.UserId != Int32.Parse(currentUserId))
            return Unauthorized("You are not authorized to update this todo");
        if (existing is null)
            return NotFound("No Todo found with given id");

        var toUpdateTodo = existing with
        {
            // Description = Data.Description?.Trim() ?? existing.Description,
            IsCompleted = Data.IsCompleted,
        };

        var didUpdate = await _todo.Update(toUpdateTodo);

        if (!didUpdate)
            return StatusCode(StatusCodes.Status500InternalServerError, "Could not update user");

        return NoContent();
    }


    [HttpDelete("{id}")]
    [Authorize]
    public async Task<ActionResult> DeleteTodo([FromRoute] long id)
    {
        var existing = await _todo.GetById(id);
         var currentUserId = GetCurrentUserId();
        if(existing.UserId != Int32.Parse(currentUserId))
            return Unauthorized("You are not authorized to delete this todo");
        if (existing is null)
            return NotFound("No user found with given user name");

        var didDelete = await _todo.Delete(id);

        return NoContent();
    }

    [HttpGet]
    [Authorize]

    public async Task<ActionResult<List<TodoDTO>>> GetAllTodo()
    {
        var TodoList = await _todo.GetList();

        // Teacher -> TeacherDTO
        // var dtoList = TodoList.Select(x => x.asDto);

        return Ok(TodoList);
    }

    private string GetCurrentUserId()
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        var userClaim = identity.Claims;
        return (userClaim.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);
    }



}
