
using ContactsWebApi.Data;
using ContactsWebApi.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace ContactApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ValuesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Values
        [HttpGet]
        public async Task <ActionResult<IEnumerable<Contact>>> GetContacts()

        {
            var contacts = await _context.Contact.FromSqlRaw("EXEC spGetContacts1").AsNoTracking().ToListAsync();

            return Ok(contacts);
        }

        // GET: api/Values/5
        [HttpGet("{id}")]

        public async Task<ActionResult<Contact>> GetContact(int id)
        {
            // Execute raw SQL to get data
            var contacts = await _context.Contact
                .FromSqlRaw("EXEC spGetContact1 @Id", new SqlParameter("@Id", id))
                .ToListAsync(); // Fetch all data to a list

            // Use LINQ on the list in memory
            var cont = contacts.FirstOrDefault();

            if (cont == null)
            {
                return NotFound();
            }

            return Ok(cont);
        }



        /* public async Task<ActionResult<Contact>> GetContact(int id)
         {

             var contact = await _context.Contact.FromSqlRaw("EXEC spGetContact1 @Id", new SqlParameter("@Id", id)).AsNoTracking().SingleOrDefaultAsync();
             if (contact == null)
             {
                 return NotFound();
             }
             return Ok(contact);
         }*/


        // POST: api/Values
        [HttpPost]
        public async Task<ActionResult<Contact>> CreateContact(Contact contact)
        {
            var parameters = new[]
            {
                new SqlParameter("@Name", contact.Name),
                new SqlParameter("@Email", contact.Email),
                new SqlParameter("@Phone", contact.Phone),
                new SqlParameter("@Message", contact.Message)
            };

            var result = await _context.Contact.FromSqlRaw("EXEC spAddContact1 @Name, @Email, @Phone, @Message", parameters).AsNoTracking().ToListAsync();
            var createdContact = result.FirstOrDefault();

            if (createdContact == null)
            {
                return BadRequest("Unable to create contact.");
            }

            return CreatedAtAction(nameof(GetContact), new { id = createdContact.Id }, createdContact);
        }

        // PUT: api/Values/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateContact1(int id, Contact contact)
        {
            var parameters = new[]
            {
                new SqlParameter("@Id", id),
                new SqlParameter("@Name", contact.Name),
                new SqlParameter("@Email", contact.Email),
                new SqlParameter("@Phone", contact.Phone),
                new SqlParameter("@Message", contact.Message)
            };

            var result = await _context.Contact.FromSqlRaw("EXEC spUpdateContact1 @Id, @Name, @Email, @Phone, @Message", parameters).AsNoTracking().ToListAsync();
            var updatedContact = result.FirstOrDefault();

            if (updatedContact == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        // DELETE: api/Values/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContact(int id)
        {
            var result = await _context.Database.ExecuteSqlRawAsync("EXEC spDeleteContact1 @Id", new SqlParameter("@Id", id));

            if (result == 0)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
