using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoApp.Data;
using ToDoApp.Models;

namespace ToDoApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoController : ControllerBase
    {
        private readonly ToDoContext _context;

        public ToDoController(ToDoContext context)
        {
            _context = context;
        }

        // GET: api/ToDoItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToDo>>> GetToDoItems()
        {
            try
            {
                var todoItems = await _context.ToDoItems.ToListAsync();
                return Ok(new { Message = "ToDo items retrieved successfully.", Data = todoItems });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving ToDo items: {ex.Message}");
            }
        }

        // GET: api/ToDoItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ToDo>> GetToDoItem(int id)
        {
            try
            {
                var toDoItem = await _context.ToDoItems.FindAsync(id);
                if (toDoItem == null)
                {
                    return NotFound(new { Message = $"ToDo item with Id = {id} not found." });
                }

                return Ok(new { Message = "ToDo item retrieved successfully.", Data = toDoItem });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving ToDo item: {ex.Message}");
            }
        }

        // PUT: api/ToDoItems/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutToDoItem(int id, ToDo toDoItem)
        {
            if (id != toDoItem.Id)
            {
                return BadRequest(new { Message = "Id in the URL and Id in the body do not match." });
            }

            _context.Entry(toDoItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { Message = "ToDo item updated successfully." });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ToDoItemExists(id))
                {
                    return NotFound(new { Message = $"ToDo item with Id = {id} not found." });
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error updating ToDo item.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error updating ToDo item: {ex.Message}");
            }
        }

        // POST: api/ToDoItems
        [HttpPost]
        public async Task<ActionResult<ToDo>> PostToDoItem(ToDo toDoItem)
        {
            if (!Enum.IsDefined(typeof(Category), toDoItem.Category))
            {
                return BadRequest("Invalid category.");
            }

            try
            {
                _context.ToDoItems.Add(toDoItem);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetToDoItem), new { id = toDoItem.Id }, new { Message = "ToDo item created successfully.", Data = toDoItem });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error creating ToDo item: {ex.Message}");
            }
        }

        // DELETE: api/ToDoItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteToDoItem(int id)
        {
            try
            {
                var toDoItem = await _context.ToDoItems.FindAsync(id);
                if (toDoItem == null)
                {
                    return NotFound(new { Message = $"ToDo item with Id = {id} not found." });
                }

                _context.ToDoItems.Remove(toDoItem);
                await _context.SaveChangesAsync();

                return Ok(new { Message = "ToDo item deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting ToDo item: {ex.Message}");
            }
        }

        // GET: api/ToDoItems/Search/{term}
        [HttpGet("Search/{term}")]
        public async Task<ActionResult<IEnumerable<ToDo>>> SearchToDoItems(string term)
        {
            try
            {
                var results = await _context.ToDoItems
                    .Where(t => t.Title.Contains(term) || t.Description.Contains(term))
                    .ToListAsync();

                return Ok(new { Message = "Search results retrieved successfully.", Data = results });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error searching ToDo items: {ex.Message}");
            }
        }

        // GET: api/ToDoItems/Category/{category}
        [HttpGet("Category/{category}")]
        public async Task<ActionResult<IEnumerable<ToDo>>> GetToDoItemsByCategory(string category)
        {
            if (!Enum.TryParse<Category>(category, true, out var categoryEnum))
            {
                return BadRequest($"Invalid category: {category}");
            }

            try
            {
                var todoItems = await _context.ToDoItems
                    .Where(t => t.Category == categoryEnum)
                    .ToListAsync();

                return Ok(new { Message = "ToDo items by category retrieved successfully.", Data = todoItems });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving ToDo items: {ex.Message}");
            }
        }

        private bool ToDoItemExists(int id)
        {
            return _context.ToDoItems.Any(e => e.Id == id);
        }
    }
}
