using System.Collections.Generic;
using System.Data.Entity;

namespace LoginsignupPge.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base("DefaultConnection")
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Complaint> Complaints { get; set; }
    }
}
