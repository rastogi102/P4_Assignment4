using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private static List<TodoItem> _todoItems = new List<TodoItem>
        {
            new TodoItem { Id = 1, Title = "Task 1", IsComplete = false },
            new TodoItem { Id = 2, Title = "Task 2", IsComplete = true },
            new TodoItem { Id = 3, Title = "Task 3", IsComplete = false }
        };

        [HttpGet]
        public ActionResult<IEnumerable<TodoItem>> Get()
        {
            return Ok(_todoItems);
        }

        [HttpGet("{id}")]
        public ActionResult<TodoItem> Get(int id)
        {
            var todoItem = _todoItems.Find(item => item.Id == id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return Ok(todoItem);
        }

        [HttpPost]
        public ActionResult<TodoItem> Post([FromBody] TodoItem todoItem)
        {
            if (todoItem == null)
            {
                return BadRequest();
            }

            todoItem.Id = GenerateId();
            _todoItems.Add(todoItem);

            return CreatedAtAction(nameof(Get), new { id = todoItem.Id }, todoItem);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] TodoItem updatedTodoItem)
        {
            var existingTodoItem = _todoItems.Find(item => item.Id == id);

            if (existingTodoItem == null)
            {
                return NotFound();
            }

            existingTodoItem.Title = updatedTodoItem.Title;
            existingTodoItem.IsComplete = updatedTodoItem.IsComplete;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var todoItem = _todoItems.Find(item => item.Id == id);

            if (todoItem == null)
            {
                return NotFound();
            }

            _todoItems.Remove(todoItem);

            return NoContent();
        }

        private int GenerateId()
        {
            // Generate a new unique ID for a todo item (for simplicity, just increment the last ID)
            int lastId = _todoItems.Count > 0 ? _todoItems[_todoItems.Count - 1].Id : 0;
            return lastId + 1;
        }
    }

    public class TodoItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsComplete { get; set; }
    }
}
