using ContactsWebApi.Model;
using Microsoft.EntityFrameworkCore;

namespace ContactsWebApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {


        }
        public DbSet<Contact> Contact { get; set; }
        public DbSet<ElysianContact> ElysianContactTable { get; set; }



    }
}
