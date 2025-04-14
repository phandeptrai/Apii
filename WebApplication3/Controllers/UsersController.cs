using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly PostifyContext _context;

        public UsersController(PostifyContext context)
        {
            _context = context;
        }

        // GET: api/Users or api/Users?id=5
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers([FromQuery] int? id)
        {
            if (id.HasValue)
            {
                var user = await _context.Users.FindAsync(id.Value);
                if (user == null)
                {
                    return NotFound();
                }
                return new List<User> { user };
            }

            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<User>> GetUserByPath(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users?id=5
        [HttpPut]
        public async Task<IActionResult> PutUserByQuery([FromQuery] int id, User user)
        {
            return await HandlePutUser(id, user);
        }

        // PUT: api/Users/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutUserByPath(int id, User user)
        {
            return await HandlePutUser(id, user);
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUserByPath), new { id = user.Id }, user);
        }

        // DELETE: api/Users?id=5
        [HttpDelete]
        public async Task<IActionResult> DeleteUserByQuery([FromQuery] int id)
        {
            return await HandleDeleteUser(id);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteUserByPath(int id)
        {
            return await HandleDeleteUser(id);
        }

        // ========================
        // 🔁 Shared logic handlers
        // ========================

        private async Task<IActionResult> HandlePutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest("ID trong route/query không khớp với ID trong body.");
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        private async Task<IActionResult> HandleDeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
