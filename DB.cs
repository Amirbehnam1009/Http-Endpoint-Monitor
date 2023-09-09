using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Backend
{
    public class DB : DbContext
    {
        public DB(DbContextOptions<DB> dbContextOptions):base(dbContextOptions)
        {

        }
        public DbSet<User> Users { set; get; }
        public DbSet<URL> URLs { set; get; }
        public DbSet<Request> Requests { set; get; }
    }

    public class User
    {
        [Key]
        public int Id { set; get; }
        public string FirstName { set; get; }
        public string LastName { set; get; }
        public string UserName { set; get; }
        public string Password { set; get; }
    }

    public class URL
    {
        [Key]
        public int Id { set; get; }
        public int UserId { set; get; }
        public string Address { set; get; }
        public int Threshold { set; get; }
    }
    public class Request
    {
        [Key]
        public int Id { set; get; }
        public int URLId { set; get; }
        public int Result { set; get; }
        public DateTime DateTime { set; get; }
    }
}
