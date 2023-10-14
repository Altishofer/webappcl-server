using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotenv.net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToX.Models;
using dotenv.net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing.Constraints;
using Word2vec.Tools;

namespace ToX.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private readonly IConfiguration _config;

        public TodoItemsController(ApplicationContext context, IConfiguration config)
        {
            _config = config;
            _context = context;
        }
        
        // GET: api/TodoItems/status
        [HttpGet("status")]
        [AllowAnonymous]
        public async Task<ActionResult>  GetStatus()
        {
            int length;
            int dimensions;
            DistanceTo[] closest;
            try
            {
                string relativeRootPath = _config["CONTROLLER_ROOT_PATH"] == null || _config["CONTROLLER_ROOT_PATH"] == "" ? ".." : _config["CONTROLLER_ROOT_PATH"];
                var voc = new Word2VecBinaryReader().Read(Path.GetFullPath(relativeRootPath + "/GoogleNews-vectors-negative300.bin"));
                length = voc.Words.Length;
                dimensions = voc.VectorDimensionsCount;
                closest = await Task.Run(() => voc.Distance("dog", 1));

            }
            catch (Exception ex)
            {
                return NotFound(new { message = ".bin file could not be fully loaded: _" + _config["CONTROLLER_ROOT_PATH"] + "_ " + ex.Message });
            }
            return Ok(new
            {
                length = length.ToString(),
                dimensions = dimensions.ToString(),
                similarWord = closest[0].Representation.WordOrNull,
                rootPath = _config["CONTROLLER_ROOT_PATH"]
            });
        }

        // GET: api/TodoItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems()
        {
            try
            {
                _context.Database.OpenConnection();
            }
            catch (Exception ex)
            {
                return Content("Database connection failed: " + ex.Message); 
            }
              if (_context.TodoItems == null)
              {
                  return NotFound();
              }
              return await _context.TodoItems.ToListAsync();
        }

        // GET: api/TodoItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetTodoItem(long id)
        {
            if (_context.TodoItems == null)
            {
                return NotFound();
            }
            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }

        // PUT: api/TodoItems/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(long id, TodoItem todoItem)
        {
            if (id != todoItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(todoItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoItemExists(id))
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

        // POST: api/TodoItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem todoItem)
        {
            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();

            //    return CreatedAtAction("GetTodoItem", new { id = todoItem.Id }, todoItem);
            return CreatedAtAction(nameof(PostTodoItem), new { id = todoItem.Id }, todoItem);
        }

        // DELETE: api/TodoItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(long id)
        {
            if (_context.TodoItems == null)
            {
                return NotFound();
            }
            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TodoItemExists(long id)
        {
            return (_context.TodoItems?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
