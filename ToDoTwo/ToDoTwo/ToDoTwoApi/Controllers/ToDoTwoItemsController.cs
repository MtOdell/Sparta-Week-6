using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoTwoApi.Models;

namespace ToDoTwoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoTwoItemsController : ControllerBase
    {
        private readonly ToDoTwoContext _context;

        public ToDoTwoItemsController(ToDoTwoContext context)
        {
            _context = context;
        }

        // GET: api/ToDoTwoItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToDoTwoItem>>> GetTodoTwoItems()
        {
            return await _context.TodoTwoItems.ToListAsync();
        }

        // GET: api/ToDoTwoItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ToDoTwoItem>> GetToDoTwoItem(long id)
        {
            var toDoTwoItem = await _context.TodoTwoItems.FindAsync(id);

            if (toDoTwoItem == null)
            {
                return NotFound();
            }

            return toDoTwoItem;
        }

        // PUT: api/ToDoTwoItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutToDoTwoItem(long id, ToDoTwoItem toDoTwoItem)
        {
            if (id != toDoTwoItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(toDoTwoItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ToDoTwoItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ToDoTwoItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ToDoTwoItem>> PostToDoTwoItem(ToDoTwoItem toDoTwoItem)
        {
            _context.TodoTwoItems.Add(toDoTwoItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetToDoTwoItem", new { id = toDoTwoItem.Id }, toDoTwoItem);
            return CreatedAtAction(nameof(GetToDoTwoItem), new { id = toDoTwoItem.Id }, toDoTwoItem);
        }

        // DELETE: api/ToDoTwoItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteToDoTwoItem(long id)
        {
            var toDoTwoItem = await _context.TodoTwoItems.FindAsync(id);
            if (toDoTwoItem == null)
            {
                return NotFound();
            }

            _context.TodoTwoItems.Remove(toDoTwoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ToDoTwoItemExists(long id)
        {
            return _context.TodoTwoItems.Any(e => e.Id == id);
        }
    }
}
